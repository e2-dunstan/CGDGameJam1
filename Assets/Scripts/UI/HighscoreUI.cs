using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreUI : MonoBehaviour
{
    [SerializeField] private GameObject Name;
    [SerializeField] private GameObject Score;

    public void setHighscore(string _name, int _score)
    {
        Name.GetComponent<UnityEngine.UI.Text>().text = _name;
        Score.GetComponent<UnityEngine.UI.Text>().text = _score.ToString();
    }
}
