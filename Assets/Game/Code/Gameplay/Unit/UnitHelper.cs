using Game.Code.Core;
using Game.Code.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitHelper : MonoBehaviour
    {
        public UnitModel Model;
        private IPlayerProvider _playerController;

        public bool IsDestinationValid => Model.PathPoints != null && Model.PathPoints.Length > 0;

        public bool IsEnemy => AreEnemies(_playerController.Player.Team, Model.Team);

        [Inject]
        public void Construct(IPlayerProvider playerProvider) => _playerController = playerProvider;

        public bool AreEnemies(TeamType team1, TeamType team2) => team1 != team2;

        public bool IsInRange(Vector3 center, float radius) =>
            MathExtensions.IsCirclesIntersect(center, radius, transform.position, Model.BodyRadius);
    }
}