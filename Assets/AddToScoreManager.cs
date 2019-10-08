using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToScoreManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoreManager.Instance.AddTime(ScoreManager.Instance.GetTime() + Time.deltaTime);
    }
}
