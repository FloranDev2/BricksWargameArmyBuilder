using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Enums;
using Truelch.Localization;
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
        [SerializeField] private string _name; //For inspector
        public List<TextLocData> LocNames;
        public UnitType Type;
        public Sprite Icon;
        public Color Color;

        [Header("Gameplay")]
        //Below, maybe use an enum?
        public int MinUnitIntegration = 0; //For example, Heavy can only appear in extended format
        public int IntegrationCost; //Minifigs: -1 / Light: 2, Med: 3, Heavy: 4
        public List<GearData> GearList;

        [Header("Minifig only")]


        [Header("Megafig only")]

        [Header("Dynamic Data")]
        public string CurrentName;
    }
}