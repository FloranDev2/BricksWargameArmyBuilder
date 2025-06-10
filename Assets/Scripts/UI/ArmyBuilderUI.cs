using System;
using System.Collections;
using System.Collections.Generic;
using Truelch.Managers;
using Truelch.ScriptableObjects;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmyBuilderUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        //[SerializeField] private UnitExpBtn _unitElemPrefab; //Old
        [SerializeField] private UnitElem _unitElemPrefab; //New
        [SerializeField] private Transform _unitElemWrapper;

        //Hidden
        // - Managers
        private GameManager _gameMgr;

        // - Misc
        private bool _isReady = false;

        // - Units
        //private List<UnitExpBtn> _unitElems = new List<UnitExpBtn>();
        private List<UnitElem> _unitElems = new List<UnitElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance);
            _gameMgr = GameManager.Instance;
            _isReady = true;
        }
        #endregion Initialization

        #region Delegate Event
        private void OnEnable()
        {
            GameManager.onUnitAdded += OnUnitAdded;
        }

        private void OnDisable()
        {
            GameManager.onUnitAdded -= OnUnitAdded;
        }

        private void OnUnitAdded(UnitSO unitSO)
        {
            var unitElem = Instantiate(_unitElemPrefab, _unitElemWrapper);
            unitElem.Init(this, unitSO);
        }
        #endregion Delegate Event

        #region Public
        //Options
        public void OnSaveClick()
        {
            if (!_isReady) return;
            _gameMgr.OnSaveClick();
        }

        public void OnPrintExportClick()
        {
            if (!_isReady) return;
            //TODO
        }

        //public void OnRemoveUnitClick(UnitExpBtn btn)
        public void OnRemoveUnitClick(UnitElem btn)
        {
            _unitElems.Remove(btn);

            //_gameMgr.AddUnit
        }
        #endregion Public

        #endregion METHODS
    }
}