using System.Collections;
using System.Collections.Generic;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.UI
{
    public class LanguageExpandButton : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Inspector
        //[SerializeField] private 
        [SerializeField] private List<LanguageData> _languageDataList;
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            //Create language list -> No, this is supposed to be done by the Dynamic Scroller
            /*
            foreach (var data in _languageDataList)
            {

            }
            */

            //And this is done by the base class
            //_canvasMgr.DynamicScroller.ShowDynamicScroller(_parentRt);

            //_canvasMgr.
        }

        public override void OnElemClick(int index)
        {

        }
        #endregion Public

        #endregion METHODS
    }

    [System.Serializable]
    public class LanguageData
    {
        [SerializeField] private string _name;
        public Language Language;
        public Sprite FlagIcon;
    }
}