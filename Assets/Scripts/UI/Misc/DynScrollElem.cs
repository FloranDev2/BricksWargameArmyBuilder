using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Truelch.UI
{
    public class DynScrollElem : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        public DynamicScroller DynamicScroller;
        public int Index; //for data
        public TextMeshProUGUI Text;
        public Image Image; //to recolor
        #endregion ATTRIBUTES


        #region METHODS

        #region Public
        public void OnClick()
        {

        }
        #endregion Public

        #endregion METHODS
    }
}