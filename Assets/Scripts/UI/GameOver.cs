using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject highscorePanel;

    [SerializeField] private GameObject[] highscores;

    [SerializeField] private EnterName enterName;

    [SerializeField] private Text scoreText;
    private int score = 0;
    [SerializeField] private Text timeText;
    private int timeMinutes = 0;
    private int timeSeconds = 0;
    [SerializeField] private Text finalScoreText;
    

    private enum GameOverState
    {
        ENTER_NAME, NAME_ENTERED, HIGHSCORE
    }
    private GameOverState state = GameOverState.ENTER_NAME;


    private void Start()
    {
        gameOverPanel.SetActive(true);
        highscorePanel.SetActive(false);
        finalScoreText.enabled = false;
    }
    
    private void Update()
    {
        switch (state)
        {
            case GameOverState.ENTER_NAME:
                state = enterName.GetName() ? GameOverState.NAME_ENTERED : GameOverState.ENTER_NAME;
                break;
            case GameOverState.NAME_ENTERED:
                scoreText.enabled = false;
                timeText.enabled = false;
                finalScoreText.enabled = true;
                finalScoreText.text = "Final Score:\n" + FinalScore;
                StartCoroutine(FlashScore());
                state = GameOverState.HIGHSCORE;
                break;
            case GameOverState.HIGHSCORE:
                break;
        }
    }

    private IEnumerator FlashScore()
    {
        //heh, I know this is gross and could be done in a for loop
        finalScoreText.enabled = true;
        yield return new WaitForSeconds(1.0f);
        finalScoreText.enabled = false;
        yield return new WaitForSeconds(0.5f);
        finalScoreText.enabled = true;
        yield return new WaitForSeconds(0.5f);
        finalScoreText.enabled = false;
        yield return new WaitForSeconds(0.5f);
        finalScoreText.enabled = true;
        yield return new WaitForSeconds(1.0f);

        ShowHighscores();
    }

    private void ShowHighscores()
    {
        gameOverPanel.SetActive(false);
        highscorePanel.SetActive(true);
    }

    //Add a time multiplier
    public int FinalScore
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
}
