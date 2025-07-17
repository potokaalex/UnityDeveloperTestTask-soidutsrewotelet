namespace Game.Code.Gameplay.Unit
{
    public class UnitsSelector
    {
        private UnitController _selected;

        public void Select(UnitController controller)
        {
            _selected?.OnUnSelect();
            _selected = controller;
            _selected.OnSelect();
        }
    }
}