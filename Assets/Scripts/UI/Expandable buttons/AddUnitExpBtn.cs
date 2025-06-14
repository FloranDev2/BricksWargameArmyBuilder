using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.UI
{
    /// <summary>
    /// OLD, I'll remove it at some point, and use UnitElem to replace it.
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
            //UnitData clone = _gameMgr.UnitSOs[index].Data.GetClone();
            //_gameMgr.AddUnit(clone);
            _gameMgr.AddUnit(_gameMgr.UnitSOs[index].Data); //Cloning the data is done by the game manager already
        }
        #endregion Public

        #endregion METHODS
    }
}