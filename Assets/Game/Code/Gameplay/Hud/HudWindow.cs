using System;
using Game.Code.Core;
using Game.Code.Gameplay.Player;
using TMPro;
using UniRx;
using Unity.Netcode;
using Zenject;

namespace Game.Code.Gameplay.Hud
{
    public class HudWindow : NetworkBehaviour
    {
        public TextMeshProUGUI TurnNumber;
        public TextMeshProUGUI TurnName;
        public TextMeshProUGUI TurnTime;
        public TextMeshProUGUI AttackCount;
        public TextMeshProUGUI MoveCount;
        private readonly CompositeDisposable _disposables = new();
        private MatchController _matchController;
        private ICurrentPlayerProvider _playerController;

        [Inject]
        public void Construct(MatchController matchController, ICurrentPlayerProvider playerProvider)
        {
            _playerController = playerProvider;
            _matchController = matchController;
        }

        public override void OnNetworkSpawn()
        {
            _matchController.TurnNumber.OnChangeAsObservable().Subscribe(_ => ViewTurnNumber()).AddTo(_disposables);
            _matchController.CurrentTeam.OnChangeAsObservable().Subscribe(_ => ViewTurnName()).AddTo(_disposables);
            _matchController.TurnTime.OnChangeAsObservable().Subscribe(_ => ViewTurnTime()).AddTo(_disposables);
            if (!NetworkManager.Singleton.IsServer)
            {
                _playerController.Player.AttackCount.OnChangeAsObservable().Subscribe(_ => ViewAttackCount()).AddTo(_disposables);
                _playerController.Player.MoveCount.OnChangeAsObservable().Subscribe(_ => ViewMoveCount()).AddTo(_disposables);
            }
        }

        public override void OnNetworkDespawn() => _disposables.Dispose();

        public void Show() => gameObject.SetActive(true);

        private void ViewMoveCount() => MoveCount.SetText($"Move: {_playerController.Player.MoveCount.Value}");

        private void ViewAttackCount() => AttackCount.SetText($"Attack: {_playerController.Player.AttackCount.Value}");

        private void ViewTurnNumber() => TurnNumber.SetText($"Turn: {_matchController.TurnNumber}");

        private void ViewTurnName() => TurnName.SetText(_matchController.IsMyTurn ? "YourTurn" : "OtherTurn");

        private void ViewTurnTime()
        {
            var time = TimeSpan.FromSeconds(_matchController.TurnTime.Value);
            TurnTime.SetText($"{time.Minutes:D2}:{time.Seconds:D2}");
        }
    }
}