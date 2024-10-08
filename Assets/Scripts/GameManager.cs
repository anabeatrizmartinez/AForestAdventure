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

    public int collectedObject = 0;

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
            // menu logic (show menu, buttons, pause game, etc).
            GameViewManager.sharedInstance.HideGameView();
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.ShowMainMenu();
        } else if (newGameState == GameState.inGame) {
            // prepare scene to play.
            LevelManager.sharedInstance.RemoveAllLevelsBlock();
            Invoke("ReloadLevel", 0.1f);
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameOverMenu();
            collectedObject = 0; // Initialize stars
            GameViewManager.sharedInstance.ShowGameView();
        } else if (newGameState == GameState.gameOver) {
            // prepare game for Game Over.
            MenuManager.sharedInstance.HideMainMenu();
            GameViewManager.sharedInstance.HideGameView();
            MenuManager.sharedInstance.ShowGameOverMenu();
        }

        this.currentGameState = newGameState;
    }

    void ReloadLevel() {
        LevelManager.sharedInstance.GenerateInitialLevelsBlock();
        controller.StartGame();
    }

    //  For collectable objects like stars and carrots
    public void CollectObject(Collectable collectable) {
        collectedObject += collectable.value;
    }
}
