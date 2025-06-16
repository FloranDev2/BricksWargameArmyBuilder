using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Localization;
using Truelch.UI;
using UnityEngine;

namespace Truelch.UI
{
    /// <summary>
    /// Both for changing class or selecting the class of a new unit to add.
    /// </summary>
    public class ChooseClassExpBtn : ExpandButtonBase
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private UnitElem _unitElem;
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public override void OnExpandClick()
        {
            if (!_isReady) return;

            base.OnExpandClick();

            Language language = _gameMgr.GetCurrentLanguage();

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
                _canvasMgr.DynamicScroller.CreateElem(i, name, data.Color);
            }
        }

        public override void OnElemClick(int index)
        {
            //Debug.Log("OnElemClick(index: " + index + ")");

            //New (final)
            if (_unitElem.UnitData != null)
            {
                //Debug.Log("[A] Change class!");
                _unitElem.OnChangeClassClick(_gameMgr.UnitSOs[index]);
            }
            else
            {
                //Debug.Log("[B] Add unit!");
                //Otherwise, I might risk data corruption?
                // -> GameManager is already cloning the data, just send the original from here:
                //UnitData newData = _gameMgr.UnitSOs[index].Data.GetClone();
                //_gameMgr.AddUnit(newData);
                _gameMgr.AddUnit(_gameMgr.UnitSOs[index].Data);
            }
        }
        #endregion Public

        #endregion METHODS
    }
}