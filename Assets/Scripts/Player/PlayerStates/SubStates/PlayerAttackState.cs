using Player.Data;
using Player.PlayerStates.SuperStates;
using Player.StateMachine;

namespace Player.PlayerStates.SubStates
{
    public class PlayerAttackState : PlayerAbilityState
    {
        public PlayerAttackState(StateMachine.Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
    }
}

