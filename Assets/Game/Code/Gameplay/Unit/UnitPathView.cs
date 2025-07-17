using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

namespace Game.Code.Gameplay.Unit
{
    public class UnitPathView : MonoBehaviour
    {
        public float Width;
        public MeshFilter MeshFilter;

        public void View(NavMeshPath path)
        {
            using var d = ListPool<Vector3>.Get(out var points);
            points.AddRange(path.corners);
            MeshGenerator.GenerateStripMesh(MeshFilter.mesh, points, Width);
        }

        public void Clear() => MeshFilter.mesh.Clear();
    }
}