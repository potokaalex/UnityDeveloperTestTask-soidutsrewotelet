using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace Game.Code.Gameplay.Unit
{
    public class UnitsSpawner : NetworkBehaviour
    {
        public UnitsSpawnerPreset[] Presets;
        public List<UnitController> Prefabs;
        private int _unitId;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                foreach (var preset in Presets)
                foreach (var spawnPoint in preset.SpawnPoints)
                    CreateUnit(preset, spawnPoint);
            }
        }

        private void CreateUnit(UnitsSpawnerPreset preset, UnitSpawnPoint spawnPoint)
        {
            var prefab = Prefabs.First(x => x.Type == preset.Type);
            var instance = Instantiate(prefab);
            instance.NetworkObject.Spawn();
            instance.transform.position = spawnPoint.transform.position;
            instance.Team.Value = spawnPoint.Team;
            instance.Id.Value = _unitId++;
        }
    }
}