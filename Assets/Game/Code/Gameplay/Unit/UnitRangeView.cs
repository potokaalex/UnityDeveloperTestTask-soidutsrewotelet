using UnityEngine;

namespace Game.Code.Gameplay.Unit
{
    public class UnitRangeView : MonoBehaviour
    {
        public MeshFilter MeshFilter;

        public void View(float radius, Vector3 position)
        {
            var mesh = new Mesh();
            MeshGenerator.GenerateCircleMesh(mesh, radius);
            MeshFilter.mesh = mesh;
            position.y += 0.01f;
            transform.position = position;
        }
    }
}