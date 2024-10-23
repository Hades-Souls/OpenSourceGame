using UnityEngine;

public class Door : MonoBehaviour
{
    public string sceneName;
    public string targetDoorName; // Name of the target door in the next scene

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Store the target door name for the next scene
            SpawnData.SceneName = sceneName;
            SpawnData.TargetDoorName = targetDoorName;

            // Find the SceneTransitionManager and trigger the transition
            SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
            if (transitionManager != null)
            {
                transitionManager.LoadSceneWithPlayerPosition(sceneName, targetDoorName);
            }
        }
    }
}
