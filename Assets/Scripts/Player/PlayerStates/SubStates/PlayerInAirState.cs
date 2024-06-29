using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInAirState : PlayerState
{
    private int xInput;
    private bool isGrounded;
    private bool isTounchingWall;
    private bool isTounchingWallBack;
    private bool oldIsTochingWall;
    private bool oldIsTochingWallBack;
    private bool jumpInput;
    private bool jumpInputStop;
    private bool coyoteTime;
    private bool wallJumpCoyoteTime;
    private bool isJumping;
    private bool grabInput;
    private bool isTounchingLedge;

    private float startWallJumpCoyoteTime;

    public PlayerInAirState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {
    }

    public override void DoChecks()
    {
        base.DoChecks();

        oldIsTochingWall = isTounchingWall;
        oldIsTochingWallBack = isTounchingWallBack;

        isGrounded = player.CheckIfGrounded();
        isTounchingWall = player.CheckIfTouchingWall();
        isTounchingWallBack = player.CheckIfTounchingWallBack();
        isTounchingLedge = player.CheckIfTouchingLedge();


        if (isTounchingWall && !isTounchingLedge)
        {
            player.LegdeClimbState.SetDetectedPosition(player.transform.position);
        }

        if (!wallJumpCoyoteTime && !isTounchingWall && !isTounchingWallBack && (oldIsTochingWall || oldIsTochingWallBack))
        {
            StartWallJumpCoyoteTime();
        }
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        oldIsTochingWall = false;
        oldIsTochingWallBack = false;
        isTounchingWall = false ;
        isTounchingWallBack = false ;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        CheckCoyoteTime();
        CheckWallJumpCoyoteTime();

        xInput = player.InputHandler.NormInputX;
        jumpInput = player.InputHandler.JumpInput;
        jumpInputStop = player.InputHandler.JumpInputStop;
        grabInput = player.InputHandler.GrabInput;

        CheckJumpMultiplier();

        if (isGrounded && player.CurentVelocity.y < 0.01f) 
        {
            stateMachine.ChangeState(player.LandState);
        }

        else if (isTounchingWall && !isTounchingLedge)
        {
            stateMachine.ChangeState(player.LegdeClimbState);
        }

        else if (jumpInput && (isTounchingWall || isTounchingWallBack || wallJumpCoyoteTime))
        {
            StopWallJumpCoyoteTime();
            isTounchingWall = player.CheckIfTouchingWall();
            player.WallJumpState.DetermineWallJumpDirection(isTounchingWall);
            stateMachine.ChangeState(player.WallJumpState);
        }
        
        else if(jumpInput && player.JumpState.CanJump())
        {
            stateMachine.ChangeState(player.JumpState);
        }

        else if (isTounchingWall && grabInput)
        {
            stateMachine.ChangeState(player.WallGrabState);
        }
        
        else if(isTounchingWall && xInput == player.FacingDirection && player.CurentVelocity.y <= 0)
        {
            stateMachine.ChangeState(player.WallSlideState);
        }
        
        else
        {
            player.CheckIfShouldFlip(xInput);
            player.SetVelocityX(playerData.movementVelocity * xInput);

            player.Anim.SetFloat("yVelocity", player.CurentVelocity.y);
            player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurentVelocity.x));
        }
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

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
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
        {
            wallJumpCoyoteTime = false;
        }
    }

    public void StartCoyoteTime() => coyoteTime = true;

    public void StartWallJumpCoyoteTime()
    {
        wallJumpCoyoteTime = true;
        startWallJumpCoyoteTime = Time.time;
    }

    public void StopWallJumpCoyoteTime() => wallJumpCoyoteTime = false;

    public void SetIsJumping() => isJumping = true;
}
