using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTransition : MonoBehaviour
{
    private Transform playerTransform;
    private Camera mainCamera;
    private ScreenManager sm;

    enum boundaryType
    {
        NULL,
        ENTRANCE,
        EXIT, 
        KILL
    }
    [SerializeField] boundaryType boundary;

    private void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        sm = GameObject.FindGameObjectWithTag("GameController").GetComponent<ScreenManager>();
    }

    //Move the screen back if entrance, forward if exit
    private void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.CompareTag("Player"))
        {
            playerTransform = _col.gameObject.transform;
            switch (boundary)
            {
                case boundaryType.ENTRANCE:
                    sm.currentScreen--;
                    print("Hit Entrance");
                    break;

                case boundaryType.EXIT:
                    sm.currentScreen++;
                    print("Hit Exit");
                    break;
            }
            print("Screen" + sm.currentScreen);
            MovePlayerAndCamera();
        }
    }

    //Set the new position of the player to the new level
    private void SetPlayerPosition()
    {
        switch (boundary)
        {
            case boundaryType.ENTRANCE:
                Player.Instance().transform.position = new Vector2(
                sm.screenPlayerEnd[sm.currentScreen].position.x,
                Player.Instance().transform.position.y
                );
                break;

            case boundaryType.EXIT:

                if (sm.currentScreen != 8)
                {
                    Player.Instance().transform.position = new Vector2(
                    sm.screenPlayerSpawn[sm.currentScreen].position.x,
                    Player.Instance().transform.position.y
                    );
                }
                else
                {
                    Debug.Log("Hit");
                    Player.Instance().transform.position = sm.screenPlayerSpawn[sm.currentScreen].position;
                }

                break;

            case boundaryType.KILL:
                Player.Instance().PlayerMovement.SetPlayerVelocity(new Vector2(0, 0));
                Player.Instance().transform.position = new Vector2(
                sm.screenPlayerSpawn[sm.currentScreen].position.x,
                sm.screenPlayerSpawn[sm.currentScreen].position.y
                );
                Player.Instance().PlayerHealth.TakeDamage();
                break;
        }

        
    }

    //Set the new position of the camera to the new level
    private void SetCameraPoint()
    {
        mainCamera.transform.position = new Vector3(
            sm.screenCameraPosition[sm.currentScreen].position.x,
            sm.screenCameraPosition[sm.currentScreen].position.y,
            mainCamera.transform.position.z
            );
    }
    private void MovePlayerAndCamera()
    {
        SetPlayerPosition();
        SetCameraPoint();
    }
}
