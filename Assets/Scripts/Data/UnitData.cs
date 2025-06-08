using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Enums;
using Truelch.Localization;
using Truelch.ScriptableObjects;
using UnityEngine;

namespace Truelch.Data
{
    /// <summary>
    /// Regroup both minifigs (troops or heroes) and megafigs.
    /// Is this actually a good idea?
    /// 
    /// Minifig classes:
    /// - Commando
    /// - Medic
    /// - Infantry
    /// - (...)
    /// 
    /// Megafig classes:
    /// - Light
    /// - Medium
    /// - Heavy
    /// 
    /// Categories will be a mandatory option:
    /// - Terrestre (Ground)
    /// - A gravite (Levitation)
    /// - Marcheur (Walker / Mech)
    /// - Volante (Flying)
    /// - Creature (Creature)
    /// - Soutien (Support)
    /// </summary>
    [System.Serializable]
    public class UnitData
    {
        [Header("Infos")]
        //[SerializeField] private string _name; //For inspector (unnecessary, it's hold within a SO)
        public List<TextLocData> LocNames;
        public UnitType Type;
        public Sprite Icon;
        public Color Color;

        [Header("Gameplay")]
        //Below, maybe use an enum?
        //0 is not limit (I considered using -1, but this value will never be used anyway)
        public int MaxAmount; //only for heroes, refered as "Valeur d'Integration" (extended rules, otherwise, it's 1 for heroes I think?)
        public int MinUnitIntegration = 0; //For example, Heavy can only appear in extended format
        public int IntegrationCost; //Minifigs: -1 / Light: 2, Med: 3, Heavy: 4 (or the other way around?)
        public int MaxGear = 3;
        //public List<GearSO> AvailableGear; //Or, I do that by script

        [Header("Minifig only")]
        public MinifigType MiniType;
        public List<MinifigAbility> Abilities;

        [Header("Megafig only")]
        public MegafigType MegaType;
        [Min(0)] public int Sturdiness = 3;
        [Min(0)] public int Recuperation = 4; //Salvage, Reclamation?
        //[Min(0)] public int Load = 4; //We'll use the MaxGear stat instead
        [Range(1, 3)] public int Speed = 3; //1: White, 2: White + Grey, 3: White, Grey and Black)

        [Header("Dynamic Data")]
        public string CurrentName;
        public List<GearData> GearList;
    }
}