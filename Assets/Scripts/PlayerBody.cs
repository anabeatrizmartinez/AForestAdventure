using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerBody : MonoBehaviour {

    [SerializeField] private Rigidbody2D rbPlayer;
    [SerializeField] private GameObject player;
    [SerializeField] private SpriteRenderer spriteRendererPlayer;
    public int enemyDamage = 2;
    public float forceDamageX  = -1.5f;
    public float forceDamageY  = 1.5f;

    void Update() {
        if (spriteRendererPlayer.color != Color.white) {
            StartCoroutine(ResetPlayerColorAfterDelay(0.1f)); // Reset palyer's original color after delay
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Enemy>()) {
            // Reduce Player's life
            player.GetComponent<PlayerController>().CollectHealth(-enemyDamage);

            // To make the player jump horizontally when collides with the enemy.
            rbPlayer.velocity = Vector2.zero;
            Vector3 playerPos = collision.transform.position; // Player's position
            Vector3 currentPos = transform.position; // Enemy's position
            Vector3 dir = playerPos - currentPos; // Direction for the force
            dir.Normalize();
            rbPlayer.AddForce(new Vector2(dir.x * forceDamageX, forceDamageY), ForceMode2D.Impulse);

            // To make the player blink in red
            StartCoroutine(BlinkPlayer());
        }
    }

    private IEnumerator BlinkPlayer() {
        Color originalColor = spriteRendererPlayer.color; // Player's original color
        Color blinkColor = Color.red; // Blinking color

        for (int i = 0; i < 3; i++) {
            spriteRendererPlayer.color = blinkColor; // Change player color
            yield return new WaitForSeconds(0.1f); // Blinking time (pause for 0.1 seconds)
            spriteRendererPlayer.color = originalColor; // Change to the original color
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ResetPlayerColorAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        spriteRendererPlayer.color = Color.white; // Reset palyer's original color
    }
}
