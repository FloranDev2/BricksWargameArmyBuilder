using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.UI
{
    /// <summary>
    /// Enumerate enums values:
    /// https://www.pietschsoft.com/post/2024/05/03/csharp-iterate-over-enum
    /// </summary>
    public class AddUnitExpBtn : ExpandButtonBase
    {
        #region ATTRIBUTES

        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            Language language = _gameMgr.GetCurrentLanguage();

            int index = 0;

            for (int i = 0; i < _gameMgr.UnitSOs.Count; i++)
            {
                UnitData data = _gameMgr.UnitSOs[i].Data;

                //TODO: concatenate this function, somewhere...
                string name = "Unit";
                foreach (var d in data.LocNames)
                {
                    if (d.Language == language)
                    {
                        name = d.Txt;
                        break;
                    }
                }
                _canvasMgr.DynamicScroller.CreateElem(index, name);
                index++;
            }
        }

        public override void OnElemClick(int index)
        {
            Debug.Log("OnElemClick(index: " + index + ")");
            //_gameMgr.AddUnit(_gameMgr.UnitSOs[index].Data);
            UnitData clone = _gameMgr.UnitSOs[index].Data.GetClone();
            _gameMgr.AddUnit(clone);
        }
        #endregion Public

        #endregion METHODS
    }
}