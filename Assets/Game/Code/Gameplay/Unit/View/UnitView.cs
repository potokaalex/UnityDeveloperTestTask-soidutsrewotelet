using System.Collections.Generic;
using SaintsField;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Unit.View
{
    public class UnitView : MonoBehaviour
    {
        public GameObject SelectionIndicator;
        public GameObject CanBeAttackedIndicator;
        public MeshRenderer MeshRenderer;
        public SaintsDictionary<TeamType, Material> MaterialsPerTeam;
        private UnitRangeView _rangeView;
        private UnitPathView _pathView;

        [Inject]
        public void Construct(UnitRangeView rangeView, UnitPathView pathView)
        {
            _pathView = pathView;
            _rangeView = rangeView;
        }

        public void Setup(TeamType team)
        {
            if (team != TeamType.None)
                MeshRenderer.sharedMaterial = MaterialsPerTeam[team];
        }

        public void ViewSelect() => SelectionIndicator.SetActive(true);

        public void ViewUnSelect() => SelectionIndicator.SetActive(false);

        public void ViewMove(Vector3 center, float speed) => _rangeView.ViewMove(center, speed);

        public void ClearMove()
        {
            _rangeView.ClearMove();
            _pathView.Clear();
        }

        public void OnDestinationChanged(bool value, Vector3[] pathPoints)
        {
            if (value)
                _pathView.View(pathPoints);
            else
                _pathView.Clear();
        }

        public void ViewAttack(Vector3 point, float radius, List<UnitController> forAttack, List<UnitController> enemies)
        {
            ClearAttack(enemies);
            foreach (var unit in forAttack)
                unit.View.CanBeAttackedIndicator.SetActive(true);

            _rangeView.ViewAttack(point, radius);
        }

        public void ClearAttack(List<UnitController> enemies)
        {
            foreach (var unit in enemies)
                unit.View.CanBeAttackedIndicator.SetActive(false);
            _rangeView.ClearAttack();
        }
    }
}