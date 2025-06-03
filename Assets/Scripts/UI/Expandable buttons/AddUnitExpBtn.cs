using System;
using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Enums;
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

            //Minifigs
            for (int i = 0; i < _gameMgr.MinifigSOs.Count; i++)
            {
                MinifigData data = _gameMgr.MinifigSOs[i].Data;

                //TODO: concatenate this function, somewhere...
                //_canvasMgr.DynamicScroller.CreateElem(index, data.CurrentName);
                string name = "Minifig";
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

            //Megafigs
            _canvasMgr.DynamicScroller.CreateElem(index, "Mégafig");
            //foreach (var cat in Enum.GetValues<MegafigCategory>()) //Doesn't work
            /*
            foreach (string name in Enum.GetNames(typeof(MegafigCategory)))
            {
                MegafigCategory value = Enum.Parse<MegafigCategory>(name, true);
                _canvasMgr.DynamicScroller.CreateElem(index, "Mégafig");
                index++;
            }
            */
        }

        public override void OnElemClick(int index)
        {
            Debug.Log("OnElemClick(index: " + index + ")");
            _gameMgr.AddUnit(_gameMgr.MinifigSOs[index].Data);
        }
        #endregion Public

        #endregion METHODS
    }
}