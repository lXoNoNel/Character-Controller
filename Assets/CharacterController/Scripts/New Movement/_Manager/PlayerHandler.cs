using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private MovementHandler movementHandler;
    private InputHandler inputHandler;


    void Awake()
    {
        movementHandler = GetComponent<MovementHandler>();
        inputHandler = GetComponent<InputHandler>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    void Update()
    {
        movementHandler.MovementUpdate();
        inputHandler.HandleAllInputs();   
    }

    void FixedUpdate()
    {
        movementHandler.MovementFixedUpdate();
    }

    // Update is called once per frame
    // void FixedUpdate()
    // {
    //     movementHandler.HandleAllMovement();
    // }
}
