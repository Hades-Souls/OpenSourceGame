using UnityEngine;

public class GhostTrail : MonoBehaviour
{
    public GameObject ghostPrefab;  // Prefab for the ghost trail
    public float ghostSpawnInterval = 0.1f;  // Interval between each ghost spawn
    public float ghostLifetime = 0.5f;  // Lifetime of each ghost
    public Color ghostColor = new Color(1f, 1f, 1f, 0.5f);  // Color of the ghost (white with 50% transparency)
    public bool canCreateGhosts = true;  // Control when ghosts can be created

    private float timer;

    private void Awake()
    {
        canCreateGhosts=false;
    }
    void Update()
    {
        // Check if ghosts can be created
        if (!canCreateGhosts)
            return;

        // Update the timer
        timer += Time.deltaTime;

        // Check if it's time to spawn a new ghost
        if (timer >= ghostSpawnInterval)
        {
            // Reset the timer
            timer = 0f;

            // Create a new ghost
            GameObject currentGhost = Instantiate(ghostPrefab, transform.position, transform.rotation);

            // Set the ghost's sprite to match the player's sprite
            SpriteRenderer ghostSpriteRenderer = currentGhost.GetComponent<SpriteRenderer>();
            SpriteRenderer playerSpriteRenderer = GetComponent<SpriteRenderer>();
            ghostSpriteRenderer.sprite = playerSpriteRenderer.sprite;

            // Set the ghost color based on the player's color with the desired transparency
            Color playerColor = playerSpriteRenderer.color;
            Color finalGhostColor = new Color(playerColor.r, playerColor.g, playerColor.b, ghostColor.a);
            ghostSpriteRenderer.color = finalGhostColor;

            // Set the ghost's scale to match the player's scale
            currentGhost.transform.localScale = transform.localScale;

            // Ensure the ghost matches the player's rotation
            currentGhost.transform.rotation = transform.rotation;

            // Destroy the ghost after its lifetime
            Destroy(currentGhost, ghostLifetime);
        }
    }
}
