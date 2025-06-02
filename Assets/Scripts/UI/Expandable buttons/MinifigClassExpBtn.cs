using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.UI
{
    public class MinifigClassExpBtn : ExpandButtonBase
    {
        #region ATTRIBUTES
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public override void OnElemClick(int index)
        {
            if (!_isReady) return;

            base.OnExpandClick();


        }
        #endregion Public

        #endregion METHODS
    }
}