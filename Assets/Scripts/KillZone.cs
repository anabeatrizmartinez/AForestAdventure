using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KillZone : MonoBehaviour {
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) { // When a collider enters another.
        if (collision.tag == "Player") {
            PlayerController controller = collision.GetComponent<PlayerController>();
            controller.Die();
        }
    }

    // private void OnTriggerStay2D(Collider2D collision) { // While a collider stays inside another.
        
    // }

    // private void OnTriggerExit2D(Collider2D collision) { // When a collider comes out of another.

    // }
}
