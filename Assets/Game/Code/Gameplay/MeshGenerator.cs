using System.Collections.Generic;
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

            CreateTrianglesBetweenPairs(segments, triangles);
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }


        public static void GenerateStripMesh(Mesh mesh, List<Vector3> points, float width)
        {
            mesh.Clear();

            if (points.Count < 2)
                return;

            var count = points.Count;
            var vertices = new Vector3[count * 2];
            var triangles = new int[(count - 1) * 6];

            for (var i = 0; i < count; i++)
            {
                Vector3 forward;

                //calculating the direction of the segment:
                if (i == 0)
                {
                    //for the first point, we are looking at the direction to the next one
                    forward = (points[i + 1] - points[i]).normalized;
                }
                else if (i == count - 1)
                {
                    //for the last point — from the previous one
                    forward = (points[i] - points[i - 1]).normalized;
                }
                else
                {
                    //for intermediate ones, we average the directions before and after
                    var dir1 = (points[i] - points[i - 1]).normalized;
                    var dir2 = (points[i + 1] - points[i]).normalized;
                    forward = ((dir1 + dir2) * 0.5f).normalized;
                }

                //get a vector perpendicular to the direction of motion in the XZ plane
                var side = Vector3.Cross(forward, Vector3.up).normalized * (width * 0.5f);

                //adding 2 vertices: the left and the right relative to the current point
                vertices[i * 2] = points[i] - side;
                vertices[i * 2 + 1] = points[i] + side;
            }

            CreateTrianglesBetweenPairs(count - 1, triangles, false);
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
        }

        private static void CreateTrianglesBetweenPairs(int count, int[] triangles, bool flip = true)
        {
            var tri = 0;
            for (var i = 0; i < count; i++)
            {
                var i0 = i * 2;
                var i1 = i * 2 + 1;
                var i2 = i * 2 + 2;
                var i3 = i * 2 + 3;

                if (flip) //normals down
                {
                    triangles[tri++] = i0;
                    triangles[tri++] = i3;
                    triangles[tri++] = i1;

                    triangles[tri++] = i0;
                    triangles[tri++] = i2;
                    triangles[tri++] = i3;
                }
                else
                {
                    triangles[tri++] = i0;
                    triangles[tri++] = i1;
                    triangles[tri++] = i3;

                    triangles[tri++] = i0;
                    triangles[tri++] = i3;
                    triangles[tri++] = i2;
                }
            }
        }
    }
}