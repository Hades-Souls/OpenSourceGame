using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed = 2f; // Speed of the platform
    private Transform platform;
    private Transform pointA;
    private Transform pointB;
    private Vector3 targetPosition;


    private void Start()
    {
        // Find the child objects
        platform = transform.Find("Platform");
        pointA = transform.Find("A");
        pointB = transform.Find("B");

        // Check if all required child objects were found
        if (platform == null || pointA == null || pointB == null)
        {
            Debug.LogError("Platform, Point A, or Point B not found as children of the object. Please make sure they exist and are named correctly.");
            return;
        }

        // Set the initial target position to point B
        targetPosition = pointB.position;
    }

    private void Update()
    {
        // Move the platform towards the target position
        platform.position = Vector3.MoveTowards(platform.position, targetPosition, speed * Time.deltaTime);

        // If the platform reaches the target position, switch to the other point
        if (platform.position == targetPosition)
        {
            targetPosition = targetPosition == pointA.position ? pointB.position : pointA.position;
        }
    }
}
