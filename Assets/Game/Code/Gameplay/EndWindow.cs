using System;
using Game.Code.Core;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay
{
    public class EndWindow : MonoBehaviour, IInitializable, IDisposable
    {
        public TextMeshProUGUI WinnerName;
        private readonly CompositeDisposable _disposables = new();
        private MatchController _matchController;

        [Inject]
        public void Construct(MatchController matchController) => _matchController = matchController;

        public void Initialize()
        {
            _matchController.WinnerNV.OnChangeAsObservable().Subscribe(_ =>
            {
                gameObject.SetActive(true);
                WinnerName.SetText($"Winner: {_matchController.Winner}");
            }).AddTo(_disposables);
        }

        public void Dispose() => _disposables.Dispose();
    }
}