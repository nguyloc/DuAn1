using Player.Data;
using Player.StateMachine;

namespace Player.PlayerStates.SuperStates
{
    public class PlayerGroundedState : PlayerState
    {
        protected int XInput;
        private bool jumpInput;
        private bool grabInput;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isTouchingLedge;
        private bool dashInput;

        public PlayerGroundedState(Player.StateMachine.Player player, PlayerStateMachine stateMachine
                                 , PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData
          , animBoolName) { }

        public override void DoChecks() {
            base.DoChecks();

            isGrounded = player.CheckIfGrounded();
            isTouchingWall = player.CheckIfTouchingWall();
            isTouchingLedge = player.CheckIfTouchingLedge();
        }

        public override void Enter() {
            base.Enter();

            player.JumpState.ResetAmountOfJumpsLeft();
            player.DashState.ResetCanDash();
        }


        public override void LogicUpdate() {
            base.LogicUpdate();
            XInput = player.InputHandler.NormInputX;
            jumpInput = player.InputHandler.JumpInput;
            grabInput = player.InputHandler.GrabInput;
            dashInput = player.InputHandler.DashInput;

            if (jumpInput && player.JumpState.CanJump()) {
                stateMachine.ChangeState(player.JumpState);
            }

            else if (!isGrounded) {
                player.InAirState.StartCoyoteTime();
                stateMachine.ChangeState(player.InAirState);
            }

            else if (isTouchingWall && grabInput && isTouchingLedge) {
                stateMachine.ChangeState(player.WallGrabState);
            }

            else if (dashInput && player.DashState.CheckIfCanDash()) {
                stateMachine.ChangeState(player.DashState);
            }
        }
    }
}
