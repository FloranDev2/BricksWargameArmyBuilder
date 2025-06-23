using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using Truelch.ScriptableObjects;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class GearData
    {
        #region ATTRIBUTES
        public GearSO SO; //I use that to compare Gear (because cloned Gear isn't equal to its "source")
        public bool IsReal = false; //Null stuff, when displayed in the inspector, isn't null anymore.
        public Color Color = Color.white;
        public Sprite Icon;

        [Header("Limitations")]
        public Period Period;
        public UnitType UnitType;
        public List<MegafigCategory> RestrictedMegaCategories; //if empty, no restriction
        public List<MegafigSize> RestrictedMegaSizes; //if empty, no restriction
        public bool IsSingleton = true;
        [Range(1, 2)] public int SlotSize = 1; //Simple: 1, Double: 2 (wait, transport can take more?)
        public bool TurretPossible = false;

        //TODO: regrouper les deux en-dessous en un nouveau type de Data plutot qu'utiliser TextLocData
        [Header("Strings")]
        public List<TextLocData> LocNames;
        public List<TextLocData> LocDescriptions;
        public string ExportString;
        #endregion ATTRIBUTES


        #region METHODS
        public GearData GetClone()
        {
            GearData clone = new GearData();

            clone.SO = SO;

            clone.IsReal = IsReal;

            clone.Period = Period;
            clone.Color = Color;
            clone.Icon = Icon;
            
            if (LocNames != null)
            {
                clone.LocNames = new List<TextLocData>();
                foreach (var name in LocNames)
                {
                    clone.LocNames.Add(name.GetClone());
                }
            }

            if (LocDescriptions != null)
            {
                clone.LocDescriptions = new List<TextLocData>();
                foreach (var desc in LocDescriptions)
                {
                    clone.LocDescriptions.Add(desc.GetClone());
                }
            }

            clone.ExportString = ExportString;
            clone.SlotSize = SlotSize;

            return clone;
        }
        #endregion METHODS
    }
}