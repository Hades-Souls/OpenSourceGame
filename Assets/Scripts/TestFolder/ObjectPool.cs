using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;  // The prefab to pool
    public int poolSize = 10;  // Initial size of the pool

    private Queue<GameObject> poolQueue;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);  // Prevent this object from being destroyed on scene load

        poolQueue = new Queue<GameObject>();

        // Initialize the pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    // Get an object from the pool
    public GameObject GetFromPool()
    {
        if (poolQueue.Count > 0)
        {
            GameObject obj = poolQueue.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(true);
            return obj;
        }
    }

    // Return an object to the pool
    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
    }
}
