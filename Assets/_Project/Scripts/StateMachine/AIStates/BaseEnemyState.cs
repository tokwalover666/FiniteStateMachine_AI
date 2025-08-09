using UnityEngine;

namespace Platformer
{
    public abstract class BaseEnemyState : IState
    {
        protected EnemyController enemy;
        protected Animator animator;
        protected readonly float crossFadeDuration = 0.1f;

        protected static readonly int LocomotionHash = Animator.StringToHash("Locomotion");
        protected static readonly int SprintHash = Animator.StringToHash("Sprint");
        protected static readonly int DieHash = Animator.StringToHash("Die");
        protected static readonly int AttackHash = Animator.StringToHash("Attack");

        protected BaseEnemyState(EnemyController enemy, Animator animator)
        {
            this.enemy = enemy;
            this.animator = animator;
        }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
    
    }


}
