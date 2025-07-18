using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Code.Gameplay.Unit
{
    /*
    public class UnitAttackController
    {
        private readonly List<UnitView> _unitsForAttack = new();
        private readonly UnitsContainer _container;
        private readonly UnitRangeView _rangeView;

        public UnitAttackController(UnitsContainer container, UnitRangeView rangeView)
        {
            _rangeView = rangeView;
            _container = container;
        }

        public void Set(Vector3 center, float radius)
        {
            using var d = ListPool<UnitView>.Get(out var units);
            _container.Get(units);

            _unitsForAttack.Clear();
            foreach (var unit in units)
            {
                if (unit.IsEnemy && unit.IsInRange(center, radius))
                {
                    unit.ShowCanBeAttacked();
                    _unitsForAttack.Add(unit);
                }
            }

            _rangeView.ViewAttack(radius, center);
        }

        public void Clear()
        {
            using var d = ListPool<UnitView>.Get(out var units);
            _container.Get(units);
            foreach (var unit in units)
                if (unit.IsEnemy)
                    unit.HideCanBeAttacked();
            _rangeView.ClearAttack();
        }
    }
    */
}