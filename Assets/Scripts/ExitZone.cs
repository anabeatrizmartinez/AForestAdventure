using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitZone : MonoBehaviour {
    GameObject gameManagerObj;
    GameManager gameManager;


    void Awake() {
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerController>()) {
            if (!gameManager.fromGameOver) { // To avoid an error in this specific situation, where the Exit-zone is called a second time after pressing the Retry button in the Game Over Menu.
                for (int i = 0; i < 5; i++) {
                    LevelManager.sharedInstance.AddLevelBlock(true);
                }
                LevelManager.sharedInstance.RemoveLevelBlock();
            }
        }
    }
}
