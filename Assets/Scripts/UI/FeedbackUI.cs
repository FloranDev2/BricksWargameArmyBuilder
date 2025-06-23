using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.UI
{
    public class FeedbackUI : MonoBehaviour
    {
        #region ATTRIBUTES
        #endregion ATTRIBUTES


        #region METHODS
        public void ShowTempMsg(string msg)
        {
            Debug.Log("ShowTempMsg(msg: " + msg + ")");
        }

        public void ShowPopUp(string msg)
        {
            Debug.Log("ShowPopUp(msg: " + msg + ")");
        }
        #endregion METHODS
    }
}