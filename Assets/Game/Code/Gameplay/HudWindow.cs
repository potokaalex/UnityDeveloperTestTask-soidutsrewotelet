using System;
using Game.Code.Core;
using Game.Code.Gameplay.Player;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game.Code.Gameplay
{
    public class HudWindow : MonoBehaviour, IInitializable, IDisposable
    {
        public TextMeshProUGUI TurnNumber;
        public TextMeshProUGUI TurnName;
        public TextMeshProUGUI TurnTime;
        public TextMeshProUGUI AttackCount;
        public TextMeshProUGUI MoveCount;
        public Button FinishTurnButton;
        private readonly CompositeDisposable _disposables = new();
        private MatchController _matchController;
        private IPlayerProvider _playerProvider;

        [Inject]
        public void Construct(MatchController matchController, IPlayerProvider playerProvider)
        {
            _playerProvider = playerProvider;
            _matchController = matchController;
        }

        public void Initialize()
        {
            _matchController.TurnNumberNV.OnChangeAsObservable().Subscribe(_ => ViewTurnNumber()).AddTo(_disposables);
            ViewTurnNumber();
            _matchController.CurrentTeamNV.OnChangeAsObservable().Subscribe(_ => ViewTurnName()).AddTo(_disposables);
            _playerProvider.PlayerRP.Skip(1).Subscribe(_ => ViewTurnName()).AddTo(_disposables);
            ViewTurnName();
            _matchController.TurnTimeNV.OnChangeAsObservable().Subscribe(_ => ViewTurnTime()).AddTo(_disposables);
            ViewTurnTime();
            _playerProvider.PlayerRP.Select(p => p?.AttackCountNV.OnChangeAsObservable() ?? Observable.Empty<UniRx.Unit>()).Switch()
                .Subscribe(_ => ViewAttackCount()).AddTo(_disposables);
            _playerProvider.PlayerRP.Select(p => p?.MoveCountNV.OnChangeAsObservable() ?? Observable.Empty<UniRx.Unit>()).Switch()
                .Subscribe(_ => ViewMoveCount()).AddTo(_disposables);

            _matchController.CurrentTeamNV.OnChangeAsObservable()
                .Subscribe(_ => FinishTurnButton.gameObject.SetActive(_matchController.IsMyTurn)).AddTo(_disposables);
            FinishTurnButton.OnClickAsObservable().Subscribe(_ => _matchController.FinishTurn()).AddTo(_disposables);
        }

        public void Dispose() => _disposables?.Dispose();

        private void ViewMoveCount() => MoveCount.SetText($"Move: {_playerProvider.Player.MoveCount}");

        private void ViewAttackCount() => AttackCount.SetText($"Attack: {_playerProvider.Player.AttackCount}");

        private void ViewTurnNumber() => TurnNumber.SetText($"Turn: {_matchController.TurnNumber}");

        private void ViewTurnName() => TurnName.SetText(_matchController.IsMyTurn ? "YourTurn" : "OtherTurn");

        private void ViewTurnTime()
        {
            var time = TimeSpan.FromSeconds(_matchController.TurnTime);
            TurnTime.SetText($"{time.Minutes:D2}:{time.Seconds:D2}");
        }
    }
}