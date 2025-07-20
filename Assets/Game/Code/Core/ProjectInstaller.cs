using Game.Code.Core.Network;
using Game.Code.Core.Network.LifeTime;
using Zenject;

namespace Game.Code.Core
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<Instantiator>().AsSingle().CopyIntoAllSubContainers();
            BindNetwork();
        }

        private void BindNetwork()
        {
            Container.BindInterfacesTo<NetworkController>().AsSingle();
            Container.BindInterfacesTo<NetworkPrefabsController>().AsSingle();
            Container.BindInterfacesTo<NetworkLifetimeController>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}