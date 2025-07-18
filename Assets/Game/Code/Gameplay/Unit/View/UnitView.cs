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
            if(team != TeamType.None)
                MeshRenderer.sharedMaterial = MaterialsPerTeam[team];
        }

        public void ViewSelect(Vector3 center, float speed)
        {
            SelectionIndicator.SetActive(true);
            _rangeView.ViewMove(center, speed);
        }

        public void ViewUnSelect(List<UnitController> enemies)
        {
            SelectionIndicator.SetActive(false);
            _rangeView.ClearMove();
            _pathView.Clear();
            ClearAttack(enemies);
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

        private void ClearAttack(List<UnitController> enemies)
        {
            foreach (var unit in enemies)
                unit.View.CanBeAttackedIndicator.SetActive(false);
            _rangeView.ClearAttack();
        }
    }
}