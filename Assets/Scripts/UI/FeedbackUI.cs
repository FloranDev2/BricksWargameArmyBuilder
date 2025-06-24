using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Truelch.UI
{
    public class FeedbackUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private GameObject _popUpGo;
        [SerializeField] private TextMeshProUGUI _txt;
        #endregion ATTRIBUTES


        #region METHODS

        #region Init
        private void Awake()
        {
            _popUpGo.SetActive(false);
        }
        #endregion Init

        #region Public
        // --- Called from other scripts ---
        public void ShowTempMsg(string msg)
        {
            Debug.Log("ShowTempMsg(msg: " + msg + ")");
        }

        public void ShowPopUp(string msg)
        {
            Debug.Log("ShowPopUp(msg: " + msg + ")");
            _popUpGo.SetActive(true);
            _txt.text = msg;
        }

        // --- Button callbacks ---
        public void OnClosePopUpClick()
        {
            _popUpGo.SetActive(false);
        }
        #endregion Public

        #endregion METHODS
    }
}