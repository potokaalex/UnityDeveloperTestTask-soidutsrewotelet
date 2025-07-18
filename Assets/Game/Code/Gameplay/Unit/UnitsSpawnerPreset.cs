using System;

namespace Game.Code.Gameplay.Unit
{
    [Serializable]
    public class UnitsSpawnerPreset
    {
        public UnitSpawnPoint[] SpawnPoints;
        public int Type;
    }
}