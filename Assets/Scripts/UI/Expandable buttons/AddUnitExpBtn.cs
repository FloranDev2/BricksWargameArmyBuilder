using System;
using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Enums;
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

            int max = _gameMgr.MinifigSOs.Count; //tmp

            int index = 0;

            //Minifigs
            for (int i = 0; i < max; i++)
            {
                //UnitData data = _gameMgr.Unit[i];
                _canvasMgr.DynamicScroller.CreateElem(index, "");
                index++;
            }

            //Megafigs
            //foreach (var cat in Enum.GetValues<MegafigCategory>()) //Doesn't work
            foreach (string name in Enum.GetNames(typeof(MegafigCategory)))
            {
                MegafigCategory value = Enum.Parse<MegafigCategory>(name, true);
                _canvasMgr.DynamicScroller.CreateElem(index, "");
                index++;
            }
        }

        public override void OnElemClick(int index)
        {

        }
        #endregion Public

        #endregion METHODS
    }
}