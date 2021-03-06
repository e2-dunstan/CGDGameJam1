﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private enum MovingTo
    {
        UP = 1,
        DOWN = 0
    }

    public int scoreAmount = 300;

    public int wonderAmount = 5;

    private bool isMoving = false;
    private Vector2 movingToTransform;

    public AudioClip collectableSound;
    MovingTo movingTo = MovingTo.UP;

    public float movementSpeed = 3.0f;
    public void Update()
    {
        if(isMoving == false)
        {
            if (movingTo == MovingTo.UP)
            {
                movingTo = MovingTo.DOWN;
                movingToTransform = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - (wonderAmount / 2));
                isMoving = true;
            }
            else
            {
                movingTo = MovingTo.UP;
                movingToTransform = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (wonderAmount / 2));
                isMoving = true;

            }
        }

        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, movingToTransform, movementSpeed * Time.deltaTime);

        if((Vector2)gameObject.transform.position == movingToTransform)
        {
            isMoving = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {

            AudioManager.Instance.PlaySpecificClip(collectableSound, gameObject.transform);
            ScoreManager.Instance.AddScore(scoreAmount);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine("DestroyAfterTime", 0.4f);
            
        }
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
