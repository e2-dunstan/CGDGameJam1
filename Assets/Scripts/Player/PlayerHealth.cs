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
    private SpriteRenderer playerSprite;

    private void Start()
    {
        currentLives = maxLives;
        playerSprite = GetComponent<SpriteRenderer>();
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
            StartCoroutine("FlickerSprite");
        }
    }

    public void CheckDeath()
    {
        if(currentLives == 0)
        {
            SceneManager.LoadScene(deathSceneName);
        }
    }

    IEnumerator FlickerSprite()
    {
        Color spriteCol = playerSprite.color;
        int evenFlashes = 8;
        for (int i = 0; i <= evenFlashes; i++)
        {
            spriteCol.a = i % 2 == 0 ? 1.0f : 0.3f;
            playerSprite.color = spriteCol;
            yield return new WaitForSeconds(iFrameDuration / evenFlashes);
        }
    }
}
