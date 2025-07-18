using System.Collections.Generic;
using System.Linq;

namespace Game.Code.Gameplay.Player
{
    public class PlayersContainer : ICurrentPlayerProvider
    {
        private readonly List<PlayerController> _players = new();

        public PlayerController Player => _players.First(x => x.IsOwner);

        public void Add(PlayerController player) => _players.Add(player);

        public void Remove(PlayerController player) => _players.Remove(player);

        public void Get(List<PlayerController> outList)
        {
            outList.Clear();
            outList.AddRange(_players);
        }

        public bool TryGet(TeamType team, out PlayerController outPlayer)
        {
            foreach (var player in _players)
            {
                if (player.Team.Value == team)
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