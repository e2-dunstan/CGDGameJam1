using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 1f;
    public float maxSpeed = 5f;
    public float jumpVelocity = 1f;

    private Rigidbody2D rigidbody = null;
    private BoxCollider2D collider = null;
    private float inputHorizontal = 0;
    private float lastHorizontalInput = 0;
    private bool isGrounded = true;
    private bool triggerJump = false;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        isGrounded = CheckGrounded();
        ApplyExternalForcesToVelocity();
        HandlePlayerInput();
        UpdateMovement();
        UpdateJump();
        UpdateLastHorizontalInput();
    }

    private bool CheckGrounded()
    {
        Vector2 raycastOrigin = new Vector2(collider.bounds.center.x, collider.bounds.min.y + 0.1f);
        Debug.DrawRay(raycastOrigin, Vector2.down * 0.5f, Color.blue, 10);
        RaycastHit2D rayHit = Physics2D.Raycast(raycastOrigin, Vector2.down, 0.5f);
        return rayHit.collider.CompareTag("Ground");
    }

    private void HandlePlayerInput()
    {
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        triggerJump = Input.GetButtonDown("Jump");
    }

    private void ApplyExternalForcesToVelocity()
    {
        Vector2 currentVel = rigidbody.velocity;
        Vector2 newVel = Vector2.zero;

        //Horizontal drag
        float dragAmount = rigidbody.drag * Time.fixedDeltaTime;
        float newXVel = currentVel.x < 0 ? currentVel.x + dragAmount : currentVel.x - dragAmount;
        //Bitwise comparison to see if they are the same sign or if its gone from negative to possitive or visa versa then zero vel;
        newVel.x = !((newXVel >= 0) ^ (currentVel.x < 0)) || currentVel.x == 0 ? 0 : newXVel;
        //Cap the speed so it dont go crazy
        if(newXVel > 0)
        {
            newVel.x = newXVel > maxSpeed ? maxSpeed : newXVel;
        }
        else if(newXVel < 0)
        {
            newVel.x = newXVel < -maxSpeed ? -maxSpeed : newXVel;
        }

        //Vertical drag / gravity
        float gravityAmount = Physics2D.gravity.y * Time.fixedDeltaTime;
        float newYVel = currentVel.y - gravityAmount;
        newVel.y = CheckGrounded() ? 0 : newYVel;

        rigidbody.velocity = newVel;
    }

    private void UpdateMovement()
    {
        Vector2 newVel = rigidbody.velocity;
        //If the player switch direction then it zeros out the velocity to stop sliding
        newVel.x = !((lastHorizontalInput >= 0) ^ (inputHorizontal < 0)) || inputHorizontal == 0 ? 0 : newVel.x;
        newVel.x += inputHorizontal * movementSpeed * Time.fixedDeltaTime;
        rigidbody.velocity = newVel;
    }

    private void UpdateJump()
    {
        if (triggerJump && isGrounded)
        {
            triggerJump = false;
            Vector2 currentVel = rigidbody.velocity;
            currentVel.y = jumpVelocity;
            rigidbody.velocity = currentVel;
        }
    }

    private void UpdateLastHorizontalInput()
    {
        lastHorizontalInput = Input.GetAxisRaw("Horizontal");
    }
}
