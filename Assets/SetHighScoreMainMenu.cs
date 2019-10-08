using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetHighScoreMainMenu : MonoBehaviour
{
    private SaveLoadManager saveLoadManager;
    
    // Start is called before the first frame update
    void Awake()
    {
        saveLoadManager = SaveLoadManager.Instance;

        int highScore = saveLoadManager.GetHighScore();

        gameObject.GetComponent<Text>().text = "High Score: " + highScore;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
