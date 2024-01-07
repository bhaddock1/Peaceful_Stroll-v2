using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeft : MonoBehaviour
{
    private float speed;
    private float leftBound;
    private PlayerController playerControllerScript;

    // Set initial values and get player controller
    private void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        speed = GameManager.speed;
        leftBound = -15;
    }

    // Update is called once per frame
    private void Update()
    {
        ControlTransform();
    }

    private void ControlTransform()
    {
        speed = GameManager.speed;
        if (!GameManager.gameOver)
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed);
        }

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
    
    
}
