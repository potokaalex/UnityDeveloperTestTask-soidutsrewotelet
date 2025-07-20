using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Code.Gameplay.Unit
{
    public class UnitModel : NetworkBehaviour
    {
        public int Type;
        public float Speed;
        public float AttackRadius;
        public float BodyRadius = 0.125f;
        public float PositionVelocity = 5;
        public NavMeshObstacle NavMeshObstacle;

        public ulong Id => NetworkObjectId;

        public NetworkVariable<TeamType> TeamNV { get; } = new();

        public TeamType Team => TeamNV.Value;

        public Vector3[] PathPoints { get; set; }
    }
}