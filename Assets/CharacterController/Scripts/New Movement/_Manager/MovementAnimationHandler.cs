

using UnityEngine;

public class MovementAnimationHandler : MonoBehaviour
{

    public Animator Anim { get; private set; }
    public Rigidbody RB { get; private set; }
    public PlayerGroundCheck playerGroundCheck { get; private set; }

    public bool isAnimatingAirborne;

    // Cache hashes for maximum performance
    public static readonly int ANIM_SpeedHash = Animator.StringToHash("MovementSpeed");
    public static readonly int ANIM_inAirTimer = Animator.StringToHash("inAirTimer");
    public static readonly int ANIM_Trigger_Fall = Animator.StringToHash("Fall");
    public static readonly int ANIM_Trigger_Land = Animator.StringToHash("Land");
    public static readonly int ANIM_Bool_ActiveLanding = Animator.StringToHash("ActiveLanding");

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
    }
    
    public void HandleMovement_Anim()
    {
        float currentSpeed = RB.linearVelocity.magnitude; 

        // Update the blend tree seamlessly
        Anim.SetFloat(ANIM_SpeedHash, currentSpeed, 0.1f, Time.deltaTime);
    }

//? ====================================================================
//? ==================== FALLING ANIMATIONS =================================
//? ====================================================================


    public void airborneEnter_Anim()
    {
        // Anim.SetLayerWeight()
        if(shouldAnimate())
        {
            isAnimatingAirborne = true;  
        } else
        {
            isAnimatingAirborne = false;  
        }
    }

    public void airborneExit_Anim()
    {
        Anim.SetFloat(ANIM_inAirTimer, 0, 0, 1);

        isAnimatingAirborne = false;
    }

//*===========================================
    public void activeFallingHandling_Anim(float inAirTimer)
    {
        if(!isAnimatingAirborne) return;

        Anim.SetFloat(ANIM_inAirTimer, inAirTimer, 0.1f, Time.deltaTime);
    }

//*===========================================

    public void fallingEnter_Anim()
    {
        if(!isAnimatingAirborne) return;


        Anim.SetTrigger(ANIM_Trigger_Fall);
    }

    public void fallingExit_Anim()
    {
        if(!isAnimatingAirborne) return;
    }

    public void landingEnter_Anim()
    {
        if(!isAnimatingAirborne) return;

        Anim.SetTrigger(ANIM_Trigger_Land);

        Anim.SetBool(ANIM_Bool_ActiveLanding, true);
    }

    public void landingExit_Anim()
    {
        if(!isAnimatingAirborne) return;
        
    }

    public bool shouldAnimate()
    {
        return !playerGroundCheck.IsGrounded_ForAnim;
    }

//?=====================================================================
//!=======================TRANSITION BOOLS=================================
//?=====================================================================

    public bool isInLandingAnimation()
    {
        return Anim.GetBool(ANIM_Bool_ActiveLanding);
    }
}