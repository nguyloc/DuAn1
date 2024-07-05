using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            core.Movement.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (xInput != 0)
                {
                    stateMachine.ChangeState(player.MoveState);
                }
                else if (yInput == -1)
                {
                    stateMachine.ChangeState(player.CrouchIdleState);
                }
            }       
        
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}