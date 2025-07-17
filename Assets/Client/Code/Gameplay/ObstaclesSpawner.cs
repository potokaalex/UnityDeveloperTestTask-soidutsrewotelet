using UnityEngine;
using Random = UnityEngine.Random;

namespace Client.Code.Gameplay
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        public ObstacleSpawnPreset[] Presets;
        public float AreaSize;

        public void Awake()
        {
            foreach (var preset in Presets)
                CreateObstacles(preset);
        }

        private void CreateObstacles(ObstacleSpawnPreset preset)
        {
            for (var i = 0; i < Random.Range(preset.MinCount, preset.MaxCount + 1); i++)
            {
                var halfSize = AreaSize / 2f;
                var position = transform.position + new Vector3(Random.Range(-halfSize, halfSize), 0, Random.Range(-halfSize, halfSize));
                var instance = Instantiate(preset.Prefab.gameObject, transform);
                instance.transform.position = position;
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(AreaSize, 0, AreaSize));
            Gizmos.color = Color.white;
        }
    }
}