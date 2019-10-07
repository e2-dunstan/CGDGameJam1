using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float iFrameDuration = 2.0f;
    [SerializeField] private string deathSceneName;

    private int currentLives = 0;
    private float iFrameTimer = 0.0f;

    private void Start()
    {
        currentLives = maxLives;
    }

    private void Update()
    {
        if (iFrameTimer > 0)
        {
            iFrameTimer -= Time.deltaTime;
            if(iFrameTimer < 0)
            {
                iFrameTimer = 0;
            }
        }
    }

    public int GetCurrentLives()
    {
        return currentLives;
    }

    public void TakeDamage()
    {
        if (iFrameTimer == 0)
        {
            iFrameTimer = iFrameDuration;
            currentLives--;
            CheckDeath();
        }
    }

    public void CheckDeath()
    {
        if(currentLives == 0)
        {
            SceneManager.LoadScene(deathSceneName);
        }
    }
}
