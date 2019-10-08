using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEntranceTrigger : MonoBehaviour
{
    public bool hasBeenTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            hasBeenTriggered = true;
        }
    }
}
