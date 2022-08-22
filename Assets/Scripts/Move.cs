using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Variables
    public float speed = 0.5f;
    private Vector2 endPosition = new Vector2();
    private Vector2 direction = new Vector2();
    private GameObject pacman;
    
    bool collision(Vector2 direction)
    {
        Vector2 currentPosition = pacman.transform.position;
        RaycastHit2D barrier = Physics2D.Linecast(currentPosition + direction, currentPosition);
        if (GetComponent<Collider2D>() == barrier.collider)
            return true;
        return false;
    }
    void changeDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    void move()
    {
        endPosition = (Vector2)pacman.transform.position + direction;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        pacman = GameObject.FindWithTag("Player");
        endPosition = (Vector2)pacman.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Soft moving to endPosition
        Vector2 pacmanPosition = Vector2.MoveTowards(pacman.transform.position, endPosition, speed);
        GetComponent<Rigidbody2D>().MovePosition(pacmanPosition);
        
        // Moving / Changing direction
        if ((Vector2)pacman.transform.position == endPosition)
        {
            if (Input.GetKey(KeyCode.LeftArrow) && collision(Vector2.left))
                changeDirection(Vector2.left);
            if (Input.GetKey(KeyCode.UpArrow) && collision(Vector2.up))
                changeDirection(Vector2.up);
            if (Input.GetKey(KeyCode.RightArrow) && collision(Vector2.right))
                changeDirection(Vector2.right);
            if (Input.GetKey(KeyCode.DownArrow) && collision(Vector2.down))
                changeDirection(Vector2.down);
            if (collision(direction))
                move();
        }
        
        Vector2 currentDirection = endPosition - (Vector2)pacman.transform.position;
        GetComponent<Animator>().SetFloat("X_direction", currentDirection.x);
        GetComponent<Animator>().SetFloat("Y_direction", currentDirection.y);
    }
}
