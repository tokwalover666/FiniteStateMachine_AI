using UnityEngine;

namespace Platformer
{

    public class EnemyAttackState : BaseEnemyState
    {


        private float lastNormalizedTime;
        public EnemyAttackState(EnemyController enemy, Animator animator) : base(enemy, animator) 
        {

        }

        public override void OnEnter()
        {
            animator.CrossFade(AttackHash, crossFadeDuration);
            enemy.AttackPlayer();
        }

        public override void FixedUpdate()
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float normalizedTime = stateInfo.normalizedTime % 1;

            if (normalizedTime < lastNormalizedTime) // Loop restart detected
            {
                enemy.AttackPlayer();
            }

            lastNormalizedTime = normalizedTime;
        }
    }
}