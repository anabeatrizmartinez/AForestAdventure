using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour {

    PlayerController controller;

    private void Awake() {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision) { // When a collider enters another.
        if (collision.tag == "Player") {
            controller.Die();
        }
    }

    // private void OnTriggerStay2D(Collider2D collision) { // While a collider stays inside another.
        
    // }

    // private void OnTriggerExit2D(Collider2D collision) { // When a collider comes out of another.

    // }
}
