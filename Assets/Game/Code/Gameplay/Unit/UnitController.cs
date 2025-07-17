using Game.Code.Gameplay.Player;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitController : NetworkBehaviour, IPlayerInteractive
    {
        public int Type;
        public float MoveDistance;
        public float AttackRange;
        public GameObject SelectionIndicator;
        private UnitsSelector _selector;
        
        [Inject]
        public void Construct(UnitsSelector selector) => _selector = selector;

        public void Interact()
        {
            if (IsOwner)
                _selector.Select(this);
        }

        public void OnSelect() => SelectionIndicator.SetActive(true);

        public void OnUnSelect() => SelectionIndicator.SetActive(false);
    }
}