using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.UI
{
    public class PeriodExpandButton : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private TextMeshProUGUI _text;
        //In fact, I need to move this data in the game manager and set an index to know in which period we are. (maybe save?)
        [SerializeField] private List<PeriodData> _periodDataList;
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        protected override IEnumerator /*void*/ ExtraInit()
        {
            //No need to call base.ExtraInit()

            Language language = _gameMgr.CurrentLanguage;

            _text.text = GetLocName(_periodDataList[0], language);

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
        public override void OnClick()
        {
            if (!_isReady) return;

            base.OnClick();

            Language language = _gameMgr.CurrentLanguage;

            for (int i = 0; i < _periodDataList.Count; i++)
            {
                PeriodData data = _periodDataList[i];
                _canvasMgr.DynamicScroller.CreateElem(i, GetLocName(data, language));
            }
        }

        public override void OnElemClick(int index)
        {

        }
        #endregion Public

        #endregion METHODS
    }

    
}