using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingPacFood : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ScoreCounter.instance.PelletCounter -= 1;
            
            if (gameObject.tag == "Pellet")
            {
                Destroy(gameObject);
                ScoreCounter.instance.AddPoints(10);
                PacmanMovement.instance.playPelletSound();
            }

            else if (gameObject.tag == "PowerPellet")
            {
                Destroy(gameObject);
                ScoreCounter.instance.AddPoints(50);
                PacmanMovement.instance.energize();
            }
        }
    }
}