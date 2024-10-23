using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectForLedge : MonoBehaviour
{
    public List<Collider2D> detectedcolliders = new List<Collider2D>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ledge"))
        {
            detectedcolliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ledge"))
        {
            detectedcolliders.Remove(collision);
        }
    }

    public bool AreAnyCollidersDetected()
    {
        return detectedcolliders.Count > 0;
    }
}
