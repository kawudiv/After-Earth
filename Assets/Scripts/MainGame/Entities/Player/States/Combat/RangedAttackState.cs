using Player.Base;
using UnityEngine;

namespace Player.States.Combat
{
    public class RangedAttackState : State
    {
        private float attackCooldown;
        private bool hasShot;

        public RangedAttackState(PlayerBase _player, StateMachine _stateMachine) : base(_player, _stateMachine)
        {
            attackCooldown = _player.attackCooldown;
        }

        public override void Enter()
        {
            base.Enter();
            hasShot = false;
            player.animator.SetTrigger("RangedAttack");
            player.StartCoroutine(Shoot());
        }

        private System.Collections.IEnumerator Shoot()
        {
            hasShot = true;
            yield return new WaitForSeconds(attackCooldown);
            stateMachine.ChangeState(player.IdleState);
        }

        public override void Exit()
        {
            base.Exit();
            hasShot = false;
        }
    }
}
