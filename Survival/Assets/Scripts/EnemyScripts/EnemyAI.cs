using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [
        Range(0, 1),
        SerializeField,
        Tooltip("More weight = more frequent spawn")
    ]
    private float spawnWeight;
    [SerializeField] private int enemyCost = 1;
    public BaseState CurrentState;

    public Transform Target;
    [SerializeField] protected Transform attackSphereProjector;
    [SerializeField] protected Transform player;
    [SerializeField] private LayerMask groundLayer;
    public Transform Player => player;
    public Transform AttackSphereProjector => attackSphereProjector;

    [SerializeField] protected int damage = 5;
    [SerializeField] protected float agroDistance = 10;
    [SerializeField] protected float idleDistance = 20;
    [SerializeField] protected float attackDistance = 2;

    private NavMeshAgent agent;

    public float SpawnWeight => spawnWeight;
    public int Cost => enemyCost;
    public int Damage => damage;
    public float AgroDistance => agroDistance;
    public float IdleDistance => idleDistance;
    public float AttackDistance => attackDistance;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;

        if (Physics.Raycast(transform.position, -transform.up,out RaycastHit hit, float.MaxValue, groundLayer))
        {
            transform.position = hit.point;
        }
        agent.enabled = true;
    }
    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        Target = Player;

        if (CurrentState == null)
            ChangeState(new ChaseState());
    }
    private void Update()
    {
        if (Target == null)
        {
            Target = Player;
        }
        CurrentState.Update();
    }
    public void ChangeState(BaseState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.SetContext(this);
        CurrentState.Enter();
    }

    public float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, Target.position);
    }
    public void SetPlayer(Transform _player)
    {
        player = _player;
    }
}
