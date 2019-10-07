using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSwing : MonoBehaviour
{
    private PlayerMovement.MovementDirection swingDir;

    private Transform webOrigin;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;

    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private Vector3[] lineVerts = new Vector3[2];

    private bool isSwinging = false;

    [SerializeField] private float xOffset = 1;

    private float initialSwingPos;

    public bool disableCollisionsWhenSwinging = true;

    private float maxSwingSpeed;


    private void Start()
    {
        webOrigin = transform;
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();

        playerTransform = Player.Instance().transform;
        playerMovement = Player.Instance().PlayerMovement;
        maxSwingSpeed = playerMovement.GetMaxSpeed() * 2;

        lineVerts[0] = webOrigin.position;

        isSwinging = false;

        //Ensures everything is initialised
        Invoke("UpdateSwinging", 0.1f);
    }


    private void Update()
    {
        if (!isSwinging) return;
        
        swingDir = UpdateSwingDirection();

        lineVerts[0] = webOrigin.position;
        lineVerts[1] = playerTransform.position;
        lineRenderer.SetPositions(lineVerts);

        // -- Left and right to speed up/down -- //

        Vector2 velocity = playerMovement.Rigidbody.velocity;

        float horizontal = InputManager.Instance().GetHorizontalInput();

        playerMovement.PlayerMovementDirection = swingDir;

        if ((swingDir == PlayerMovement.MovementDirection.LEFT && horizontal < 0 
            && playerMovement.Rigidbody.velocity.x > -maxSwingSpeed)
            ||
            (swingDir == PlayerMovement.MovementDirection.RIGHT && horizontal > 0
            && playerMovement.Rigidbody.velocity.magnitude < maxSwingSpeed))
        {
            velocity.x += horizontal * 50 * Time.deltaTime;
        }

        playerMovement.Rigidbody.velocity = velocity;
    }


    private PlayerMovement.MovementDirection UpdateSwingDirection()
    {
        if (playerMovement.Rigidbody.velocity.x < 0)
            return PlayerMovement.MovementDirection.LEFT;
        else if (playerMovement.Rigidbody.velocity.x > 0)
            return PlayerMovement.MovementDirection.RIGHT;
        else
            return swingDir;
    }


    public void ToggleSwinging()
    {
        isSwinging = !isSwinging;
        Update();

        AudioManager.Instance.PlayRandomClip(AudioManager.ClipType.WEB, playerTransform);

        if (isSwinging)
        {
            initialSwingPos = springJoint.distance;
            UpdateWebOrigin();
            Player.Instance().gameObject.layer = disableCollisionsWhenSwinging ? 11 : 0;
        }
        else
        {
            springJoint.autoConfigureDistance = true;
            Player.Instance().gameObject.layer = 0;
            Player.Instance().PlayerMovement.CarryOverVelocityFromSwinging();
        }

        Player.Instance().PlayerMovement.Rigidbody.drag = isSwinging ? 0f : 0.1f;
        
        UpdateSwinging();
    }


    public void MoveVertically(float input)
    {
        springJoint.autoConfigureDistance = false;

        springJoint.distance += input * Time.deltaTime * 50;
    }


    private void UpdateSwinging()
    {
        springJoint.enabled = isSwinging;
        lineRenderer.enabled = isSwinging;

        springJoint.connectedBody = Player.Instance().PlayerMovement.Rigidbody;

        Player.Instance().CurrentPlayerState = isSwinging ? Player.PlayerState.WEBBING : Player.PlayerState.AIRBORNE;
        Player.Instance().PlayerMovement.Rigidbody.gravityScale = isSwinging ? 100 : 1;
    }


    private void UpdateWebOrigin()
    {
        float _offset = xOffset + Player.Instance().PlayerMovement.PlayersVelocity.magnitude / 10;

        if (Player.Instance().PlayerMovement.PlayerMovementDirection == PlayerMovement.MovementDirection.LEFT)
        {
            _offset = -_offset;
        }

        _offset += playerTransform.position.x;

        webOrigin.position = new Vector2(_offset, webOrigin.position.y);
    }
}
