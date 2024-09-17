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
    public float jumpRayDistance = 0.2f;

    [SerializeField] private Rigidbody2D rigidBody;
    Vector3 startPosition; // Variables are private by default.
    Animator animator; // To control animations
    SpriteRenderer spriteRenderer; // To control the sprite

    const string STATE_ALIVE = "isAlive"; // "isAlive" is the parameter in the Animator section of Unity.
    const string STATE_TAKE_OF = "takeOf";
    const string STATE_RUNNING = "isRunning";
    const string STATE_CLIMBING = "isClimbing";

    [SerializeField] private int healthPoints; // Carrots in this game
    [SerializeField] private int manaPoints; // When destroying enemies
    
    public const int INITIAL_HEALTH = 5, MAX_HEALTH = 10, MIN_HEALTH = 1,
        INITIAL_MANA = 15, MAX_MANA = 30, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 5; // With mana
    public const float SUPERJUMP_FORCE = 1.2f;

    public LayerMask groundMask; // To identify the ground

    // For Climbing
    private float vertical; // vertical input.
    private float speed = 1f;
    private bool isLadder; // If the player is standing next to a ladder.
    private bool isClimbing; // If the player is already climbing the ladder or not.

    // Awake is called before Start, it's the first to load.
    void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        // For Climbing
        vertical = Input.GetAxis("Vertical"); // Return a value between -1 and 1 depending on the button pressed.

        if (isLadder && Math.Abs(vertical) > 0f) {
            isClimbing = true;
        }


        // // Gizmos - works for debugging, view lines in game mode
        // float characterHeight = GetComponent<Collider2D>().bounds.size.y; // Character's height
        // jumpRayDistance = 0.8f * characterHeight; // 80% of the height, since the bottom edge of the collider is a little above the real bottom edge of the object, so 50% still doesn't get to touch the ground layer.

        // Vector2 direction = Vector2.down; // Ray direction down
        // direction.Normalize(); // Normalize the vector. This ensures that the direction has 1 unit of length and the result is more accurate.

        // Debug.DrawRay(this.transform.position, direction * jumpRayDistance, purple); // this.transform.position: Actual position from the center of the character
    }

    // FixedUpdate is dedicated for physics. Applies input from player. Is called once per fixed ratio - to prevent lag due to a fps drop in Update()
    void FixedUpdate() {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            // Move player
            HandleMovement();

            // Change sprite direction as needed
            Flip();

            // For jumping
            if (jump) {
                Jump();
                jump = false;
            }

            // For climbing
            if (isClimbing) {
                animator.SetBool(STATE_CLIMBING, true);
                rigidBody.gravityScale = 0f;
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, vertical * speed);
            } else {
                animator.SetBool(STATE_CLIMBING, false);
                rigidBody.gravityScale = 1f; // Normal value;
            }
        } else { // If not in game.
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
        }
    }

    private void HandleMovement() {
        animator.SetBool(STATE_RUNNING, true);

        if (Math.Abs(rigidBody.velocity.x) < runningSpeed) {
            rigidBody.velocity = new Vector2(horizontalMove * runningSpeed, rigidBody.velocity.y);
        }

        if (horizontalMove == 0) {
            animator.SetBool(STATE_RUNNING, false);
        }
    }

    private void Flip() {
        if ((horizontalMove > 0 && !facingRight) || (horizontalMove < 0 && facingRight)) {
            facingRight = !facingRight; // It's important to make the flip correctly.

            spriteRenderer.flipX = !spriteRenderer.flipX; // Turn the sprite
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
        jumpRayDistance = 0.8f * characterHeight; // 80% of the height, since the bottom edge of the collider is a little above the real bottom edge of the object, so 50% still doesn't get to touch the ground layer.

        Vector2 direction = Vector2.down; // Ray direction down
        direction.Normalize(); // Normalize the vector. This ensures that the direction has 1 unit of length and the result is more accurate.

        // In Raycast the order of parameters is important.
        if (Physics2D.Raycast(this.transform.position, // Actual position from the center of the character
                                direction, // Ray direction
                                jumpRayDistance, // Max distance of the ray
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
        float distance = this.transform.position.x - startPosition.x;

        if (distance < 0) {
            distance = 0;
        }
        
        return distance;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // For climbing
        if (collision.CompareTag("Stair")) {
            isLadder = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {
        // For climbing
        if (collision.CompareTag("Stair")) {
            isLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        // For climbing
        if (collision.CompareTag("Stair")) {
            animator.SetBool(STATE_CLIMBING, false);
            animator.speed = 2f; // Increases animation speed (2x faster)
            isLadder = false;
            isClimbing = false;

            // Reset animation speed to normal after a while
            StartCoroutine(ResetAnimationSpeed());
        }
    }

    private IEnumerator ResetAnimationSpeed() {
        yield return new WaitForSeconds(0.3f); // Waiting time
        animator.speed = 1f; // Resets animation speed to normal
    }
}
