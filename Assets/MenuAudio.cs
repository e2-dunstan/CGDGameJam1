using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAudio : MonoBehaviour
{

    public AudioClip [] audioClip;
    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.Instance.PlaySpecificClip(audioClip[0], transform);
    }

    // Update is called once per frame
    void Update()
    {
        if(InputManager.Instance().GetActionButton0Down())
        {
            AudioManager.Instance.PlaySpecificClip(audioClip[1], transform);
        }
    }
}
