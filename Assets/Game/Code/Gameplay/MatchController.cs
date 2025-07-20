using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay
{
    public class MatchController : NetworkBehaviour
    {
        public float TurnDuration = 60;
        private PlayersContainer _playersContainer;
        private IPlayerProvider _playerProvider;
        private UnitsSelector _unitsSelector;

        public float TurnTime { get => TurnTimeNV.Value; private set => TurnTimeNV.Value = value; }

        public NetworkVariable<float> TurnTimeNV { get; } = new();

        public int TurnNumber { get => TurnNumberNV.Value; private set => TurnNumberNV.Value = value; }

        public NetworkVariable<int> TurnNumberNV { get; } = new(1);

        public TeamType CurrentTeam { get => CurrentTeamNV.Value; set => CurrentTeamNV.Value = value; }

        public NetworkVariable<TeamType> CurrentTeamNV { get; } = new(TeamType.A);

        public bool IsMyTurn
        {
            get
            {
                if (!_playerProvider.Player)
                    return false;
                return _playerProvider.Player.Team == CurrentTeam;
            }
        }

        [Inject]
        public void Construct(PlayersContainer playersContainer, IPlayerProvider playerProvider, UnitsSelector unitsSelector)
        {
            _unitsSelector = unitsSelector;
            _playerProvider = playerProvider;
            _playersContainer = playersContainer;
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                TurnTime = TurnDuration;   
        }

        public void Update()
        {
            if (IsSpawned && IsServer)
            {
                TurnTime -= Time.deltaTime;
                if (TurnTime <= 0 || (_playersContainer.TryGet(CurrentTeam, out var currentPlayer) && !currentPlayer.CanAction))
                {
                    MoveNextTeam();
                    TurnNumber++;
                    UnSelectSelectedClientRpc();
                    TurnTime = TurnDuration;
                }
            }
        }

        [ClientRpc]
        public void UnSelectSelectedClientRpc()
        {
            if(_unitsSelector.HasSelected)
                _unitsSelector.UnSelect(_unitsSelector.Selected);
        }

        private void MoveNextTeam() => CurrentTeam = CurrentTeam == TeamType.A ? TeamType.B : TeamType.A;
    }
}