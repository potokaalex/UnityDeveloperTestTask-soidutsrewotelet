using System.Collections.Generic;

namespace Game.Code.Gameplay.Unit
{
    public class UnitsContainer
    {
        private readonly List<UnitController> _units = new();

        public void Add(UnitController unit) => _units.Add(unit);

        public void Remove(UnitController unit) => _units.Remove(unit);

        public void Get(List<UnitController> outList)
        {
            outList.Clear();
            foreach (var unit in _units)
                outList.Add(unit);
        }

        public void Get(TeamType team, List<UnitController> outList)
        {
            outList.Clear();
            foreach (var unit in _units)
                if (unit.Team == team)
                    outList.Add(unit);
        }

        public UnitController Get(ulong id)
        {
            foreach (var unit in _units)
                if (unit.Id == id)
                    return unit;
            return null;
        }
    }
}