using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallClimbState : PlayerTouchingWallState
    {
        public PlayerWallClimbState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!isExitingState)
            {
                player.SetVelocityY(playerData.wallClimbVelocity);

                if (YInput != 1)
                {
                    stateMachine.ChangeState(player.WallGrabState);
                }
            }
        }
    }
}
