using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour {

    public float runningSpeed = 1f;
    public bool facingRight;
    public int enemyDamage = 2;
    public float forceDamageX  = 1f;
    public float forceDamageY  = 2.5f;

    Rigidbody2D rigidBody;
    private Vector3 startPosition;
    private GameObject player;
    private Rigidbody2D rbPlayer;
    private SpriteRenderer spriteRenderer;


    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        player = GameObject.Find("Player");
        rbPlayer = player.GetComponent<Rigidbody2D>();
        spriteRenderer = player.GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start() {
        this.transform.position = startPosition;
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Star" || collision.tag == "Carrot") {
            return;
        }

        if (collision.tag == "Player") {
            player.GetComponent<PlayerController>().CollectHealth(-enemyDamage); // Reduce Player's life

            // To make the player jump when collides with the enemy.
            Vector3 playerPos = collision.transform.position; // Player's position
            Vector3 currentPos = transform.position; // Enemy's position
            Vector3 dir = playerPos - currentPos; // Direction for the force
            dir.Normalize();

            Debug.Log("Horizontal direction: " + dir);
            Debug.Log("Force: " + dir * forceDamageX);

            rbPlayer.AddForce(new Vector2(dir.x * forceDamageX, forceDamageY), ForceMode2D.Impulse);

            return;
        }

        // At this point, the enemy hasn't hit any stars, carrots, or the player.
        // So, in this new case, it must be another enemy or a scenery object.
        facingRight = !facingRight; // Rotate enemy.
    }
}
