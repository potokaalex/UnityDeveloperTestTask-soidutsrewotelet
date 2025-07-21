using Game.Code.Core.Network;
using Game.Code.Gameplay.Player;
using Unity.Netcode;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitSelectionController : NetworkBehaviour
    {
        public UnitModel Model;
        public UnitHelper Helper;
        private MatchController _matchController;
        private PlayersContainer _playersContainer;
        private UnitsSelector _unitsSelector;
        private UnitController _unit;

        [Inject]
        public void Construct(MatchController matchController, PlayersContainer playersContainer, UnitsSelector unitsSelector)
        {
            _matchController = matchController;
            _playersContainer = playersContainer;
            _unitsSelector = unitsSelector;
        }

        public void Select(UnitController unit)
        {
            if (!IsServer && IsOwner)
            {
                _unit = unit;
                SelectServerRpc();
            }
        }

        [ServerRpc]
        public void OnSelectServerRpc() => Model.NavMeshObstacle.enabled = false;

        [ServerRpc]
        public void OnUnSelectServerRpc() => Model.NavMeshObstacle.enabled = true;

        [ServerRpc]
        private void SelectServerRpc(ServerRpcParams rpcParams = default)
        {
            var sender = rpcParams.Receive.SenderClientId;
            if (_playersContainer.TryGet(sender, out var player))
                if (!Helper.AreEnemies(player.Team, Model.Team) && _matchController.IsHisTurn(player))
                    SelectClientRpc(new ClientRpcParams().For(sender));
        }

        [ClientRpc]
        private void SelectClientRpc(ClientRpcParams _) => _unitsSelector.Select(_unit);
    }
}