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

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<Collider2D>();
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
            Collect();
        }
    }
}
