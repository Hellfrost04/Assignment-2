using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class FirebaseManger : MonoBehaviour
{
    DatabaseReference dbPlayerStatsReference;
    DatabaseReference dbLeaderBoardRefernce;

    public void Awake()
    {
        InitialiazeFirebase();
    }
    public void InitialiazeFirebase()
    {
        dbPlayerStatsReference = FirebaseDatabase.DefaultInstance.GetReference("playerStats");
        dbLeaderBoardRefernce = FirebaseDatabase.DefaultInstance.GetReference("leaderBoard");

    }
    
    public void UpdatePlayerStats(string uuid,int xp,int lvl, int shortestTimeTaken,int recentTimeTaken,int numberOfThingsShot, int numberOfTries,int time, string displayName)
    {
        Query playerQuery = dbPlayerStatsReference.Child(uuid);

        //READ the data first and check whether there has been an entry based on my uuid    
        playerQuery.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("sorry, there was an error creating your entries :ERROR" + task.Exception);
            }else if (task.IsCompleted)
            {
                DataSnapshot playerStats = task.Result;
                //check if there is an entry created 
                if (playerStats.Exists)
                {
                    //update player stats
                    //compare existing highscore and set new highscore
                    //add xp per game
                    //add time spent -> this is dependent on your own timer

                    //create a temp object sp which stores info from player stats
                    DuckPlayerStats sp = JsonUtility.FromJson<DuckPlayerStats>(playerStats.GetRawJsonValue());
                    sp.xp += 10;
                    sp.lvl = lvl;
                    sp.recentTimeTaken = recentTimeTaken;
                    sp.numberOfTries += 1;
                    sp.numberOfThingsShot += numberOfThingsShot;
                    sp.totalTimeSpent += time;
                    sp.updatedOn = sp.GetTimeUnix();

                    //check if there's a new highscore
                    if (recentTimeTaken >sp.shortestTimeTaken)
                    {
                       
                        sp.lvl = lvl;
                        sp.shortestTimeTaken = shortestTimeTaken;
                        UpdatePlayerLeaderBoardEntry(uuid, sp.shortestTimeTaken, sp.lvl, sp.updatedOn);
                    }

                    //update with entire temp sp object
                    //path: playerStats/$uuid
                    dbPlayerStatsReference.Child(uuid).SetRawJsonValueAsync(sp.DuckPlayerStatsToJson()); 
                }
                else
                {
                    //create player stats
                    //if there's no existing data, it's our first time player
                    DuckPlayerStats sp = new DuckPlayerStats(displayName, xp, lvl, shortestTimeTaken, recentTimeTaken,numberOfTries , numberOfThingsShot, time);

                    DuckLeaderBoard lb = new DuckLeaderBoard(displayName,shortestTimeTaken, lvl );

                    //create new entries into firebase
                    dbPlayerStatsReference.Child(uuid).SetRawJsonValueAsync(sp.DuckPlayerStatsToJson());
                    dbLeaderBoardRefernce.Child(uuid).SetRawJsonValueAsync(lb.DuckLeaderBoardToJson());
                }
            }
        });
    }

    public async Task<List<DuckLeaderBoard>> GetLeaderboard(int limit = 5)
    {
        Query q = dbLeaderBoardRefernce.OrderByChild("shortestTimeTaken").LimitToLast(limit);

        List<DuckLeaderBoard> leaderBoardList = new List<DuckLeaderBoard>();

       await q.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("sorry, there was an error getting leaderboard entries: ERROR:" + task.Exception);
                
            }else if (task.IsCompleted)
            {
                DataSnapshot ds = task.Result;
                if (ds.Exists)
                {
                    int rankCounter = 1;
                    foreach(DataSnapshot d in ds.Children)
                    {   

                        //create temp object based on the result
                        DuckLeaderBoard lb = JsonUtility.FromJson<DuckLeaderBoard>(d.GetRawJsonValue());

                        //add item to list
                        leaderBoardList.Add(lb);

                        //Debug.LogFormat("leaderboard :rank{0} playername {1} highscore {2}",
                            //rankCounter ,lb.userName,lb.highScore);
                    }
                    leaderBoardList.Reverse();
                    //for each duckleaderboard object inside our leaderboardlist
                    foreach(DuckLeaderBoard lb in leaderBoardList)
                    {
                        Debug.LogFormat("leaderboard :rank{0} playername {1} time {2} lvl {3} ",
                           rankCounter, lb.userName, lb.shortestTimeTaken, lb.lvl);
                        rankCounter++;
                    }
                }
            }
        });

        return leaderBoardList;
    }
    public void UpdatePlayerLeaderBoardEntry(string uuid ,int shortestTimeTaken, int lvl, long updatedOn)
    {   

        //path: leaderboards/$uuid/highscore
        //path: leaderboards/$uuid/updatedOn
        dbLeaderBoardRefernce.Child(uuid).Child("shortestTimeTake").SetValueAsync(shortestTimeTaken);
        dbLeaderBoardRefernce.Child(uuid).Child("lvl").SetValueAsync(lvl);
        dbLeaderBoardRefernce.Child(uuid).Child("updatedOn").SetValueAsync(updatedOn);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public async Task<DuckPlayerStats> GetPlayerStats(string uuid)
    {
        Query q = dbPlayerStatsReference.Child(uuid);
        DuckPlayerStats playerStats = null;

        await dbPlayerStatsReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("sorry, there was an error retreiving player stats : ERROR" + task.Exception);
                
            }else if (task.IsCompleted)
            {
                DataSnapshot ds = task.Result;
                if (ds.Child(uuid).Exists)
                {
                    //path to datasnapshot playerstats/$uuid/ <we want this value>
                    playerStats = JsonUtility.FromJson<DuckPlayerStats>(ds.Child(uuid).GetRawJsonValue());

                    Debug.Log("ds..." + ds.GetRawJsonValue());
                    Debug.Log("player stats value.." + playerStats.DuckPlayerStatsToJson());
                }
                
            }
        });

        return playerStats;
    }

    public void DeletePlayerStats(string uuid)
    {
        dbPlayerStatsReference.Child(uuid).RemoveValueAsync();
        dbLeaderBoardRefernce.Child(uuid).RemoveValueAsync();
    }
}


