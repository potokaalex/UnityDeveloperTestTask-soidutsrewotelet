using System.Collections.Generic;

namespace Game.Code.Gameplay.Unit
{
    public class UnitsContainer
    {
        private readonly Dictionary<UnitController, TeamType> _units = new();

        public void Add(UnitController unit, TeamType team) => _units.Add(unit, team);

        public void Get(TeamType team, List<UnitController> outList)
        {
            outList.Clear();
            foreach (var unit in _units)
                if (unit.Value == team)
                    outList.Add(unit.Key);
        }

        public void Get(List<UnitController> outList)
        {
            outList.Clear();
            foreach (var unit in _units)
                outList.Add(unit.Key);
        }
    }
}