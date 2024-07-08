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

        private int xInput;

        private float velocityToSet;

        private bool setVelocity;
        private bool shouldCheckFlip;

        public PlayerAttackState(StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            setVelocity = false;

            weapon.EnterWeapon();
        }

        public override void Exit()
        {
            base.Exit();

            weapon.ExitWeapon();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            xInput = player.InputHandler.NormInputX;

            if (shouldCheckFlip)
            {
                core.Movement.CheckIfShouldFlip(xInput);
            }


            if (setVelocity)
            {
                core.Movement.SetVelocityX(velocityToSet * core.Movement.FacingDirection);
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            this.weapon.InitializeWeapon(this);
        }

        public void SetPlayerVelocity(float velocity)
        {
            core.Movement.SetVelocityX(velocity * core.Movement.FacingDirection);

            velocityToSet = velocity;
            setVelocity = true;
        }

        public void SetFlipCheck(bool value)
        {
            shouldCheckFlip = value;
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();

            isAbilityDone = true;
        }
    }
}

