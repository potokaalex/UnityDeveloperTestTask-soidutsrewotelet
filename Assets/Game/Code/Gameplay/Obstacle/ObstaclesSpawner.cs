using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Game.Code.Gameplay.Obstacle
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        public List<ObstacleController> Prefabs;
        public ObstacleSpawnPreset[] Presets;
        public float AreaSize;

        public void Spawn(int seed)
        {
            var random = new System.Random(seed);
            foreach (var preset in Presets)
                CreateObstacles(preset, random);
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(AreaSize, 0, AreaSize));
            Gizmos.color = Color.white;
        }

        private void CreateObstacles(ObstacleSpawnPreset preset, System.Random random)
        {
            var count = random.Next(preset.MinCount, preset.MaxCount);

            for (var i = 0; i < count; i++)
            {
                var halfSize = AreaSize / 2f;
                var position = transform.position + new Vector3(NextFloat(random, -halfSize, halfSize), 0, NextFloat(random, -halfSize, halfSize));
                CreateObstacle(preset.Type, position);
            }
        }

        private void CreateObstacle(int type, Vector3 position)
        {
            var prefab = Prefabs.First(x => x.Type == type);
            var instance = Instantiate(prefab, transform);
            instance.transform.position = position;
        }

        private float NextFloat(System.Random random, float min, float max) => (float)random.NextDouble() * (max - min) + min;
    }
}