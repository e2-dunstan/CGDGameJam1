using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebSwing : MonoBehaviour
{
    private Transform webOrigin;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;

    private Transform playerTransform;

    private Vector3[] lineVerts = new Vector3[2];

    private bool isSwinging = false;

    [SerializeField] private float xOffset = 1;

    private float initialSwingPos;

    public bool disableCollisionsWhenSwinging = true;


    private void Start()
    {
        webOrigin = transform;
        lineRenderer = GetComponent<LineRenderer>();
        springJoint = GetComponent<SpringJoint2D>();

        playerTransform = Player.Instance().transform;

        lineVerts[0] = webOrigin.position;

        isSwinging = false;

        //Ensures everything is initialised
        Invoke("UpdateSwinging", 0.1f);
    }


    private void Update()
    {
        lineVerts[0] = webOrigin.position;
        lineVerts[1] = playerTransform.position;
        lineRenderer.SetPositions(lineVerts);
    }


    public void ToggleSwinging()
    {
        isSwinging = !isSwinging;

        if (isSwinging)
        {
            initialSwingPos = springJoint.distance;
            UpdateWebOrigin();
            Player.Instance().GetComponent<Collider2D>().enabled = disableCollisionsWhenSwinging ? false : true;
        }
        else
        {
            springJoint.autoConfigureDistance = true;
            Player.Instance().GetComponent<Collider2D>().enabled = true;
        }

        Player.Instance().PlayerMovement.Rigidbody.drag = isSwinging ? 0f : 0.1f;
        
        UpdateSwinging();
    }


    public void MoveVertically(float input)
    {
        springJoint.autoConfigureDistance = false;

        springJoint.distance -= input * Time.deltaTime * 50;
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
