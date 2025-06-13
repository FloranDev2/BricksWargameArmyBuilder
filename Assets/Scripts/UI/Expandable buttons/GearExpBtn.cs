using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Localization;
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

        #region Public
        public void Init(UnitElem unitElem, int index)
        {
            _unitElem = unitElem;
            Index = index;
        }

        //From ExpandButtonBase
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            Language language = _gameMgr.GetCurrentLanguage();

            //foreach (var gearSO in _gameMgr.GearSOs)
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
            _unitElem.OnGearChanged(index, _gameMgr.GearSOs[Index]);
        }

        public void OnDeleteClick()
        {
            //_unitElem.OnDestroyGear(this);
        }

        public void OnInfosClick()
        {

        }
        #endregion Public

        #endregion METHODS
    }
}