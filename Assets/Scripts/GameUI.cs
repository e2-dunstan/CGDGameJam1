using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [SerializeField] private GameObject[] lives;
    [SerializeField] private Text scoreElement;
    [SerializeField] private Text timeElement;
    private int score = 0;

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
        }
    }


    public void RemoveLife()
    {
        if (lives[2].activeInHierarchy)
        {
            lives[2].SetActive(false);
            Debug.Log("2 LIVES LEFT");
        }
        else if (lives[1].activeInHierarchy)
        {
            lives[1].SetActive(false);
            Debug.Log("1 LIFE LEFT");
        }
        else if (lives[0].activeInHierarchy)
        {
            lives[0].SetActive(false);
            Debug.Log("OUT OF LIVES");
        }
    }
    

    public void AddToScore(int _score)
    {
        score += _score;
        scoreElement.text = score.ToString();
    }


    private void ResetUI()
    {
        lives[0].SetActive(true);
        lives[1].SetActive(true);
        lives[2].SetActive(true);

        score = 0;
    }
}
