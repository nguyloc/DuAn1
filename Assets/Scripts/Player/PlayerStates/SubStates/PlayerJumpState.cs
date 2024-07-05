using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int amountOfJumpsLeft;

        public PlayerJumpState(StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter()
        {
            base.Enter();
            player.InputHandler.UseJumpInput();
            core.Movement.SetVelocityY(playerData.jumpVelocity);
            isAbilityDone = true;
            amountOfJumpsLeft--;
            player.InAirState.SetIsJumping();
        }

        public bool CanJump()
        {
            if (amountOfJumpsLeft > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

        public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    }
}
