using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerBody>()) {
            LevelManager.sharedInstance.AddLevelBlock(true);
            LevelManager.sharedInstance.RemoveLevelBlock();
        }
    }
}
