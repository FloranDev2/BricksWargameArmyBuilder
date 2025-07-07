using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.Localization
{
    [System.Serializable]
    public class TextLocData
    {
        #region ATTRIBUTES
        [SerializeField] private string _name;
        public Language Language;
        [TextArea(1, 10)] public string Txt;
        #endregion ATTRIBUTES


        #region METHODS
        public TextLocData GetClone()
        {
            TextLocData clone = new TextLocData();
            clone._name = _name;
            clone.Language = Language;
            clone.Txt = Txt;
            return clone;
        }
        #endregion METHODS
    }
}