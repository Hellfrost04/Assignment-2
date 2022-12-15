/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SnakeController : MonoBehaviour {

    public AuthManager auth;
    public FirebaseManger firebaseMgr;
    public bool isPlayerStatsUpdated;

    // Settings
    public float MoveSpeed = 6;
    public float SteerSpeed = 250;
    public float BodySpeed = 6;
    public int Gap = 100;
    public static int score;
    public static int points;
    public int longestTime;
    public int numberOfApples;
    public int numberOfPills;
    public int lvl;
    private int endScore;
    public int recentTimeTaken;
    public int shortestTimeTaken;
    public int numberOfThingsShot;
    public int numberOfTries;
    public int xp;  


    // References
    public GameObject BodyPrefab;
    public GameObject Apple;
    public GameObject pill;
    public GameObject DeathMenu;
    public GameObject GameUIMenu;
    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    // Start is called before the first frame update
    void Start() {
        numberOfThingsShot = 0;
        numberOfTries = 0;
        shortestTimeTaken = 0;
        recentTimeTaken = 0;
        isPlayerStatsUpdated = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update() {
        //debuff
        Debuff(); 

        //Died

        // Move forward
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;

        // Steer
        float steerDirection = Input.GetAxis("Horizontal"); // Returns value -1, 0, or 1
        transform.Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        // Store position history
        PositionsHistory.Insert(0, transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts) {
            Vector3 point = PositionsHistory[Mathf.Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position += moveDirection * BodySpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt(point);

            index++;
        }
        
    }
    /// <summary>
    /// when reached certain score apply debuffs on player
    /// </summary>
    private void Debuff()
    {
        //Debuffs
        if (score > 500)
        {
            MoveSpeed = 5;
            BodySpeed = 5;
            SteerSpeed = 220;
        }
        if (score > 1000)
        {
            MoveSpeed = 4;
            BodySpeed = 4;
            SteerSpeed = 200;
        }
        if (score > 2000)
        {
            MoveSpeed = 3;
            BodySpeed = 3;
            SteerSpeed = 170;
        }
    }
    public void PointsCounter()
    {
        if (longestTime > 60)
        {
            points = 100 + score;
        }
        else
        {
            points = score;
        }
        if (longestTime > 150)
        {
            points = 400 + score;
        }
        if(longestTime > 250)
        {
            points = longestTime * 2 + score;
        }
    }
    /// <summary>
    /// when player dies
    /// </summary>
    private void Died()
    {
        Time.timeScale = 0;
        int recentTimeTaken = Mathf.RoundToInt(GameUI.time);
        int shortestTimeTaken = Mathf.RoundToInt(GameUI.time);
        int timePlayed = Mathf.RoundToInt(GameUI.time);

        if (!isPlayerStatsUpdated)
        {
            UpdatePlayerStat(xp ,lvl, shortestTimeTaken, recentTimeTaken, numberOfTries, numberOfThingsShot ,timePlayed);

        }
        isPlayerStatsUpdated = true;
        DeathMenu.SetActive(true);
        GameUIMenu.SetActive(false);
      
    }
    public void UpdatePlayerStat(int xp,int lvl, int shortestTimeTaken, int recentTimeTaken, int numberOfThingsShot, int numberOfTries, int time)
    {
        firebaseMgr.UpdatePlayerStats(auth.GetCurrentUser().UserId, xp, lvl, shortestTimeTaken, recentTimeTaken, numberOfThingsShot,numberOfTries, time, auth.GetCurrentUserDisplayName());
        
    }   
    //st
    private void GrowSnake() {
        // Instantiate body instance and
        // add it to the list
        GameObject body = Instantiate(BodyPrefab);
        BodyParts.Add(body);
    }
    private void OnTriggerEnter(Collider other)
    {   //when collect grow one body part
        if (other.tag == "food")
        {
            Destroy(other.gameObject);
            GrowSnake();
            score += 50;
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-8.0f, 8.0f), 0, Random.Range(-8.0f, 8.0f));
            Instantiate(Apple,randomSpawnLocation,Quaternion.identity );
            numberOfApples += 1;
        }
        // when collected grow 2 body parts
        if (other.tag == "Megafood")
        {
            Destroy(other.gameObject);
            GrowSnake();
            GrowSnake();
            score += 100;
            Vector3 randomSpawnLocation = new Vector3(Random.Range(-8.0f, 8.0f), 0, Random.Range(-8.0f, 8.0f));
            Instantiate(pill, randomSpawnLocation, Quaternion.identity);
            numberOfPills += 1;
        }
        // if collides with body or wall lose game
        if (other.tag == "chick")
        {
            Died();
            endScore += score;
        }
        if (other.tag == "wall")
        {
            Died();
            endScore += score;
        }
    }
}*/