using UnityEngine;

namespace Game.Code.Core
{
    public static class MathUtilities
    {
        public static bool IsCirclesIntersect(Vector3 centerA, float radiusA, Vector3 centerB, float radiusB)
        {
            var dx = centerA.x - centerB.x;
            var dz = centerA.z - centerB.z;
            var distanceSquared = dx * dx + dz * dz;
            var sumRadius = radiusA + radiusB;
            return distanceSquared <= sumRadius * sumRadius;
        }
    }
}