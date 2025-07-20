using Game.Code.Core.Network;
using Unity.Netcode;
using Zenject;

namespace Game.Code.Gameplay.Player
{
    public class PlayerController : NetworkBehaviour
    {
        private PlayersContainer _container;

        public ulong ClientId => ClientIdNV.Value;

        public NetworkVariable<ulong> ClientIdNV { get; } = new();

        public TeamType Team => TeamNV.Value;

        public NetworkVariable<TeamType> TeamNV { get; } = new();

        public int AttackCount { get => AttackCountNV.Value; set => AttackCountNV.Value = value; }

        public NetworkVariable<int> AttackCountNV { get; } = new(1);

        public int MoveCount { get => MoveCountNV.Value; set => MoveCountNV.Value = value; }

        public NetworkVariable<int> MoveCountNV { get; } = new(1);

        public bool CanAction => AttackCount > 0 || MoveCount > 0;

        [Inject]
        public void Construct(PlayersContainer container) => _container = container;

        public void Initialize(ulong clientId, TeamType team)
        {
            ClientIdNV.Initialize(this, clientId);
            TeamNV.Initialize(this, team);
        }

        public override void OnNetworkSpawn() => _container.Add(this);

        public override void OnNetworkDespawn() => _container.Remove(this);
    }
}