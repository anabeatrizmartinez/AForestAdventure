using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Variables of player movement
    public float jumpForce = 3f;  // Public variables can be edited from Unity.
    public float runningSpeed = 1f;
    float horizontalMove = 0f;
    bool facingRight;
    bool jump = false;
    bool isSuperJump = false;
    Color purple = new Color(0.6f, 0.0f, 0.8f);

    public CharacterController controller;
    Rigidbody2D rigidBody; // Variables are private by default.
    Animator animator; // To control animations
    Vector3 startPosition;

    const string STATE_ALIVE = "isAlive"; // "isAlive" is the parameter in the Animator section of Unity.
    const string STATE_TAKE_OF = "takeOf";
    const string STATE_RUNNING = "isRunning";

    private int healthPoints; // Carrots in this game
    private int manaPoints; // When destroying enemies
    
    public const int INITIAL_HEALTH = 5, MAX_HEALTH = 10, MIN_HEALTH = 1,
        INITIAL_MANA = 15, MAX_MANA = 30, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5; // With mana
    public const float SUPERJUMP_FORCE = 1.2f;

    public LayerMask groundMask; // To identify the ground


    // Awake is called before Start, it's the first to load.
    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start() {
        startPosition = this.transform.position;
    }

    public void StartGame() { // Personalized function
        animator.SetBool(STATE_ALIVE, true);
        facingRight = true;

        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;

        Invoke("RestartPosition", 0.2f); // Delay player reposition to wait until the animation of death is over.
    }

    void RestartPosition() {
        this.transform.position = startPosition;
        this.rigidBody.velocity = Vector2.zero; // Since the player falls at a high velocity at the moment of death, it's better to restart the velocity to prevent the player from going over the edge of the ground.
        
        Vector3 theScale = this.transform.localScale;
        theScale.x = Math.Abs(theScale.x); // To ensure the player is facing right.
        this.transform.localScale = theScale;

        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    // Update() is called once per frame. Get Input from player.
    void Update() {
        if (Input.GetButtonDown("Jump")) {
            jump = true;
            isSuperJump = false;
        }

        if (Input.GetButtonDown("Superjump")) {
            jump = true;
            isSuperJump = true;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal"); // Detect if player looking right or left. If press => or D results in 1, but if press <= or A results in -1.
        // Input.GetAxisRaw("Horizontal") gets the keys from Unity: | Edit | Project Settings | Input Manager | Axes | Horizontal |


        // // Gizmos - works for debuggin, view lines in game mode
        // float characterHeight = GetComponent<Collider2D>().bounds.size.y; // Character's height
        // float rayDistance = 0.6f * characterHeight; // 60% of the height, since the bottom edge of the collider is a little above the real bottom edge of the object, so 50% still doesn't get to touch the ground layer.

        // Vector2 direction = Vector2.down; // Ray direction down
        // direction.Normalize(); // Normalize the vector. This ensures that the direction has 1 unit of length and the result is more accurate.

        // Debug.DrawRay(this.transform.position, direction * rayDistance, purple); // this.transform.position: Actual position from the center of the character
    }

    // FixedUpdate is dedicated for physics. Applies input from player. Is called once per fixed ratio - to prevent lag due to a fps drop in Update()
    void FixedUpdate() {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            // Move player
            HandleMovement();

            Flip();

            if (jump) {
                Jump();
                jump = false;
            }
        } else { // If not in game.
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    private void HandleMovement() {
        animator.SetBool(STATE_RUNNING, true);

        if (rigidBody.velocity.x < runningSpeed) {
            rigidBody.velocity = new Vector2(horizontalMove * runningSpeed, rigidBody.velocity.y);
        }

        if (horizontalMove == 0) {
            animator.SetBool(STATE_RUNNING, false);
        }
    }

    private void Flip() {
        if ((horizontalMove > 0 && !facingRight) || (horizontalMove < 0 && facingRight)) {
            facingRight = !facingRight; // It's important to make the flip correctly.

            Vector3 theScale = this.transform.localScale;

            theScale.x *= -1;
            this.transform.localScale = theScale;
        }
    }

    void Jump() {
        float jumpForceFactor = jumpForce;

        if (isSuperJump && manaPoints >= SUPERJUMP_COST) {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }

        if (IsTouchingTheGround()) {
            rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);

            animator.SetTrigger(STATE_TAKE_OF);
        }
    }

    // Indicates if the character is touching or not the ground
    bool IsTouchingTheGround() { 
        float characterHeight = GetComponent<Collider2D>().bounds.size.y; // Character's height
        float rayDistance = 0.6f * characterHeight; // 60% of the height, since the bottom edge of the collider is a little above the real bottom edge of the object, so 50% still doesn't get to touch the ground layer.

        Vector2 direction = Vector2.down; // Ray direction down
        direction.Normalize(); // Normalize the vector. This ensures that the direction has 1 unit of length and the result is more accurate.

        // In Raycast the order of parameters is important.
        if (Physics2D.Raycast(this.transform.position, // Actual position from the center of the character
                                direction, // Ray direction
                                rayDistance, // Max distance of the ray
                                groundMask)) { // Against the layer of the ground

            // If the conditions above are true, the character is touching the ground.
            return true; 
        } else {
            return false;
        }
    }

    public void Die() {
        // Max score
        float travelledDistance = GetTravelledDistance();
        float previousMaxDistance = PlayerPrefs.GetFloat("maxscore", 0f);
        if (travelledDistance > previousMaxDistance) {
            PlayerPrefs.SetFloat("maxscore", travelledDistance);
        }

        animator.SetBool(STATE_ALIVE, false);

        GameManager.sharedInstance.GameOver();
    }

    public void CollectHealth(int points) {
        this.healthPoints += points;

        if (this.healthPoints >= MAX_HEALTH) {
            this.healthPoints = MAX_HEALTH;
        }
    }

    public void CollectMana(int points) {
        this.manaPoints += points;

        if (this.manaPoints >= MAX_MANA) {
            this.manaPoints = MAX_MANA;
        }
    }

    public int GetHealth() {
        return healthPoints;
    }

    public int GetMana() {
        return manaPoints;
    }

    public float GetTravelledDistance() {
        return this.transform.position.x - startPosition.x;
    }
}
