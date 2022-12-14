using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;


public class LeaderBoardManager : MonoBehaviour
{
    public FirebaseManger fbMgr;
    public GameObject rowPrefab;
    public Transform tableContent;

    void Start()
    {
        GetLeaderboard();
    }
    public void GetLeaderboard()
    {
        UpdateLeaderboardUI();
    }
    /// <summary>
    /// Get and update LeaderboardUI
    /// </summary>
    public async void UpdateLeaderboardUI()
    {
        var leaderBoardList = await fbMgr.GetLeaderboard(5);
        int rankCounter = 1;

        //clear all leaderboard entries in UI
        foreach(Transform item in tableContent)
        {
            Destroy(item.gameObject);
        }

        //create prefabs of our rows
        //assign each value from list to prefab text content
        foreach(DuckLeaderBoard lb in leaderBoardList)
        {
            float minutes = Mathf.FloorToInt(lb.shortestTimeTaken / 60);
            float seconds = Mathf.FloorToInt(lb.shortestTimeTaken % 60);
            string timer = string.Format("{0:00} : {1:00}", minutes, seconds);
            Debug.LogFormat("leaderboard :rank{0} playername {1} time {2} lvl {3} ",
                           rankCounter, lb.userName, lb.shortestTimeTaken,lb.lvl);
            //create prefabes in the positions of tablecontent
            GameObject entry = Instantiate(rowPrefab, tableContent);
            TextMeshProUGUI[] leaderBoardDetails = entry.GetComponentsInChildren<TextMeshProUGUI>();
            leaderBoardDetails[0].text = rankCounter.ToString();
            leaderBoardDetails[1].text = lb.userName;
            leaderBoardDetails[2].text = timer;
            leaderBoardDetails[3].text = "lvl" + lb.lvl.ToString();

            rankCounter++;
        }
    }
}
