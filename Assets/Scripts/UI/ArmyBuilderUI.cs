using System;
using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Managers;
using Truelch.ScriptableObjects;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmyBuilderUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private UnitElem _unitElemPrefab;
        [SerializeField] private Transform _unitElemWrapper;

        //Hidden
        // - Managers
        private GameManager _gameMgr;

        // - Misc
        private bool _isReady = false;

        // - Units
        [SerializeField] private List<UnitElem> _unitElems = new List<UnitElem>();
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

        //private void OnUnitAdded(UnitSO unitSO)
        private void OnUnitAdded(UnitData unitData)
        {
            Debug.Log("OnUnitAdded");
            var unitElem = Instantiate(_unitElemPrefab, _unitElemWrapper);
            //unitElem.Init(this, unitData);
            unitElem.Init(this, unitData);
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

        public void OnRemoveUnitClick(UnitElem btn)
        {
            _gameMgr.RemoveUnit(btn.UnitData);
            _unitElems.Remove(btn);
            Destroy(btn.gameObject);
        }
        #endregion Public

        #endregion METHODS
    }
}