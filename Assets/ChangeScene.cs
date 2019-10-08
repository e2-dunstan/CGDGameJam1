using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var input = InputManager.Instance();
        if(input.GetActionButton0Down())
        {
            if (SceneManager.GetActiveScene().name == "MenuScene")
            {
                SceneManager.LoadScene("Level");
            }
        }
    }
}
