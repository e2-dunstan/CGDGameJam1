using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum MovementDirection
    {
        LEFT,
        RIGHT
    }

    [Header("Player Movement Settings")]
    [Tooltip("Determines how quickly the players movement speed ramps up, works in conjunction with Rigidbody's 'Linear Drag'")]
    [SerializeField] private float movementSpeed = 1f;
    [Tooltip("Determines what the Horizontal velocity will be capped at")]
    [SerializeField] private float maxMovementSpeed = 5f;
    [Tooltip("Determines how quickly the player breaks whilst grounded, if var == 2 then 2x the drag")]
    [SerializeField] private float groundBreakingModifier = 0.5f;
    [Tooltip("Determines how quickly the player breaks whilst airborn, if var == 2 then 2x the drag")]
    [SerializeField] private float airBreakingModifier = 0.5f;
    [Tooltip("Determines the modifier applied to the players acceleration if turning direction")]
    [SerializeField] private float turnModifier = 0.5f;
    [Tooltip("Determines the initial burst of velocity applied to the Rigidbody's Y axis when jumping")]
    [SerializeField] private float jumpVelocity = 1f;
    [Tooltip("Determines how much the Y velocity decreases if the jump button is released early")]
    [SerializeField] private float jumpQuickReleaseModifier = 0.5f;
    [Header("Grounded Raycast Settings")]
    [Tooltip("Determines which layers count as floor for the CheckGrounded() raycast")]
    [SerializeField] private LayerMask groundedLayers;
    [Tooltip("Determines the length of the raycast shot from the base of the player to check grounded status")]
    [SerializeField] private float groundedRaycastLength = 0.5f;

    //Getters for private variables
    private Vector2 playerVelocity = Vector2.zero;
    public Vector2 PlayersVelocity { get => playerVelocity; private set => playerVelocity = value; }

    private Rigidbody2D rb2d;
    public Rigidbody2D Rigidbody { get => rb2d; private set => rb2d = value; }
    private MovementDirection movDir = MovementDirection.LEFT;
    public MovementDirection PlayerMovementDirection { get => movDir; set => movDir = value; }

    private CapsuleCollider2D col2d;
    private Player playerSingleton = null;
    private InputManager inputSingleton = null;
    private float inputHorizontal = 0;
    private float lastHorizontalInput = 0;
    private bool triggerJump = false;
    private bool jumpRelease = false;
    private bool horizontalCapOverride = false;
    private float horizontalOverrideCap = 0.0f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<CapsuleCollider2D>();
        playerSingleton = Player.Instance();
        inputSingleton = InputManager.Instance();
    }

    private void Update()
    {
        HandlePlayerInput();
    }

    private void FixedUpdate()
    {
        if (playerSingleton.CurrentPlayerState != Player.PlayerState.WEBBING && playerSingleton.CurrentPlayerState != Player.PlayerState.CLIMBING)
        {
            UpdateMovement();
            UpdateJump();
            ApplyVelocityToRigidbody();
            UpdateLastHorizontalInput();
        }
    }

    public bool CheckGrounded()
    {
        /*Changes the point of the raycast depending on direction of movement
        so that if any of the player is on an edge you will be able to jump */
        Vector2 raycastLeft = Vector2.zero;
        Vector2 raycastRight = Vector2.zero;
        raycastLeft.y = raycastRight.y = col2d.bounds.min.y + 0.1f;
        raycastRight.x = col2d.bounds.max.x;
        raycastLeft.x = col2d.bounds.min.x;
        //If left or right raycast registers as grounded then the player is grounded
        RaycastHit2D left = Physics2D.Raycast(raycastLeft, Vector2.down, groundedRaycastLength, groundedLayers);
        RaycastHit2D right = Physics2D.Raycast(raycastRight, Vector2.down, groundedRaycastLength, groundedLayers);
        return left | right;
    }

    private void HandlePlayerInput()
    {
        inputHorizontal = inputSingleton.GetHorizontalInput();
        if (inputSingleton.GetActionButton0Down())
        {
            if (playerSingleton.CurrentPlayerState == Player.PlayerState.GROUNDED)
            {
                triggerJump = true;
                jumpRelease = false;
            }
            else if (playerSingleton.PreviousPlayerState == Player.PlayerState.CLIMBING && playerSingleton.CurrentPlayerState == Player.PlayerState.AIRBORNE)
            {
                playerSingleton.ChangePlayerState(Player.PlayerState.AIRBORNE);
            }
            else if (playerSingleton.CurrentPlayerState == Player.PlayerState.AIRBORNE || playerSingleton.CurrentPlayerState == Player.PlayerState.WEBBING)
            {
               playerSingleton.WebManager.ToggleSwinging();
            }
        }
        else if (inputSingleton.GetActionButton0Up() && playerSingleton.CurrentPlayerState == Player.PlayerState.AIRBORNE)
        {
            triggerJump = false;
            jumpRelease = true;
        }

        float inputVertical = inputSingleton.GetVerticalInput();
        if (playerSingleton.CurrentPlayerState == Player.PlayerState.WEBBING && inputVertical != 0)

        {

            playerSingleton.WebManager.MoveVertically(inputVertical);

        }
    }

    private void ApplyHorizontalDrag()
    {
        float dragAmount = rb2d.drag * Time.fixedDeltaTime;

        //Reduce velocity faster depending on current state
        if (playerSingleton.CurrentPlayerState == Player.PlayerState.AIRBORNE)
        {
            dragAmount *= airBreakingModifier;
        }
        else if (playerSingleton.CurrentPlayerState == Player.PlayerState.GROUNDED)
        {
            dragAmount *= groundBreakingModifier;
        }

        float newXVel = playerVelocity.x < 0 ? playerVelocity.x + dragAmount : playerVelocity.x - dragAmount;
        //Bitwise comparison to see if they are the same sign or if its gone from negative to possitive or visa versa then zero vel;
        playerVelocity.x = !((newXVel >= 0) ^ (playerVelocity.x < 0)) || playerVelocity.x == 0 ? 0 : newXVel;
    }

    private void ApplyVerticalDrag()
    {
        float gravityAmount = Physics2D.gravity.y * rb2d.mass * Time.fixedDeltaTime;
        float newYVel = playerVelocity.y + gravityAmount;
        playerVelocity.y = CheckGrounded() ? 0 : newYVel;
    }

    private void UpdateMovement()
    {
        if (playerSingleton.CurrentPlayerState != Player.PlayerState.WEBBING)
        {
            ApplyHorizontalDrag();
        }

        float newXAcceleration = inputHorizontal * movementSpeed * Time.fixedDeltaTime;
        //If the player is turning around give a boost to the acceleration to stop it feeling sluggish
        newXAcceleration = (playerVelocity.x >= 0) ^ (inputHorizontal < 0) ? newXAcceleration : newXAcceleration * turnModifier;
        playerVelocity.x += newXAcceleration;

        if(playerVelocity.x == 0)
        {
            //Leave movDir to its previous setting
        }
        else if(playerVelocity.x > 0)
        {
            movDir = MovementDirection.RIGHT;
        }
        else if(playerVelocity.x < 0)
        {
            movDir = MovementDirection.LEFT;
        }

        //Cap the speed so it doesnt keep rising exponentially
        if (playerVelocity.x > maxMovementSpeed)
        {
            playerVelocity.x = horizontalCapOverride ? horizontalOverrideCap : maxMovementSpeed;
        }
        else if (playerVelocity.x < -maxMovementSpeed)
        {
            playerVelocity.x = horizontalCapOverride ? horizontalOverrideCap : -maxMovementSpeed;
        }
    }

    private void UpdateJump()
    {
        if (playerSingleton.CurrentPlayerState != Player.PlayerState.WEBBING)

        {

            ApplyVerticalDrag();

        }

        if (playerSingleton.CurrentPlayerState == Player.PlayerState.AIRBORNE && CheckGrounded())
        {
            playerVelocity.y = 0;
            horizontalCapOverride = false;
            playerSingleton.ChangePlayerState(Player.PlayerState.GROUNDED);
        }
        if (playerSingleton.CurrentPlayerState == Player.PlayerState.GROUNDED && triggerJump)
        {
            AudioManager.Instance.PlayRandomClip(AudioManager.ClipType.JUMP, transform);
            triggerJump = false;
            playerVelocity.y = jumpVelocity;
            playerSingleton.ChangePlayerState(Player.PlayerState.AIRBORNE);
        }
        else if (playerSingleton.CurrentPlayerState == Player.PlayerState.AIRBORNE && jumpRelease && playerVelocity.y > 0)
        {
            playerVelocity.y *= jumpQuickReleaseModifier;
            jumpRelease = playerVelocity.y > 0;
        }
    }

    private void ApplyVelocityToRigidbody()
    {
        rb2d.velocity = playerVelocity;
    }

    private void UpdateLastHorizontalInput()
    {
        lastHorizontalInput = inputSingleton.GetHorizontalInput();
    }

    public void CarryOverVelocityFromSwinging()
    {
        horizontalCapOverride = true;
        horizontalOverrideCap = rb2d.velocity.x;
        playerVelocity = rb2d.velocity;
    }

    public float GetMaxSpeed()
    {
        return maxMovementSpeed;
    }
}
