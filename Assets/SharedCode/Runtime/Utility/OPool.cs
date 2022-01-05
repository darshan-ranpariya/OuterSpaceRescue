using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OPool : MonoBehaviour
{
    [System.Serializable]
    public class ObjectDetails {
        [HideInInspector] public string key;
        public Transform prefab;
        public int count;
        [HideInInspector]
        public Transform holder;
    }

    public string key;
    public ObjectDetails[] objects = new ObjectDetails[] { };
    public Dictionary<string, ObjectDetails> objectsDictionary = new Dictionary<string, ObjectDetails>();
    public static Dictionary<string, OPool> poolsDictionary = new Dictionary<string, OPool>();
    Transform rootParent;
    public bool isReady;

    void OnEnable() {
        if (poolsDictionary.ContainsKey(key)) poolsDictionary.Remove(key);
        poolsDictionary.Add(key, this);
        objectsDictionary.Clear();

        rootParent = new GameObject("RuntimeObjects").transform;
        rootParent.SetParent(transform);
        rootParent.localPosition = Vector3.zero;
        rootParent.localScale = Vector3.one;

        for (int i = 0; i < objects.Length; i++)
        {
            if (!objectsDictionary.ContainsKey(objects[i].key))
            {
				objects[i].key = objects[i].prefab.gameObject.name;

                GameObject objParent = new GameObject(objects[i].key);
                objParent.transform.SetParent(rootParent);
                objParent.transform.localPosition = Vector3.zero;
                objParent.transform.localScale = Vector3.one;

                GameObject spawnableHolder = new GameObject("Spawnables");
                spawnableHolder.SetActive(false);
                spawnableHolder.transform.SetParent(objParent.transform);
                spawnableHolder.transform.localPosition = Vector3.zero;
                spawnableHolder.transform.localScale = Vector3.one;

                GameObject spawnedHolder = new GameObject("Spawned"); 
                spawnedHolder.transform.SetParent(objParent.transform);
                spawnedHolder.transform.localPosition = Vector3.zero;
                spawnedHolder.transform.localScale = Vector3.one;

                objects[i].holder = objParent.transform;

                objectsDictionary.Add(objects[i].key, objects[i]);
            } 
        }

        foreach (var obj in objectsDictionary)
        {
            for (int i = 0; i < obj.Value.count; i++)
            {
                Transform spawnedObject = InstantiateNewTransform(obj.Value.prefab);
                spawnedObject.SetParent(obj.Value.holder.GetChild(0));
                spawnedObject.gameObject.SetActive(false);
            }
        }

        isReady = true;
    }

    Transform InstantiateNewTransform(Transform refTransform) {
        Transform instantiatedTransform = Instantiate((Object)refTransform) as Transform;
        instantiatedTransform.position = new Vector3(1000,1000,1000);
        return instantiatedTransform;
    }

    public static Transform Spawn(string poolName, string objectName, Vector3 position, Transform parent = null, bool local = false, Vector3 eulers = default(Vector3)) {
        if (poolsDictionary.ContainsKey(poolName))
        {
            return poolsDictionary[poolName].Spawn(objectName, position, parent, local, eulers);
        }
        else { 
            Debug.Log("Pool " + poolName + " not found");
            return null;
        }
    }

    public Transform Spawn(string objectName, Vector3 position, Transform parent = null, bool local = false, Vector3 eulers = default(Vector3)) {
        if (!isReady) return null;
        if (!objectsDictionary.ContainsKey(objectName))
        {
            Debug.Log("Object " + objectName + " not found");
            return null;
        }

        Transform spawnedTransform = null;

        if (objectsDictionary[objectName].holder.GetChild(0).childCount > 0)
            spawnedTransform = objectsDictionary[objectName].holder.GetChild(0).GetChild(0);
        else
            spawnedTransform = InstantiateNewTransform(objectsDictionary[objectName].prefab);

        if (parent != null) spawnedTransform.SetParent(parent);
        else spawnedTransform.SetParent(objectsDictionary[objectName].holder.GetChild(1));

        if (local) spawnedTransform.localPosition = position;
        else spawnedTransform.position = position;

        if(eulers!=null) spawnedTransform.localEulerAngles = eulers;

        spawnedTransform.localScale = Vector3.one;

        int ly = spawnedTransform.gameObject.layer;
        spawnedTransform.gameObject.SetActive(true);
        spawnedTransform.gameObject.layer = ly; 

        return spawnedTransform;
    }

    public static void Despawn(string poolName, Transform _object)
    {
        if (poolsDictionary.ContainsKey(poolName))
        {
            poolsDictionary[poolName].Despawn(_object);
        }
        else Debug.Log("Pool " + poolName + " not found");
    }

    public void Despawn(Transform _object) {
        if (!isReady) return;

		string poolName = _object.gameObject.name.Split('(')[0];
        _object.SetParent(objectsDictionary[poolName].holder.GetChild(0));
        _object.gameObject.SetActive(false);
    }



    public void DespawnAll()
    {
        if (!isReady) return;

        foreach (var item in objectsDictionary)
        {
            string objName = item.Value.key;
            Transform objTransform = null;
            while (item.Value.holder.GetChild(1).childCount > 0)
            {
                objTransform = item.Value.holder.GetChild(1).GetChild(0);
                objTransform.SetParent(objectsDictionary[objName].holder.GetChild(0));
                objTransform.gameObject.SetActive(false);
            }
        } 
    }
}
