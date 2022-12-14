using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;

public class GameMenuManger : MonoBehaviour
{
    public AuthManager authMgr;
    public DatabaseReference dbClanReference;//refrencing clan data ony
    public GameObject signOutBtn;

    public TextMeshProUGUI blueCounter;
    public TextMeshProUGUI yellowCounter;
    public TextMeshProUGUI redCounter;
    public TextMeshProUGUI displayName;


    public void Awake()
    {
        InitializeFirebase();

        SetClanMembersCount();

        //displayName.text = "Player: " + authMgr.GetCurrentUserDisplayName();
        
       
    }
    public void InitializeFirebase()
    {
        dbClanReference = FirebaseDatabase.DefaultInstance.GetReference("clans");
    }

    public void SetClanMembersCount()
    {
        long blueClanMembers = 0;
        long yellowClanMembers = 0;
        long redClanMembers = 0;

        dbClanReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("sorry, there was an error in retriving clan information, ERROR" + task.Exception);
                return;
            
            }
            else if (task.IsCompleted)
            {
                //datasnapshot just means we copy
                DataSnapshot clanSnapshot = task.Result;
                if (clanSnapshot.Exists)
                {
                    blueClanMembers = clanSnapshot.Child("blue").ChildrenCount;
                    yellowClanMembers = clanSnapshot.Child("yellow").ChildrenCount;
                    redClanMembers = clanSnapshot.Child("red").ChildrenCount;

                    blueCounter.text = blueClanMembers.ToString();
                    yellowCounter.text = yellowClanMembers.ToString();
                    redCounter.text = redClanMembers.ToString();

                    Debug.LogFormat("Number of red {0} blue {1} yellow {2} members", redClanMembers, blueClanMembers, yellowClanMembers);
                }
            }
        });
    }
    public void SignOut()
    {
        authMgr.SignOutUser();
        
    }
    public void GameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
