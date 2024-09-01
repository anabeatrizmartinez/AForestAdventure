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
    private CircleCollider2D itemCollider;

    bool hasBeenCollected = false;

    public int value = 1;

    private void Awake() {
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<CircleCollider2D>();
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
            // TODO: star logic
            break;

            case CollectableType.carrot:
            // TODO: carrot logic
            break;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player") {
            Collect();
        }
    }
}
