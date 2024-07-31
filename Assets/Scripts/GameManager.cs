using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3 possible enum to control 3 game states, whether the character is playing, in the menu or paused.
// They are declared outside the class in order to access them in any other script.
public enum GameState {
    menu,
    inGame,
    gameOver
}


public class GameManager : MonoBehaviour {
    // Init state
    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance; // This name helps to locate and understand that it's a Singleton.

    PlayerController controller;


    void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this; // Init here to ensure that the first one is the one it's going to rule.
        }
    }

    // Start is called before the first frame update
    void Start() {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame) {
            StartGame();
        }

        if (Input.GetButtonDown("Pause")) {
            BackToMenu();
        }
    }

    // 3 essential methods to control the game - are public to control them from Unity as well. - StartGame(), GameOver() and BackToMenu().
    public void StartGame() {
        SetGameState(GameState.inGame);
    }

    public void GameOver() {
        SetGameState(GameState.gameOver);
    }

    public void BackToMenu() {
        SetGameState(GameState.menu);
    }


    // To modify the game state, like traffic lights - It's better to have a single method to control possible changes to state variables.
    private void SetGameState(GameState newGameState) {
        if (newGameState == GameState.menu) {
            // TODO: menu logic (show menu, buttons, pause game, etc).
        } else if (newGameState == GameState.inGame) {
            // prepare scene to play.
            LevelManager.sharedInstance.RemoveAllLevelsBlock();
            Invoke("ReloadLevel", 0.1f);
        } else if (newGameState == GameState.gameOver) {
            // TODO: prepare game for Game Over.
        }

        this.currentGameState = newGameState;
    }

    void ReloadLevel() {
        LevelManager.sharedInstance.GenerateInitialLevelsBlock();
        controller.StartGame();
    }
}
