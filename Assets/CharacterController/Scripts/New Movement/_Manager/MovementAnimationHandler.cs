

using UnityEngine;

public class MovementAnimationHandler : MonoBehaviour
{

    public Animator Anim { get; private set; }
    public Rigidbody RB { get; private set; }

    // Cache hashes for maximum performance
    public static readonly int SpeedHash = Animator.StringToHash("MovementSpeed");
    public static readonly int JumpTriggerHash = Animator.StringToHash("JumpTrigger");
    public static readonly int IsGroundedHash = Animator.StringToHash("isGrounded");

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
    }
    
    public void HandleMovement_Anim()
    {
        float currentSpeed = RB.linearVelocity.magnitude; 

        // Update the blend tree seamlessly
        Anim.SetFloat(SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    }
}