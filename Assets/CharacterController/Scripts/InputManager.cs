using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    public float moveAmount;
    public float verticalInput;
    public float horizontalInput;

    public bool b_Input;
    public bool jump_input;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void OnEnable() {
        if(playerControls == null) {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.B.performed += i => b_Input = true;
            playerControls.PlayerActions.B.canceled += i => b_Input = false;
            playerControls.PlayerActions.Jump.performed += i => jump_input = true;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleSprintingInput();
        HandleJumpInput();
    }

    private void HandleMovementInput() {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.isSprinting);
    }

    private void HandleSprintingInput()
    {
        if (b_Input && moveAmount > 0.5f)
        {
            playerLocomotion.isSprinting = true;
        }
        else
        {
            playerLocomotion.isSprinting = false;
        }
    }

    private void HandleJumpInput()
    {
        if(jump_input)
        {
            jump_input = false;
            playerLocomotion.HandleJumping();
        }
    }
}
