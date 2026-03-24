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
        [Header("Infos")]
        public string Id;
        public GearSO SO; //I use that to compare Gear (because cloned Gear isn't equal to its "source")
        public bool IsReal = false; //Null stuff, when displayed in the inspector, isn't null anymore.
        public Color Color = Color.white;
        public Color TextColor = Color.black;
        public Sprite Icon;

        [Header("Limitations (Common)")]
        public bool IsSingleton = true;
        public Period Period; //Useless now?
        public UnitType UnitType; //Minifig / Megafig
        public List<string> IncompatibleGears = new List<string>();

        [Header("Limitations (Minifig)")]
        public MinifigType MiniType = MinifigType.Both; //Hero, Troop or Both
        public RangeType RangeType = RangeType.All; //should I rather make a list? And if empty, no limitation then. NO

        [Header("Limitations (Megafig)")]
        public List<MegafigCategory> RestrictedMegaCategories; //if empty, no restriction
        public List<MegafigSize> RestrictedMegaSizes; //if empty, no restriction
        [Range(1, 2)] public int SlotSize = 1; //Now, only used by Megafigs. Simple: 1, Double: 2 (wait, transport can take more?)
        public bool TurretPossible = false;

        [Header("Usage")]
        public int Cost = 0; //Cost to use this action in Action Points
        public int Limit = 0; //Amount of time this action can be performed per turn (0 is infinite)

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

            clone.Id = Id;

            clone.IsReal = IsReal;

            // --- Infos ---
            clone.Color = Color;
            clone.Icon = Icon;

            // --- Limitations (Common) ---
            clone.IsSingleton = IsSingleton;
            clone.Period      = Period;
            clone.UnitType    = UnitType;

            foreach (string s in IncompatibleGears)
            {
                clone.IncompatibleGears.Add(s);
            }

            // --- Limitations (Minifig) ---
            clone.MiniType = MiniType;

            // --- Limitations (Megafig) ---
            foreach (var cat in RestrictedMegaCategories)
            {
                clone.RestrictedMegaCategories.Add(cat);
            }
            foreach (var size in RestrictedMegaSizes)
            {
                clone.RestrictedMegaSizes.Add(size);
            }
            clone.SlotSize = SlotSize;
            clone.TurretPossible = TurretPossible;

            // --- Usage ---
            clone.Cost  = Cost;
            clone.Limit = Limit;

            // --- Strings ---
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