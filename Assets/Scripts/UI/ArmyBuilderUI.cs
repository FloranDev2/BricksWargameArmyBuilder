using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Localization;
using Truelch.Managers;
using Truelch.ScriptableObjects;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmyBuilderUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [Header("Top")]
        [SerializeField] private TextLocSO _integrationValLoc;
        [SerializeField] private TextMeshProUGUI _integrationTxt;

        [Header("Bot - Units")]
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
            GameManager.onUnitClassChanged += OnUnitClassChanged;
        }

        private void OnDisable()
        {
            GameManager.onUnitAdded -= OnUnitAdded;
        }

        private void OnUnitAdded(UnitData unitData)
        {
            var unitElem = Instantiate(_unitElemPrefab, _unitElemWrapper);
            unitElem.Init(this, unitData);
        }

        private void OnUnitClassChanged(int unitIndex, UnitData newClass)
        {

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

            Language language = _gameMgr.GetCurrentLanguage();

            string export = "";
            foreach (UnitData unit in _gameMgr.ArmyUnits)
            {
                string unitName = "";
                foreach (var locName in unit.LocNames)
                {
                    if (locName.Language == language)
                    {
                        unitName = locName.Txt;
                    }
                }

                if (!string.IsNullOrEmpty(unit.CurrentName) && unit.CurrentName != unitName)
                {
                    export += "\n- " + unit.CurrentName + " (" + unitName + ")";
                }
                else
                {
                    export += "\n- " + unitName;
                }
                
                foreach (var gear in unit.GearList)
                {
                    if (gear != null)
                    {
                        export += " " + gear.ExportString;
                    }                    
                }
            }

            Utils.Clipboard = export;

            Debug.Log("Exported: " + export);
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