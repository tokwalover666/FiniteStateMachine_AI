using UnityEngine;
using UnityEngine.AI;

namespace Platformer
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private NavMeshAgent agent;
        [SerializeField] private PlayerDetector detector;

        [SerializeField] private Transform[] patrolPoints;

        private StateMachine stateMachine;

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

            // Convert patrol Transforms to Vector3 positions
            Vector3[] patrolPositions = new Vector3[patrolPoints.Length];
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                patrolPositions[i] = patrolPoints[i].position;
            }

            var patrol = new PatrolState(this, agent, patrolPositions, animator);
            var chase = new EnemySprintState(this, animator);
            var attack = new EnemyAttackState(this, animator);

            At(patrol, chase, new FuncPredicate(() => detector.CanDetectPlayer()));
            At(chase, attack, new FuncPredicate(() => detector.CanAttackPlayer()));
            At(attack, chase, new FuncPredicate(() => !detector.CanAttackPlayer()));
            At(chase, patrol, new FuncPredicate(() => !detector.CanDetectPlayer()));

            stateMachine.SetState(patrol);
        }

        private void At(IState from, IState to, IPredicate condition) =>
            stateMachine.AddTransition(from, to, condition);

        private void Any(IState to, IPredicate condition) =>
            stateMachine.AddAnyTransition(to, condition);

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
        }


        public void AttackPlayer()
        {
            animator.SetTrigger("Attack");
        }

        public Transform[] GetPatrolPoints() => patrolPoints;
        public Transform GetPlayer() => detector.Player;
    }
}
