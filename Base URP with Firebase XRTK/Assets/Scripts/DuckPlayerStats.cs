using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DuckPlayerStats
{
    public string userName;
    public int totalTimeSpent;
    public int recentTimeTaken;
    public int shortestTimeTaken;
    public int lvl;
    public int numberOfTries;
    public int numberOfThingsShot;
    public int xp;
    public long updatedOn;
    public long createdOn;

    //simple constructor
    public DuckPlayerStats()
    {

    }

    public DuckPlayerStats (string userName, int recentTimeTaken, int xp ,int lvl ,int shortestTimeTaken
        ,int numberOfTries , int numberOfThingsShot , int totalTimeSpent )
    {
        this.userName = userName;
        this.recentTimeTaken = recentTimeTaken;
        this.xp = xp;
        this.shortestTimeTaken = shortestTimeTaken;
        this.lvl = lvl;
        this.numberOfTries = numberOfTries;
        this.numberOfThingsShot = numberOfThingsShot;
        this.totalTimeSpent = totalTimeSpent;


        var timestamp = this.GetTimeUnix();
        this.updatedOn = timestamp;
        this.createdOn = timestamp;
    }

    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }

    public string DuckPlayerStatsToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
