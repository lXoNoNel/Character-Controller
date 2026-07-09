using UnityEngine;

public class MovingObjectDetector : MonoBehaviour
{
    [HideInInspector]
    public enum MovementState { NotColliding, Colliding }

    [SerializeField]
    private string itemsLayerTag;

    [Header("Current Status")]
    public MovementState currentState = MovementState.NotColliding;

    private GameObject foundObject;

    // Triggered when THIS moving object enters a zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the zone we just entered has the tag "WaterZone"
        if (other.gameObject.CompareTag(itemsLayerTag))
        {
            currentState = MovementState.Colliding;
            foundObject = other.gameObject;
        }
    }

    // Triggered while THIS moving object stays inside the zone
    //private void OnTriggerStay(Collider other) {}

    // Triggered when THIS moving object leaves the zone
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(itemsLayerTag))
        {
            currentState = MovementState.NotColliding;
            foundObject = null;
        }
    }


    public bool Colliding()
    {
        return this.currentState == MovementState.Colliding;
    }

    public GameObject GetCollidingObject()
    {
        if (this.currentState == MovementState.Colliding && this.foundObject != null) 
        {
            return this.foundObject;
        }
        return null;
    }
}