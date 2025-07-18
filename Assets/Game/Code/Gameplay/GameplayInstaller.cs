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
        public PlayerController PlayerController;
        
        public override void InstallBindings()
        {
            BindUnit();
            Container.Bind<CameraController>().FromInstance(CameraController).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInput>().AsSingle();
            Container.Bind<PlayerController>().FromInstance(PlayerController).AsSingle();
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