using UnityEngine;

namespace Platformer
{
    public class EnemyAttackState : BaseEnemyState
    {
        public EnemyAttackState(EnemyController enemy, Animator animator) : base(enemy, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(AttackHash, crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            enemy.HandleMovement();
        }
    }
}