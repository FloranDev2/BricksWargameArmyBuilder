using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.Data
{
    //Should I merge Megafig and Minifigs?
    //One one hand, they have some different specifics features.
    //One the other hand, it'd make them very easier to manage in the UnitManager.
    //Wait, what if I just make a mother class for both?
    //But I think it'll be a problem for the inspector.
    //Let's see...
    [System.Serializable]
    public class MinifigData
    {
        [Header("Constant Data")]
        public Period Period;
        public Color Color;
        public Sprite Icon;
        public List<TextLocData> LocNames;
        public List<MinifigAbility> Abilities;

        [Header("Dynamic Data")]
        public string CurrentName;
    }

    [System.Serializable]
    public class MinifigAbility
    {
        public Sprite Icon;
        public List<TextLocData> LocDescriptions;
    }
}