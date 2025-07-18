using Game.Code.Gameplay.Hud;
using UniRx;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Code.MainMenu
{
    public class MainMenuWindow : MonoBehaviour
    {
        public Button StartServerButton;
        public Button StartClientButton;
        public Button ExitButton;
        private readonly CompositeDisposable _disposables = new();
        private HudWindow _hudWindow;

        [Inject]
        public void Construct(HudWindow hudWindow) => _hudWindow = hudWindow;

        public void OnEnable()
        {
            StartServerButton.OnClickAsObservable().Subscribe(_ => StartServer()).AddTo(_disposables);
            StartClientButton.OnClickAsObservable().Subscribe(_ => StartClient()).AddTo(_disposables);
            ExitButton.OnClickAsObservable().Subscribe(_ => { Exit(); }).AddTo(_disposables);
        }

        public void OnDisable() => _disposables.Dispose();

        private void StartServer()
        {
            NetworkManager.Singleton.StartServer();
            Close();
        }

        private void StartClient()
        {
            NetworkManager.Singleton.StartClient();
            Close();
        }

        private void Close()
        {
            gameObject.SetActive(false);
            _hudWindow.Show();
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