using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCrawling : MonoBehaviour
{
    public Collider2D building;
    Player player;
    InputManager input;
    bool zipping = false;
    LineRenderer lineRenderer;
    Vector2 webZipPoint;
    float webZipTime = 0.0f;
    [Range(0.0f, 50.0f)]
    [SerializeField] float webZipLength = 10.0f;
    [Range(0.0f, 5.0f)]
    [SerializeField] float webZipSpeed = 1.0f;
    private Vector3[] lineVerts = new Vector3[2];
    float gravityScale = 0.0f;
    [SerializeField] Vector2 jumpOffForce = new Vector2(10.0f, 0.0f);
    bool jumpedOff = false;
    float countdown = 1.0f;
    [Tooltip("Determines which layers count as floor for the CheckGrounded() raycast")]
    [SerializeField] private LayerMask wallClimbLayers;
    [Tooltip("Determines the length of the raycast shot from the base of the player to check grounded status")]
    [SerializeField] private float wallClimbRaycastLength = 10.0f;
    private CapsuleCollider2D col2d;

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance();
        input = InputManager.Instance();
        lineRenderer = GetComponent<LineRenderer>();
        col2d = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if(jumpedOff)
        {
            countdown -= Time.deltaTime;

            if(countdown <= 0.0f)
            {
                countdown = 1.0f;
                jumpedOff = false;
            }
        }
        if (player.CurrentPlayerState != Player.PlayerState.CLIMBING) return;

            var playerPos = player.transform.position;
        if (player.CurrentPlayerState == Player.PlayerState.CLIMBING)
        {
            if (!zipping)
            {
                WallCrawl();

                //EnableWebZip(playerPos);

                if (input.GetActionButton0Down())
                {
                    player.ChangePlayerState(Player.PlayerState.NOINPUT);
                    player.GetComponent<Rigidbody2D>().gravityScale = 3.6f;

                    Vector2 vel = Vector2.zero;

                    Vector2 raycastLeft = Vector2.zero;
                    Vector2 raycastRight = Vector2.zero;
                    raycastLeft.y = raycastRight.y = col2d.bounds.min.y + 0.1f;
                    raycastRight.x = col2d.bounds.max.x;
                    raycastLeft.x = col2d.bounds.min.x;
                    //If left or right raycast registers as grounded then the player is grounded
                    RaycastHit2D left = Physics2D.Raycast(raycastLeft, Vector2.left, wallClimbRaycastLength, wallClimbLayers);
                    RaycastHit2D right = Physics2D.Raycast(raycastRight, Vector2.right, wallClimbRaycastLength, wallClimbLayers);

                    //Debug.DrawRay(raycastLeft, Vector2.left * groundedRaycastLength, left ? Color.green : Color.red, 10);
                    //Debug.DrawRay(raycastRight, Vector2.right* groundedRaycastLength, right ? Color.green : Color.red, 10);
                    if (right)
                    {
                        Debug.Log(right.collider.gameObject.name);
                        player.PlayerMovement.PlayerMovementDirection = PlayerMovement.MovementDirection.LEFT;
                        vel.x = -jumpOffForce.x;
                        vel.y = jumpOffForce.y;
                    }
                    else if (left)
                    {
                        Debug.Log(left.collider.gameObject.name);
                        player.PlayerMovement.PlayerMovementDirection = PlayerMovement.MovementDirection.RIGHT;
                        vel.x = jumpOffForce.x;
                        vel.y = jumpOffForce.y;
                    }

                    player.PlayerMovement.SetPlayerVelocity(vel);
                    player.PlayerMovement.ApplyVelocityToRigidbody();
                    StartCoroutine(player.PlayerMovement.DelayRayCast());
                    //Debug.Log(player.GetComponent<Rigidbody2D>().velocity);
                    //Debug.Log(player.CurrentPlayerState);
                    //Debug.Log(player.PlayerMovement.PlayerMovementDirection);
                    //Debug.Log(player.GetComponent<Rigidbody2D>().gravityScale);
                    jumpedOff = true;
                }
            }
            else
            {
                //WebZip();
            }
        }
    }

    void WallCrawl()
    {
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(input.GetHorizontalInput() * 30.0f, -input.GetVerticalInput() * 30.0f);

        if (player.GetComponent<Rigidbody2D>().velocity.x < 0)
        {
            player.PlayerMovement.PlayerMovementDirection = PlayerMovement.MovementDirection.LEFT;
        }
        else if (player.GetComponent<Rigidbody2D>().velocity.x > 0)
        {
            player.PlayerMovement.PlayerMovementDirection = PlayerMovement.MovementDirection.RIGHT;
        }
    }

    void WebZip()
    {
        webZipTime += Time.deltaTime * webZipSpeed;

        lineVerts[0] = player.transform.position;
        lineVerts[1] = webZipPoint;

        lineRenderer.SetPositions(lineVerts);
        player.transform.position = Vector2.Lerp(player.transform.position, webZipPoint, webZipTime);

        if(webZipTime >= 1.0f)
        {
            zipping = false;
            building.enabled = true;
            webZipTime = 0.0f;
        }
    }

    bool EnableWebZip(Vector3 playerPos)
    {
        if (input.GetActionButton1Down())
        {
            Debug.Log("E");
            webZipPoint = playerPos;
            if (input.GetHorizontalInput() < 0)
            {
                EnableVerticalWebZip();
                webZipPoint.x = webZipPoint.x - webZipLength;
                zipping = true;
            }
            else if (input.GetHorizontalInput() > 0)
            {  
                EnableVerticalWebZip();
                webZipPoint.x = webZipPoint.x + webZipLength;
                zipping = true;
            }
            else if (EnableVerticalWebZip())
            {
                zipping = true;
            }

            if (!building.bounds.Contains(new Vector3(webZipPoint.x, webZipPoint.y, 1.0f)))
            {
                webZipPoint = playerPos;
                zipping = false;
                return false;
            }
            player.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
            building.enabled = false;
            return true;
        }
        return false;
    }

    private bool EnableVerticalWebZip()
    {
        if (input.GetVerticalInput() < 0)
        {
            webZipPoint.y = webZipPoint.y + webZipLength;
            return true;
        }
        else if (input.GetVerticalInput() > 0)
        {
            webZipPoint.y = webZipPoint.y - webZipLength;
            return true;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Building")
        {
            building = collision;
            if ((!jumpedOff && input.GetVerticalInput() < 0 && player.CurrentPlayerState != Player.PlayerState.CLIMBING))
            {
                if (player.CurrentPlayerState == Player.PlayerState.WEBBING)
                { 
                    player.WebManager.ToggleSwinging();
                }
                player.ChangePlayerState(Player.PlayerState.CLIMBING);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                gravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Building")
        {
            building = collision;
            if (input.GetVerticalInput() < 0 && player.CurrentPlayerState != Player.PlayerState.CLIMBING && !jumpedOff)
            {
                Debug.Log("Climb");
                if (player.CurrentPlayerState == Player.PlayerState.WEBBING)
                {
                    player.WebManager.ToggleSwinging();
                }
                player.ChangePlayerState(Player.PlayerState.CLIMBING);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                gravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Building" && player.CurrentPlayerState == Player.PlayerState.CLIMBING)
        {
            player.ChangePlayerState(Player.PlayerState.AIRBORNE);
            player.GetComponent<Rigidbody2D>().gravityScale = gravityScale;
            player.PlayerMovement.SetPlayerVelocity(new Vector2(0,0));
            player.PlayerMovement.ApplyVelocityToRigidbody();
            Debug.Log(player.GetComponent<Rigidbody2D>().velocity);
            Debug.Log(player.CurrentPlayerState);
        }
    }
}
