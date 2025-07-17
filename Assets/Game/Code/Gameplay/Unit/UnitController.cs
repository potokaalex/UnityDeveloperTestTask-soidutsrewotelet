using Game.Code.Gameplay.Player;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitController : NetworkBehaviour, IPlayerInteractive
    {
        public int Type;
        public float MoveRange;
        public float AttackRange;
        public GameObject SelectionIndicator;
        private UnitsSelector _selector;
        private UnitRangeView _rangeView;

        [Inject]
        public void Construct(UnitsSelector selector, UnitRangeView unitRangeView)
        {
            _rangeView = unitRangeView;
            _selector = selector;
        }

        public void Interact()
        {
            if (IsOwner)
                _selector.Select(this);
        }

        public void OnSelect()
        {
            SelectionIndicator.SetActive(true);
            _rangeView.View(MoveRange, transform.position);
        }

        public void OnUnSelect() => SelectionIndicator.SetActive(false);
    }
}