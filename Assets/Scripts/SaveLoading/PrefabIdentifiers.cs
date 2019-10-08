using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PrefabIdentifiers : MonoBehaviour
{
    
	public List<PrefabIdentifier> Prefabs;
	//new List<PrefabIdentifier>{
	//  new PrefabIdentifier("7a9227a4-c4e9-4c80-96b0-b8199ec54f30", "Standard Grunt", null),
	//  new PrefabIdentifier("cbce2d30 - 0c24-44ba-893b-ab023581a5fc", "Shooting Grunt", null),
	//  new PrefabIdentifier("297612b0-0bbd-4e57-ac8f-bd6c3cd205f7", "Charging Grunt", null),
	//  new PrefabIdentifier("5c6157e9-3bee-4e3c-9c0c-86df6fd883bc", "Vulture Grunt", null)};

    public GameObject findPrefabFromGUID(string _guid)
    {
        GameObject foundObject = Prefabs.Where(x => x.prefabId == _guid).FirstOrDefault().prefabGameObject;

        return foundObject;
    }

}
