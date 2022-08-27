using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PacmanMovement : MonoBehaviour
{
    public static PacmanMovement instance;
    // Variables
    public enum PacmanStates
    {
        Normal,
        Energized,
    }
    public enum PacmanDirections
    {
        left,
        up,
        right,
        down,
    }
    public PacmanStates pacmanState;
    public PacmanDirections pacmanDirection;

    // TimerVariables
    public float energizerTimer = 6;
    private bool energizerCounting = false;
    private float pauseTimer;
    private bool pauseCounting = false;
    
    // Enemies
    public float ghostSeries = 0;
    public GameObject Blinky;
    public GameObject Pinky;
    public GameObject Inky;
    public GameObject Clyde;
    
    // SoundEffects
    public AudioSource pacmanLoopSound;
    public AudioSource pacmanWinningSound;
    public AudioSource pacmanDeathSound;
    public AudioSource pelletSound;
    public AudioSource powerPelletSound;
    
    // Private Variables
    private GameObject pacman;
    private float speed = 0.22f;
    private Vector2 endPosition = new Vector2();

    // Pausing
    public void PauseGame ()
    {
        pacmanLoopSound.Stop();
        if (ScoreCounter.instance.PelletCounter == 0)
            pacmanWinningSound.Play();
        else
            pacmanDeathSound.Play();
        pauseTimer = 2;
        pauseCounting = true;
        Time.timeScale = 0;
    }
    
    void ResumeGame ()
    {
        Time.timeScale = 1;
        ScoreCounter.instance.RemoveText();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private void checkPause() 
    {
        if (pauseTimer > 0)
        {
            pauseTimer -= Time.unscaledDeltaTime;
        }
        else if (pauseCounting)
        {
            ResumeGame();
            pauseCounting = false;
            pauseTimer = 0;
        }
    }
    
    private void checkTimer() 
    {
        if (energizerCounting && energizerTimer > 0)
            energizerTimer -= Time.deltaTime;
        else
        {
            normalize();
            energizerTimer = 0;
        }
    }
    
    // Teleporting
    public void teleport(Vector2 coordinates)
    {
        pacman.transform.position = coordinates;
        endPosition = coordinates;
    }
    
    // Sound Effects
    public void playPelletSound()
    {
        pelletSound.Play();
    }
    
    // Checks direction if its free to move
    bool canMove(Vector2 direction)
    {
        Vector2 currentPosition = pacman.transform.position;
        RaycastHit2D barrier = Physics2D.Linecast(currentPosition + direction, currentPosition);
        if (GetComponent<Collider2D>() == barrier.collider)
            return true;
        return false;
    }

    // Updating States
    private void resetGhosts()
    {
        Ghost blinky = Blinky.GetComponent<Ghost>();
        blinky.resetEaten();
        Ghost pinky = Pinky.GetComponent<Ghost>();
        pinky.resetEaten();
        Ghost inky = Inky.GetComponent<Ghost>();
        inky.resetEaten();
        Ghost clyde = Clyde.GetComponent<Ghost>();
        clyde.resetEaten();
    }
    
    private void resetGhostSeries()
    {
        ghostSeries = 0;
    }

    public void normalize()
    {
        pacmanState = PacmanStates.Normal;
        energizerCounting = false;
        resetGhostSeries();
    }
    
    public void energize()
    {
        resetGhosts();
        powerPelletSound.Play();
        pacmanState = PacmanStates.Energized;
        energizerTimer = 6;
        energizerCounting = true;
    }
    
    // Setting new waypoints, directions and moving
    public Vector2 direction2Vector2(PacmanDirections direction)
    {
        if (direction == PacmanDirections.left)
            return Vector2.left;
        if (direction == PacmanDirections.up)
            return Vector2.up;
        if (direction == PacmanDirections.right)
            return Vector2.right;
        if (direction == PacmanDirections.down)
            return Vector2.down;
        return Vector2.zero;
    }

    private void changeDirection(PacmanDirections newDirection)
    {
        pacmanDirection = newDirection;
    }
    
    private void move()
    {
        endPosition = (Vector2)pacman.transform.position + direction2Vector2(pacmanDirection);
    }
    
    private void setNextWaypoint()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && canMove(Vector2.left))
            changeDirection(PacmanDirections.left);
        if (Input.GetKey(KeyCode.UpArrow) && canMove(Vector2.up))
            changeDirection(PacmanDirections.up);
        if (Input.GetKey(KeyCode.RightArrow) && canMove(Vector2.right))
            changeDirection(PacmanDirections.right);
        if (Input.GetKey(KeyCode.DownArrow) && canMove(Vector2.down))
            changeDirection(PacmanDirections.down);
        if (canMove(direction2Vector2(pacmanDirection)))
            move();
    }

    // Updates animations
    private void updateAnimator()
    {
        if (pacmanDirection == PacmanDirections.left)
            GetComponent<Animator>().SetInteger("Direction", 0);
        else if (pacmanDirection == PacmanDirections.up)
            GetComponent<Animator>().SetInteger("Direction", 1);
        else if (pacmanDirection == PacmanDirections.right)
            GetComponent<Animator>().SetInteger("Direction", 2);
        else if (pacmanDirection == PacmanDirections.down)
            GetComponent<Animator>().SetInteger("Direction", 3);
    }

    // Called at the beginning;
    public void Awake()
    {
        instance = this;
        normalize();
        pacmanLoopSound.Play();
        pacman = GameObject.FindWithTag("Player");
        endPosition = (Vector2)pacman.transform.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        // Endgame, winning
        if (ScoreCounter.instance.PelletCounter == 0)
        {
            PauseGame();
            ScoreCounter.instance.YouWon();
        }
        
        checkPause();
        checkTimer();

        // Soft moving to endPosition
        Vector2 pacmanPosition = Vector2.MoveTowards(pacman.transform.position, endPosition, speed);
        GetComponent<Rigidbody2D>().MovePosition(pacmanPosition);
        
        // Moving / Changing direction
        if ((Vector2) pacman.transform.position == endPosition)
            setNextWaypoint();

        updateAnimator();
    }
}