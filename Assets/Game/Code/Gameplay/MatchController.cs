using Game.Code.Gameplay.Player;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay
{
    public class MatchController : NetworkBehaviour
    {
        public float TurnDuration = 60;
        private PlayersContainer _playersContainer;

        public NetworkVariable<float> TurnTime { get; } = new();

        public NetworkVariable<int> TurnNumber { get; } = new();

        public NetworkVariable<TeamType> CurrentTeam { get; } = new(TeamType.A);

        public bool IsMyTurn => false;//_playerController.Team.Value == CurrentTeam.Value;// взять игрока с 
        
        [Inject]
        public void Construct(PlayersContainer playersContainer) => _playersContainer = playersContainer;

        public override void OnNetworkSpawn()
        {
            if(IsServer)
                TurnTime.Value = TurnDuration;
        }

        public void Update()
        {
            if (IsSpawned && IsServer)
            {
                TurnTime.Value -= Time.deltaTime;
                if (TurnTime.Value <= 0 || (_playersContainer.TryGet(CurrentTeam.Value, out var currentPlayer) && !currentPlayer.CanAction))
                {
                    MoveNextTeam();
                    TurnNumber.Value++;
                    TurnTime.Value = TurnDuration;
                }
            }
        }

        private void MoveNextTeam() => CurrentTeam.Value = CurrentTeam.Value == TeamType.A ? TeamType.B : TeamType.A;
    }
}