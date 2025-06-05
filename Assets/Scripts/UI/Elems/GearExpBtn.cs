using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        //Hidden
        private UnitExpBtn _unitElem;
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public void Init(UnitExpBtn unitElem, int index)
        {
            _unitElem = unitElem;
            Index = index;
        }

        //From ExpandButtonBase
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            
        }

        public override void OnElemClick(int index)
        {

        }
        #endregion Public

        #endregion METHODS
    }
}