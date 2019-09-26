using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum PlayerState
    {
        GROUNDED,
        JUMPING,
        WEBBING
    }

    [Header("Player Movement Settings")]
    [Tooltip("Determines how quickly the players movement speed ramps up, works in conjunction with Rigidbody's 'Linear Drag'")]
    public float movementSpeed = 1f;
    [Tooltip("Determines what the Horizontal velocity will be capped at")]
    public float maxMovementSpeed = 5f;
    [Tooltip("Determines how quickly the player breaks when not inputting a direction whilst grounded")]
    public float groundBreakingModifier = 0.5f;
    [Tooltip("Determines how quickly the player breaks when not inputting a direction whilst airborn")]
    public float airBreakingModifier = 0.5f;
    [Tooltip("Determines how much momentum is passed into the new direction of movement when switch directions in percentage")]
    public float turnModifier = 0.5f;
    [Tooltip("Determines the initial burst of velocity applied to the Rigidbody's Y axis when jumping")]
    public float jumpVelocity = 1f;
    [Tooltip("Determines how much the Y velocity decreases if the jump button is released early")]
    public float jumpQuickReleaseModifier = 0.5f;
    [Header("Grounded Raycast Settings")]
    [Tooltip("Determines which layers count as floor for the CheckGrounded() raycast")]
    public LayerMask groundedLayers;
    [Tooltip("Determines the length of the raycast shot from the base of the player to check grounded status")]
    public float groundedRaycastLength = 0.5f;

    public PlayerState PlayersMovementState { get; private set; }
    public Vector2 PlayersVelocity { get; private set; }

    private Rigidbody2D rb2d;
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

    private void FixedUpdate()
    {
        HandlePlayerInput();
        UpdateMovement();
        UpdateJump();
        ApplyVelocityToRigidbody();
        UpdateLastHorizontalInput();
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
        triggerJump = Input.GetButtonDown("Jump");
        jumpRelease = Input.GetButtonUp("Jump");
    }

    private void ApplyHorizontalDrag()
    {
        float dragAmount = rb2d.drag * Time.fixedDeltaTime;
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
        ApplyHorizontalDrag();
        
        //if the player isnt inputting anything reduce velocity faster than normal
        if(inputHorizontal == 0)
        {
            if(currentPlayerState == PlayerState.JUMPING)
            {
                playerVelocity.x *= airBreakingModifier;
            }
            else if(currentPlayerState == PlayerState.GROUNDED)
            {
                playerVelocity.x *= groundBreakingModifier;
            }
        }

        //If the player switches direction pass on some of the momentum from traveling in the previous direction to stop it feeling unresponsive
        if (!((lastHorizontalInput >= 0) ^ (inputHorizontal < 0)))
        {
            playerVelocity.x *= -turnModifier;
        }

        playerVelocity.x += inputHorizontal * movementSpeed * Time.fixedDeltaTime;

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
        ApplyVerticalDrag();

        if (currentPlayerState == PlayerState.JUMPING && jumpRelease)
        {
            playerVelocity.y *= jumpQuickReleaseModifier;
        }
        else if (currentPlayerState == PlayerState.GROUNDED && triggerJump)
        {
            playerVelocity.y = jumpVelocity;
            currentPlayerState = PlayerState.JUMPING;
        }
        else if (currentPlayerState == PlayerState.JUMPING && CheckGrounded())
        {
            playerVelocity.y = 0;
            currentPlayerState = PlayerState.GROUNDED;
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
