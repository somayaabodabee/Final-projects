using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundFXPooler : MonoBehaviour
{
    public static SoundFXPooler current;
    public GameObject pooledObject;
    public int pooledAmount = 10;
    public bool willGrow = true;
    public List<GameObject> pooledObjects;

    void Awake()
    {
        current = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < pooledAmount; i++)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = Instantiate(pooledObject);
            obj.SetActive(false);
            pooledObjects.Add(obj);
            return obj;
        }

        return null;
    }
}
