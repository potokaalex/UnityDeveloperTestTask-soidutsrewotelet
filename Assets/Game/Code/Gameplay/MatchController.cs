using Game.Code.Core.Network;
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

        public bool IsMyTurn => IsHisTurn(_playerProvider.Player);

        [Inject]
        public void Construct(PlayersContainer playersContainer, IPlayerProvider playerProvider, UnitsSelector unitsSelector)
        {
            _unitsSelector = unitsSelector;
            _playerProvider = playerProvider;
            _playersContainer = playersContainer;
        }

        public void Initialize()
        {
            if (IsServer)
                TurnTime = TurnDuration;
        }

        public void Update()
        {
            if (!IsSpawned || !IsServer)
                return;

            TurnTime -= Time.deltaTime;
            var player = _playersContainer.Get(CurrentTeam);

            if (TurnTime <= 0 || (player && !player.CanAction))
            {
                MoveNextTeam();
                TurnNumber++;
                TurnTime = TurnDuration;

                if (player)
                    OnNewTurnPreviousClientRpc(new ClientRpcParams().For(player.ClientId));

                if (_playersContainer.TryGet(CurrentTeam, out var nextPlayer))
                {
                    nextPlayer.AttackCount = 1;
                    nextPlayer.MoveCount = 1;
                    //options
                }
            }
        }

        [ClientRpc]
        public void OnNewTurnPreviousClientRpc(ClientRpcParams _)
        {
            if (_unitsSelector.HasSelected)
                _unitsSelector.UnSelect(_unitsSelector.Selected);
            ;
        }

        public bool IsHisTurn(PlayerController player)
        {
            if (!player)
                return false;
            return player.Team == CurrentTeam;
        }

        private void MoveNextTeam() => CurrentTeam = CurrentTeam == TeamType.A ? TeamType.B : TeamType.A;
    }
}