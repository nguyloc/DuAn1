using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData
                             , string animBoolName) : base(player, stateMachine, playerData, animBoolName) { }


        public override void Enter() {
            base.Enter();
            player.SetVelocityX(0f);
        }

        public override void LogicUpdate() {
            base.LogicUpdate();

            if (XInput != 0 && !isExitingState) stateMachine.ChangeState(player.MoveState);
        }
    }
}
