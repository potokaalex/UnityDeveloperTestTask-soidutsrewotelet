using System;

namespace Game.Code.Gameplay.Obstacle
{
    [Serializable]
    public class ObstacleSpawnPreset
    {
        public int Type;
        public int MinCount;
        public int MaxCount;
    }
}