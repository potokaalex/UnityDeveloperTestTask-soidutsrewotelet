using UnityEngine;

namespace Game.Code.Gameplay
{
    public static class MeshGenerator
    {
        public static void GenerateCircleMesh(Mesh mesh, float outerRadius, float innerRadius = 0, int segments = 64)
        {
            mesh.Clear();
            var vertexCount = (segments + 1) * 2; // 2 vertices per segment (inner and outer), plus 2 for closure
            var vertices = new Vector3[vertexCount];
            var triangles = new int[segments * 6]; //2 triangles per segment
            var angleStep = 2 * Mathf.PI / segments;

            //create vertices in a circle
            for (var i = 0; i <= segments; i++)
            {
                var angle = i * angleStep;
                var cos = Mathf.Cos(angle);
                var sin = Mathf.Sin(angle);

                //vertices on the inner and outer radius
                vertices[i * 2] = new Vector3(cos * innerRadius, 0, sin * innerRadius);
                vertices[i * 2 + 1] = new Vector3(cos * outerRadius, 0, sin * outerRadius);
            }

            //create triangles between pairs of vertices
            var tri = 0;
            for (var i = 0; i < segments; i++)
            {
                var i0 = i * 2;
                var i1 = i * 2 + 1;
                var i2 = i * 2 + 2;
                var i3 = i * 2 + 3;

                //first triangle
                triangles[tri++] = i0;
                triangles[tri++] = i3;
                triangles[tri++] = i1;

                //second triangle
                triangles[tri++] = i0;
                triangles[tri++] = i2;
                triangles[tri++] = i3;
            }

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }
    }
}