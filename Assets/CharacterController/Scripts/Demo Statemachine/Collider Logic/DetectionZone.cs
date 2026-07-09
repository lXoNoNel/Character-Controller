using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    // Triggered when another collider enters this object's space
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            Debug.Log($"{other.name} has entered the area!");
        }
    }

    // Triggered every frame another collider remains inside this object's space
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{other.name} is still inside!");
        }
    }

    // Triggered when another collider leaves this object's space
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"{other.name} has left the area.");
        }
    }
}