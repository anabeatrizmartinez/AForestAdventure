using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovingWithPlayer : MonoBehaviour {
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // Make player platform's child
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            // Remove child relation
            collision.transform.SetParent(null);
        }
    }
}
