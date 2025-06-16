using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Localization;
using Truelch.Managers;
using Truelch.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class UnitElem : MonoBehaviour
    {
        #region ATTRIBUTES
        //Public
        /*[NonSerialized]*/ public UnitData UnitData;

        //Inspector
        // - Top
        [Header("UI Refs")]
        [SerializeField] private Image _bgColorImg;
        [SerializeField] private Image _classIconImg; //Commando, Medic, etc.
        [SerializeField] private TextMeshProUGUI _placeHolderTxt;
        [SerializeField] private TextMeshProUGUI _finalTxt; //I may not use it

        // - Equipment
        [Header("Gear")]
        [SerializeField] private Transform _gearWrapper; //Call it gear instead?
        [SerializeField] private GearExpBtn _gearElemPrefab; //unless the gear elem prefab IS the placeholder??

        //Hidden
        // - Managers
        private GameManager _gameMgr; //only useful for tests?

        // - Misc
        private ArmyBuilderUI _armyBuilderUI;
        [Header("Debug")]
        [SerializeField] private List<GearExpBtn> _gearElems = new List<GearExpBtn>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        IEnumerator Start()
        {
            yield return new WaitUntil(() => GameManager.Instance != null);
            _gameMgr = GameManager.Instance;
        }
        #endregion Init

        #region Misc
        IEnumerator CR_RefreshClass()
        {
            if (_gameMgr == null) yield return new WaitUntil(() => _gameMgr != null);

            //Name
            string name = "Unit";
            Language language = _gameMgr.GetCurrentLanguage();
            foreach (var locName in UnitData.LocNames)
            {
                if (locName.Language == language)
                {
                    name = locName.Txt;
                    break;
                }
            }
            _placeHolderTxt.text = string.IsNullOrEmpty(UnitData.CurrentName) ? name : UnitData.CurrentName;

            //Icon
            _classIconImg.sprite = UnitData.Icon;
            _bgColorImg.color = UnitData.Color;

            //Gears! (new!)
            // - TODO: check if we can keep previous upgrades?

            if (UnitData.GearList == null || UnitData.GearList.Count == 0)
            {
                //Debug.Log("No gear!!");

                // - Destroy previous ?
                foreach (GearExpBtn gearBtn in _gearElems)
                {
                    Destroy(gearBtn.gameObject);
                }

                // - Create new
                for (int i = 0; i < UnitData.MaxGear; i++)
                {
                    GearExpBtn gearBtn = Instantiate(_gearElemPrefab, _gearWrapper);
                    gearBtn.Init(this, i, null);
                    _gearElems.Add(gearBtn);
                }

                // - Update Data
                UnitData.GearList.Clear();
                for (int i = 0; i < UnitData.MaxGear; i++)
                {
                    UnitData.GearList.Add(null);
                }
            }
            else
            {
                //No need to add them again
                //Edit: except if it's from a duplicate and not a change class!

                //TODO: check if there are mismatch between data and gear buttons...
                if (_gearElems.Count == 0)
                {
                    //Debug.Log("[DUPLICATE] We should create gear elems here!");
                    for (int i = 0; i < UnitData.GearList.Count; i++)
                    {
                        GearData gear = UnitData.GearList[i];
                        GearExpBtn gearBtn = Instantiate(_gearElemPrefab, _gearWrapper);
                        gearBtn.Init(this, i, gear);
                        _gearElems.Add(gearBtn);
                    }
                }
                else
                {
                    //Debug.Log("[CHANGE CLASS] We should NOT create new gear elems!");
                }
            }                
        }
        #endregion Misc

        #region Public
        public void Init(ArmyBuilderUI ui, UnitData unitData)
        {
            _armyBuilderUI = ui;

            UnitData = unitData; //it was already cloned, it's not the source itself
            StartCoroutine(CR_RefreshClass());
        }

        public void OnChangeClassClick(UnitSO unitSO)
        {
            //Debug.Log("OnChangeClassClick");
            if (_gameMgr.ArmyUnits.Contains(UnitData))
            {
                int index = _gameMgr.ArmyUnits.IndexOf(UnitData);

                //Things to keep:
                // - CurrentName (custom name written in the text input)
                // - GearList (new!)
                string name = UnitData.CurrentName;
                List<GearData> gears = new List<GearData>();
                foreach (GearData gear in UnitData.GearList)
                {
                    if (gear != null)
                    {
                        gears.Add(gear.GetClone());
                    }
                    else
                    {
                        gears.Add(null);
                    }
                }
                //Debug.Log("[BEFORE] gears: " + gears.Count);

                UnitData = unitSO.Data.GetClone();

                //Give back the things saved, IF they are relevant
                //
                if (!string.IsNullOrEmpty(name))
                {
                    UnitData.CurrentName = name;
                }
                UnitData.GearList = gears;

                //Debug.Log("[AFTER] UnitData.GearList: " + UnitData.GearList.Count);

                //End
                _gameMgr.ArmyUnits[index] = UnitData;
                StartCoroutine(CR_RefreshClass());
            }
            else
            {
                Debug.Log("WTF");
            }
        }

        public void OnShowInfosClick()
        {

        }

        public void OnDuplicateClick()
        {
            _gameMgr.AddUnit(UnitData);
        }

        public void OnDeleteClick()
        {
            _armyBuilderUI.OnRemoveUnitClick(this);
        }

        // --- Gear ---
        public void OnGearChanged(int gearIndex, GearData newGear)
        {
            GearData clonedGear = newGear.GetClone();
            UnitData.GearList[gearIndex] = clonedGear;
        }

        public void OnDestroyGear(GearExpBtn gear)
        {
            UnitData.GearList[gear.Index] = null;
        }

        // --- UI Events ---
        public void OnEndEdit(string name)
        {
            UnitData.CurrentName = name;
        }
        #endregion Public

        #endregion METHODS
    }
}