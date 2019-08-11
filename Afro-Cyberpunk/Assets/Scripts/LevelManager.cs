using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class LevelManager : MonoBehaviour
{
    /*public float healthTicTimer;
    private float healthTicCountdown;

    public float homeTicTimer;
    private float homeTicTimerCountdown;*/
    public GameObject thePlayer;
    public GameObject theLevelSelectCanvas;
    public GameObject pressUpCanvas;
    public float yOffset;
    // Use this for initialization
    public delegate void BuildingToggle();
    public static event BuildingToggle buildingToggler;

    void Start()
    {
        /*healthTicCountdown = healthTicTimer;
        homeTicTimerCountdown = homeTicTimer;*/
        theLevelSelectCanvas.SetActive(false);
        pressUpCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void FixedUpdate()
    {
        if(Constants.PlayerInput.pressedBuildingToggle) {
            buildingToggler(); // emit event to be received
        }
    }

    public void EnableLevelSelect()
    {
        theLevelSelectCanvas.SetActive(true);
    }

    public void EnablePressUpCanvas()
    {
        pressUpCanvas.SetActive(true);

    }

    public void MaintainPressUpCanvas()
    {
        pressUpCanvas.transform.position = new Vector3(thePlayer.transform.position.x, thePlayer.transform.position.y + yOffset);
    }

    public void DisableLevelSelect()
    {
        theLevelSelectCanvas.SetActive(false);
    }

    public void DisablePressUpCanvas()
    {
        pressUpCanvas.SetActive(false);

    }

    public void DisablePlayerControls()
    {
        thePlayer.GetComponent<PlayerPlatformer>().enabled = false;
    }

    public void EnablePlayerControls()
    {
        thePlayer.GetComponent<PlayerPlatformer>().enabled = true;
    }
    /*public void GameOverWill()
    {
        SceneManager.LoadScene("GameOverWill");
    }
    public void youWin()
    {
        SceneManager.LoadScene("WinScene");
    }*/

    public void NextLevel(string nextLevel)
    {
        SceneManager.LoadScene(nextLevel);
    }


}
