using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour {

    public GameObject player;

    [Header("Timer Settings (NOT IN USE):")]
    [Tooltip("Timer text prefab.")]
    public Text timer;
    [Tooltip("Current minutes.")]
    public int minutes;
    [Tooltip("Current seconds.")]
    public int seconds;
    [Tooltip("Start Time, set a value (before the game starts) if you want the game time to be a countdown.")]
    public float startTime;
    [SerializeField]
    [Tooltip("Current time.")]
    private float currentTime;
    [SerializeField] private bool pauseTime = false;
    [SerializeField]
    [Tooltip("Counts down from current time.")]
    private bool countDown = false;
    [Tooltip("Used for calculation. Should not appear in editor.")]
    int mval;

    [Header("HealthBar Settings:")]
    [Tooltip("Healthbar Foreground.")]
    public RectTransform healthBar;
    [Tooltip("Healthbar Background.")]
    public RectTransform healthBarBg;
    [Tooltip("Healthbar Text Object.")]
    public Text healthText;
    [SerializeField]
    [Tooltip("Max Health/Starting health.")]
    static float maxHealth = 100;
    [SerializeField]
    [Tooltip("Player's current health.")]
    static float currentHealth;
    public float healthSpeed = 1;

    [Header("StaminaBar Settings:")]
    [Tooltip("Staminabar Foreground.")]
    public RectTransform staminaBar;
    [Tooltip("Staminabar Background.")]
    public RectTransform staminaBarBg;
    [SerializeField]
    [Tooltip("Max Stamina/Starting stamina.")]
    static float maxStamina = 100;
    [SerializeField]
    [Tooltip("Player's current stamina.")]
    static float currentStamina;

    [Header("Miscellaneous (DO NOT TOUCH):")]
    public bool disableLogging = false;

    // Use this for initialization
    void Start () {
        if (!player) {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        if(!timer) {
            timer = GameObject.Find("timer").GetComponent<Text>();
            timer.gameObject.SetActive(true);
        }
        if(!healthText) {
            healthText = GameObject.Find("health").GetComponent<Text>();
        }
        currentTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        StartCoroutine(FixHealthBar());
        fixStaminaBar();
        SetTimer();
        if (Time.timeScale == 0) {
            pauseTime = true;
        } else {
            pauseTime = false;
        }
        currentTime = (minutes * 60) + seconds;
    }

    public void TakeDamage() {
        //startFlash = true;
        StartCoroutine(FixHealthBar());
        Log<string>("You've been hurt.");
    }

    public void SetTimer() {
        if (!pauseTime) {
            timer.color = Color.white;
            float time;
            if (!countDown)
                time = Time.time - startTime;
            else
                time = startTime - Time.time;

            minutes = ((int)time / 60);
            string min = minutes.ToString();
            seconds = ((int)time % 60);
            string sec = seconds.ToString("f0");
            if (sec == "60")
            {
                sec = "59";
                int.TryParse(min, out mval);
                mval += 1;
            }

            if (sec.Length == 1)
            {
                sec = "0" + sec;
            }

            string value = min + ":" + sec;
            timer.text = value;

        } else {
            timer.color = Color.yellow;
        }
    }

    public void OldFixHealthBar() {
        float health = currentHealth;
        float newHealth = CalculateHealth();
        float value = Mathf.Lerp(health, newHealth, Time.deltaTime * healthSpeed);
        healthBar.sizeDelta = new Vector3(value, healthBarBg.sizeDelta.y);
    }

    public IEnumerator FixHealthBar() {
        float health = currentHealth;
        float newHealth = CalculateHealth();
        float t = 0;
        while(t < 1) {
            t += Time.deltaTime * healthSpeed;
            //float value = Mathf.Lerp(health, newHealth, t);
            healthBar.sizeDelta = new Vector3(Mathf.Lerp(health, newHealth, t), healthBarBg.sizeDelta.y);
            yield return null;
        }
        yield return null;
    }

    public float CalculateHealth() {
        float health = getHealth();
        float percentHealth = (health * 100) / maxHealth;
        float width = (percentHealth * healthBarBg.sizeDelta.x) / 100;
        healthText.text = percentHealth.ToString();
        return width;
    }

    public void Log<T>(T value) {
        if (!disableLogging) {
            Debug.Log("Hud Log: " + value);
        }
    }

    public float getHealth() {
        return player.GetComponent<Health>().currentHealth;
    }

    public float getStamina() {
        return player.GetComponent<Character>().currentStamina;
    }

    public float CalculateStamina() {
        float stamina = getStamina();
        float percentStamina = (stamina * 100) / maxStamina;
        float width = (percentStamina * staminaBarBg.sizeDelta.x) / 100;
        return width;
    }

    public void fixStaminaBar() {
        float newStamina = CalculateStamina();
        staminaBar.sizeDelta = new Vector2(newStamina, staminaBarBg.sizeDelta.y);
    }
}
