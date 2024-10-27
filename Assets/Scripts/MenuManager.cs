using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    public static MenuManager sharedInstance;
    public GameObject mainMenu, gameOverMenu, pauseMenu;
    public Button playButtonMainMenu, retryButton, playButtonPauseMenu, continueButton;

    public TMP_Text starsText, scoreText;
    public TMP_SpriteAsset starsTextSpriteAsset;

    private PlayerController controller;


    public void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        }

        gameOverMenu.SetActive(false);
    }

    public void ShowMainMenu() {
        mainMenu.SetActive(true);
    }

    public void HideMainMenu() {
        mainMenu.SetActive(false);
    }

    public void ShowGameOverMenu() {
        gameOverMenu.SetActive(true);
    }

    public void HideGameOverMenu() {
        gameOverMenu.SetActive(false);
    }

    public void ShowPauseMenu() {
        pauseMenu.SetActive(true);
    }

    public void HidePauseMenu() {
        pauseMenu.SetActive(false);
    }

    public void ExitGame() {
        // Depends of the platform
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    // Start is called before the first frame update
    void Start() {
        controller = GameObject.Find("Player").GetComponent<PlayerController>();

        playButtonMainMenu.interactable = true;
        retryButton.interactable = true;
        playButtonPauseMenu.interactable = true;
        continueButton.interactable = true;

        playButtonMainMenu.onClick.AddListener(() => ResetButtonState(playButtonMainMenu));
        retryButton.onClick.AddListener(() => ResetButtonState(retryButton));
        playButtonPauseMenu.onClick.AddListener(() => ResetButtonState(playButtonPauseMenu));
        continueButton.onClick.AddListener(() => ResetButtonState(continueButton));
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.sharedInstance.currentGameState == GameState.gameOver) {
            int stars = GameManager.sharedInstance.collectedObject;
            float score = controller.GetTravelledDistance(); // it's better to reference objects at the start and not in the update();

            starsText.spriteAsset = starsTextSpriteAsset;
            starsText.text = "<sprite=72> " + stars.ToString();
            scoreText.text = "Score: " + score.ToString("f1");
        }
    }

    void ResetButtonState(Button button) {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
