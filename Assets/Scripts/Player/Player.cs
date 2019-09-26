using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //TODO add extra references to player subsystems
    [SerializeField] private PlayerMovement playerMovement = null;
    public PlayerMovement PlayerMovement { get => playerMovement; private set => playerMovement = value; }
    [SerializeField] private WebSwing webSwing = null;
    public WebSwing WebSwing { get => webSwing; private set => webSwing = value; }


    private static Player _instance = null;

    private void Awake()
    {
        if (_instance == null) _instance = this;
    }

    public static Player Instance()
    {
        if(_instance == null)
        {
            _instance = new Player();
        }
        return _instance;
    }
}
