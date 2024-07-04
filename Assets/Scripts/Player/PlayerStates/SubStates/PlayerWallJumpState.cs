using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallJumpState : PlayerAbilitiesState
    {

        private int wallJumpDirection;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public PlayerWallJumpState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.InputHandler.UseJumpInput();
            player.JumpState.ResetAmountOfJumpsLeft();
            player.SetVelocity(playerData.wallJumpVelocity, playerData.wallJumpAngle, wallJumpDirection);
            player.CheckIfShouldFlip(wallJumpDirection);
            player.JumpState.DecreaseAmountOfJumpsLeft();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Anim.SetFloat(YVelocity, player.CurentVelocity.y);
            player.Anim.SetFloat(XVelocity, Mathf.Abs(player.CurentVelocity.x));

            if (Time.time >= startTime + playerData.wallJumpTime)
            {
                IsAbilityDone = true;
            }
        }

        public void DetermineWallJumpDirection(bool isTouchingWall)
        {
            if (isTouchingWall)
            {
                wallJumpDirection = - player.FacingDirection;
            }
            else
            {
                wallJumpDirection = player.FacingDirection;
            }
        }
    }
}

