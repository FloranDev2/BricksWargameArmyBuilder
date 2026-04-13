using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class UnitData
    {
        #region ATTRIBUTES
        [Header("Infos")]
        public List<TextLocData> LocNames;
        public UnitType Type;
        public Sprite Icon;
        public Color Color;
        public Color TextColor = Color.white;

        [Header("Gameplay")]
        //Below, maybe use an enum?
        //0 is not limit (I considered using -1, but this value will never be used anyway)
        public int MaxAmount; //only for heroes, refered as "Valeur d'Integration" (extended rules, otherwise, it's 1 for heroes I think?)
        public int MinUnitIntegration = 0; //For example, Heavy can only appear in extended format
        public int IntegrationCost; //Minifigs: -1 / Light: 2, Med: 3, Heavy: 4 (or the other way around?)
        public int MaxGear = 3;

        [Header("Minifig only")]
        public MinifigType MiniType;
        public RangeType RangeType;
        public List<MinifigAbility> Abilities;

        [Header("Megafig only")]
        public MegafigSize MegaSize;
        public MegafigCategory MegaCategory = MegafigCategory.Creature; //tmp
        [Min(0)] public int Sturdiness = 3;
        [Min(0)] public int Recuperation = 4; //Salvage, Reclamation?
        //[Min(0)] public int Load = 4; //We'll use the MaxGear stat instead
        [Range(1, 3)] public int Speed = 3; //1: White, 2: White + Grey, 3: White, Grey and Black)

        [Header("Dynamic Data")]
        public string CurrentName;
        /*[System.NonSerialized]*/ public List<GearData> GearList;
        #endregion ATTRIBUTES


        #region METHODS
        public UnitData GetClone()
        {
            //Debug.Log("UnitData.GetClone()");

            //Create a fresh clone
            UnitData clone = new UnitData();

            //Infos
            clone.LocNames = new List<TextLocData>();
            foreach (TextLocData locName in LocNames)
            {
                clone.LocNames.Add(locName.GetClone());
            }
            clone.Type = Type;
            clone.Icon = Icon;
            clone.Color = Color;
            clone.TextColor = TextColor;

            //Gameplay
            //Example: you can't deploy more than 1 Commando (in 1.8, all heroes have max amount == 1 and troops is infinite. But maybe it's different in Epic format)
            clone.MaxAmount = MaxAmount;
            clone.MinUnitIntegration = MinUnitIntegration;
            clone.IntegrationCost = IntegrationCost;
            clone.MaxGear = MaxGear;

            //Minifig only
            clone.MiniType = MiniType;
            clone.Abilities = new List<MinifigAbility>();
            foreach (MinifigAbility ability in Abilities)
            {
                clone.Abilities.Add(ability.GetClone());
            }

            //Megafig only
            clone.MegaSize     = MegaSize;
            clone.MegaCategory = MegaCategory;
            clone.Sturdiness   = Sturdiness;
            clone.Recuperation = Recuperation;
            clone.Speed        = Speed;

            //Dynamic data
            clone.CurrentName = CurrentName;
            clone.GearList = new List<GearData>();
            for (int i = 0; i < MaxGear; i++)
            {
                if (GearList != null && GearList.Count > i)
                {
                    var gear = GearList[i];
                    clone.GearList.Add(gear.GetClone());
                }
                else
                {
                    //Debug.Log("SAFETY WORKED, GEAR ADDED");
                    clone.GearList.Add(new GearData());
                }
            }

            //Return
            return clone;
        }

        public void DebugGears()
        {
            Debug.Log("DebugGears (gear count: " + GearList.Count + ")");
            foreach (var gear in GearList)
            {
                Debug.Log(" -> gear: " + GearData.GetId(gear));
            }
        }
        #endregion METHODS
    }
}