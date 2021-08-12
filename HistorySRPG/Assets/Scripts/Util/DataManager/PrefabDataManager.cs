using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/*
 * 用来管理Prefab的读取
 */
public static class PrefabDataManager
{
    public static string prefabsPath = "Prefabs/";

    public static Dictionary<string, UnityEngine.Object> objectDictionary;

    //    public static int loadingCount;
    //    public static int loadingTotal;

    static PrefabDataManager()
    {
        objectDictionary = new Dictionary<string, UnityEngine.Object>();
    }

    public static UnityEngine.Object getPrefab(String filename)
    {
        if (objectDictionary.ContainsKey(filename))
        {
            return objectDictionary[filename];
        }
        else
        {
            //Debug.Log (prefabsPath + filename);
            UnityEngine.Object ob = Resources.Load(prefabsPath + filename);
            objectDictionary.Add(filename, ob);
            return ob;
        }
    }

    public static void PreloadPrefab(String filename)
    {
        if (objectDictionary.ContainsKey(filename))
        {
            return;
        }
        UnityEngine.Object ob = Resources.Load(prefabsPath + filename, typeof(GameObject));
        if (ob != null)
        {
            objectDictionary.Add(filename, ob);
        }
    }

    public static void Clear()
    {
        //		foreach (var ob in objectDictionary) {
        //			if (ob.Value != null) {
        //				Resources.UnloadAsset (ob.Value);
        //			}
        //		}
        objectDictionary.Clear();
    }
}

