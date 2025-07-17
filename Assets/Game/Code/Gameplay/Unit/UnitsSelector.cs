namespace Game.Code.Gameplay.Unit
{
    public class UnitsSelector
    {
        public UnitController Selected { get; private set; }

        public void Select(UnitController controller)
        {
            Selected?.OnUnSelect();
            Selected = controller;
            Selected.OnSelect();
        }
    }
}