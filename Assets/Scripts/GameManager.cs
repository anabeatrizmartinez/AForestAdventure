using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 3 possible enum to control 3 game states, whether the character is playing, in the menu or paused.
// They are declared outside the class in order to access them in any other script.
public enum GameState {
    menu,
    inGame,
    pauseGame,
    gameOver
}


public class GameManager : MonoBehaviour {
    // Init state
    public GameState currentGameState = GameState.menu;

    public static GameManager sharedInstance; // This name helps to locate and understand that it's a Singleton.

    PlayerController controller;

    public int collectedObject = 0;

    private int conForStartGame = 0;

    public string gameMenuState = "other";

    [SerializeField] private AudioSource backgroundMusic;
    float initVolume;


    void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this; // Init here to ensure that the first one is the one it's going to rule.
        }

        initVolume = backgroundMusic.volume;
    }

    // Start is called before the first frame update
    void Start() {
        SetGameState(GameState.menu);
        controller = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update() {
        // if (Input.GetButtonDown("Submit") && currentGameState != GameState.inGame) {
        //     StartGame();
        // }

        if (Input.GetButtonDown("Pause") && currentGameState == GameState.inGame) {
            PauseMenu();
        }
    }

    // 3 essential methods to control the game - are public to control them from Unity as well. - StartGame(), GameOver() and PauseMenu().
    public void StartGame(string state) {
        gameMenuState = state; // To avoid an error in this specific situation, where the Exit-zone is called a second time after pressing the Retry button in the Game Over Menu, or after pressing the Start Game button in the Pause Menu.
        conForStartGame++;
        SetGameState(GameState.inGame);
    }

    public void GameOver() {
        SetGameState(GameState.gameOver);
    }

    public void PauseMenu() {
        SetGameState(GameState.pauseGame);
    }


    // To modify the game state, like traffic lights - It's better to have a single method to control possible changes to state variables.
    private void SetGameState(GameState newGameState) {
        if (newGameState == GameState.menu) {
            // menu logic (show menu, buttons, etc).
            GameViewManager.sharedInstance.HideGameView();
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.HidePauseMenu();
            MenuManager.sharedInstance.ShowMainMenu();
        } else if (newGameState == GameState.inGame) {
            // prepare scene to play.
            backgroundMusic.volume = initVolume; // Reset volume of the background music.

            if (conForStartGame > 1) { // If it's not the first time the game is loaded.
                // Reset the background music's state
                backgroundMusic.Stop();
                backgroundMusic.Play();

                // Remove all level blocks
                LevelManager.sharedInstance.RemoveAllLevelsBlock();
            }            
            
            Invoke("ReloadLevel", 0.1f);

            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.HidePauseMenu();
            collectedObject = 0; // Initialize stars
            GameViewManager.sharedInstance.ShowGameView();
        } else if (newGameState == GameState.pauseGame) {
            // pause menu logic
            backgroundMusic.volume = 0.25f; // Low the background music.

            GameViewManager.sharedInstance.HideGameView();
            MenuManager.sharedInstance.HideGameOverMenu();
            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.ShowPauseMenu();
        } else if (newGameState == GameState.gameOver) {
            // prepare game for Game Over.
            backgroundMusic.Stop(); // Stop the background music.

            MenuManager.sharedInstance.HideMainMenu();
            MenuManager.sharedInstance.HidePauseMenu();
            GameViewManager.sharedInstance.HideGameView();
            MenuManager.sharedInstance.ShowGameOverMenu();
        }

        this.currentGameState = newGameState;
    }

    void ReloadLevel() {
        LevelManager.sharedInstance.GenerateInitialLevelsBlock();
        controller.StartGame();
        Invoke("ResetStringGameMenuState", 0.2f);
    }

    void ResetStringGameMenuState() {
        gameMenuState = "other"; // For the Exit-zone to work properly again.
    }

    public void ContinueGame() { // A separate method of SetGameState() to be able to continue the game without restarting it, while in pause menu.
        backgroundMusic.volume = initVolume; // Reset volume of the background music.

        MenuManager.sharedInstance.HidePauseMenu();
        GameViewManager.sharedInstance.ShowGameView();
        this.currentGameState = GameState.inGame;
    }

    //  For collectable objects like stars and carrots
    public void CollectObject(Collectable collectable) {
        collectedObject += collectable.value;
    }
}
