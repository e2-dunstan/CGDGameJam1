using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public int currentScreen;
    [Tooltip("Where the camera moves after a screen transition")]
    public Transform[] screenCameraPosition;
    [Tooltip("Where the player moves after a screen transition forward.")]
    public Transform[] screenPlayerSpawn;
    [Tooltip("Where the player moves after a screen transition backward.")]
    public Transform[] screenPlayerEnd;

    // Start is called before the first frame update
    void Start()
    {
        currentScreen = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }


}
