using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*************************************************************************
 * initializes sound effects and controls the player jump action
 * 
 * Bryce Haddock
 * October 31,2023     Version 1.0
 * **********************************************************************/

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float jumpForce, gravityModifier;
    [SerializeField] private ParticleSystem dirtParticle, explosionParticle;
    [SerializeField] private AudioClip jumpSound, crashSound;
    [SerializeField] private GameManager gameManager;
    private Animator playerAnimation;
    private AudioSource playerAudio;
    private Rigidbody playerRB;
    private bool isOnGround;

    public bool gameOver { get; private set; }

    // initializes player rigidbody, sets ground to true and sets gravity
    private void Start()
    {
        if(GameManager.gameInProgress)
        {
            gameManager.BackToGame();
        }
       
        
            transform.position = new Vector3(12.8f, .12f, .2f);
            
        
        
        
        playerAudio = GetComponent<AudioSource>();
        playerAnimation = GetComponent<Animator>();

        playerRB = GetComponent<Rigidbody>();
        isOnGround = true;
        Physics.gravity *= gravityModifier;
       
    }
    // on collision with ground sets ground equal to true
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Ground")
        {
            if(!GameManager.gameOver)
            {
                dirtParticle.Play();
                Debug.Log("on ground");
            }
            isOnGround = true;
        }
        if(collision.gameObject.tag == "Obstacle")
        {
            playerAnimation.Play("Death");
            GameManager.gameOver = true;
            explosionParticle.Play();
            dirtParticle.Stop();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            
        }
    }
    // if grounded and jump input is pressed the character will be launched vertically equal to jump force
    private void OnJump(InputValue input)
    {
        if(isOnGround && !GameManager.gameOver)
        {
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();
            isOnGround = false;
            playerAnimation.SetTrigger("Jump_trig");
            playerRB.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("jump is on");
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Scoreable")
        {
            GameManager.ChangeScore(2f);
        }

    }
}
