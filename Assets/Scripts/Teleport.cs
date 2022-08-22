using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public GameObject portal;

    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (player.transform.position.x > 14)
                player.transform.position = new Vector2(0, portal.transform.position.y);
            if (player.transform.position.x < 14)
                player.transform.position = new Vector2(27, portal.transform.position.y);
        }
    }
}
