using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Enums;
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
        [SerializeField] private TextMeshProUGUI _integrationTxt;
        [SerializeField] private TextLocSO _integrationValLoc;
        [SerializeField] private Color _correctColor = Color.green;
        [SerializeField] private Color _incorrectColor = Color.red;

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

            UpdateIntegrationValue();
        }
        #endregion Initialization

        #region Delegate Event
        private void OnEnable()
        {
            GameManager.onUnitAdded += OnUnitAdded;
            GameManager.onUnitClassChanged += OnUnitClassChanged;
            GameManager.onGearChanged += OnGearChanged;
        }

        private void OnDisable()
        {
            GameManager.onUnitAdded -= OnUnitAdded;
            GameManager.onUnitClassChanged -= OnUnitClassChanged;
            GameManager.onGearChanged -= OnGearChanged;
        }

        private void OnUnitAdded(UnitData unitData)
        {
            var unitElem = Instantiate(_unitElemPrefab, _unitElemWrapper);
            unitElem.Init(this, unitData);
            UpdateIntegrationValue();
        }

        private void OnUnitClassChanged(int unitIndex, UnitData newClass)
        {
            //Debug.Log("OnUnitClassChanged(unitIndex: " + unitIndex + ", newClass: " + newClass + ")");
            UpdateIntegrationValue();
        }
        
        private void OnGearChanged(int unitIndex, int gearIndex, GearData newGear)
        {
            //Prepare data
            List<SpecializationGearData> spe = new List<SpecializationGearData>();
            foreach (GearSO gearSO in _gameMgr.GearSOs)
            {
                if (gearSO.Data.SlotSize == 2 && gearSO.Data.UnitType == UnitType.Minifig)
                {
                    spe.Add(new SpecializationGearData(gearSO.Data));
                }
            }

            //Check for army specialization
            foreach (UnitData unit in _gameMgr.ArmyUnits)
            {
                foreach (var gear in unit.GearList)
                {

                }
            }
        }
        #endregion Delegate Event

        #region Misc
        void UpdateIntegrationValue()
        {
            //Compute integration value
            int minifigVal = 0;
            int megafigVal = 0;

            foreach (var unit in _gameMgr.ArmyUnits)
            {
                if (unit.IntegrationCost > 0)
                {
                    megafigVal += unit.IntegrationCost;
                }
                else
                {
                    minifigVal += Mathf.Abs(unit.IntegrationCost);
                }
            }

            string prefix = "";
            if (_gameMgr != null)
            {
                Language language = _gameMgr.GetCurrentLanguage();
                foreach (var intValLoc in _integrationValLoc.Data)
                {
                    if (intValLoc.Language == language)
                    {
                        prefix = intValLoc.Txt;
                        break;
                    }
                }
            }

            _integrationTxt.text = prefix + megafigVal + " / " + minifigVal;
            if (megafigVal <= minifigVal)
            {
                _integrationTxt.color = _correctColor;
            }
            else
            {
                _integrationTxt.color = _incorrectColor;
            }
        }
        #endregion Misc

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