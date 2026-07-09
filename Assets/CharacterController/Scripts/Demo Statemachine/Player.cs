using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MovingObjectDetector))]

public class Player : MonoBehaviour
{
    private StateMachine stateMachine = new StateMachine();
    private MovingObjectDetector movingObjectDetector;

    [SerializeField]
    private LayerMask itemsLayer;

    [SerializeField]
    private float viewRange;

    [SerializeField]
    private string itemsLayerTag;

    private Rigidbody rb;

    private Vector3 lastPosition;
    [SerializeField]
    private float movementThreshold = 0.001f;

    private NavMeshAgent navMeshAgent;

    public bool onTheWaytoFoodObj = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        movingObjectDetector = GetComponent<MovingObjectDetector>();   
        this.stateMachine.ChangeState(new SearchFor(this.itemsLayer, this.gameObject, this.viewRange, itemsLayerTag, this.FoodFound));
    }

    void Update()
    {
        this.stateMachine.ExecuteStateUpdate();

        if(onTheWaytoFoodObj)
        {
            float distanceMoved = Vector3.Distance(transform.position, lastPosition);

            if(movingObjectDetector.Colliding() && distanceMoved < movementThreshold)
            {
                this.stateMachine.ChangeState(new FoundFood(movingObjectDetector.GetCollidingObject(), (food) => Destroy(food, 0.5f)));
            }
                //Check if the player keeps moving   
            if (!movingObjectDetector.Colliding() && distanceMoved < movementThreshold)
            {
                this.stateMachine.ChangeState(new SearchFor(this.itemsLayer, this.gameObject, this.viewRange, itemsLayerTag, this.FoodFound));
            }

            // CRITICAL: Update the last position at the very end of the frame
            lastPosition = transform.position;
        }
    }

    public void FoodFound(SearchResults searchResults)
    {
        var foundFoodItems = searchResults.allHitObjectsWithRequiredTag;

        foreach(var item in foundFoodItems)
        {
            this.navMeshAgent.SetDestination(item.gameObject.transform.position);
            onTheWaytoFoodObj = true;
        }
    }
}
