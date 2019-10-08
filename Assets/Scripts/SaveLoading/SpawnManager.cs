using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

 public class SpawnManager : MonoBehaviour
{
    public List<GameObject> Enemies;

    public Camera cam;

    public void Start()
    {
        foreach(var enemy in Enemies)
        {
            enemy.SetActive(false);
        }
    }

    public void Update()
    {
        Vector3 lowerLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, 0));

        //lowerRight.x will be the X coordinate of the rightmost visible pixel
        Vector3 lowerRight = cam.ViewportToWorldPoint(new Vector3(1, 0, 0));

        //Debug.Log("Left most = " + lowerLeft.x + "Right most = " + lowerRight.x);

        CheckIfEnemyIsOnScreen(lowerLeft.x, lowerRight.x);
    }

    private void CheckIfEnemyIsOnScreen(float xLeftPos, float xRightPos)
    {
        List<GameObject> enemiesOnScreen = Enemies.Where<GameObject>(x => x.transform.position.x > xLeftPos && x.transform.position.x < xRightPos).ToList();

        foreach(var enemy in enemiesOnScreen)
        {
            if(!enemy.activeSelf && enemy.GetComponent<Enemy>().enemyState != Enemy.EnemyState.DYING)
            {
                enemy.SetActive(true);
            }
        }
    }

}