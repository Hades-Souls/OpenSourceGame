using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchingTest : MonoBehaviour
{
    public LayerMask wallLayer; // LayerMask to specify the wall layer
    public Vector2 boxSize = new Vector2(1f, 1f); // Size of the box
    public float boxAngle = 0f; // Angle of the box (rotation)
    public Color gizmoColor = Color.red; // Color of the gizmo to visualize the cast
    public Transform wallcheck; // Transform to specify the wall check position
    public bool isWalled;

    void Update()
    {
        // Check for colliders within the box
        isWalled = Physics2D.OverlapBox(wallcheck.position, boxSize, boxAngle, wallLayer);
        if (isWalled)
        {
            Debug.Log("Collider detected within the box.");
        }
    }

    void OnDrawGizmos()
    {
        if (wallcheck != null)
        {
            Gizmos.color = gizmoColor;
            // Draw the wireframe of the box
            Gizmos.DrawWireCube(wallcheck.position, boxSize);
        }
    }
}
