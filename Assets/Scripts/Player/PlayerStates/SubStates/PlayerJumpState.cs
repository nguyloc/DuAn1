using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerJumpState : PlayerAbilitiesState
    {
        private int amountOfJumpsLeft;

        public PlayerJumpState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData
                             , string animBoolName) : base(player, stateMachine, playerData, animBoolName) {
            amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter() {
            base.Enter();

            player.InputHandler.UseJumpInput();
            player.SetVelocityY(playerData.jumpVelocity);
            IsAbilityDone = true;
            amountOfJumpsLeft--;
            player.InAirState.SetIsJumping();
        }

        public bool CanJump() {
            if (amountOfJumpsLeft > 0)
                return true;
            else
                return false;
        }

        public void ResetAmountOfJumpsLeft() {
            amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public void DecreaseAmountOfJumpsLeft() {
            amountOfJumpsLeft--;
        }
    }
}
