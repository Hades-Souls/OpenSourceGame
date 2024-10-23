using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }

    }

    public void LoadSceneWithPlayerPosition(string sceneName, string targetDoorName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName, targetDoorName));
    }

    private IEnumerator LoadSceneCoroutine(string sceneName, string targetDoorName)
    {
        // Find the SceneFadeManager and trigger the fade out
        SceneFadeManager fadeManager = FindObjectOfType<SceneFadeManager>();
        if (fadeManager != null)
        {
            yield return StartCoroutine(fadeManager.FadeOut());
        }

        // Load the scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Wait until the scene is fully loaded
        yield return new WaitForSeconds(0.1f);

        // Find the target door object in the new scene
        GameObject targetDoor = GameObject.Find(targetDoorName);
        if (targetDoor != null)
        {
            // Set the player's position to the target door's position
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                player.transform.position = targetDoor.transform.position;
            }
        }

        // Find the SceneFadeManager and trigger the fade in
        if (fadeManager != null)
        {
            yield return StartCoroutine(fadeManager.FadeIn());
        }
    }
}
