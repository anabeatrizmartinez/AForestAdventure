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

        GameObject objectSelected = Instantiate(allTheObjects[randomIdx]);
        Rigidbody2D rbObjectSelected = objectSelected.GetComponent<Rigidbody2D>();
        Vector3 spawnPosition = this.transform.position;

        objectSelected.transform.SetParent(this.transform, false); // Add object to the scene
        Vector3 correction = new Vector3(
            spawnPosition.x,
            spawnPosition.y + 0.05f, // to avoid interference with AddForce() when the location is the same as the parent.
            0
        );
        objectSelected.transform.position = correction;
        rbObjectSelected.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
    }

    public void GenerateObjects() {
        if (!opened) {
            for (int i = 0; i < 10; i++) {
                AddObject();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        animator.enabled = true;
        GenerateObjects();
        opened = true; // to open the chest only once.
    }
}
