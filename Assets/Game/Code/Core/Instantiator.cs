using UnityEngine;
using Zenject;

namespace Game.Code.Core
{
    public class Instantiator
    {
        private readonly DiContainer _container;

        public Instantiator(DiContainer container) => _container = container;

        public T InstantiatePrefabForComponent<T>(GameObject go) where T : Component
        {
            var wasActive = go.activeSelf;
            go.SetActive(false);
            var instance = Object.Instantiate(go);
            go.SetActive(wasActive);
            _container.InjectGameObject(instance);
            instance.SetActive(wasActive);
            return instance.GetComponentInChildren<T>();
        }
    }
}