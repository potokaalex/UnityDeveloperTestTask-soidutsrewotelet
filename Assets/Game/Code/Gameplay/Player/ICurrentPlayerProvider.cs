namespace Game.Code.Gameplay.Player
{
    public interface ICurrentPlayerProvider
    {
        PlayerController Player { get; }
    }
}