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

        [SerializeField] private TextMeshProUGUI _megaTypeTxt;
        [SerializeField] private GameObject _chooseMegaCatGo;

        // - Equipment
        [Header("Gear")]
        [SerializeField] private Transform _gearWrapper; //Call it gear instead?
        [SerializeField] private GearExpBtn _gearElemPrefab; //unless the gear elem prefab IS the placeholder??

        //Hidden
        // - Managers
        private CanvasManager _canvasMgr;
        private DataManager _gameMgr; //only useful for tests?

        // - Misc
        private ArmyBuilderUI _armyBuilderUI;
        [Header("Debug")]
        [SerializeField] private List<GearExpBtn> _gearElems = new List<GearExpBtn>();
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        IEnumerator Start()
        {
            yield return new WaitUntil(() =>
                CanvasManager.Instance != null &&
                DataManager.Instance != null);
            _canvasMgr = CanvasManager.Instance;
            _gameMgr   = DataManager.Instance;
        }
        #endregion Init

        #region Misc
        public void RefreshMe()
        {
            StartCoroutine(CR_RefreshClass());
        }

        IEnumerator CR_RefreshClass()
        {
            //Debug.Log("CR_RefreshClass");
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

            //Megafig Category
            _chooseMegaCatGo.SetActive(UnitData.Type == UnitType.Megafig);

            foreach (var megaCat in _gameMgr.MegafigCategoryLocDataList)
            {
                if (megaCat.MegafigCategory == UnitData.MegaCategory)
                {
                    foreach (var locName in megaCat.LocNames)
                    {
                        if (locName.Language == language)
                        {
                            _megaTypeTxt.text = locName.Txt;
                        }
                    }
                }
            }

            //Icon
            _classIconImg.sprite = UnitData.Icon;
            _bgColorImg.color = UnitData.Color;
            //Debug.Log("UnitData: " + UnitData);
            //Debug.Log("New color: " + UnitData.Color);
            _finalTxt.color = UnitData.TextColor;
            _placeHolderTxt.color = UnitData.TextColor;

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
                int oldAmount = _gearElems.Count;
                int newAmount = UnitData.GearList.Count;
                int diff = newAmount - oldAmount;

                if (diff > 0)
                {
                    //Add
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
                    //Debug.Log("REMOVE!");

                    for (int i = newAmount; i < oldAmount; i++)
                    {
                        //GearExpBtn gearBtn = _gearElems[i];
                        //GearData gearData = _gearElems[i].Data;
                        //if (gearBtn.)
                        //{
                        //    Debug.Log("HERE!!!");
                        //}

                        Destroy(_gearElems[_gearElems.Count - 1].gameObject);
                        _gearElems.RemoveAt(_gearElems.Count - 1);
                    }
                }
            }                
        }

        public void RefreshGear()
        {
            int max = Mathf.Min(_gearElems.Count, UnitData.GearList.Count);
            for (int i = 0; i < max; i++)
            {
                _gearElems[i].UpdateGear(UnitData.GearList[i], null, false); //idk for null
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

        // --- From ArmyBuilderUI ---
        public void SetMegaCatText(string name)
        {
            _megaTypeTxt.text = name;
        }

        // --- From Children (GearExpBtn, ChooseMegaCatExpBtn, ...) ---
        public void OnChangeClassClick(UnitSO newUnitSO)
        {
            _gameMgr.ChangeUnitClass(UnitData, newUnitSO);
        }

        public void OnMegaCatChanged(MegafigCategory newMegaCat)
        {
            int index = _gameMgr.ArmyUnits.IndexOf(UnitData);
            _gameMgr.ChangeUnitMegaCategory(index, newMegaCat);
        }

        //old gear is likely to be null
        public void OnAttemptingToChangeGear(int gearIndex, GearData newGear, GearData oldGear)
        {
            _gameMgr.TryToChangeGear(_gameMgr.ArmyUnits.IndexOf(UnitData), gearIndex, newGear.GetClone(), oldGear);
        }

        public void OnDestroyGear(GearExpBtn gearBtn)
        {
            int index = _gameMgr.ArmyUnits.IndexOf(UnitData); //I should move that into a separate function
            _gameMgr.RemoveGear(index, gearBtn.Index, gearBtn.Data);
        }

        // --- OTHER BUTTONS ---
        public void OnShowInfosClick()
        {
            Language language = _gameMgr.GetCurrentLanguage();
            string msg = "";
            //foreach (var ability in UnitData.Abilities)
            for (int i = 0; i < UnitData.Abilities.Count; i++)
            {
                var ability = UnitData.Abilities[i];
                foreach (var loc in ability.LocDescriptions)
                {
                    if (loc.Language == language)
                    {
                        if (i != 0) msg += "\n\n";
                        msg += loc.Txt;
                    }
                }
            }
            _canvasMgr.FeedbackUI.ShowPopUp(msg);
        }

        public void OnDuplicateClick()
        {
            _gameMgr.AddUnit(UnitData);
        }

        public void OnDeleteClick()
        {
            _armyBuilderUI.OnRemoveUnitClick(this);
        }

        // --- UI Events ---
        public void OnEndEdit(string name)
        {
            Debug.Log("OnEndEdit(name: " + name + ")");
            UnitData.CurrentName = name;
        }
        #endregion Public

        #endregion METHODS
    }
}