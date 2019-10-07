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

    private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite[] sprites;
    /* 0 = idle
     * 1 = webbing
     * 2 = punching
     * 3 = run frame 1
     * 4 = run frame 2
     */

    private float runTimeElapsed = 0;


    private void Awake()
    {
        if (_instance == null) _instance = this;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public static Player Instance()
    {
        if(_instance == null)
        {
            _instance = new Player();
        }
        return _instance;
    }

    private void Update()
    {
        if (InputManager.Instance().GetActionButton1Down() && currentPlayerState == PlayerState.GROUNDED)
        {
            AudioManager.Instance.PlayRandomClip(AudioManager.ClipType.IMPACT, playerMovement.transform);
        }
        if (InputManager.Instance().GetActionButton1Held())
        {
            spriteRenderer.sprite = sprites[2];
            return;
        }

        switch (currentPlayerState)
        {
            case PlayerState.GROUNDED:
                spriteRenderer.sprite = GetGroundedSprite();
                break;
            case PlayerState.AIRBORNE:
            case PlayerState.WEBBING:
            case PlayerState.CLIMBING:
                spriteRenderer.sprite = sprites[1];
                break;
        }

        Vector3 rot = transform.eulerAngles;
        if (playerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.LEFT) rot.y = 180;
        if (playerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.RIGHT) rot.y = 0;
        transform.eulerAngles = rot;
    }

    private Sprite GetGroundedSprite()
    {
        if (PlayerMovement.PlayersVelocity.magnitude > 20f)
        {
            runTimeElapsed += Time.deltaTime;
            if (runTimeElapsed > 0.2f)
            {
                if (runTimeElapsed > 0.4f) runTimeElapsed = 0;
                return sprites[4];
            }
            else
            {
                return sprites[3];
            }
        }
        else
        {
            return sprites[0];
        }
    }
}
