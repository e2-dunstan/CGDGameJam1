using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    public int averageTimeInSeconds = 180;
    private int _score = 0;
    private int _time = 0;
    private float playerTime = 0.0f; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    public void AddScore(int score)
    {
        _score += score;
    }

    public int GetScore()
    {
        return _score;
    }

    public void AddTime(float time)
    {
        playerTime = time;
    }

    public float GetTime()
    {
        return playerTime;
    }

    public void ResetScore()
    {
        _score = 0;
    }

    public int GetFinalScore()
    {
        int bonusMultiplier = _time < averageTimeInSeconds ? 1 - (_time / averageTimeInSeconds) : 0;

        return _score + (bonusMultiplier * _score);
    }

    public string GetFinalTime()
    {
        string str_time = "";
        float min = playerTime / 60.0f;
        float sec = (int)playerTime % 60;
        str_time = Mathf.FloorToInt(min).ToString() + ":" + Mathf.FloorToInt(sec).ToString();
        return str_time;
    }

    public void UpdateTime(int currentTime)
    {
        _time = currentTime;
    }
}
