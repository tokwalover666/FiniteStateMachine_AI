using UnityEngine;
using UnityEngine.AI;
using Platformer;

public class PatrolState : IState
{
    private EnemyController enemy;
    private NavMeshAgent agent;
    private Vector3[] patrolPoints;
    private int currentPoint;
    private Animator animator;

    private static readonly int SprintHash = Animator.StringToHash("Sprint");
    private static readonly int LocomotionHash = Animator.StringToHash("Locomotion");

    private float crossFadeDuration = 0.1f;

    private float idleTimer = 0f;
    private float idleDuration = 0f;
    private bool isIdling = false;

    public PatrolState(EnemyController enemy, NavMeshAgent agent, Vector3[] points, Animator animator)
    {
        this.enemy = enemy;
        this.agent = agent;
        this.patrolPoints = points;
        this.animator = animator;
        currentPoint = 0;
    }

    public void OnEnter()
    {
        GoToNextPoint();
    }

    public void Update()
    {
        if (patrolPoints.Length == 0) return;

        if (isIdling)
        {
            idleTimer += Time.deltaTime;

            if (idleTimer >= idleDuration)
            {
                isIdling = false;
                GoToNextPoint(); 
            }
            return;
        }

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            StartIdle();
        }
    }

    public void FixedUpdate() { }

    public void OnExit()
    {
        agent.isStopped = true;
    }

    private void GoToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        animator.CrossFade(SprintHash, crossFadeDuration);

        agent.isStopped = false;
        agent.SetDestination(patrolPoints[currentPoint]);
    }

    private void StartIdle()
    {
        agent.isStopped = true;
        animator.CrossFade(LocomotionHash, crossFadeDuration); 
        idleDuration = Random.Range(1f, 3f); 
        idleTimer = 0f;
        isIdling = true;

        currentPoint = (currentPoint + 1) % patrolPoints.Length;
    }
}
