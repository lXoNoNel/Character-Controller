using UnityEngine;

public class PlayerManager : MonoBehaviour
{
   InputManager inputManager;
   PlayerLocomotion playerLocomotion;
   Animator animator;

   public bool isInteracting;

   void Awake()
   {
       inputManager = GetComponent<InputManager>();
       //cameraManager = FindObjectOfType<CameraManager>();
       playerLocomotion = GetComponent<PlayerLocomotion>();
       animator = GetComponent<Animator>();
   }

   void Update()
   {
       inputManager.HandleAllInputs();
   }

   void FixedUpdate()
   {
       playerLocomotion.HandleAllMovement();
   }

    void LateUpdate()
    {
        isInteracting = animator.GetBool("isInteracting");

        playerLocomotion.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerLocomotion.isGrounded);
    }
}
