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
        public Period Period;
        public Color Color = Color.white;
        public Sprite Icon;
        //TODO: regrouper les deux en-dessous en un nouveau type de Data plutot qu'utiliser TextLocData
        public List<TextLocData> LocNames;
        public List<TextLocData> LocDecriptions;
        [Range(1, 2)] public int SlotSize = 1; //Simple: 1, Double: 2 (wait, transport can take more?)
    }
}