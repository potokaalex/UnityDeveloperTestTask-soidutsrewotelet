using UnityEngine;
using Zenject;

namespace Game.Code.Core
{
    public class Instantiator
    {
        private readonly DiContainer _container;

        public Instantiator(DiContainer container) => _container = container;

        public T InstantiatePrefabForComponent<T>(GameObject gameObject) where T : Component =>
            InstantiatePrefabForComponent<T>(_container, gameObject);

        public T InstantiatePrefabForComponent<T>(DiContainer container, GameObject gameObject) where T : Component
        {
            var wasActive = gameObject.activeSelf;
            gameObject.SetActive(false);
            var instance = Object.Instantiate(gameObject);
            gameObject.SetActive(wasActive);
            container.InjectGameObject(instance);
            instance.SetActive(wasActive);
            return instance.GetComponentInChildren<T>();
        }
    }
}