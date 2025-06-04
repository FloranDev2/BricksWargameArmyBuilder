using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Enums;
using UnityEngine;

namespace Truelch.Data
{
    /// <summary>
    /// Regroup both minifigs (troops or heroes) and megafigs.
    /// Is this actually a good idea?
    /// </summary>
    [System.Serializable]
    public class UnitData
    {
        public string Name;
        public UnitType Type;

        [Header("Minfig only")]
        public MinifigData MinifigData;
        public List<GearData> GearList;

        [Header("Megafig only")]
        public MegafigSize MegafigType;
    }
}