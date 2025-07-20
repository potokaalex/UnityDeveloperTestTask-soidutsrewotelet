using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Code.MainMenu
{
    public class MainMenuWindow : MonoBehaviour
    {
        public Button StartServerButton;
        public Button StartClientButton;
        public Button ExitButton;
        private readonly CompositeDisposable _disposables = new();

        public void OnEnable()
        {
            StartServerButton.OnClickAsObservable().Subscribe(_ => StartServer()).AddTo(_disposables);
            StartClientButton.OnClickAsObservable().Subscribe(_ => StartClient()).AddTo(_disposables);
            ExitButton.OnClickAsObservable().Subscribe(_ => { Exit(); }).AddTo(_disposables);
        }

        public void OnDisable() => _disposables.Dispose();

        private void StartServer()
        {
            NetworkManager.Singleton.OnServerStarted += LoadGameplay;
            NetworkManager.Singleton.StartServer();

            void LoadGameplay()
            {
                NetworkManager.Singleton.SceneManager.LoadScene("Gameplay", LoadSceneMode.Single);
                NetworkManager.Singleton.OnServerStarted -= LoadGameplay;
            }
        }

        private void StartClient()
        {
            if (!NetworkManager.Singleton.IsServer && !NetworkManager.Singleton.IsClient)
                NetworkManager.Singleton.StartClient();
        }

        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}