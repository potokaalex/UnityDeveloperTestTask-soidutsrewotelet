using Game.Code.Gameplay.Hud;
using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit;
using Game.Code.Gameplay.Unit.View;
using Zenject;

namespace Game.Code.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public CameraController CameraController;
        public UnitRangeView UnitRangeView;
        public UnitPathView UnitPathView;
        public MatchController MatchController;
        public HudWindow HudWindow;

        public override void InstallBindings()
        {
            BindUnit();
            Container.Bind<CameraController>().FromInstance(CameraController).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayersContainer>().AsSingle();
            Container.Bind<MatchController>().FromInstance(MatchController).AsSingle();
            Container.Bind<HudWindow>().FromInstance(HudWindow).AsSingle();
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