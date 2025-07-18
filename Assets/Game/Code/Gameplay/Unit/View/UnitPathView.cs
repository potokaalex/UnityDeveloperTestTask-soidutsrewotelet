using Game.Code.Core;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Code.Gameplay.Unit.View
{
    public class UnitPathView : MonoBehaviour
    {
        public float Width;
        public MeshFilter MeshFilter;

        public void View(Vector3[] path)
        {
            using var d = ListPool<Vector3>.Get(out var points);
            points.AddRange(path);
            MeshGenerator.GenerateStripMesh(MeshFilter.mesh, points, Width);
        }

        public void Clear() => MeshFilter.mesh.Clear();
    }
}