using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.Localization
{
    [System.Serializable]
    public class TextLocData
    {
        [SerializeField] private string _name;
        public Language Language;
        [TextArea] public string Txt;
    }
}