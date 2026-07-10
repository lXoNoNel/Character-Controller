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
    public float pressureWhileMoving;


    [Header("Movement Speeds")]
    public float rotationSpeed = 15f;



    [Header("Sharp Turn Settings")]
    public float sharpTurnAngleThreshold = 40f; // The angle limit you requested

    [Range(0.1f, 1f)]
    public float sharpTurnSpeedMultiplier = 0.3f; // Slow down to 30% of normal speed during a sharp turn

    [Range(0.1f, 1f)]
    public float sharpTurnRotationMultiplier = 0.5f; // Cut rotation speed in half while pivoting

    [Header("Movement Smoothness")]
    [Tooltip("Higher values mean faster acceleration (snappier). Lower values mean slower acceleration (weightier).")]
    public float accelerationSpeed = 8f;


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
    PlayerSlopeHandler slopeHandler;

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
        slopeHandler = GetComponent<PlayerSlopeHandler>();

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
    // 1. Calculate raw camera direction
    Vector3 rawMoveDirection = cameraObject.forward * inputHandler.verticalInput;
    rawMoveDirection += cameraObject.right * inputHandler.horizontalInput;
    rawMoveDirection.y = 0;
    rawMoveDirection.Normalize();

    Vector3 currentVelocity = playerRigidbody.linearVelocity;

    if (rawMoveDirection.magnitude > 0.1f)
    {
        // 2. Check if a steep slope blocks us
        if (slopeHandler.IsSlopeTooSteep(rawMoveDirection))
        {
            playerRigidbody.linearVelocity = Vector3.Lerp(currentVelocity, new Vector3(0, currentVelocity.y, 0), accelerationSpeed * Time.fixedDeltaTime);
            return;
        }

        // 3. Sharp Turn Speed Throttle
        float activeSpeed = movementSpeed;
        if (isMakingSharpTurn)
        {
            activeSpeed *= sharpTurnSpeedMultiplier;
        }

        // 4. Calculate the absolute goal velocity we WANT to reach
        Vector3 targetMoveDirection = slopeHandler.GetSlopeMoveDirection(rawMoveDirection);
        Vector3 goalVelocity = targetMoveDirection * activeSpeed;

        if (slopeHandler.IsGrounded)
        {
            // NON-LINEAR SMOOTHING: Blends current velocity into the goal velocity exponentially over time
            playerRigidbody.linearVelocity = Vector3.Lerp(currentVelocity, goalVelocity, accelerationSpeed * Time.fixedDeltaTime);
        }
        else
        {
            // In the air, blend horizontal speed smoothly but preserve exact falling gravity (currentVelocity.y)
            goalVelocity.y = currentVelocity.y; 
            
            // Separate XZ and Y blending so horizontal air-control is smooth but gravity falls uninterrupted
            Vector3 smoothAirVelocity = Vector3.Lerp(currentVelocity, goalVelocity, accelerationSpeed * Time.fixedDeltaTime);
            smoothAirVelocity.y = currentVelocity.y; 
            
            playerRigidbody.linearVelocity = smoothAirVelocity;
        }
        }
        else
        {
            // 5. Smoothly decelerate to a stop when input is released (prevents abrupt stopping)
            if (slopeHandler.IsGrounded)
            {
                playerRigidbody.linearVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, accelerationSpeed * Time.fixedDeltaTime);
            }
            else
            {
                // Let gravity bring them down naturally while sliding to a halt horizontally
                Vector3 stopAirVelocity = Vector3.Lerp(currentVelocity, new Vector3(0, currentVelocity.y, 0), accelerationSpeed * Time.fixedDeltaTime);
                stopAirVelocity.y = currentVelocity.y;
                playerRigidbody.linearVelocity = stopAirVelocity;
            }
        }
    }


    public void HandleMovement_End(float movementSpeed) {
        moveDirection = cameraObject.forward * inputHandler.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputHandler.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;


        Vector3 movementVelocity =  moveDirection * 0.45f;
        moveDirection.Normalize();
        playerRigidbody.linearVelocity = movementVelocity;
    }
    

    private bool isMakingSharpTurn;

    private void HandleRotation() 
    {
        Vector3 targetDirection = Vector3.forward;

        targetDirection = cameraObject.forward * inputHandler.verticalInput;
        targetDirection += cameraObject.right * inputHandler.horizontalInput;
        targetDirection.y = 0;
        targetDirection.Normalize();

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
            isMakingSharpTurn = false; // Reset if standing still
        }
        else
        {
            // 1. Calculate the angle between where we are currently looking vs where we want to go
            float turnAngle = Vector3.Angle(transform.forward, targetDirection);

            // 2. Determine if this constitutes a "Sharp Turn"
            isMakingSharpTurn = turnAngle > sharpTurnAngleThreshold;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        // 3. Dynamically adjust rotation speed. If making a sharp turn, multiply it by your reduction variable.
        float dynamicRotationSpeed = rotationSpeed;
        if (isMakingSharpTurn)
        {
            dynamicRotationSpeed *= sharpTurnRotationMultiplier;
        }

        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, dynamicRotationSpeed * Time.fixedDeltaTime);
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
