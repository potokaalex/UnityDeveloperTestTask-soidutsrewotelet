namespace Game.Code.Gameplay.Unit
{
    public class UnitsSelector
    {
        private readonly MatchController _matchController;

        public UnitController Selected { get; private set; }

        public bool HasSelected => Selected;

        public void Select(UnitController unit)
        {
            Selected?.OnUnSelect();
            Selected = unit;
            Selected.OnSelect();
        }

        public void UnSelect(UnitController unit)
        {
            if (Selected == unit)
            {
                unit.OnUnSelect();
                Selected = null;
            }
        }
    }
}