using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactivePlatform : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectList = new List<GameObject>();
    [SerializeField] private float reactivationDelay = 1f; // The delay before reactivating the object

    private void Start()
    {
        // Start the coroutine to check for inactive objects
        StartCoroutine(CheckAndReactivateObjects());
    }

    private IEnumerator CheckAndReactivateObjects()
    {
        while (true)
        {
            foreach (GameObject obj in objectList)
            {
                if (!obj.activeInHierarchy)
                {
                    yield return new WaitForSeconds(reactivationDelay);
                    obj.SetActive(true);
                }
            }
            yield return null; // Wait for the next frame before checking again
        }
    }
}
