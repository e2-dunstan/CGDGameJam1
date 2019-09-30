using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum PlayerState
    {
        GROUNDED,
        AIRBORNE,
        WEBBING,
        CLIMBING
    }

    //TODO add extra references to player subsystems
    [SerializeField] private PlayerMovement playerMovement = null;
    public PlayerMovement PlayerMovement { get => playerMovement; private set => playerMovement = value; }
    [SerializeField] private WebSwing webManager = null;
    public WebSwing WebManager { get => webManager; private set => webManager = value; }

    private static Player _instance = null;
    private PlayerState currentPlayerState = PlayerState.GROUNDED;
    public PlayerState CurrentPlayerState { get => currentPlayerState; set => currentPlayerState = value; }

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
