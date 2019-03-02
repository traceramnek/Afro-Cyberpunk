using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class LevelManager : MonoBehaviour {
    public float healthTicTimer;
    private float healthTicCountdown;

    public float homeTicTimer;
    private float homeTicTimerCountdown;
    private PlayerPlatformer thePlayer;
    // Use this for initialization

    void Start () {
        healthTicCountdown = healthTicTimer;
        homeTicTimerCountdown = homeTicTimer;
        thePlayer = FindObjectOfType<PlayerPlatformer>();
	}
	
	// Update is called once per frame
	void Update () {

        /*if(!thePlayer.isHome())
        {
            if (homeTicTimerCountdown <= homeTicTimer) { homeTicTimerCountdown = homeTicTimer; }

            healthTicCountdown -= Time.deltaTime;
            if (healthTicCountdown <= 0f) {
                thePlayer.takeHealthDamage(1);
                healthTicCountdown = healthTicTimer;
            }
        }
        if( (thePlayer.isHome()))
        {
            if (healthTicCountdown <= healthTicTimer) { healthTicCountdown = healthTicTimer; }
            homeTicTimerCountdown -= Time.deltaTime;
            if (homeTicTimerCountdown <= 0f)
            {
                thePlayer.healHealth(5);
                thePlayer.takeWillDamage(5);
                homeTicTimerCountdown = homeTicTimer;
            }
        }*/
        
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void GameOverWill()
    {
        SceneManager.LoadScene("GameOverWill");
    }
    public void youWin()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void NextLevel(string nextLevel)
    {
        SceneManager.LoadScene(nextLevel);
    }


}
