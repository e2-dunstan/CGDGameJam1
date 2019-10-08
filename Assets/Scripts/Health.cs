using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health = 10;

    public void TakeDamage(int _damage)
    {
        health -= _damage;
    }
}
