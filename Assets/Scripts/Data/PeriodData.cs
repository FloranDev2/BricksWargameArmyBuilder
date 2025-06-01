using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class PeriodData
    {
        [SerializeField] private string _name;
        //public string Name; //Need a list of string, one name for each language
        public List<TextLocData> LocNames;
        public Period Period;
        public Sprite PeriodIcon;
    }
}