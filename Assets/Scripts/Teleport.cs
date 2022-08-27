using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject portal;
    public GameObject Blinky;
    public GameObject Pinky;
    public GameObject Inky;
    public GameObject Clyde;
    public GameObject leftWaypoint;
    public GameObject rightWaypoint;
    public GameObject Pacman;
    private PacmanMovement pacman;

    // Start is called before the first frame update
    void Awake()
    {
        pacman = Pacman.GetComponent<PacmanMovement>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (Pacman.transform.position.x > 14)
				pacman.teleport(new Vector2(portal.transform.position.x + 2, portal.transform.position.y));
            else if (Pacman.transform.position.x < 14)
				pacman.teleport(new Vector2(portal.transform.position.x - 2, portal.transform.position.y));
        }
        if (collision.name == "Blinky")
        {
            Ghost blinky = Blinky.GetComponent<Ghost>();
            if (Blinky.transform.position.x > 14)
                blinky.teleport(new Vector2(portal.transform.position.x + 2, portal.transform.position.y), leftWaypoint);
            else if (Blinky.transform.position.x < 14)
                blinky.teleport(new Vector2(portal.transform.position.x - 2, portal.transform.position.y), rightWaypoint);
        }
        if (collision.name == "Pinky")
        {
            Ghost pinky = Pinky.GetComponent<Ghost>();
            if (Pinky.transform.position.x > 14)
                pinky.teleport(new Vector2(portal.transform.position.x + 2, portal.transform.position.y), leftWaypoint);
            else if (Pinky.transform.position.x < 14)
                pinky.teleport(new Vector2(portal.transform.position.x - 2, portal.transform.position.y), rightWaypoint);
        }
        if (collision.name == "Inky")
        {
            Ghost inky = Inky.GetComponent<Ghost>();
            if (Inky.transform.position.x > 14)
                inky.teleport(new Vector2(portal.transform.position.x + 2, portal.transform.position.y), leftWaypoint);
            else if (Inky.transform.position.x < 14)
                inky.teleport(new Vector2(portal.transform.position.x - 2, portal.transform.position.y), rightWaypoint);
        }
        if (collision.name == "Clyde")
        {
            Ghost clyde = Clyde.GetComponent<Ghost>();
            if (Clyde.transform.position.x > 14)
                clyde.teleport(new Vector2(portal.transform.position.x + 2, portal.transform.position.y), leftWaypoint);
            else if (Clyde.transform.position.x < 14)
                clyde.teleport(new Vector2(portal.transform.position.x - 2, portal.transform.position.y), rightWaypoint);
        }
    }
}