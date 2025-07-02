using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Localization;
using Truelch.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    /// <summary>
    /// 
    /// </summary>
    public class GearExpBtn : ExpandButtonBase
    {
        #region ATTRIBUTES
        [Header("Gear")] //To separate from Expand Button Base stuff
        //Public
        public int Index;

        //Inspector
        [SerializeField] private TextMeshProUGUI _nameTxt; //For rename + recolor
        [SerializeField] private Image _bgImg; //For recolor
        [SerializeField] private Image _gearImg;

        //Hidden
        private UnitElem _unitElem;
        [NonSerialized] public GearData Data;
        #endregion ATTRIBUTES


        #region METHODS

        #region Misc
        public void UpdateGear(GearData newGearData, GearData oldGearData, bool callEvent)
        {
            if (newGearData == null || !newGearData.IsReal)
            {
                _nameTxt.gameObject.SetActive(true);
                _gearImg.gameObject.SetActive(false);
                return;
            }

            Data = newGearData;

            //_nameTxt.text = ""; //or set active false?
            _nameTxt.gameObject.SetActive(false); //because text loc will write over this
            _gearImg.gameObject.SetActive(true);
            _gearImg.sprite = newGearData.Icon;

            if (callEvent) //to prevent infinite loop
            {
                _unitElem.OnGearChanged(Index, newGearData, oldGearData); //this might create infinite loop, no?
            }            
        }

        public void ClearGear()
        {
            Data = null;
            _nameTxt.gameObject.SetActive(true);
            _nameTxt.GetComponent<TextLoc>().UpdateLoc(); //this will reset to the "Choose an option" text
            _gearImg.gameObject.SetActive(false);
        }
        #endregion Misc

        #region Public
        public void Init(UnitElem unitElem, int index, GearData gearData)
        {
            _unitElem = unitElem;
            Data = gearData;
            Index = index;

            //Long story short: Gear Data has been displayed somewhere in an inspector, instantiating an initially null ref. Gah
            //Oh, it may happened that it's also null...
            if (gearData == null || !gearData.IsReal)
            {
                //Debug.Log("Gear Data is null OR not real"); //some girl on an airplane, probably...
                //initial init or init after delete gear
                _gearImg.gameObject.SetActive(false);
            }
            else
            {
                //Debug.Log("Gear Data exists!");
                //init after duplicating a unit with existing gear
                UpdateGear(gearData, null, false); //wouldn't cause an infinite loop, but is unnecessary anyway (I think?)
            }
        }

        //From ExpandButtonBase
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            Language language = _gameMgr.GetCurrentLanguage();
            List<GearSO> availableGears = _gameMgr.GetGearSOs(_unitElem.UnitData);

            for (int i = 0; i < availableGears.Count; i++)
            {
                var gearSO = availableGears[i];
                string name = "Gear";
                foreach (var locName in gearSO.Data.LocNames)
                {
                    if (locName.Language == language)
                    {
                        name = locName.Txt;
                        break;
                    }
                }
                _canvasMgr.DynamicScroller.CreateElem(i, name, gearSO.Data.Color);
            }
        }

        public override void OnElemClick(int index)
        {
            List<GearSO> availableGears = _gameMgr.GetGearSOs(_unitElem.UnitData);
            _unitElem.OnGearChanged(Index, availableGears[index].Data, Data);
        }

        public void OnDeleteClick()
        {
            //Or maybe revert to "select" gear mode instead of removing the gameobject?
            _unitElem.OnDestroyGear(this);

            //Clear
            ClearGear();
        }

        public void OnInfosClick()
        {
            if (Data != null & Data.IsReal)
            {
                Language language = _gameMgr.GetCurrentLanguage();
                foreach (var locDesc in Data.LocDescriptions)
                {
                    if (locDesc.Language == language)
                    {
                        _canvasMgr.FeedbackUI.ShowPopUp(locDesc.Txt);
                        break;
                    }
                }
            }
        }
        #endregion Public

        #endregion METHODS
    }
}