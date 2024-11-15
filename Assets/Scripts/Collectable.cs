using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum CollectableType {
    carrot,
    star
}

public class Collectable : MonoBehaviour {

    public CollectableType type = CollectableType.star;

    private SpriteRenderer sprite;
    private Collider2D itemCollider;

    bool hasBeenCollected = false;

    public int value = 1;

    GameObject player;
    AudioSource audioSource;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        player = GameObject.Find("Player");
    }

    void Show() {
        sprite.enabled = true;
        itemCollider.enabled = true;
        hasBeenCollected = false;
    }

    void Hide() {
        sprite.enabled = false;
        itemCollider.enabled = false;
    }

    void Collect() {
        // Check audio source and start it.
        if (audioSource != null) {
            audioSource.Play();
        }

        Hide();
        hasBeenCollected = true;

        switch (this.type) {
            case CollectableType.star:
                GameManager.sharedInstance.CollectObject(this);
                break;

            case CollectableType.carrot:
                player.GetComponent<PlayerController>().CollectHealth(this.value);
                break;
        }

        // TODO: Collect mana when destroying enemies.
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Player") {
            // Destroy(gameObject); // Just for testing

            // To avoid an error in this specific situation, where after restarting the game, and before the player is set to the start position, he falls into a level with a collectable, and that activates this function, so by the time the player is set to the start position, he is set but not with the initial values of collectables.
            if (GameManager.sharedInstance.gameMenuState != "gameOver" && GameManager.sharedInstance.gameMenuState != "pause") {
                Collect();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) { // For objects with "Is trigger" disabled.
        if (collision.gameObject.CompareTag("Player")) {
            // Destroy(gameObject); // Just for testing
            Collect();
        }
    }
}
