using UnityEngine;

namespace Platformer
{
    public class EnemyLocomotionState : BaseEnemyState
    {
        public EnemyLocomotionState(EnemyController enemy, Animator animator) : base(enemy, animator) { }

        public override void OnEnter()
        {
            animator.CrossFade(LocomotionHash, crossFadeDuration);
        }

        public override void FixedUpdate()
        {
            //enemy.HandleMovement();
        }
    }
}