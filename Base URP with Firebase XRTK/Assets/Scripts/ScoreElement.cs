using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreElement : MonoBehaviour
{

    public TMP_Text usernameText;
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text pointsText;

    public void NewScoreElement (string _username, float _time, int _score, int _points)
    {
        usernameText.text = _username;
        timeText.text = _time.ToString();
        scoreText.text = _score.ToString();
        pointsText.text = _points.ToString();
    }

}
