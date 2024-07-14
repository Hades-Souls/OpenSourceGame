using UnityEngine;
using System.Collections;

public class GhostManager : MonoBehaviour
{
    public ObjectPoolManager poolManager;
    public float ghostdelay = 1f;
    private float ghostdelayseconds;
    public bool makeghost;
    void Start()
    {
        ghostdelayseconds = ghostdelay;
    }

    void Update()
    {
        if (makeghost)
        {
            if (ghostdelayseconds > 0)
            {
                ghostdelayseconds -= Time.deltaTime;
            }
            else
            {
                GameObject currentghost = poolManager.GetGhost();
                if (currentghost != null)
                {
                    // Set the position, rotation, and scale for the ghost
                    currentghost.transform.position = transform.position;
                    currentghost.transform.rotation = transform.rotation;
                    currentghost.transform.localScale = transform.localScale;

                    Sprite currentsprite = GetComponent<SpriteRenderer>().sprite;
                    currentghost.GetComponent<SpriteRenderer>().sprite = currentsprite;

                    // Return the ghost to the pool after 1 second
                    StartCoroutine(ReturnGhostToPool(currentghost, 1f));

                    ghostdelayseconds = ghostdelay;
                }
            }
        }
    }

    private IEnumerator ReturnGhostToPool(GameObject ghost, float delay)
    {
        yield return new WaitForSeconds(delay);
        poolManager.ReturnGhost(ghost);
    }
}
