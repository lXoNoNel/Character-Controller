using UnityEngine;

public class PlayerSlopeCheck : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheckObj;

    [Header("Detection Settings")]
    public float castRadius = 0.2f;
    public float castLength = 0.5f;
    public float heightOffset = 0.1f;
    public LayerMask groundLayer;

    [Header("Slope Limits")]
    public float maxSlopeAngle = 45f;

    private RaycastHit slopeHit;
    private bool isGrounded;
    private float currentSlopeAngle;

    public bool IsGrounded => isGrounded;
    public Vector3 SlopeNormal => slopeHit.normal;

    private void FixedUpdate()
    {
        Vector3 origin = groundCheckObj.position + (Vector3.up * heightOffset);
        
        // Shoot a spherecast down to detect the ground and its exact angle
        isGrounded = Physics.SphereCast(origin, castRadius, Vector3.down, out slopeHit, castLength, groundLayer);

        if (isGrounded)
        {
            // Calculate how steep the slope is
            currentSlopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            
            // If it's a wall or too steep, treat it as not grounded
            if (currentSlopeAngle > maxSlopeAngle)
            {
                isGrounded = false;
            }
        }
        else
        {
            currentSlopeAngle = 0f;
        }
    }

    /// <summary>
    /// Projects a flat, raw input direction perfectly along the slope surface.
    /// </summary>
    public Vector3 GetSlopeMoveDirection(Vector3 rawMoveDirection)
    {
        if (!isGrounded) return rawMoveDirection;
        
        // Projects your movement vector seamlessly onto the angle of the hill face
        return Vector3.ProjectOnPlane(rawMoveDirection, slopeHit.normal).normalized;
    }

    private void OnDrawGizmos()
    {
        if (groundCheckObj == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Vector3 origin = groundCheckObj.position + (Vector3.up * heightOffset);
        Vector3 end = origin + (Vector3.down * castLength);
        
        Gizmos.DrawWireSphere(origin, castRadius);
        Gizmos.DrawWireSphere(end, castRadius);
        Gizmos.DrawLine(origin, end);
    }
}