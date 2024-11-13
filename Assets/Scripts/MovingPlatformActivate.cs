using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformActivate : MonoBehaviour {

    public GameObject box; // block to activate the moving platform.
    public GameObject underground; // Ground that, upon collision with the platform, activates the moving platform mechanism.
    public GameObject movingPlatform; // the moving platform to activate with the mechanism.
    public float minMass = 3.5f; // min mass to activate the mechanism and push the platform with the box down.

    private Rigidbody2D rbPlatform; // platform where the box will be to push it down and activate the moving platform.
    private Rigidbody2D rbBox;
    private Collider2D colPlatform;
    private Collider2D colUnderground;
    private Vector3 startPosition; // initial position of the platform;
    private bool onCollision = false;
    private Animator animator; // moving platform animation.
    
    private AudioSource audioSource;


    private void Start() {
        audioSource = GetComponent<AudioSource>();
        rbPlatform = GetComponent<Rigidbody2D>();
        rbBox = box.GetComponent<Rigidbody2D>();
        colPlatform = GetComponent<Collider2D>();
        colUnderground = underground.GetComponent<Collider2D>();
        startPosition = this.transform.position;

        // Disable platform gravity from the start
        rbPlatform.gravityScale = 0;

        // Initially ignore collision between platform and underground
        Physics2D.IgnoreCollision(colPlatform, colUnderground, true);
        
        // Disable moving platform movement.
        animator = movingPlatform.GetComponent<Animator>();
        animator.enabled = false;
    }

    private void Update() {
        float totalMass = rbPlatform.mass + rbBox.mass;

        if (onCollision && totalMass >= minMass) {
            // Enable gravity and collision
            rbPlatform.gravityScale = 1;
            Physics2D.IgnoreCollision(colPlatform, colUnderground, false);
        } else {
            // Keep platform initial position and disable collision
            rbPlatform.gravityScale = 0;
            rbPlatform.position = startPosition;
            rbPlatform.velocity = Vector2.zero;
            Physics2D.IgnoreCollision(colPlatform, colUnderground, true);
        }
    }
    
    void OnCollisionEnter2D (Collision2D collision) {
        if (collision.gameObject.CompareTag("Box")) {
            audioSource.Play();
            
            onCollision = true;

            // Activate moving platform movement.
            animator.enabled = true;
        }
    }

    void OnCollisionExit2D (Collision2D collision) {
        if (collision.gameObject.CompareTag("Box")) {
            onCollision = false;

            // Disable moving platform movement.
            animator.enabled = false;
        }
    }
}
