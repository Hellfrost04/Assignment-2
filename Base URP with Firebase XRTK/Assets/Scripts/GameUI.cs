using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    
    //references
    public TextMeshProUGUI theTimer;

    //variables
    public static float time;
   
    // Use this for initialization
    void Start()
    {
        time = 0;
        theTimer.text = "Timer: --";
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        updateTimer(time);
    }
    //timer
    void updateTimer(float currentTime)
    {
        currentTime += 1;

        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);
        theTimer.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    //reset game
    public void ResetTheGame()
    {   
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    
    //mainmenu
    public void MainMenu()
    {   
        SceneManager.LoadScene("MainMenu");
        
    }
   
}
