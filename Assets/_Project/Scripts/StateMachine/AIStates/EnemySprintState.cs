using UnityEngine;

namespace Platformer
{
    public class EnemySprintState : BaseEnemyState
    {
        private static readonly int SprintHash = Animator.StringToHash("Sprint");

        public EnemySprintState(EnemyController enemy, Animator animator)
            : base(enemy, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(SprintHash, crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            enemy.HandleMovement();
        }
    }
}
