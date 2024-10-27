using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {

    public float runningSpeed = 0.7f;
    public bool facingRight;

    Rigidbody2D rigidBody;
    private Vector3 startPosition; 
    private Animator animator;

    const string STATE_ENEMY_ALIVE = "isAlive";


    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        // startPosition = this.transform.position; // Enemy's start position for the first time.
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        animator.SetBool(STATE_ENEMY_ALIVE, true);
        // this.transform.position = startPosition; // Reset to the original position every time it's instantiated.
        facingRight = false;
    }

    private void FixedUpdate() {
        Flip();
    }

    private void Flip() {
        float currentRunningSpeed = runningSpeed;

        if (facingRight) {
            // Looking right (+ force)
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0); // To rotate 180°
        } else {
            // Looking left (- force)
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero; // To return to original position
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame) { // Move only inside Game.
            rigidBody.velocity = new Vector2(currentRunningSpeed, rigidBody.velocity.y);
        } else {
            rigidBody.velocity = Vector2.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Star" || collision.tag == "Carrot" || collision.tag == "Exit-zone" || collision.tag == "WalkBlock") {
            return;
        }

        if (collision.tag == "Player") {            
            // The logic is in the child script, both player and enemy.
            return;
        }

        // At this point, the enemy hasn't hit any stars, carrots, or the player.
        // So, in this new case, it must be another enemy or a scenery object.
        facingRight = !facingRight; // Rotate enemy.
    }
}