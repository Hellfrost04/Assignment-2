using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class DuckLeaderBoard 
{
    public string userName;
    public int shortestTimeTaken;
    public int lvl;
    public long updatedOn;

    //simple constructor
    public DuckLeaderBoard()
    {

    }
    //constructor with parameters
    public DuckLeaderBoard(string userName, 
        int shortestTimeTaken , int lvl)
    {
        this.userName = userName;
        this.shortestTimeTaken = shortestTimeTaken;
        this.lvl = lvl;

        this.updatedOn = GetTimeUnix();
    }

    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
    }
    public string DuckLeaderBoardToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
