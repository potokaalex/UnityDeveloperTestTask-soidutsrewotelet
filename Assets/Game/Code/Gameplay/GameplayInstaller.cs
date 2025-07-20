using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit;
using Game.Code.Gameplay.Unit.View;
using Unity.Netcode;
using Zenject;

namespace Game.Code.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public MapController MapController;
        public CameraController CameraController;
        public PlayersSpawner PlayersSpawner;
        public MatchController MatchController;
        public HudWindow HudWindowPrefab;
        public UnitRangeView UnitRangeView;
        public UnitPathView UnitPathView;
        public EndWindow EndWindow;

        public override void InstallBindings()
        {
            Container.Bind<MapController>().FromInstance(MapController).AsSingle();
            Container.Bind<CameraController>().FromInstance(CameraController).AsSingle();
            BindPlayer();
            Container.Bind<MatchController>().FromInstance(MatchController).AsSingle();
            BindUnit();
            Container.BindInterfacesTo<EndWindow>().FromInstance(EndWindow).AsSingle();

            if (!NetworkManager.Singleton.IsServer)
                Container.BindInterfacesTo<HudWindow>().FromComponentInNewPrefab(HudWindowPrefab).AsSingle();
        }

        private void BindPlayer()
        {
            Container.Bind<PlayersSpawner>().FromInstance(PlayersSpawner).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayersContainer>()
                .AsSingle(); //no need to have a container on all clients, but I create all players on all clients.
        }

        private void BindUnit()
        {
            Container.Bind<UnitsContainer>().AsSingle();
            Container.Bind<UnitsSelector>().AsSingle();
            Container.Bind<UnitRangeView>().FromInstance(UnitRangeView).AsSingle();
            Container.Bind<UnitPathView>().FromInstance(UnitPathView).AsSingle();
        }
    }
}