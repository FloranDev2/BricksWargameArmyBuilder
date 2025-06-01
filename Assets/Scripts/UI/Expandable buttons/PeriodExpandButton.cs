using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.UI
{
    public class PeriodExpandButton : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private List<PeriodData> _periodDataList;
        #endregion ATTRIBUTES


        #region METHODS

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

    [System.Serializable]
    public class PeriodData
    {
        [SerializeField] private string _name;
        //public string Name; //Need a list of string, one name for each language
        public List<TextLocData> LocNames;
        public Period Period;
        public Sprite PeriodIcon;
    }


}