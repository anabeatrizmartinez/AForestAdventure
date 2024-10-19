using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chest : MonoBehaviour {

    public List<GameObject> allTheObjects = new List<GameObject>();  // All the objects avalaibles.
    public float upForce = 3f;
    private Animator animator;
    private bool opened = false;
    
    // Start is called before the first frame update
    void Start() {
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddObject() {
        int randomIdx = Random.Range(0, allTheObjects.Count);
        int randomDir = Random.Range(-1, 1);
        if (randomDir == 0) {
            randomDir = 1;
        }

        GameObject objectSelected = Instantiate(allTheObjects[randomIdx]);
        Rigidbody2D rbObjectSelected = objectSelected.GetComponent<Rigidbody2D>();
        Collider2D colliderObjectSelected = objectSelected.GetComponent<Collider2D>();
        
        objectSelected.transform.SetParent(this.transform, false); // Add object to the scene
        Vector3 spawnPosition = this.transform.position;
        
        Vector3 correction = new Vector3(
            spawnPosition.x,
            spawnPosition.y + 0.05f, // to avoid interference with AddForce() when the location is the same as the parent.
            0
        );
        objectSelected.transform.position = correction;
        
        // To allow the object to be fully displayed before it executes the functions that come with its collider, such as disappearing after being counted for player's points.
        colliderObjectSelected.enabled = false;
        rbObjectSelected.AddForce(new Vector2(randomDir * 0.1f, upForce), ForceMode2D.Impulse);
        StartCoroutine(ResetObjectCollider(colliderObjectSelected));
    }

    public void GenerateObjects() {
        if (!opened) {
            for (int i = 0; i < 10; i++) {
                AddObject();
            }
        }
    }

    private IEnumerator ResetObjectCollider(Collider2D collider) {
        yield return new WaitForSeconds(0.1f);
        collider.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            animator.enabled = true;
            GenerateObjects();
            opened = true; // to open the chest only once.
        }
    }
}
