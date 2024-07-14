using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class TestScripts2 : MonoBehaviour
{
    public testScripts3<SpriteRenderer> ghostPool; // Reference to the Object Pool
    public float ghostInterval = 0.1f; // Interval between each ghost instantiation
    public float ghostLifetime = 0.5f; // Lifetime of each ghost

    private float ghostTimer;

    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            ghostTimer -= Time.deltaTime;
            if (ghostTimer <= 0)
            {
                CreateGhost();
                ghostTimer = ghostInterval;
            }
        }
    }

    void CreateGhost()
    {
        SpriteRenderer ghost = ghostPool.GetFromPool();
        ghost.transform.position = transform.position;
        ghost.transform.rotation = transform.rotation;

        SpriteRenderer playerRenderer = GetComponent<SpriteRenderer>();

        // Copy the player's sprite and other properties to the ghost
        ghost.sprite = playerRenderer.sprite;
        ghost.flipX = playerRenderer.flipX;

        StartCoroutine(ReturnGhostToPool(ghost, ghostLifetime));
    }

    IEnumerator ReturnGhostToPool(SpriteRenderer ghost, float delay)
    {
        yield return new WaitForSeconds(delay);
        ghostPool.ReturnToPool(ghost);
    }
}
