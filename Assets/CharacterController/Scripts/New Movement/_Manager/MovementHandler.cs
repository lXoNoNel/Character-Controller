using Unity.Collections;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    InputHandler inputHandler;

    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody playerRigidbody;

    public Transform groundCheckObj;

    [Header("Falling")]
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;

    [Header("Slope")]
    public float maxStepDownHeight;
    public float maxSlopeSpeedLimit = 8f;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;
    public float pressureWhileMoving;


    [Header("Movement Speeds")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 6f;
    public float sprintingSpeed = 8f;
    public float rotationSpeed = 15f;

    [Header("Jump Speeds")]
    public float jumpHeight = 3f;
    public float gravityIntensity = -15f;


//? ==========================================================================


    [Header("STATE DEBUG VALUES")]

    // 1. This holds the actual data (hidden so it doesn't clutter the UI)
    
    [ReadOnly]
    [SerializeField]
    private string _currentPrimaryStateString = "NOT DEFINED"; 

    PlayerStateDebugger playerStateDebugger;
    PlayerGroundCheck playerGroundCheck;
    PlayerSlopeCheck playerSlopeCheck;

//? ==========================================================================

    private MovementStateMachine movementStateMachine;


    public PlayerData data;

    private MovementStatemachineHandler movementStatemachineHandler = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerStateDebugger = GetComponent<PlayerStateDebugger>();
        playerGroundCheck = GetComponent<PlayerGroundCheck>();
        playerSlopeCheck = GetComponent<PlayerSlopeCheck>();

        cameraObject = Camera.main.transform;

        if(movementStateMachine == null)
        {
            movementStateMachine = new MovementStateMachine();
        }

        if(movementStatemachineHandler == null)
        {
            movementStatemachineHandler = new MovementStatemachineHandler(HandleMovement, 
            activeFallingHandling, HandleRotation, this, 
            playerGroundCheck,
            movementStateMachine, data);
        }
    }

    void Start()
    {
        movementStatemachineHandler.locomotionStart();
        
    }

    void Update()
    {
        _currentPrimaryStateString = playerStateDebugger.GetCurrentStateAsString();

        // Debug.Log(isPlayerObjGrounded());

        movementStatemachineHandler.finalUpdate();
    }

    void FixedUpdate()
    {
        //HandleRotation();

        movementStatemachineHandler.finalFixedUpdate();
    }

    // public void HandleSlopes()
    // {
        
    // }

    // public void HandleSlopes()
    // {
    //     if (moveDirection.magnitude > 0.1f)
    //     {
    //         // Convert flat horizontal movement to perfectly match the hill's slope angle
    //         targetMoveDirection = playerSlopeCheck.GetSlopeMoveDirection(moveDirection);

    //         // Calculate target velocity (Notice it now includes the correct Y tilt inherently)
    //         targetVelocity = targetMoveDirection * 6f;

    //         // Apply to the Rigidbody
    //         playerRigidbody.linearVelocity = targetVelocity;
    //     }
    //     else
    //     {
    //         // 4. PREVENT SLIDING DOWN HILLS WHEN STANDING STILL:
    //         // If we are on a slope and not moving, neutralize gravity to freeze completely
    //         if (playerSlopeCheck.IsGrounded && Vector3.Angle(Vector3.up, playerSlopeCheck.SlopeNormal) > 2f)
    //         {
    //             playerRigidbody.linearVelocity = Vector3.zero;
    //             // Optional: You can temporarily change Rigidbody constraints or turn off gravity here
    //         }
    //     }
    // }

//?===================================================================================0
//!================TO STATEMACHINE GIVEN FUNCTIONS===============================
//?===================================================================================0


    private void HandleMovement(float movementSpeed) 
    {
        // 1. Calculate raw direction based on camera view
        Vector3 rawMoveDirection = cameraObject.forward * inputHandler.verticalInput;
        rawMoveDirection += cameraObject.right * inputHandler.horizontalInput;
        
        // Flatten it initially so camera tilt up/down doesn't make the player fly/burrow
        rawMoveDirection.y = 0;
        rawMoveDirection.Normalize();

        // 2. Capture the current physics velocity so we don't destroy gravity
        Vector3 currentVelocity = playerRigidbody.linearVelocity;

        if (rawMoveDirection.magnitude > 0.1f)
        {
            // 3. Project the direction onto the slope if grounded
            Vector3 targetMoveDirection = playerSlopeCheck.GetSlopeMoveDirection(rawMoveDirection);
            Vector3 targetVelocity = targetMoveDirection * movementSpeed;

            if (playerSlopeCheck.IsGrounded)
            {
                // On the ground, we fully control X, Y, and Z via the slope calculation
                playerRigidbody.linearVelocity = targetVelocity;
            }
            else
            {
                // In the air, we update horizontal speed but PRESERVE falling gravity (currentVelocity.y)
                targetVelocity.y = currentVelocity.y; 
                playerRigidbody.linearVelocity = targetVelocity;
            }
        }
        else
        {
            // 4. Handle Stopping / Idle states
            if (playerSlopeCheck.IsGrounded)
            {
                // Prevent sliding down ramps when standing still
                playerRigidbody.linearVelocity = Vector3.zero;
            }
            else
            {
                // Just let gravity do its job if we are falling without input
                playerRigidbody.linearVelocity = new Vector3(0, currentVelocity.y, 0);
            }
        }
    }


    public void HandleMovement_End(float movementSpeed) {
        moveDirection = cameraObject.forward * inputHandler.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputHandler.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;


        Vector3 movementVelocity =  moveDirection * 0.5f;
        moveDirection.Normalize();
        playerRigidbody.linearVelocity = movementVelocity;
    }
    

    private void HandleRotation() {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = cameraObject.forward * inputHandler.verticalInput;
        targetDirection = targetDirection + cameraObject.right * inputHandler.horizontalInput;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation; 
    }

    public MovementStateEnum detectedMovementStateByInput()
    {
        if(inputHandler.getMoveAmount() < 0.5f && inputHandler.getMoveAmount() > 0.05f)
        {
            return MovementStateEnum.Walking;
        }
        else if (inputHandler.getMoveAmount() >= 0.5f) {
            return checkSprintingInput() ? MovementStateEnum.Sprinting : MovementStateEnum.Running;
        }
        else
        {
            return MovementStateEnum.Idle;
        }
    }

    public bool checkSprintingInput()
    {
        return inputHandler.b_Input;
    }


//?====================================================================0

    #region METHODS FOR FALLING AND LANDING

    public bool isPlayerObjGrounded()
    {
        return playerGroundCheck.isPlayerObjGrounded();
    }

    private void activeFallingHandling()
    {
        inAirTimer = inAirTimer + Time.deltaTime;
        playerRigidbody.AddForce(transform.forward * leapingVelocity);
        playerRigidbody.AddForce(-Vector3.up * (fallingVelocity * 10f * inAirTimer + 20f));
    }

    public void activeFallingHandling_OnExit()
    {
        inAirTimer = 0;
    }


    #endregion

//?====================================================================0


}
