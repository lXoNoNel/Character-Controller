using UnityEngine;

public class PlayerSlopeHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheckObj;

    [Header("Detection Settings")]
    public float rayLength = 0.4f;
    public float heightOffset = 0.2f;
    public float stepLookAhead = 0.3f; // How far in front of the capsule we check
    public LayerMask groundLayer;

    [Header("Slope Limits")]
    public float maxSlopeAngle = 45f;

    private RaycastHit groundHit;
    private bool isGrounded;

    public bool IsGrounded => isGrounded;
    public Vector3 SlopeNormal => groundHit.normal;

    private void FixedUpdate()
    {
        // Simple downward ray for grounding and clean surface angles
        Vector3 origin = groundCheckObj.position + (Vector3.up * heightOffset);
        isGrounded = Physics.Raycast(origin, Vector3.down, out groundHit, rayLength + heightOffset, groundLayer);
    }

    /// <summary>
    /// Checks if a steep slope or wall blocks the intended raw movement vector.
    /// </summary>
    public bool IsSlopeTooSteep(Vector3 rawMoveDirection)
    {
        if (rawMoveDirection.magnitude < 0.01f) return false;

        // Origin point at calf/shin level
        Vector3 origin = groundCheckObj.position + (Vector3.up * heightOffset);

        // Cast forward in the direction the player wants to walk
        if (Physics.Raycast(origin, rawMoveDirection.normalized, out RaycastHit wallHit, stepLookAhead, groundLayer))
        {
            float wallAngle = Vector3.Angle(Vector3.up, wallHit.normal);
            
            // If the angle directly in front of us is too steep, flag it immediately
            if (wallAngle > maxSlopeAngle)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 rawMoveDirection)
    {
        if (!isGrounded) return rawMoveDirection;
        return Vector3.ProjectOnPlane(rawMoveDirection, groundHit.normal).normalized;
    }

    private void OnDrawGizmos()
    {
        if (groundCheckObj == null) return;
        
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 origin = groundCheckObj.position + (Vector3.up * heightOffset);
        Gizmos.DrawLine(origin, origin + (Vector3.down * (rayLength + heightOffset)));
    }
}