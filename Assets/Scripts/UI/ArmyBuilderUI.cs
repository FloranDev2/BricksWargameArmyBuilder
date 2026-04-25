using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Enums;
using Truelch.Localization;
using Truelch.Managers;
using UnityEngine;

namespace Truelch.UI
{
    public class ArmyBuilderUI : FadePanel
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
        private CanvasManager _canvasMgr;
        private DataManager _dataMgr;

        // - Misc
        private bool _isReady = false;

        // - Units
        /*[SerializeField]*/ private List<UnitElem> _unitElems = new List<UnitElem>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            HideInstantly();

            yield return new WaitUntil(() =>
                CanvasManager.Instance &&
                DataManager.Instance
            );

            _canvasMgr = CanvasManager.Instance;
            _dataMgr = DataManager.Instance;

            _isReady = true;
        }
        #endregion Initialization

        #region Delegate Event
        private void OnEnable()
        {
            DataManager.onUnitAdded          += OnUnitAdded;
            DataManager.onUnitRemoved        += OnUnitRemoved;
            DataManager.onUnitClassChanged   += OnUnitClassChanged;
            DataManager.onUnitMegaCatChanged += OnUnitMegaCatChanged;
            DataManager.onRefreshed          += RefreshAll;
        }

        private void OnDisable()
        {
            DataManager.onUnitAdded          -= OnUnitAdded;
            DataManager.onUnitRemoved        -= OnUnitRemoved;
            DataManager.onUnitClassChanged   -= OnUnitClassChanged;
            DataManager.onUnitMegaCatChanged -= OnUnitMegaCatChanged;
            DataManager.onRefreshed          -= RefreshAll;
        }

        private void OnUnitAdded(UnitData unitData)
        {
            var unitElem = Instantiate(_unitElemPrefab, _unitElemWrapper);
            unitElem.Init(this, unitData);
            _unitElems.Add(unitElem);
            UpdateIntegrationValue();
        }

        private void OnUnitRemoved(UnitData unitData)
        {
            UpdateIntegrationValue();
        }

        private void OnUnitClassChanged(int unitIndex, UnitData newClass)
        {
            _unitElems[unitIndex].UnitData = newClass;
            _unitElems[unitIndex].RefreshMe();
            UpdateIntegrationValue();

            RefreshAll(); //test
        }

        private void OnUnitMegaCatChanged(int unitIndex, MegafigCategory newCat)
        {
            Language language = _dataMgr.GetCurrentLanguage();

            MegafigCategoryLocData data = null;
            foreach (MegafigCategoryLocData megaCatLoc in _dataMgr.MegafigCategoryLocDataList)
            {
                if (megaCatLoc.MegafigCategory == newCat)
                {
                    data = megaCatLoc;
                    break;
                }
            }

            if (data != null)
            {
                foreach (var locName in data.LocNames)
                {
                    if (locName.Language == language)
                    {
                        _unitElems[unitIndex].SetMegaCatText(locName.Txt);
                        break;
                    }
                }
            }
            else
            {
                Debug.Log("Oh no");
            }

            RefreshAll();
        }
        #endregion Delegate Event

        #region Misc
        public void RefreshAll()
        {
            UpdateIntegrationValue();

            for (int unitIndex = 0; unitIndex < _unitElems.Count; unitIndex++)
            {
                _unitElems[unitIndex].RefreshMe();
                _unitElems[unitIndex].RefreshGear();
            }
        }

        void UpdateIntegrationValue()
        {
            //Compute integration value
            int minifigVal = 0;
            int megafigVal = 0;

            foreach (var unit in _dataMgr.ArmyUnits)
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
            if (_dataMgr != null)
            {
                Language language = _dataMgr.GetCurrentLanguage();
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

        void DestroyElems()
        {
            //Debug.Log("DestroyElems");
            foreach (var elem in _unitElems)
            {
                Destroy(elem.gameObject);
            }
            _unitElems.Clear();
        }

        void CreateElems(ArmyData armyData)
        {
            //Debug.Log("CreateElems");
            foreach (var unit in armyData.Units)
            {
                OnUnitAdded(unit); //TODO: refactor this
            }
        }
        #endregion Misc

        #region Public
        //Options
        //public void OnSaveClick()
        //{
        //    if (!_isReady) return;
        //    _dataMgr.OnSaveClick();
        //}

        public void OnPrintExportClick()
        {
            if (!_isReady) return;

            Language language = _dataMgr.GetCurrentLanguage();

            string export = "";
            foreach (UnitData unit in _dataMgr.ArmyUnits)
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
            _dataMgr.RemoveUnit(btn.UnitData);
            _unitElems.Remove(btn);
            Destroy(btn.gameObject);
        }

        //Quit army manager and return armies management UI.
        //Should I ask if the player wants to save?
        public void ReturnToArmiesManagementUI()
        {
            if (!_isReady) return;

            _canvasMgr.OnReturnToArmiesManagementClicked();
        }

        public void Open()
        {
            ShowInstantly();

            //Here: refresh using units from current army!
            if (_isReady)
            {
                DestroyElems();
                CreateElems(_dataMgr.CurrArmy);
            }
            else
            {
                Debug.Log("WTF! I should create a routine to wait for ArmyBuilderUI to be ready...");
            }

            //If not, maybe wait until ready?

            UpdateIntegrationValue();
        }

        public void Close()
        {
            HideInstantly();
        }
        #endregion Public

        #endregion METHODS
    }
}