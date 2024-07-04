using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallSlideState : PlayerTouchingWallState
    {
        public PlayerWallSlideState(Player.StateMachine.Player player, PlayerStateMachine stateMachine
                                  , PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData
          , animBoolName) { }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (!isExitingState) {
                player.SetVelocityY(-playerData.wallSlideVelocity);

                if (GrabInput && YInput == 0) stateMachine.ChangeState(player.WallGrabState);
            }
        }
    }
}
