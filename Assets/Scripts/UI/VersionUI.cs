using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Truelch.UI
{
    public class VersionUI : MonoBehaviour
    {
        #region ATTRIBUTES
        //Inspector
        [SerializeField] private TextMeshProUGUI _text;
        #endregion ATTRIBUTES


        #region METHODS
        void Start()
        {
            _text.text = "Bricks Wargame 1.8\nBWAB " + Application.version;
        }
        #endregion METHODS
    }
}