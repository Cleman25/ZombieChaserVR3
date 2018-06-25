using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOver : MonoBehaviour {
    public Button mainmenu;
    public Button quitGame;
    public Text gameStatus;
    public Text healthText;
    public Text speedText;
    public Text staminaText;
    public Text coinsText;
    public Text walletText;
    public Text timeText;
    // Use this for initialization
    void Start () {
        mainmenu.onClick.AddListener(() =>
        {
            GameManager.instance.MainMenu();
        });
        quitGame.onClick.AddListener(() =>
        {
            GameManager.instance.QuitGame();
        });
        GameManager.instance.DeActivatePanel(GameManager.instance.mainPanel);
	}
	
	// Update is called once per frame
	void Update () {
        gameStatus.text = GameManager.instance.gameStatus;
        timeText.text = "TimeLeft: " + GameManager.instance.timeLeft.ToString();
        staminaText.text = "Stamina: " + GameManager.instance.staminaPowerUps.ToString();
        speedText.text = "Speed: " + GameManager.instance.speedPowerUps.ToString();
        healthText.text = "Health: " + GameManager.instance.healthPowerUps.ToString();
        coinsText.text = "Coins: " + GameManager.instance.coins.ToString();
        walletText.text = "Wallet: " + GameManager.instance.wallet.ToString();
    }
}
