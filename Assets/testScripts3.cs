using System.Collections.Generic;
using UnityEngine;

public class testScripts3<T> : MonoBehaviour where T : Component
{
    public T prefab;
    public int poolSize = 10;

    private Queue<T> pool = new Queue<T>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            T obj = Instantiate(prefab);
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public T GetFromPool()
    {
        if (pool.Count > 0)
        {
            T obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            T obj = Instantiate(prefab);
            return obj;
        }
    }

    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
