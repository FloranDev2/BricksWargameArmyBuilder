using System.Collections;
using System.Collections.Generic;
using TMPro;
using Truelch.Data;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.UI
{
    public class ChooseMegaCatExpBtn : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private UnitElem _unitElem;
        [SerializeField] private TextMeshProUGUI _text;
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        protected override IEnumerator ExtraInit()
        {
            Language language = _gameMgr.GetCurrentLanguage();
            _text.text = GetLocName(_gameMgr.MegafigCategoryLocDataList[0], language);
            yield return null;
        }
        #endregion Init

        #region Misc
        private string GetLocName(MegafigCategoryLocData data, Language language)
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

            for (int i = 0; i < _gameMgr.MegafigCategoryLocDataList.Count; i++)
            {
                MegafigCategoryLocData data = _gameMgr.MegafigCategoryLocDataList[i];
                _canvasMgr.DynamicScroller.CreateElem(i, GetLocName(data, language));
            }
        }

        public override void OnElemClick(int index)
        {
            _unitElem.OnMegaCatChanged(_gameMgr.MegafigCategoryLocDataList[index].MegafigCategory);
        }
        #endregion Public

        #endregion METHODS
    }
}