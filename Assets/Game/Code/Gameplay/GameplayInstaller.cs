using Game.Code.Gameplay.Unit;
using Zenject;

namespace Game.Code.Gameplay
{
    public class GameplayInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<UnitsContainer>().AsSingle();
        }
    }
}