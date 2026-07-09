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
    public float rayCastHeightOffset = 0.5f;
    public float rayCastMaxDistance = 2f;
    public LayerMask groundLayer;

    [Header("Movement Flags")]
    public bool isSprinting;
    public bool isGrounded;
    public bool isJumping;


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

        cameraObject = Camera.main.transform;

        if(movementStateMachine == null)
        {
            movementStateMachine = new MovementStateMachine();
        }

        if(movementStatemachineHandler == null)
        {
            movementStatemachineHandler = new MovementStatemachineHandler(HandleMovement, HandleRotation, this, movementStateMachine, data);
        }
    }

    void Start()
    {
        movementStatemachineHandler.locomotionStart();
        
    }

    void Update()
    {
        _currentPrimaryStateString = playerStateDebugger.GetCurrentStateAsString();
    }

    void FixedUpdate()
    {
        //HandleRotation();

        movementStatemachineHandler.finalUpdate();
    }

    private void HandleMovement(float movementSpeed) {
         moveDirection = cameraObject.forward * inputHandler.verticalInput;
        moveDirection = moveDirection + cameraObject.right * inputHandler.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        moveDirection = moveDirection * movementSpeed;


        Vector3 movementVelocity =  moveDirection;
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
}
