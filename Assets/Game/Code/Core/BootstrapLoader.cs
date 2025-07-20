using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Code.Core
{
    public class BootstrapLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        public void Awake()
        {
            if (!FindObjectsOfType<BootstrapLoader>().Except(this).Any())
            {
                var bootSceneBindIndex = 0;

                if (SceneManager.GetActiveScene().buildIndex != bootSceneBindIndex)
                {
                    foreach (var m in FindObjectsOfType<Behaviour>())
                        if (m != this)
                            m.gameObject.SetActive(false);

                    SceneManager.LoadScene(bootSceneBindIndex);
                }

                DontDestroyOnLoad(this);
            }
        }
#endif
    }
}