using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Ghost : MonoBehaviour
{
	// State Variables
    public enum GhostStates
    {
        Chasing,
        Scatter,
        Frightened,
		Blinking,
        Respawning,
    }

    public enum GhostTypes
    {
	    blinky,
	    pinky,
	    inky,
	    clyde,
    }
    
    public enum Directions
    {
	    left,
	    up,
	    right,
	    down,
	    none,
    }
    
    public GhostStates ghostState;
    public GhostTypes ghostType;
 	private Directions currentDirection;

	public SpriteRenderer spriteRenderer;
    
    // Pacman
    public GameObject Pacman;
    private PacmanMovement pacman;

    // Audio
    public AudioSource ghostEatenSound;

    // Movement
	public GameObject home;
	public GameObject start;
	public GameObject Scatter;
	private GameObject waypoint;
	private GameObject nextWaypoint;
	
	// Exclusive for Inky
	public GameObject Blinky;

	// Eyes
	public SpriteRenderer spriteRendererEyes;
	public Sprite Up;
	public Sprite Right;
	public Sprite Down;
	public Sprite Left;
	
	// Timer Variables
	private float Timer;
	private bool wasChasing = false; 

	// Private Variables
	private float speed = 0.12f;
	private bool respawned;
    private bool leftHome;
	private bool eaten;

	public void teleport(Vector2 coordinates, GameObject portal)
	{
		transform.position = coordinates;
		nextWaypoint = portal;
	}
    void setColor()
    {
	    if (ghostState == GhostStates.Chasing || ghostState == GhostStates.Scatter)
	    {
		    if (ghostType == GhostTypes.blinky)
		    {
			    spriteRenderer.color = new Color(1,0,0,1);
		    }

		    if (ghostType == GhostTypes.pinky)
		    {
			    spriteRenderer.color = new Color(1,0.15f,1,1);
		    }

		    if (ghostType == GhostTypes.inky)
		    {
			    spriteRenderer.color = new Color(0,1,1,1);
		    }

		    if (ghostType == GhostTypes.clyde)
		    {
			    spriteRenderer.color = new Color(1,0.59f,0,1);
		    }
	    }
	    else if (ghostState == GhostStates.Respawning)
		    spriteRenderer.color = Color.clear;
	    else 
		    spriteRenderer.color = Color.white;
		    
    }

    public void changeEyes(Directions direction)
    {	
	    if (direction == Directions.up)
		    spriteRendererEyes.sprite = Up;
	    else if (direction == Directions.right)
		    spriteRendererEyes.sprite = Right;
	    else if (direction == Directions.down)
		    spriteRendererEyes.sprite = Down;
	    else if (direction == Directions.left)
		    spriteRendererEyes.sprite = Left;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

	    if (collision.tag == "Player")
	    {
		    if (ghostState == GhostStates.Chasing || ghostState == GhostStates.Scatter)
		    {
			    ScoreCounter.instance.YouLost();
			    pacman.PauseGame();
		    }

		    else if (ghostState == GhostStates.Frightened || ghostState == GhostStates.Blinking)
		    {
			    eaten = true;
			    ghostEatenSound.Play();
			    ScoreCounter.instance.AddPoints(200 * (int)Mathf.Pow(2, pacman.ghostSeries));
			    pacman.ghostSeries += 1;
				respawn();
			}
		}
    }

    void respawn()
	{
		ghostState = GhostStates.Respawning;
		nextWaypoint = home;
		respawned = false;
		currentDirection = Directions.up;
	}
    
	public void resetEaten()
	{
		eaten = false;
	}

    void resetConditions()
    {
	    if (wasChasing)
		    ghostState = GhostStates.Chasing;
	    else
		    ghostState = GhostStates.Scatter;
	    nextWaypoint = home;
	    respawned = true;
	    leftHome = false;
	    speed = 0.12f;
	    setColor();
	    
	    if (ghostType == GhostTypes.blinky)
	    {
		    currentDirection = Directions.right;
	    }
	    if (ghostType == GhostTypes.pinky)
	    {
		    currentDirection = Directions.right;
	    }
	    if (ghostType == GhostTypes.inky)
	    {
		    currentDirection = Directions.left;
	    }
	    if (ghostType == GhostTypes.clyde)
	    {
		    currentDirection = Directions.left;
	    }
    }

    public void checkState()
	{
		setColor();
		if (ghostState == GhostStates.Chasing || ghostState == GhostStates.Scatter)
		{
			speed = 0.12f;
			GetComponent<Animator>().SetBool("isFrightened", false);
			GetComponent<Animator>().SetBool("isBlinking", false);
		}
		else if (ghostState == GhostStates.Frightened)
		{
			speed = 0.10f;
			GetComponent<Animator>().SetBool("isFrightened", true);
			GetComponent<Animator>().SetBool("isBlinking", false);
		}
		else if (ghostState == GhostStates.Blinking)
		{
			speed = 0.10f;
			GetComponent<Animator>().SetBool("isFrightened", false);
			GetComponent<Animator>().SetBool("isBlinking", true);
		}
		else if (ghostState == GhostStates.Respawning)
		{
			speed = 0.08f;
			if (nextWaypoint.transform.position == transform.position)
				ghostState = GhostStates.Chasing;
			GetComponent<Animator>().SetBool("isFrightened", false);
			GetComponent<Animator>().SetBool("isBlinking", false);
		}
	}

    public void updateState()
    {
	    if (pacman.pacmanState == PacmanMovement.PacmanStates.Normal)
	    {
		    resetEaten();
		    if (Timer > 0 && (ghostState == GhostStates.Scatter || ghostState == GhostStates.Chasing))
			    Timer -= Time.deltaTime;
		    else if (ghostState == GhostStates.Blinking)
		    {
			    if (wasChasing)
				    ghostState = GhostStates.Chasing;
			    else
				    ghostState = GhostStates.Scatter;
			}
	    else
		    {
			    if (ghostState == GhostStates.Scatter)
			    {
				    ghostState = GhostStates.Chasing;
				    wasChasing = true;
				    Timer = 20;
			    }
			    else if (ghostState == GhostStates.Chasing)
			    {
				    ghostState = GhostStates.Scatter;
				    wasChasing = false;
				    Timer = 7;
			    }
		    }
	    }
	    else if (pacman.pacmanState == PacmanMovement.PacmanStates.Energized && (!eaten || pacman.ghostSeries == 0) && respawned && leftHome)
	    {
		    // Causes Ghost to get scared and turn around
		    if (ghostState == GhostStates.Chasing || ghostState == GhostStates.Scatter)
			    nextWaypoint = waypoint;

		    // Frighten or Blink
		    if (pacman.energizerTimer > 2)
			    ghostState = GhostStates.Frightened;
		    else
			    ghostState = GhostStates.Blinking;
	    }
    }

    /// <summary>
    /// PLACE FOR ALGORITHMs
    /// </summary>
    /// <returns></returns>

    GameObject getBestDirection(Vector2 enemy)
    {
	    float shortestDistance = -1; // -1 as non existent
	    GhostWaypointsController currentWaypoint = waypoint.GetComponent<GhostWaypointsController>();
	    GameObject shortestWay = waypoint;
		Directions shortestDirection = Directions.none;
	    
	    
	    if (currentWaypoint.canMove(Vector2.up) && currentDirection != Directions.down)
	    {
		    float distance = Vector2.Distance(currentWaypoint.upWaypoint.transform.position, enemy);
		    if (distance < shortestDistance || shortestDistance == -1)
		    {
			    shortestDistance = distance;
			    shortestDirection = Directions.up;
			    shortestWay = currentWaypoint.upWaypoint;
		    }
	    }

	    if (currentWaypoint.canMove(Vector2.left) && currentDirection != Directions.right)
	    {
		    float distance =  Vector2.Distance(currentWaypoint.leftWaypoint.transform.position, enemy);
		    if (distance < shortestDistance || shortestDistance == -1)
		    {
			    shortestDistance = distance;
			    shortestDirection = Directions.left;
			    shortestWay = currentWaypoint.leftWaypoint;
		    }
	    }
	    
	    if (currentWaypoint.canMove(Vector2.down) && currentDirection != Directions.up)
	    {
		    float distance = Vector2.Distance(currentWaypoint.downWaypoint.transform.position, enemy);
		    if (distance < shortestDistance || shortestDistance == -1)
		    {
			    shortestDistance = distance;
			    shortestDirection = Directions.down;
			    shortestWay = currentWaypoint.downWaypoint;
		    }
	    }
	    
	    if (currentWaypoint.canMove(Vector2.right) && currentDirection != Directions.left)
	    {
		    float distance = Vector2.Distance(currentWaypoint.rightWaypoint.transform.position, enemy);
		    if (distance < shortestDistance || shortestDistance == -1)
		    {
			    shortestDistance = distance;
			    shortestDirection = Directions.right;
			    shortestWay = currentWaypoint.rightWaypoint;
		    }
	    }
	    currentDirection = shortestDirection;
	    return shortestWay;
    }

    // Ghost ScatterModes
    GameObject getScatterWaypoint()
    {
	    return getBestDirection(Scatter.transform.position);
    }
    
    // Ghost A.I.
    GameObject BlinkyAI()
    {
	    return getBestDirection(pacman.transform.position);
    }

    GameObject PinkyAI()
    {
	    Vector2 pacmanDirection = pacman.direction2Vector2(pacman.pacmanDirection);
	    Vector2 pacmanPosition = (Vector2)pacman.transform.position + pacmanDirection * 4;
	    return getBestDirection(pacmanPosition);
    }
    
    GameObject InkyAI()
    {
	    Vector2 pacmanPosition = ((pacman.transform.position - Blinky.transform.position) * 2);
	    return getBestDirection(pacmanPosition);
    }
    
    GameObject ClydeAI()
    {
	    if (Vector2.Distance(transform.position, pacman.transform.position) < 10)
		    return getBestDirection(pacman.transform.position);
	    else
		    return getScatterWaypoint();

    }
    

    // Random directions
    GameObject setRandomWaypoint()
	{
		GhostWaypointsController currentWaypoint = waypoint.GetComponent<GhostWaypointsController>();
		int x = Random.Range (0, 4);
		switch (x)
		{
			case 0:
				if (currentWaypoint.canMove(Vector2.left) && currentDirection != Directions.right)
				{
					currentDirection = Directions.left;
					return currentWaypoint.leftWaypoint;
				}
				else
					return waypoint;
			case 1:
				if (currentWaypoint.canMove(Vector2.up) && currentDirection != Directions.down)
				{
					currentDirection = Directions.up;
					return currentWaypoint.upWaypoint;
				}
				else
					return waypoint;
			case 2:
				if (currentWaypoint.canMove(Vector2.right) && currentDirection != Directions.left)
				{
					currentDirection = Directions.right;
					return currentWaypoint.rightWaypoint;
				}
				else
					return waypoint;
			case 3:
				if (currentWaypoint.canMove(Vector2.down) && currentDirection != Directions.up)
				{
					currentDirection = Directions.down;
					return currentWaypoint.downWaypoint;
				}
				else
					return waypoint;
			default:
				return waypoint;
		}
	}

    void setNextWaypoint()
    {
	    if (ghostType == GhostTypes.blinky)
	    {
		    if (ghostState == GhostStates.Chasing)
			    nextWaypoint = BlinkyAI();
		    else if (ghostState == GhostStates.Scatter)
			    nextWaypoint = getScatterWaypoint();
		    else
			    nextWaypoint = setRandomWaypoint();
	    }
	        
	    if (ghostType == GhostTypes.pinky)
	    {
		    if (ghostState == GhostStates.Chasing)
			    nextWaypoint = PinkyAI();
		    else if (ghostState == GhostStates.Scatter)
			    nextWaypoint = getScatterWaypoint();
		    else
			    nextWaypoint = setRandomWaypoint();
	    }
	        
	    if (ghostType == GhostTypes.inky)
	    {
		    if (ghostState == GhostStates.Chasing)
			    nextWaypoint = InkyAI();
		    else if (ghostState == GhostStates.Scatter)
			    nextWaypoint = getScatterWaypoint();
		    else
			    nextWaypoint = setRandomWaypoint();
	    }
	        
	    if (ghostType == GhostTypes.clyde)
	    {
		    if (ghostState == GhostStates.Chasing)
			    nextWaypoint = ClydeAI();
		    else if (ghostState == GhostStates.Scatter)
			    nextWaypoint = getScatterWaypoint();
		    else
			    nextWaypoint = setRandomWaypoint();
	    }
    }
	
	// Called at start
    void Awake()
    {
	    pacman = Pacman.GetComponent<PacmanMovement>();
	    resetConditions();
	    Timer = 7;
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
	    // State change
		updateState();
		checkState();

		// Soft moving to endPosition
        if (nextWaypoint.transform.position == transform.position)
        {
	        if (home.transform.position == transform.position)
		        resetConditions();

	        if (start.transform.position == transform.position)
		        leftHome = true;
	        
	        waypoint = nextWaypoint;
	        setNextWaypoint();
	        changeEyes(currentDirection);
        }
        else 
		{
            Vector2 ghostPosition = Vector2.MoveTowards(transform.position, nextWaypoint.transform.position, speed);
            GetComponent<Rigidbody2D>().MovePosition(ghostPosition);
        }
    }
}
