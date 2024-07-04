using Player.Data;
using Player.StateMachine;

namespace Player.PlayerStates.SuperStates
{
    public class PlayerTouchingWallState : PlayerState
    {
        protected bool IsGrounded;
        protected bool IsTouchingWall;
        protected bool GrabInput;
        protected bool JumpInput;
        protected int XInput;
        protected int YInput;
        protected bool IsTouchingLedge;


        public PlayerTouchingWallState(Player.StateMachine.Player player, PlayerStateMachine stateMachine
                                     , PlayerData playerData, string animBoolName) : base(player, stateMachine
          , playerData, animBoolName) { }


        public override void DoChecks() {
            base.DoChecks();

            IsGrounded = player.CheckIfGrounded();
            IsTouchingWall = player.CheckIfTouchingWall();
            IsTouchingLedge = player.CheckIfTouchingLedge();

            if (IsTouchingWall && !IsTouchingLedge)
                player.LegdeClimbState.SetDetectedPosition(player.transform.position);
        }


        public override void LogicUpdate() {
            base.LogicUpdate();

            XInput = player.InputHandler.NormInputX;
            YInput = player.InputHandler.NormInputY;
            GrabInput = player.InputHandler.GrabInput;
            JumpInput = player.InputHandler.JumpInput;

            if (JumpInput) {
                player.WallJumpState.DetermineWallJumpDirection(IsTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
            }

            else if (IsGrounded && !GrabInput) {
                stateMachine.ChangeState(player.IdleState);
            }
            else if (!IsTouchingWall || (XInput != player.FacingDirection && !GrabInput)) {
                stateMachine.ChangeState(player.InAirState);
            }
            else if (IsTouchingWall && !IsTouchingLedge) {
                stateMachine.ChangeState(player.LegdeClimbState);
            }
        }
    }
}
