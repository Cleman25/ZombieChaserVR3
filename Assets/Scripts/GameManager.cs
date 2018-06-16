using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Utilities;

public class GameManager : MonoBehaviour {
    static GameManager _instance = null;
    public GameObject playerPrefab;
    public Character player;
    public SceneField gameScene;
    public SceneField startScene;
    public bool gameWon;
    public bool gameLost;
    public bool timeIsUp;
    public bool gameOver;
    public GameObject mainPanel;
    public GameObject gameOverPanel;
    public Button startButton;
    public Text gameStatus;
    public Text healthText;
    public Text speedText;
    public Text staminaText;
    public Text coinsText;
    public Text walletText;
    public Text timeText;
    public int powerUpsPickedUp = 0;
    public int healthPowerUps = 0;
    public int speedPowerUps = 0;
    public int staminaPowerUps = 0;
    public float coins;
    public float wallet;
    public float surviveFor;
    //public SceneField gameOverScene;
    // Use this for initialization
    void Awake () {
        if(instance) {
            DestroyImmediate(gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad(this);
        }
        coins = 0;
        wallet = 0;
        if(playerPrefab) {
            player = playerPrefab.GetComponent<Character>();
        }
        if(surviveFor <= 0) {
            surviveFor = 300;
        }
        if(mainPanel) {
            TogglePanel(mainPanel);
        }
        if(startButton) {
            startButton.onClick.AddListener(() => {
                StartGame();
            });
        }
        SwitchToVR();
	}
	
	// Update is called once per frame
	void Update () {
		if(!player.isAlive || timeIsUp) {
            gameOver = true;
        }
        if (gameOver) {
            EndGame();
        }

        if(Input.GetKeyDown(KeyCode.N)) {
            gameOver = true;
        }
        if(Input.GetKeyDown(KeyCode.T)) {
            LoseGame();
        }
        if(Input.GetKeyDown(KeyCode.M)) {
            WinGame();
        }

        if(gameLost || gameWon) {
            TogglePanel(gameOverPanel);
        }
        staminaText.text = staminaPowerUps.ToString();
        speedText.text = speedPowerUps.ToString();
        healthText.text = healthPowerUps.ToString();
        coinsText.text = coins.ToString();
        walletText.text = wallet.ToString();
    }

    public static GameManager instance {
        get { return _instance; }
        set { _instance = value; }
    }

    public void LoseGame() {
        gameLost = true;
        gameWon = false;
        gameStatus.text = "You Lost";
    }

    public void WinGame() {
        gameLost = false;
        gameWon = true;
        gameStatus.text = "You Won";
    }

    public void ActivatePanel(GameObject panel) {
        if(!panel.activeSelf) {
            panel.SetActive(true);
        }
    }

    public void DeActivatePanel(GameObject panel) {
        if(panel.activeSelf) {
            panel.SetActive(false);
        }
    }

    public void TogglePanel(GameObject panel) {
        GameObject[] panels =  new GameObject[2];
        panels[0] = mainPanel;
        panels[1] = gameOverPanel;
        foreach(GameObject p in panels) {
            if(p != panel) {
                DeActivatePanel(p);
            } else {
                ActivatePanel(panel);
            }
        }
    }

    public IEnumerator SwitchToVR() {
        string desiredDevice = "cardboard";
        if (string.Compare(XRSettings.loadedDeviceName, desiredDevice, true) != 0) {
            XRSettings.LoadDeviceByName(desiredDevice);
            yield return null;
        }
        XRSettings.enabled = true;
    }

    public IEnumerator SwitchTo2D() {
        XRSettings.LoadDeviceByName("");
            
        yield return null;
        ResetCameras();
    }

    public void ResetCameras() {
        for (int i = 0; i < Camera.allCameras.Length; i++) {
            Camera cam = Camera.allCameras[i];
            if (cam.enabled && cam.stereoTargetEye != StereoTargetEyeMask.None) {
                cam.transform.localPosition = Vector3.zero;
                cam.transform.localRotation = Quaternion.identity;
            }
        }
    }

    public void EndGame() {
        int totalCoins = (int)coins;
        int totalWallet = (int)wallet;
    }

    public void StartGame() {
        DeActivatePanel(mainPanel);
        SwitchScenes(gameScene);
        //SwitchToVR();
    }

    public void SwitchScenes(SceneField scene) {
        SceneManager.LoadScene(scene);
    }

    public void MainMenu() {
        TogglePanel(mainPanel);
        SwitchScenes(startScene);
        //SwitchTo2D();
    }

    public void QuitGame() {
        Application.Quit();
    }
}
