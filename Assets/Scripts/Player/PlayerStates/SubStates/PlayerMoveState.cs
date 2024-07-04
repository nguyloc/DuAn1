using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerMoveState : PlayerGroundedState
    {
        public PlayerMoveState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData
                             , string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }

        public override void LogicUpdate() {
            base.LogicUpdate();

            player.CheckIfShouldFlip(XInput);

            player.SetVelocityX(playerData.movementVelocity * XInput);

            if (XInput == 0 && !isExitingState) stateMachine.ChangeState(player.IdleState);
        }
    }
}
