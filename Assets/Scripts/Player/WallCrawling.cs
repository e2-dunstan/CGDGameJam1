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

    // Start is called before the first frame update
    void Start()
    {
        player = Player.Instance();
        input = InputManager.Instance();
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        var playerPos = player.transform.position;
        if (player.CurrentPlayerState == Player.PlayerState.CLIMBING)
        {
            if (!zipping)
            {
                WallCrawl();

                EnableWebZip(playerPos);

                if (input.GetActionButton0Down())
                {
                    player.ChangePlayerState(Player.PlayerState.AIRBORNE);
                    player.GetComponent<Rigidbody2D>().gravityScale = 3.6f;
                }

            }
            else
            {
                WebZip();
            }
        }
    }

    void WallCrawl()
    {
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(input.GetHorizontalInput() * 30.0f, -input.GetVerticalInput() * 30.0f);

        if (input.GetHorizontalInput() < 0)
        {
            player.PlayerMovement.PlayerMovementDirection = PlayerMovement.MovementDirection.LEFT;
        }
        else if (input.GetHorizontalInput() > 0)
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
            webZipTime = 0.0f;
            //Debug.Log(zipping);
        }
    }

    bool EnableWebZip(Vector3 playerPos)
    {
       // Debug.Log("func");
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
                Debug.Log("hit");   
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
                Debug.Log("outside");
                webZipPoint = playerPos;
                zipping = false;
                return false;
            }
            Debug.Log(zipping);
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
            if (input.GetVerticalInput() < 0 && player.CurrentPlayerState != Player.PlayerState.CLIMBING)
            {
                player.ChangePlayerState(Player.PlayerState.CLIMBING);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Building")
        {
            building = collision;
            if (input.GetVerticalInput() < 0 && player.CurrentPlayerState != Player.PlayerState.CLIMBING)
            {
                player.ChangePlayerState(Player.PlayerState.CLIMBING);
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                player.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Building")
        {
            player.ChangePlayerState(Player.PlayerState.AIRBORNE);
            player.GetComponent<Rigidbody2D>().gravityScale = 3.6f;
        }
    }
}
