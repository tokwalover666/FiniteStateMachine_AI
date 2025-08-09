using UnityEngine;

namespace Platformer
{
    public class EnemyDieState : BaseEnemyState
    {
        public EnemyDieState(EnemyController enemy, Animator animator) : base(enemy, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(DieHash, crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            //enemy.HandleMovement();
        }
    }
}