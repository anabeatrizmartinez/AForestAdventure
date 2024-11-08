using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour {

    GameObject playerGameObj;
    Rigidbody2D rbPlayer;
    PlayerController controller;
    private Collider2D currentCollider;

    GameObject gameManagerObj;
    GameManager gameManager;

    private void Awake() {
        playerGameObj = GameObject.Find("Player");
        rbPlayer = playerGameObj.GetComponent<Rigidbody2D>();
        controller = playerGameObj.GetComponent<PlayerController>();

        currentCollider = GetComponent<Collider2D>();

        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision) { // When a collider enters another.
        if (collision.GetComponent<PlayerController>()) {
            if (!gameManager.fromGameOver) { // To avoid an error in this specific situation, where the Kill-zone is called a second time after pressing the Retry button in the Game Over Menu.
                // Bounce Player
                rbPlayer.velocity = Vector2.zero;
                rbPlayer.AddForce(Vector2.up * 1.5f, ForceMode2D.Impulse);

                // Stop Player
                Invoke("StopPlayer", 0.1f);

                // Hide kill zone collider
                currentCollider.enabled = false;

                // Player die
                controller.Die();
            }
        }
    }

    private void StopPlayer() {
        rbPlayer.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    // private void OnTriggerStay2D(Collider2D collision) { // While a collider stays inside another.
        
    // }

    // private void OnTriggerExit2D(Collider2D collision) { // When a collider comes out of another.

    // }
}
