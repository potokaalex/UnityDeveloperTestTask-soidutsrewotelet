using Unity.Netcode;

namespace Game.Code.Gameplay.Unit
{
    public class UnitController : NetworkBehaviour
    {
        public int Type;
        public float MoveDistance;
        public float AttackRange;
    }
}