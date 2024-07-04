using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerWallGrabState : PlayerTouchingWallState
    {
        private Vector2 holdPosition;

        public PlayerWallGrabState(Player.StateMachine.Player player, PlayerStateMachine stateMachine
                                 , PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData
          , animBoolName) { }

        public override void Enter() {
            base.Enter();

            holdPosition = player.transform.position;
            HoldPosition();

            Debug.Log("Enter Grab");
        }


        public override void LogicUpdate() {
            base.LogicUpdate();


            if (!isExitingState) {
                HoldPosition();
                if (YInput > 0)
                    stateMachine.ChangeState(player.WallClimbState);

                else if (YInput < 0 || !GrabInput) stateMachine.ChangeState(player.WallSlideState);
            }
        }

        private void HoldPosition() {
            player.transform.position = holdPosition;

            player.SetVelocityX(0f);
            player.SetVelocityY(0f);
        }
    }
}
