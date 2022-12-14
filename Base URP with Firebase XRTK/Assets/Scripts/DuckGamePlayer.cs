using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class DuckGamePlayer
{
    //properties of DuckGamePlayer
    public string userName;
    public string displayName;
    public string email;
    public string clan;
    public bool active;
    public long lastLoggedIn;
    public long createdOn;
    public long updateOn;

  //empty constructor
  public DuckGamePlayer()
    {

    }
    public DuckGamePlayer(string userName, string displayName, string email, string clan, 
        bool active = true)
    {
        this.userName = userName;
        this.displayName = displayName;
        this.email = email;
        this.clan = clan;
        this.active = active;

        //timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        this.lastLoggedIn = timestamp;
        this.createdOn = timestamp;
        this.updateOn = timestamp;
    }

    //helper functions
    /// <summary>
    /// Simple helper functiom conver object to Json
    /// </summary>
    /// <returns></returns>
    public string DuckGamePlayerToJson()
    {
        return JsonUtility.ToJson(this);
    }
    //simple helper function to print player details
    /// <summary>
    /// return player details
    /// </summary>
    /// <returns></returns>
    public string PrintPlayer()
    {
        return string.Format("Player details: {0} \n User Name: {1}\n Email: {2}\n Clan: {3} \n Active: {4}",
            this.displayName, this.userName, this.email, this.clan, this.active
            );
        
    }
}
