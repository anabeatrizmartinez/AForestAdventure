using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExitZone : MonoBehaviour {
    
    GameObject gameManagerObj;
    GameManager gameManager;
    Collider2D currentCollider;
    GameObject playerGameObj;
    Rigidbody2D rbPlayer;


    void Awake() {
        gameManagerObj = GameObject.Find("GameManager");
        gameManager = gameManagerObj.GetComponent<GameManager>();

        currentCollider = GetComponent<Collider2D>();

        playerGameObj = GameObject.Find("Player");
        rbPlayer = playerGameObj.GetComponent<Rigidbody2D>();
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
            if (gameManager.gameMenuState != "gameOver" && gameManager.gameMenuState != "pause") { // To avoid an error in this specific situation, where the Exit-zone is called a second time after pressing the Retry button in the Game Over Menu, or after pressing the Start Game button in the Pause Menu.
                // Hide exit zone collider
                // currentCollider.enabled = false;
                
                // Add new level blocks and remove the last one.
                for (int i = 0; i < 5; i++) {
                    LevelManager.sharedInstance.AddLevelBlock(true);
                }
                LevelManager.sharedInstance.RemoveLevelBlock();
            }
        }
    }
}
