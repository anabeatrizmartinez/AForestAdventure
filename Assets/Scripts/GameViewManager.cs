using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameViewManager : MonoBehaviour {

    public static GameViewManager sharedInstance;
    public Canvas gameViewCanvas;

    public TMP_Text starsText, scoreText, maxScoreText;
    public TMP_SpriteAsset starsTextSpriteAsset;

    public void Awake() {
        if (sharedInstance == null) {
            sharedInstance = this;
        }

        gameViewCanvas.enabled = false;
    }

    public void ShowGameView() {
        gameViewCanvas.enabled = true;
    }

    public void HideGameView() {
        gameViewCanvas.enabled = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame) {
            int stars = 0;
            float score = 0;
            float maxScore = 0;

            starsText.spriteAsset = starsTextSpriteAsset;
            starsText.text = "<sprite=72> " + stars.ToString();
            scoreText.text = "Score: " + score.ToString("f1");
            maxScoreText.text = "Max Score: " + maxScore.ToString("f1");
        }
    }
}
