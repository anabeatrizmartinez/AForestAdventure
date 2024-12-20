using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerEnemyHeadCheck : MonoBehaviour {

    [SerializeField] private Rigidbody2D rbPlayer;
    public float bounceForce = 2f;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<EnemyPlayerHeadCheck>()) {
            // Bounce Player
            rbPlayer.velocity = Vector2.zero;
            rbPlayer.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }
}
