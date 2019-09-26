using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Inputs _inputs;
    [SerializeField] private float runningSpeed;
    [SerializeField] private bool isjumping;
    // Start is called before the first frame update
    private void Awake()
    {
        _inputs = GameObject.FindGameObjectWithTag("GameController").GetComponent<Inputs>();
    }
    void Start()
    {
        runningSpeed = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!isjumping)
        {
            if (_inputs.bothAxes == new Vector2(1, 0))
            {
                transform.Translate(new Vector2((runningSpeed / 300), 0));
            }
            else if (_inputs.bothAxes == new Vector2(-1, 0))
            {
                transform.Translate(new Vector2(-(runningSpeed / 300), 0));
            }

            if (_inputs.action)
            {
                if (_inputs.bothAxes == new Vector2(1, 0))
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(500, 1500), ForceMode2D.Impulse);
                }
                else if (_inputs.bothAxes == new Vector2(-1, 0))
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(-500, 1500), ForceMode2D.Impulse);
                }
                else
                {
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1500), ForceMode2D.Impulse);
                }
                isjumping = true;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isjumping = false;
        }
    }
}
