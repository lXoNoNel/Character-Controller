using System;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    [Header("References & Settings")]
    public Transform groundCheckObj;
    public float rayCastHeightOffset = 0.5f;
    public float rayCastMaxDistance = 1.0f;
    public LayerMask groundLayer;

    [SerializeField]
    private bool drawGizmos = true;

    public bool isPlayerObjGrounded()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = groundCheckObj.position;
        rayCastOrigin.y = rayCastOrigin.y + rayCastHeightOffset;

        // Firing straight down
        return Physics.SphereCast(rayCastOrigin, 0.2f, -Vector3.up, out hit, rayCastMaxDistance, groundLayer);
    }

    // ==========================================
    // VISUAL DEBUGGER FOR YOUR METHOD
    // ==========================================
    private void OnDrawGizmos()
    {
        // 1. Replicate your exact origin logic
        if(!drawGizmos) return;
        if (groundCheckObj == null) return;
        
        Vector3 rayCastOrigin = groundCheckObj.position;
        rayCastOrigin.y += rayCastHeightOffset;

        float radius = 0.2f; // Your exact radius
        Vector3 direction = -Vector3.up; // Your exact direction (Down)

        // 2. Perform a duplication of your cast to find the visual stopping point
        RaycastHit hit;
        bool didHit = Physics.SphereCast(rayCastOrigin, radius, direction, out hit, rayCastMaxDistance, groundLayer);
        float currentDistance = didHit ? hit.distance : rayCastMaxDistance;

        // Color coding: Red if touching the ground, Green if airborne
        Gizmos.color = didHit ? Color.red : Color.green;

        // 3. Draw the starting sphere (at the offset origin height)
        Gizmos.DrawWireSphere(rayCastOrigin, radius);

        // 4. Draw the ending sphere (where it stops or hits the ground)
        Vector3 endPosition = rayCastOrigin + (direction * currentDistance);
        Gizmos.DrawWireSphere(endPosition, radius);

        // 5. Connect the two spheres with side lines to form a vertical tube/capsule
        // Because we are casting straight down, the outer edges are along the forward and right axes
        Vector3 forwardOffset = Vector3.forward * radius;
        Vector3 rightOffset = Vector3.right * radius;

        Gizmos.DrawLine(rayCastOrigin + forwardOffset, endPosition + forwardOffset);
        Gizmos.DrawLine(rayCastOrigin - forwardOffset, endPosition - forwardOffset);
        Gizmos.DrawLine(rayCastOrigin + rightOffset, endPosition + rightOffset);
        Gizmos.DrawLine(rayCastOrigin - rightOffset, endPosition - rightOffset);

        // 6. Draw a yellow point exactly where the sphere makes contact with the ground layer
        if (didHit)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawSphere(hit.point, 0.05f);
        }
    }
}