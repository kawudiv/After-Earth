using Player.Base;
using UnityEngine;

namespace Player.States.Combat
{
    public class MeleeAttackState : State
    {
        private float attackCooldown;
        private bool hasAttacked;

        public MeleeAttackState(PlayerBase _player, StateMachine _stateMachine) : base(_player, _stateMachine)
        {
            attackCooldown = _player.attackCooldown;
        }

        public override void Enter()
        {
            base.Enter();
            hasAttacked = false;
            player.animator.SetTrigger("MeleeAttack");
            player.StartCoroutine(Attack());
        }

        private System.Collections.IEnumerator Attack()
        {
            hasAttacked = true;
            yield return new WaitForSeconds(attackCooldown);
            stateMachine.ChangeState(player.IdleState);
        }

        public override void Exit()
        {
            base.Exit();
            hasAttacked = false;
        }
    }
}
