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
        public bool IsReal = false; //Null stuff, when displayed in the inspector, isn't null anymore.
        public Period Period;
        public Color Color = Color.white;
        public Sprite Icon;
        //TODO: regrouper les deux en-dessous en un nouveau type de Data plutot qu'utiliser TextLocData
        public List<TextLocData> LocNames;
        public List<TextLocData> LocDescriptions;
        [Range(1, 2)] public int SlotSize = 1; //Simple: 1, Double: 2 (wait, transport can take more?)
        #endregion ATTRIBUTES


        #region METHODS
        public GearData GetClone()
        {
            GearData clone = new GearData();

            clone.IsReal = IsReal;

            clone.Period = Period;
            clone.Color = Color;
            clone.Icon = Icon;
            
            if (LocNames != null)
            {
                //Debug.Log("LocNames is ok :)");
                clone.LocNames = new List<TextLocData>();
                foreach (var name in LocNames)
                {
                    clone.LocNames.Add(name.GetClone());
                }
            }
            else
            {
                Debug.Log("LocNames DOES NOT EXIST :(");
            }

            if (LocDescriptions != null)
            {
                //Debug.Log("LocDescriptions is ok :)");
                clone.LocDescriptions = new List<TextLocData>();
                foreach (var desc in LocDescriptions)
                {
                    clone.LocDescriptions.Add(desc.GetClone());
                }
                clone.SlotSize = SlotSize;
            }
            else
            {
                Debug.Log("LocDescriptions DOES NOT EXIST :(");
            }

            return clone;
        }
        #endregion METHODS
    }
}