using System.Collections.Generic;
using UniRx;

namespace Game.Code.Gameplay.Player
{
    public class PlayersContainer : IPlayerProvider
    {
        private readonly List<PlayerController> _players = new();

        public PlayerController Player { get => PlayerRP.Value; private set => PlayerRP.Value = value; }

        public ReactiveProperty<PlayerController> PlayerRP { get; } = new();

        public void Add(PlayerController player)
        {
            _players.Add(player);
            if (player.IsOwner) 
                Player = player;
        }

        public void Remove(PlayerController player)
        {
            _players.Remove(player);
            if (player == Player) 
                Player = null;
        }

        public void Get(List<PlayerController> outList)
        {
            outList.Clear();
            outList.AddRange(_players);
        }

        public bool TryGet(TeamType team, out PlayerController outPlayer)
        {
            foreach (var player in _players)
            {
                if (player.Team == team)
                {
                    outPlayer = player;
                    return true;
                }
            }

            outPlayer = null;
            return false;
        }
    }
}