using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class SaveLoadManager : MonoBehaviour
{

    public static SaveLoadManager saveLoadManager;
    private string jsonSavePath;
    public GameData gameData;
    public SpawnManager spawnManager;
    public PrefabIdentifiers prefabs;

    public List<PrefabIdentifier> instantiateableObjects = new List<PrefabIdentifier>();

    void Awake()
    {
        if (saveLoadManager == null)
        {
            saveLoadManager = this;
        }
        else if (saveLoadManager != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        gameData = new GameData();

        LoadData();
    }

    void Start()
    {
        jsonSavePath = Application.dataPath + "/SaveFile.json";

    }

    //[ContextMenu("Do Something")]
    //void SaveNotDuringRuntime()
    //{
    //    Debug.Log("Saving Enemies");

    //    List<GameObject> enemiesList = GameObject.FindGameObjectsWithTag("Enemy").ToList<GameObject>();

    //    SaveSceneSnapshot(enemiesList);

    //}

    //private void SaveSceneSnapshot(List<GameObject> _enemiesList)
    //{
    //    foreach(var enemy in _enemiesList)
    //    {
    //        gameData.enemyMetaDataList.Add(enemy.GetComponent<EnemyMetaData>());
    //    }

    //    string jsonData = JsonUtility.ToJson(gameData, true);
    //    File.WriteAllText(jsonSavePath, jsonData);
    //}

    private void SaveData()
    {
        foreach (var enemy in spawnManager.Enemies)
        {
            gameData.enemyMetaDataList.Add(enemy.GetComponent<EnemyMetaData>());
        }

        string jsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(jsonSavePath, jsonData);

    }

    public void ForceSave()
    {
        print("Forcing save of current gamedata");
        string jsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(jsonSavePath, jsonData);
    }

    public void LoadData()
    {
        gameData = JsonUtility.FromJson<GameData>(File.ReadAllText(Application.dataPath + "/SaveFile.json"));

        foreach(var enemy in gameData.enemyMetaDataList)
        {
            switch(enemy.enemyType)
            {
                case EnemyMetaData.EnemyType.STANDARD_GRUNT:
                    spawnManager.Enemies.Add(prefabs.findPrefabFromGUID(enemy.enemyId));
                    break;
                case EnemyMetaData.EnemyType.SHOOTING_GRUNT:
                    break;

                case EnemyMetaData.EnemyType.CHARGING_GRUNT:

                    break;

                case EnemyMetaData.EnemyType.BOSS:

                    break;
                default:

                    break;
            }
        }
    }

    public List<LeaderboardEntry> GetLeaderboardEntries()
    {
        return gameData.leaderboardList;
    }

}
