using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPlayerHeadCheck : MonoBehaviour {

    private Collider2D currentCollider;
    private Collider2D colliderEnemy;
    private Rigidbody2D rbEnemy;
    private Animator animatorEnemy;
    private SpriteRenderer spriteRendererEnemy;
    private GameObject player;
    private int newMana = 7;

    const string STATE_ENEMY_ALIVE = "isAlive";


    private void Awake() {
        currentCollider = GetComponent<Collider2D>();

        colliderEnemy = this.transform.parent.GetComponent<Collider2D>();
        rbEnemy = this.transform.parent.GetComponent<Rigidbody2D>();
        animatorEnemy = this.transform.parent.GetComponent<Animator>();
        spriteRendererEnemy = this.transform.parent.GetComponent<SpriteRenderer>();

        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerEnemyHeadCheck>()) {
            // Destroy enemy
            rbEnemy.velocity = Vector2.zero;
            rbEnemy.constraints = RigidbodyConstraints2D.FreezePosition;
            animatorEnemy.SetBool(STATE_ENEMY_ALIVE, false);
            currentCollider.enabled = false;
            colliderEnemy.enabled = false;
            Invoke("HideAfterDelay", 0.5f); // Hide enemy

            // Increase Mana
            player.GetComponent<PlayerController>().CollectMana(+newMana);
        }
    }

    private void HideAfterDelay() {
        spriteRendererEnemy.enabled = false;
        // rbEnemy.constraints = RigidbodyConstraints2D.None;
        // rbEnemy.constraints = RigidbodyConstraints2D.FreezePositionY;
    }
}
