using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance = null;

    public int averageTimeInSeconds = 180;
    private int _score = 0;
    private int _time = 0;

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

    public void ResetScore()
    {
        _score = 0;
    }

    public int GetFinalScore()
    {
        int bonusMultiplier = _time < averageTimeInSeconds ? 1 - (_time / averageTimeInSeconds) : 0;

        return _score + (bonusMultiplier * _score);
    }

    public void UpdateTime(int currentTime)
    {
        _time = currentTime;
    }
}
