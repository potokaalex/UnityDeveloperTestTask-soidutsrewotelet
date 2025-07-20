using System.Collections.Generic;
using System.Linq;
using Game.Code.Core;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitsSpawner : MonoBehaviour
    {
        public UnitsSpawnerPreset[] Presets;
        public List<UnitController> Prefabs;
        private Instantiator _instantiator;

        [Inject]
        public void Construct(Instantiator instantiator) => _instantiator = instantiator;

        public void Spawn()
        {
            foreach (var preset in Presets)
            foreach (var spawnPoint in preset.SpawnPoints)
                CreateUnit(preset, spawnPoint);
        }

        private void CreateUnit(UnitsSpawnerPreset preset, UnitSpawnPoint spawnPoint)
        {
            var prefab = Prefabs.First(x => x.Type == preset.Type).gameObject;
            var instance = _instantiator.InstantiatePrefabForComponent<UnitController>(prefab);
            instance.Initialize(spawnPoint.transform.position, spawnPoint.Team);
            instance.NetworkObject.Spawn();
            instance.NetworkObject.DontDestroyWithOwner = true;
        }
    }
}