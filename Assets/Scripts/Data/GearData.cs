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
        public bool IsReal = false; //Null stuff, when displayed in the inspector, isn't null anymore.
        public Color Color = Color.white;
        public Color TextColor = Color.black;
        public Sprite Icon; //won't work with save/load
        public string IconName; //save/load

        [Header("Limitations (Common)")]
        public bool IsSingleton = true;
        public UnitType UnitType; //Minifig / Megafig
        public List<string> IncompatibleGears = new List<string>();

        [Header("Limitations (Minifig)")]
        public MinifigType MiniType = MinifigType.Both; //Hero, Troop or Both
        public List<RangeType> AuthorizedRangeTypes; //if empty, no need to check

        [Header("Limitations (Megafig)")]
        public List<MegafigCategory> RestrictedMegaCategories; //if empty, no restriction
        public List<MegafigSize> RestrictedMegaSizes; //if empty, no restriction
        [Range(1, 2)] public int SlotSize = 1; //Now, only used by Megafigs. Simple: 1, Double: 2 (wait, transport can take more?)
        public bool IsTurret = false; //lol
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
        public GearData()
        {
            Id = "";
            IsReal = false; //Most important!
            Color = Color.white;
            TextColor = Color.black;
            Icon = null;

            //Limitations (Common)
            IsSingleton = true;
            UnitType = UnitType.Minifig;
            if (IncompatibleGears != null)
            {
                IncompatibleGears.Clear();
            }
            else
            {
                IncompatibleGears = new List<string>();
            }

            //Limitations (Minifig)
            MiniType = MinifigType.Both;
            if (AuthorizedRangeTypes != null)
            { 
                AuthorizedRangeTypes.Clear();
            }
            else
            {
                AuthorizedRangeTypes = new List<RangeType>();
            }

            //Limitations (Minifig)
            if (RestrictedMegaCategories != null)
            {
                RestrictedMegaCategories.Clear();
            }
            else
            {
                RestrictedMegaCategories = new List<MegafigCategory>();
            }

            if (RestrictedMegaSizes != null)
            {
                RestrictedMegaSizes.Clear();
            }
            else
            {
                RestrictedMegaSizes = new List<MegafigSize>();
            }

            SlotSize = 1;
            IsTurret = false;
            TurretPossible = false;

            //Usage
            Cost = 0;
            Limit = 0;

            //Strings
            if (LocNames != null)
            {
                LocNames.Clear();
            }
            else
            {
                LocNames = new List<TextLocData>();
            }

            if (LocDescriptions != null)
            {
                LocDescriptions.Clear();
            }
            else
            {
                LocDescriptions = new List<TextLocData>();
            }
            ExportString = "";
        }

        public GearData GetClone()
        {
            GearData clone = new GearData();

            clone.Id = Id;

            clone.IsReal = IsReal;

            // --- Infos ---
            clone.Color = Color;
            clone.TextColor = TextColor;
            clone.Icon = Icon;

            // --- Limitations (Common) ---
            clone.IsSingleton = IsSingleton;
            clone.UnitType    = UnitType;

            foreach (string s in IncompatibleGears)
            {
                clone.IncompatibleGears.Add(s);
            }

            // --- Limitations (Minifig) ---
            clone.MiniType = MiniType;
            clone.AuthorizedRangeTypes = new List<RangeType>();
            if (AuthorizedRangeTypes != null)
            {
                foreach (var rrt in AuthorizedRangeTypes)
                {
                    clone.AuthorizedRangeTypes.Add(rrt);
                }
            }
            else
            {
                Debug.Log("RestrictedRangeTypes is null!");
            }

            // --- Limitations (Megafig) ---
            clone.RestrictedMegaCategories = new List<MegafigCategory>();
            if (RestrictedMegaCategories != null)
            {
                foreach (var cat in RestrictedMegaCategories)
                {
                    clone.RestrictedMegaCategories.Add(cat);
                }
            }
            else
            {
                Debug.Log("RestrictedMegaCategories is null!");
            }
            clone.RestrictedMegaSizes = new List<MegafigSize>();
            foreach (var size in RestrictedMegaSizes)
            {
                clone.RestrictedMegaSizes.Add(size);
            }
            clone.SlotSize = SlotSize;
            clone.IsTurret = IsTurret;
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

            return clone;
        }

        //I'm not sure I want to nullify
        public void ClearMe()
        {
            //Debug.Log("ClearMe()");

            //Infos
            Id = "";
            IsReal = false; //Most important!
            Color = Color.white;
            TextColor = Color.black;
            Icon = null;

            //Limitations (Common)
            IsSingleton = true;
            UnitType = UnitType.Minifig;
            if (IncompatibleGears != null) IncompatibleGears.Clear();

            //Limitations (Minifig)
            MiniType = MinifigType.Both;
            if (AuthorizedRangeTypes != null) AuthorizedRangeTypes.Clear();

            //Limitations (Minifig)
            if (RestrictedMegaCategories != null) RestrictedMegaCategories.Clear();
            if (RestrictedMegaSizes != null) RestrictedMegaSizes.Clear();
            SlotSize = 1;
            IsTurret = false;
            TurretPossible = false;

            //Usage
            Cost = 0;
            Limit = 0;

            //Strings
            if (LocNames != null) LocNames.Clear();
            if (LocDescriptions != null) LocDescriptions.Clear();
            ExportString = "";
        }

        public static string GetId(GearData gear)
        {
            return gear != null ? gear.Id : "(null)";
        }
        #endregion METHODS
    }
}