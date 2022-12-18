using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public AuthManager auth;
    public FirebaseManger firebaseMgr;
    public bool isPlayerStatsUpdated;
    public int recentTimeTaken;
    public int shortestTimeTaken;
    public int numberOfThingsShot;
    public int numberOfTries;
    public int xp;
    public int lvl;
    public static int shots;
    public GameObject GameUIMenu;
    public GameObject DeathMenu;
    public GameObject EndGameMenu;


    void Start()
    {   
        Bullet.shots = 0;
        numberOfTries = 0;
        shortestTimeTaken = 0;
        recentTimeTaken = 0;
        isPlayerStatsUpdated = false;
        Time.timeScale = 1;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Destructible"))
        {
            Destroy(collision.gameObject);
            DeathMenu.SetActive(true);
            GameUIMenu.SetActive(false);
            deathEffect();
        }
           if (collision.gameObject.CompareTag("End"))
        {
            Destroy(collision.gameObject);
            EndGameMenu.SetActive(true);
            GameUIMenu.SetActive(false);
            deathEffect();
        }
    }
    public void deathEffect()
    {
        Time.timeScale = 0;
        
        int recentTimeTaken = Mathf.RoundToInt(GameUI.time);
        int shortestTimeTaken = Mathf.RoundToInt(GameUI.time);
        int timePlayed = Mathf.RoundToInt(GameUI.time);
        

        if (!isPlayerStatsUpdated)
        {
            UpdatePlayerStat(xp, lvl, shortestTimeTaken, recentTimeTaken, numberOfTries, numberOfThingsShot, timePlayed);

        }
        isPlayerStatsUpdated = true;
    }
    public void UpdatePlayerStat(int xp, int lvl, int shortestTimeTaken, int recentTimeTaken, int numberOfThingsShot, int numberOfTries, int time)
    {
        firebaseMgr.UpdatePlayerStats(auth.GetCurrentUser().UserId, xp, lvl, shortestTimeTaken, recentTimeTaken, numberOfThingsShot, numberOfTries, time, auth.GetCurrentUserDisplayName());

    }
}
