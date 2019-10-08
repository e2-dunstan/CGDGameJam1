using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMetaData
{

    public enum EnemyType
    {
        STANDARD_GRUNT = 1,
        SHOOTING_GRUNT = 2,
        CHARGING_GRUNT = 3,
        BOSS = 4
    }

    public string enemyId = "";

    public EnemyType enemyType = EnemyType.STANDARD_GRUNT;

    public Vector3 spawnPos = new Vector3();

    EnemyMovement enemyMovement = new RhinoMovement();

    // Start is called before the first frame update
    //void Start()
    //{
    //    gameObject.transform.position = spawnPos;

    //    enemyMovement = gameObject.GetComponent<EnemyMovement>();

    //}
}
