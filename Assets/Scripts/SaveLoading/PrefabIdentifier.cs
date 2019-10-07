using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabIdentifier
{
    public PrefabIdentifier(string _prefabId, string _prefabName, GameObject _obj)
    {
        prefabId = _prefabId;
        prefabName = _prefabName;
        prefabGameObject = _obj;
    }
    
    public string prefabId = System.Guid.NewGuid().ToString();
    public string prefabName = "";
    public GameObject prefabGameObject = null;
    
}
