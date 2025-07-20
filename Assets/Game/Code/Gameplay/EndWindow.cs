using Game.Code.Core;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay
{
    public class EndWindow : MonoBehaviour
    {
        public TextMeshProUGUI WinnerName;
        private readonly CompositeDisposable _disposables = new();
        private MatchController _matchController;

        [Inject]
        public void Construct(MatchController matchController) => _matchController = matchController;

        public void Awake()
        {
            gameObject.SetActive(false);
            _matchController.WinnerNV.OnChangeAsObservable().Subscribe(_ =>
            {
                gameObject.SetActive(true);
                WinnerName.SetText($"Winner: {_matchController.Winner}");
            }).AddTo(_disposables);
        }

        public void OnDestroy() => _disposables.Dispose();
    }
}