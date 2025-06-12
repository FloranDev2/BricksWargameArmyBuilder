using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class GearData
    {
        #region ATTRIBUTES
        public Period Period;
        public Color Color = Color.white;
        public Sprite Icon;
        //TODO: regrouper les deux en-dessous en un nouveau type de Data plutot qu'utiliser TextLocData
        public List<TextLocData> LocNames;
        public List<TextLocData> LocDecriptions;
        [Range(1, 2)] public int SlotSize = 1; //Simple: 1, Double: 2 (wait, transport can take more?)
        #endregion ATTRIBUTES


        #region METHODS
        public GearData GetClone()
        {
            GearData clone = new GearData();

            clone.Period = Period;
            clone.Color = Color;
            clone.Icon = Icon;
            clone.LocNames = new List<TextLocData>();
            foreach (var name in LocNames)
            {
                clone.LocNames.Add(name.GetClone());
            }
            clone.LocDecriptions = new List<TextLocData>();
            foreach (var desc in LocDecriptions)
            {
                clone.LocDecriptions.Add(desc.GetClone());
            }
            clone.SlotSize = SlotSize;

            return clone;
        }
        #endregion METHODS
    }
}