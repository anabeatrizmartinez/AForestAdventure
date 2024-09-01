using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

    public static MenuManager sharedInstance;
    public GameObject mainMenu, gameOverMenu;

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

    public void ExitGame() {
        // Depends of the platform
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
