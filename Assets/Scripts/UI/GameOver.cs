using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] public SaveLoadManager saveLoadManager;
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

        score = ScoreManager.Instance.GetFinalScore();
        scoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
        timeText.text = "Time: " + ScoreManager.Instance.GetFinalTime();
        saveLoadManager = SaveLoadManager.Instance;
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
                if (InputManager.Instance().GetActionButton0Down())
                {
                    if (SceneManager.GetActiveScene().name == "Game Over")
                    {
                        ScoreManager.Instance.AddTime(0);
                        SceneManager.LoadScene("MenuScene");
                    }
                }
                break;
        }
    }

    private IEnumerator FlashScore()
    {
        enterName.ForceNameActive();

        AudioManager.Instance.PlayRandomClip(AudioManager.ClipType.POINTS_GAINED, AudioManager.Instance.transform);

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

        saveLoadManager.gameData.AddScoreToLeaderboard(new LeaderboardEntry { playerName = enterName.NameEntered, playerScore = FinalScore });
        saveLoadManager.ForceSave();
        ScoreManager.Instance.ResetScore();
        Debug.Log("Showing Score Screen");
        ShowHighscores();
    }

    private void ShowHighscores()
    {
        List<LeaderboardEntry> leaderboardEntries = saveLoadManager.GetLeaderboardEntries();

        for(int i = 0; i < highscores.Length; i++)
        {
            if (i < leaderboardEntries.Count)
            {
                highscores[i].GetComponent<HighscoreUI>().setHighscore(leaderboardEntries[i].playerName, leaderboardEntries[i].playerScore);
            }
            else
            {
                highscores[i].SetActive(false);
            }
        }

        gameOverPanel.SetActive(false);
        highscorePanel.SetActive(true);
    }

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
