using Zenject;

namespace Game.Code.Core
{
    public class CustomSceneContext : SceneContext
    {
        protected override void RunInternal()
        {
            base.RunInternal();
            GetComponent<SceneKernel>().Start(); //force initialization in Awake(), because network events arrive before Start().
        }
    }
}