using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.UI
{
    public class PeriodExpandButton : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private TextMeshProUGUI _text;
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        protected override IEnumerator /*void*/ ExtraInit()
        {
            //No need to call base.ExtraInit()
            Language language = _gameMgr.GetCurrentLanguage();
            _text.text = GetLocName(_gameMgr.PeriodDataList[0], language);
            yield return null; //I actually didn't needed it
        }
        #endregion Init

        #region Misc
        private string GetLocName(PeriodData data, Language language)
        {
            foreach (var locData in data.LocNames)
            {
                if (locData.Language == language)
                {
                    return locData.Txt;
                }
            }
            return "";
        }
        #endregion Misc

        #region Public
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            Language language = _gameMgr.GetCurrentLanguage();

            for (int i = 0; i < _gameMgr.PeriodDataList.Count; i++)
            {
                PeriodData data = _gameMgr.PeriodDataList[i];
                _canvasMgr.DynamicScroller.CreateElem(i, GetLocName(data, language));
            }
        }

        public override void OnElemClick(int index)
        {
            if (!_isReady) return;

            _gameMgr.SetCurrentPeriod(index);
        }
        #endregion Public

        #endregion METHODS
    }    
}