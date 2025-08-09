using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerDetector detector;
        [SerializeField] private Transform swordObject;
        [SerializeField] private Transform[] patrolPoints;

        private StateMachine stateMachine;


        [Header("Attack Settings")]
        [SerializeField] float attackCooldown = 0.5f;
        [SerializeField] float attackDistance = 1f;
        [SerializeField] int attackDamage = 0;
        void Awake()
        {
            SetupStateMachine();
        }

        void Update()
        {
            stateMachine.Update();
        }

        void FixedUpdate()
        {
            stateMachine.FixedUpdate();
        }

        public void HandleMovement()
        {
            if (agent.isOnNavMesh && detector.Player != null)
            {
                agent.isStopped = false;

                Vector3 directionToPlayer = (detector.Player.position - transform.position).normalized;
                float stopDistance = 1.5f;
                Vector3 targetPosition = detector.Player.position - directionToPlayer * stopDistance;

                agent.SetDestination(targetPosition);
            }
        }

        private void SetupStateMachine()
        {
            stateMachine = new StateMachine();

            Vector3[] patrolPositions = new Vector3[patrolPoints.Length];
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                patrolPositions[i] = patrolPoints[i].position;
            }

            var patrol = new PatrolState(this, agent, patrolPositions, animator);
            var chase = new EnemySprintState(this, animator);
            var attack = new EnemyAttackState(this, animator);
            var dead = new EnemyDieState(this, animator);

            At(patrol, chase, new FuncPredicate(() => detector.CanDetectPlayer()));
            At(chase, attack, new FuncPredicate(() => detector.CanAttackPlayer()));
            At(attack, chase, new FuncPredicate(() => !detector.CanAttackPlayer()));
            At(chase, patrol, new FuncPredicate(() => !detector.CanDetectPlayer()));

            Any(dead, new FuncPredicate(() => GetComponent<Health>().IsDead));


            stateMachine.SetState(patrol);
        }

        private void At(IState from, IState to, IPredicate condition) =>
            stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) =>
            stateMachine.AddAnyTransition(to, condition);
/*
        public void PatrolTo(Vector3 point)
        {
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
                agent.SetDestination(point);
            }
        }

        public bool ReachedDestination()
        {
            return !agent.pathPending && agent.remainingDistance < 0.5f;
        }*/


        public void AttackPlayer()
        {
            Vector3 attackPos = transform.position + transform.forward;
            Collider[] hitPlayers = Physics.OverlapSphere(attackPos, attackDistance);

            foreach (var player in hitPlayers)
            {
                Debug.Log(player.name);
                Debug.Log(attackDamage);
                if (player.CompareTag("Player"))
                {
                    player.GetComponent<Health>().TakeDamage(attackDamage);
                    
                }
            }
        }

        public Transform[] GetPatrolPoints() => patrolPoints;
        public Transform GetPlayer() => detector.Player;
    }
}
