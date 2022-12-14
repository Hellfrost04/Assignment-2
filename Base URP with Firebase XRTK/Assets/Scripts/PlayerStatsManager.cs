using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour
{

    public TextMeshProUGUI playerXp;
    public TextMeshProUGUI playerLvl;
    public TextMeshProUGUI playerTimeTaken;
    public TextMeshProUGUI playerRecentTimeTaken;
    public TextMeshProUGUI playerTries;
    public TextMeshProUGUI playerThingsShot;
    public TextMeshProUGUI playerTimePlayed;
    public TextMeshProUGUI playerLastPlayed;
    public TextMeshProUGUI playerName;

    public FirebaseManger fbMgr;
    public AuthManager auth;
    // Start is called before the first frame update
    void Start()
    {
        //empty UI in the start
        ResetStatsUI();
        //retrieve 
        UpdatePlayerStats(auth.GetCurrentUser().UserId);
    }

    public async void UpdatePlayerStats(string uuid)
    {
        
        DuckPlayerStats playerStats = await fbMgr.GetPlayerStats(uuid);
        if(playerStats != null)
        {
            Debug.Log("playerstats.... " + playerStats.DuckPlayerStatsToJson());

            float minutes = Mathf.FloorToInt(playerStats.shortestTimeTaken / 60);
            float seconds = Mathf.FloorToInt(playerStats.shortestTimeTaken % 60);
            string timer = string.Format("{0:00} : {1:00}", minutes, seconds);

            float minutes2= Mathf.FloorToInt(playerStats.recentTimeTaken / 60);
            float seconds2 = Mathf.FloorToInt(playerStats.recentTimeTaken % 60);
            string timer2 = string.Format("{0:00} : {1:00}", minutes, seconds);

            playerXp.text = playerStats.xp + "xp";
            playerLvl.text = "lvl" + playerStats.lvl;
            playerTimeTaken.text = timer;
            playerRecentTimeTaken.text = timer2;
            playerTries.text = playerStats.numberOfTries + " Tries";
            playerThingsShot.text = playerStats.numberOfThingsShot + " Things Shot";
            playerTimePlayed.text = playerStats.totalTimeSpent + " secs";
            playerLastPlayed.text = UnixToDateTime(playerStats.updatedOn);
        }
        else
        {
            //reset our UI to 0 and NA
        }
        

        playerName.text = "Player : " + auth.GetCurrentUserDisplayName();
    }

    public void ResetStatsUI()
    {
        playerXp.text = "0 XP";
        playerLvl.text = "Lvl 0";
        playerTimeTaken.text = "0";
        playerRecentTimeTaken.text = "0";
        playerTries.text = "0 Tries";
        playerThingsShot.text = "0 Things Shot";
        playerTimePlayed.text = "0";
        playerLastPlayed.text = "0";
    }

    public void DeletePlayerStats()
    {
        fbMgr.DeletePlayerStats(auth.GetCurrentUser().UserId);

        //refresh my player stats on screen
        UpdatePlayerStats(auth.GetCurrentUser().UserId);
    }
    /// <summary>
    /// conver to a readable date time value
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public string UnixToDateTime(long timestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp);
        DateTime datetime = dateTimeOffset.LocalDateTime;//convert to current time format +8 singaporetime

        return datetime.ToString("dd MMM yyy");
    }

}
