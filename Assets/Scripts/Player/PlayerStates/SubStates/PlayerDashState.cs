
using Player.AfterImage;
using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;
using UnityEngine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerDashState : PlayerAbilitiesState
    {
        public bool CanDash { get; private set; }
        private bool isHolding;
        private bool dashInputStop;

        private float lastDashTime;

        private Vector2 dashDirection;
        private Vector2 dashDirectionInput;
        private Vector2 lastAIPos;
        private static readonly int YVelocity = Animator.StringToHash("yVelocity");
        private static readonly int XVelocity = Animator.StringToHash("xVelocity");

        public PlayerDashState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();

            CanDash = false;
            player.InputHandler.UseDashInput();

            isHolding = true;
            dashDirection = Vector2.right * player.FacingDirection;

            Time.timeScale = playerData.holdTimeScale;
            startTime = Time.unscaledTime;

            player.DashDirectionIndicator.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            base.Exit();

            if (player.CurentVelocity.y >0)
            {
                player.SetVelocityY(player.CurentVelocity.y * playerData.dashEndYMultiplier);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                UpdatePlayerVelocity();

                if (isHolding)
                {
                    ProcessHoldingDash();
                }
                else
                {
                    ExecuteDash();
                }
            }
        }

        private void UpdatePlayerVelocity()
        {
            player.Anim.SetFloat(YVelocity, player.CurentVelocity.y);
            player.Anim.SetFloat(XVelocity, Mathf.Abs(player.CurentVelocity.x));
        }

        private void ProcessHoldingDash()
        {
            dashDirectionInput = player.InputHandler.DashDirectionInput;
            dashInputStop =player.InputHandler.DashInputStop;

            if (dashDirectionInput != Vector2.zero)
            {
                dashDirection = dashDirectionInput;
                dashDirection.Normalize();
            }

            float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
            player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

            if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
            {
                FinishHeldDash();
            }
        }

        private void FinishHeldDash()
        {
            isHolding = false;
            Time.timeScale = 1f;
            startTime = Time.time;
            player.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
            player.Rb.drag = playerData.drag;
            player.SetVelocity(playerData.dashVelocity, dashDirection);
            player.DashDirectionIndicator.gameObject.SetActive(false);
            PlaceAfterImage();
        }

        private void ExecuteDash()
        {
            player.SetVelocity(playerData.dashVelocity, dashDirection);
            CheckIfShouldPlaceAfterImage();

            if (Time.time >= startTime + playerData.dashTime)
            {
                FinishDash();
            }
        }

        private void FinishDash()
        {
            player.Rb.drag = 0f;
            IsAbilityDone = true;
            lastDashTime = Time.time;
        }


        private void CheckIfShouldPlaceAfterImage()
        {
            if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distBetweenAfterImages)
            {
                PlaceAfterImage();
            }
        }

        private void PlaceAfterImage()
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            lastAIPos = player.transform.position;
        }

        public bool CheckIfCanDash()
        {
            return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
        }

        public void ResetCanDash() => CanDash = true;

    }
}
