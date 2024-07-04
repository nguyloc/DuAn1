using Player.Data;
using Player.StateMachine;

namespace Player.PlayerStates.SuperStates
{
    public class PlayerAbilitiesState : PlayerState
    {
        protected bool IsAbilityDone;

        private bool isGrounded;


        public PlayerAbilitiesState(Player.StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            isGrounded = player.CheckIfGrounded();
        }

        public override void Enter()
        {
            base.Enter();

            IsAbilityDone = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (IsAbilityDone)
            {
                if (isGrounded && player.CurentVelocity.y < 0.01f)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
                else
                {
                    stateMachine.ChangeState(player.InAirState);
                }
            }
        }
    }
}
