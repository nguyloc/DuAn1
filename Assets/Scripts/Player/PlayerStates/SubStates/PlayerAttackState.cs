using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;
using Player.Inventory;
using Player.Weapons;

namespace Player.PlayerStates.SubStates
{
    public class PlayerAttackState : PlayerAbilityState
    {
        private Weapon weapon;

        public PlayerAttackState(StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            weapon.EnterWeapon();
        }

        public override void Exit()
        {
            base.Exit();

            weapon.ExitWeapon();
        }

        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            weapon.InitializeWeapon(this);
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();

            isAbilityDone = true;
        }
    }
}

