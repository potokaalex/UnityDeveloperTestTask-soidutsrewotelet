using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitView : MonoBehaviour
    {
        public GameObject SelectionIndicator;
        public GameObject CanBeAttackedIndicator;
        public UnitController Controller;
        private readonly CompositeDisposable _disposables = new();
        private UnitRangeView _rangeView;
        private UnitPathView _pathView;

        [Inject]
        public void Construct(UnitsSelector selector, UnitRangeView rangeView, UnitPathView unitPathView)
        {
            _pathView = unitPathView;
            _rangeView = rangeView;
        }

        public void Awake()
        {
            Controller.IsSelect.Skip(1).Where(x => x).Subscribe(_ => ViewSelect()).AddTo(_disposables);
            Controller.IsSelect.Skip(1).Where(x => !x).Subscribe(_ => ViewUnSelect()).AddTo(_disposables);
            Controller.PathPoints.Skip(1).Subscribe(_ => OnPathChanged()).AddTo(_disposables);
        }

        public void OnDestroy() => _disposables.Dispose();

        public void ShowCanBeAttacked() => CanBeAttackedIndicator.SetActive(true);

        public void HideCanBeAttacked() => CanBeAttackedIndicator.SetActive(false);

        private void OnPathChanged()
        {
            if (Controller.IsDestinationSet)
            {
                _pathView.View(Controller.PathPoints.Value);
                //_attackController.Set(point, AttackRange);
            }
            else
            {
                _pathView.Clear();
                //_attackController.Set(transform.position, AttackRange);
            }
        }

        private void ViewSelect()
        {
            SelectionIndicator.SetActive(true);
            _rangeView.ViewMove(Controller.Speed, transform.position);
        }

        private void ViewUnSelect()
        {
            SelectionIndicator.SetActive(false);
            _rangeView.ClearMove();
            _pathView.Clear();
        }
    }
}