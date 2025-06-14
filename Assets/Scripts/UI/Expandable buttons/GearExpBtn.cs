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
        #endregion ATTRIBUTES


        #region METHODS

        #region Misc
        private void UpdateGear(GearData gearData)
        {
            _nameTxt.text = ""; //or set active false?
            _gearImg.gameObject.SetActive(true);
            _gearImg.sprite = gearData.Icon;
            _unitElem.OnGearChanged(Index, gearData); //this might create infinite loop, no?
        }
        #endregion Misc

        #region Public
        public void Init(UnitElem unitElem, int index, GearData gearData)
        {
            _unitElem = unitElem;
            Index = index;

            //Long story short: Gear Data has been displayed somewhere in an inspector, instantiating an initially null ref. Gah
            //Oh, it may happened that it's also null...
            if (gearData == null || !gearData.IsReal)
            {
                Debug.Log("Gear Data is null OR not real"); //some girl on an airplane, probably...
                //initial init or init after delete gear
                _gearImg.gameObject.SetActive(false);

            }
            else
            {
                Debug.Log("Gear Data exists!");
                //init after duplicating a unit with existing gear
                //_gearImg.gameObject.SetActive(true);
                //_nameTxt.text = "";
                UpdateGear(gearData);
            }
        }

        //From ExpandButtonBase
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            Language language = _gameMgr.GetCurrentLanguage();

            for (int i = 0; i < _gameMgr.GearSOs.Count; i++)
            {
                var gearSO = _gameMgr.GearSOs[i];
                string name = "Gear";
                foreach (var locName in gearSO.Data.LocNames)
                {
                    if (locName.Language == language)
                    {
                        name = locName.Txt;
                        break;
                    }
                }
                _canvasMgr.DynamicScroller.CreateElem(i, name);
            }
        }

        public override void OnElemClick(int index)
        {
            UpdateGear(_gameMgr.GearSOs[index].Data);
        }

        public void OnDeleteClick()
        {
            //_unitElem.OnDestroyGear(this);
            //Or maybe revert to "select" gear mode instead of removing the gameobject?
        }

        public void OnInfosClick()
        {

        }
        #endregion Public

        #endregion METHODS
    }
}