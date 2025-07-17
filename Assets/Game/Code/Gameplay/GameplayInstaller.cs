using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit;
using Zenject;

namespace Game.Code.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public CameraController CameraController;

        public override void InstallBindings()
        {
            Container.Bind<UnitsContainer>().AsSingle();
            Container.Bind<UnitsSelector>().AsSingle();
            Container.Bind<CameraController>().FromInstance(CameraController).AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerRaycaster>().AsSingle();
        }
    }
}