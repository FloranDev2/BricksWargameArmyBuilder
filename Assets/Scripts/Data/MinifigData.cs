using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class MinifigData
    {
        public Period Period;
        public Color Color;
        public Sprite Icon;
        public List<TextLocData> LocNames;
        public List<MinifigAbility> Abilities;
    }

    [System.Serializable]
    public class MinifigAbility
    {
        public Sprite Icon;
        public List<TextLocData> LocDescriptions;
    }
}