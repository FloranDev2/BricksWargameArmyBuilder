using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class MegafigCategoryLocData
    {
        [SerializeField] private string _name;
        public MegafigCategory MegafigCategory;
        public List<TextLocData> LocNames;
    }
}