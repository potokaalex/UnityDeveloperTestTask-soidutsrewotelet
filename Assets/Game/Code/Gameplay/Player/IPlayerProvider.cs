using UniRx;

namespace Game.Code.Gameplay.Player
{
    public interface IPlayerProvider
    {
        public PlayerController Player { get; }

        public ReactiveProperty<PlayerController> PlayerRP { get; }
    }
}