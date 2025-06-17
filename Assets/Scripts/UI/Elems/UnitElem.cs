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
            //Debug.Log("CR_RefreshClass()");
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

            if (UnitData.GearList == null || UnitData.GearList.Count == 0)
            {
                //Debug.Log("No gear!!");
                // - Destroy previous ?
                foreach (GearExpBtn gearBtn in _gearElems)
                {
                    //Debug.Log("-> gearBtn: " + gearBtn);
                    if (gearBtn != null)
                    {
                        Destroy(gearBtn.gameObject);
                    }
                    else
                    {
                        Debug.Log("WTF gearBtn is null");
                    }                    
                }
                _gearElems.Clear();

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
                //Debug.Log("Else (there was some gear)");

                int oldAmount = _gearElems.Count;
                int newAmount = UnitData.GearList.Count;
                int diff = newAmount - oldAmount;

                //Debug.Log("old: " + oldAmount + ", new: " + newAmount + " -> diff: " + diff);

                if (diff > 0)
                {
                    //Add
                    //Debug.Log("Add!");
                    for (int i = oldAmount; i < newAmount; i++)
                    {
                        GearData gear = UnitData.GearList[i];
                        GearExpBtn gearBtn = Instantiate(_gearElemPrefab, _gearWrapper);
                        gearBtn.Init(this, i, gear);
                        _gearElems.Add(gearBtn);
                    }
                }
                else
                {
                    //Remove
                    //Debug.Log("Remove!");
                    for (int i = newAmount; i < oldAmount; i++)
                    {
                        Destroy(_gearElems[_gearElems.Count - 1].gameObject);
                        _gearElems.RemoveAt(_gearElems.Count - 1);
                    }
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

        public void OnChangeClassClick(UnitSO newUnitSO)
        {
            if (_gameMgr.ArmyUnits.Contains(UnitData))
            {
                int index = _gameMgr.ArmyUnits.IndexOf(UnitData);

                //Things to keep:
                // - CurrentName (custom name written in the text input)
                // - GearList (new!)

                List<GearData> gears = new List<GearData>();
                if (UnitData.Type != newUnitSO.Data.Type)
                {
                    //We must clear the gears (nothing to do, gears is already empty)
                    //Debug.Log("old: " + UnitData.Type + ", new: " + UnitData.Type + " -> clear gear!");
                }
                else
                {
                    //Update gear
                    //Debug.Log("Updating gear...");
                    for (int i = 0; i < newUnitSO.Data.MaxGear; i++)
                    {
                        GearData gear = null;
                        if (i < UnitData.GearList.Count && UnitData.GearList[i] != null)
                        {
                            gear = UnitData.GearList[i].GetClone();
                        }
                        gears.Add(gear);
                    }
                }

                string name = UnitData.CurrentName;
                UnitData = newUnitSO.Data.GetClone();

                //Give back the things saved, IF they are relevant
                if (!string.IsNullOrEmpty(name))
                {
                    UnitData.CurrentName = name;
                }
                UnitData.GearList = gears;

                //End
                _gameMgr.ChangeUnitClass(index, UnitData);
                StartCoroutine(CR_RefreshClass()); //react to the event instead?
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