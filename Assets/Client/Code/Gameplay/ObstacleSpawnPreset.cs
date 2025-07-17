using System;

namespace Client.Code.Gameplay
{
    [Serializable]
    public class ObstacleSpawnPreset
    {
        public ObstacleController Prefab;
        public int MinCount;
        public int MaxCount;
    }
}