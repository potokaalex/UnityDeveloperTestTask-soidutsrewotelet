using Game.Code.Core;
using UnityEngine;

namespace Game.Code.Gameplay.Unit.View
{
    public class UnitRangeView : MonoBehaviour
    {
        public MeshFilter MoveMeshFilter;
        public MeshFilter AttackMeshFilter;
        public float CircleThickness;
        
        public void ViewMove(Vector3 center, float radius) => View(MoveMeshFilter, radius, center);

        public void ViewAttack(Vector3 center, float radius) => View(AttackMeshFilter, radius, center);

        private void View(MeshFilter meshFilter, float radius, Vector3 position)
        {
            MeshGenerator.GenerateCircleMesh(meshFilter.mesh, radius, radius - CircleThickness);
            position.y += 0.01f;
            meshFilter.transform.position = position;
        }

        public void ClearMove() => MoveMeshFilter.mesh.Clear();

        public void ClearAttack() => AttackMeshFilter.mesh.Clear();
    }
}