using UnityEngine;

namespace Game.Code.Gameplay.Unit
{
    public class UnitRangeView : MonoBehaviour
    {
        public MeshFilter MoveMeshFilter;
        public MeshFilter AttackMeshFilter;
        public float CircleThickness;
        
        public void ViewMove(float radius, Vector3 center) => View(MoveMeshFilter, radius, center);

        public void ViewAttack(float radius, Vector3 center) => View(AttackMeshFilter, radius, center);

        private void View(MeshFilter meshFilter, float radius, Vector3 position)
        {
            MeshGenerator.GenerateCircleMesh(meshFilter.mesh, radius, radius - CircleThickness);
            position.y += 0.01f;
            transform.position = position;
        }

        public void ClearMove() => MoveMeshFilter.mesh.Clear();

        public void ClearAttack() => AttackMeshFilter.mesh.Clear();
    }
}