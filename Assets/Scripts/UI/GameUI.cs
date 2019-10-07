using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [SerializeField] private Text lives;
    [SerializeField] private Text scoreElement;
    [SerializeField] private Text timeElement;

    private int minutes = 0;
    private int seconds = 0;
    private float timeElapsed = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= 1)
        {
            seconds++;
            if (seconds >= 60)
            {
                seconds = 0;
                minutes++;
            }

            string str = minutes.ToString() + ":";
            if (seconds < 10) str += "0";
            str += seconds.ToString();

            timeElement.text = str;
            timeElapsed = 0;
            ScoreManager.Instance.UpdateTime(seconds + (minutes * 60));
        }

        if (Player.Instance())
        {
            lives.text = Player.Instance().PlayerHealth.GetCurrentLives().ToString();
        }

        if (ScoreManager.Instance)
        {
            scoreElement.text = ScoreManager.Instance.GetScore().ToString();
        }
    }

    private void ResetUI()
    {
        lives.text = 0.ToString();
        scoreElement.text = 0.ToString();
    }
}
