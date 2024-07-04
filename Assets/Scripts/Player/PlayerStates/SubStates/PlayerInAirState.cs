
using Player.Data;
using Player.StateMachine;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerInAirState : PlayerState
    {
        // Input
        private int xInput;
        private bool jumpInput;
        private bool jumpInputStop;
        private bool grabInput;
        private bool dashInput;


        // Checks
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isTouchingWallBack;
        private bool oldIsTouchingWall;
        private bool oldIsTouchingWallBack;
        private bool coyoteTime;
        private bool wallJumpCoyoteTime;
        private bool isJumping;
        private bool isTouchingLedge;

        private float startWallJumpCoyoteTime;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public PlayerInAirState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData,
                                string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            oldIsTouchingWall = isTouchingWall;
            oldIsTouchingWallBack = isTouchingWallBack;

            isGrounded = player.CheckIfGrounded();
            isTouchingWall = player.CheckIfTouchingWall();
            isTouchingWallBack = player.CheckIfTouchingWallBack();
            isTouchingLedge = player.CheckIfTouchingLedge();


            if (isTouchingWall && !isTouchingLedge) player.LegdeClimbState.SetDetectedPosition(player.transform.position);

            if (!wallJumpCoyoteTime && !isTouchingWall && !isTouchingWallBack &&
                (oldIsTouchingWall || oldIsTouchingWallBack)) StartWallJumpCoyoteTime();
        }



        public override void Exit()
        {
            base.Exit();

            oldIsTouchingWall = false;
            oldIsTouchingWallBack = false;
            isTouchingWall = false;
            isTouchingWallBack = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            UpdateInputs();
            CheckJumpMultiplier();

            if (CheckGrounded()) return;
            if (CheckForLedgeClimbing()) return;
            if (CheckForWallJump()) return;
            if (CheckForNormalJump()) return;
            if (CheckForWallGrab()) return;
            if (CheckForWallSlide()) return;
            if (CheckForDash()) return;

            UpdateMovement();
        }

        private void UpdateInputs()
        {
            xInput = player.InputHandler.NormInputX;
            jumpInput = player.InputHandler.JumpInput;
            jumpInputStop = player.InputHandler.JumpInputStop;
            grabInput = player.InputHandler.GrabInput;
            dashInput = player.InputHandler.DashInput;
        }

        private bool CheckGrounded()
        {
            if (isGrounded && player.CurentVelocity.y < 0.01f)
            {
                stateMachine.ChangeState(player.LandState);
                return true;
            }

            return false;
        }

        private bool CheckForLedgeClimbing()
        {
            if (isTouchingWall && !isTouchingLedge && !isGrounded)
            {
                stateMachine.ChangeState(player.LegdeClimbState);
                return true;
            }

            return false;
        }

        private bool CheckForWallJump()
        {
            if (jumpInput && (isTouchingWall || isTouchingWallBack || wallJumpCoyoteTime))
            {
                StopWallJumpCoyoteTime();
                isTouchingWall = player.CheckIfTouchingWall();
                player.WallJumpState.DetermineWallJumpDirection(isTouchingWall);
                stateMachine.ChangeState(player.WallJumpState);
                return true;
            }

            return false;
        }

        private bool CheckForNormalJump()
        {
            if (jumpInput && player.JumpState.CanJump())
            {
                stateMachine.ChangeState(player.JumpState);
                return true;
            }

            return false;
        }

        private bool CheckForWallGrab()
        {
            if (isTouchingWall && grabInput && isTouchingLedge)
            {
                stateMachine.ChangeState(player.WallGrabState);
                return true;
            }

            return false;
        }


        private bool CheckForWallSlide()
        {
            if (isTouchingWall && xInput == player.FacingDirection && player.CurentVelocity.y <= 0)
            {
                stateMachine.ChangeState(player.WallSlideState);
                return true;
            }

            return false;
        }

        private bool CheckForDash()
        {
            if (dashInput && player.DashState.CheckIfCanDash())
            {
                stateMachine.ChangeState(player.DashState);
                return true;
            }

            return false;
        }

        private void UpdateMovement()
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);
            player.Anim.SetFloat(YVelocity, player.CurentVelocity.y);
            player.Anim.SetFloat(XVelocity, Mathf.Abs(player.CurentVelocity.x));
        }

        private void CheckJumpMultiplier()
        {
            if (isJumping)
            {
                if (jumpInputStop)
                {
                    player.SetVelocityY(player.CurentVelocity.y * playerData.variableJumpHeightMultiplier);
                    isJumping = false;
                }
                else if (player.CurentVelocity.y <= 0f)
                {
                    isJumping = false;
                }
            }
        }


        private void CheckCoyoteTime()
        {
            if (coyoteTime && Time.time > startTime + playerData.coyoteTime)
            {
                coyoteTime = false;
                player.JumpState.DecreaseAmountOfJumpsLeft();
            }
        }

        private void CheckWallJumpCoyoteTime()
        {
            if (wallJumpCoyoteTime && Time.time > startWallJumpCoyoteTime + playerData.coyoteTime)
                wallJumpCoyoteTime = false;
        }

        public void StartCoyoteTime()
        {
            coyoteTime = true;
        }

        public void StartWallJumpCoyoteTime()
        {
            wallJumpCoyoteTime = true;
            startWallJumpCoyoteTime = Time.time;
        }

        public void StopWallJumpCoyoteTime()
        {
            wallJumpCoyoteTime = false;
        }

        public void SetIsJumping()
        {
            isJumping = true;
        }
    }
}