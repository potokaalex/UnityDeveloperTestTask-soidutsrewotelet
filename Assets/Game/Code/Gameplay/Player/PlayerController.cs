using Unity.Netcode;
using Zenject;

namespace Game.Code.Gameplay.Player
{
    public class PlayerController : NetworkBehaviour
    {
        private PlayersContainer _playersContainer;

        public NetworkVariable<int> Id { get; } = new();

        public NetworkVariable<TeamType> Team { get; } = new();

        public NetworkVariable<int> AttackCount { get; } = new(1);

        public NetworkVariable<int> MoveCount { get; } = new(1);

        public bool CanAction => AttackCount.Value > 0 && MoveCount.Value > 0;

        [Inject]
        public void Construct(PlayersContainer playersContainer) => _playersContainer = playersContainer;

        public override void OnNetworkSpawn() => _playersContainer.Add(this);

        public override void OnNetworkDespawn() => _playersContainer.Remove(this);
    }
}