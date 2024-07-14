using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public int initialPoolSize = 10; // No need for ghostPrefab as we use the current object

    private Queue<GameObject> ghostPool = new Queue<GameObject>();

    void Start()
    {
        // Initialize the pool with inactive copies of the current object
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject ghost = Instantiate(gameObject); // Instantiate the object this script is attached to
            ghost.SetActive(false);
            ghostPool.Enqueue(ghost);
        }
    }

    public GameObject GetGhost()
    {
        if (ghostPool.Count > 0)
        {
            GameObject ghost = ghostPool.Dequeue();
            ghost.SetActive(true);
            return ghost;
        }
        else
        {
            // Optionally, you can instantiate more objects if needed
            GameObject ghost = Instantiate(gameObject); // Instantiate the object this script is attached to
            return ghost;
        }
    }

    public void ReturnGhost(GameObject ghost)
    {
        ghost.SetActive(false);
        ghostPool.Enqueue(ghost);
    }
}
