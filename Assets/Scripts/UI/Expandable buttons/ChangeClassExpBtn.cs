using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Localization;
using Truelch.UI;
using UnityEngine;

namespace Truelch.UI
{
    public class ChangeClassExpBtn : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Inspector
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
            _gameMgr.AddUnit(_gameMgr.UnitSOs[index]);
        }
        #endregion Public

        #endregion METHODS
    }
}