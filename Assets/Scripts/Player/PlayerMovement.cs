using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        GROUNDED,
        JUMPING,
        WEBBING,
        CRAWLING
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
    public PlayerState PlayersMovementState { get => currentPlayerState;  set => currentPlayerState = value; }
    public Vector2 PlayersVelocity { get => playerVelocity; private set => playerVelocity = value; }

    private Rigidbody2D rb2d;
    public Rigidbody2D Rigidbody { get => rb2d; private set => rb2d = value; }

    private BoxCollider2D col2d;
    private Vector2 playerVelocity = Vector2.zero;
    private PlayerState currentPlayerState = PlayerState.GROUNDED;
    private float inputHorizontal = 0;
    private float lastHorizontalInput = 0;
    private bool triggerJump = false;
    private bool jumpRelease = false;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        col2d = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HandlePlayerInput();
    }

    private void FixedUpdate()
    {
        if (currentPlayerState != PlayerState.WEBBING)
        {
            UpdateMovement();
            UpdateJump();
            ApplyVelocityToRigidbody();
            UpdateLastHorizontalInput();
        }
    }

    private bool CheckGrounded()
    {
        /*Changes the point of the raycast depending on direction of movement
        so that if any of the player is on an edge you will be able to jump */
        Vector2 raycastOrigin = Vector2.zero;
        raycastOrigin.y = col2d.bounds.min.y + 0.1f;
        if (inputHorizontal > 0)
        {
            raycastOrigin.x = col2d.bounds.min.x;
        }
        else if(inputHorizontal < 0)
        {
            raycastOrigin.x = col2d.bounds.max.x;
        }
        else
        {
            raycastOrigin.x = col2d.bounds.center.x;
        }

        if(Physics2D.Raycast(raycastOrigin, Vector2.down, groundedRaycastLength, groundedLayers))
        {
            Debug.DrawRay(raycastOrigin, Vector2.down * groundedRaycastLength, Color.green, 10);
            return true;
        }
        Debug.DrawRay(raycastOrigin, Vector2.down * groundedRaycastLength, Color.red, 10);
        return false;
    }

    private void HandlePlayerInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if (currentPlayerState == PlayerState.GROUNDED)
            {
                triggerJump = true;
                jumpRelease = false;
            }
            else if (currentPlayerState == PlayerState.JUMPING || currentPlayerState == PlayerState.WEBBING)
            {
                Player.Instance().WebSwing.ToggleSwinging();
            }
        }
        else if (Input.GetButtonUp("Jump") && currentPlayerState == PlayerState.JUMPING)
        {
            triggerJump = false;
            jumpRelease = true;
        }
    }

    private void ApplyHorizontalDrag()
    {
        float dragAmount = rb2d.drag * Time.fixedDeltaTime;

        //Reduce velocity faster depending on current state
        if (currentPlayerState == PlayerState.JUMPING)
        {
            dragAmount *= airBreakingModifier;
        }
        else if (currentPlayerState == PlayerState.GROUNDED)
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
        if (currentPlayerState != PlayerState.WEBBING)
        {
            ApplyHorizontalDrag();
        }

        float newXAcceleration = inputHorizontal * movementSpeed * Time.fixedDeltaTime;
        //If the player is turning around give a boost to the acceleration to stop it feeling sluggish
        newXAcceleration = (playerVelocity.x >= 0) ^ (inputHorizontal < 0) ? newXAcceleration : newXAcceleration * turnModifier;
        playerVelocity.x += newXAcceleration;

        //Cap the speed so it doesnt keep rising exponentially
        if (playerVelocity.x > maxMovementSpeed)
        {
            playerVelocity.x = maxMovementSpeed;
        }
        else if (playerVelocity.x < -maxMovementSpeed)
        {
            playerVelocity.x = -maxMovementSpeed;
        }
    }

    private void UpdateJump()
    {
        if (currentPlayerState != PlayerState.WEBBING)
        {
            ApplyVerticalDrag();
        }

        if (currentPlayerState == PlayerState.JUMPING && CheckGrounded())
        {
            playerVelocity.y = 0;
            currentPlayerState = PlayerState.GROUNDED;
        }
        if (currentPlayerState == PlayerState.GROUNDED && triggerJump)
        {
            triggerJump = false;
            playerVelocity.y = jumpVelocity;
            currentPlayerState = PlayerState.JUMPING;
        }
        else if (currentPlayerState == PlayerState.JUMPING && jumpRelease && playerVelocity.y > 0)
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
        lastHorizontalInput = Input.GetAxisRaw("Horizontal");
    }
}
