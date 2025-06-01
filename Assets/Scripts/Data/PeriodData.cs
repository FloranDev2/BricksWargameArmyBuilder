using System.Collections;
using System.Collections.Generic;
using Truelch.Enums;
using UnityEngine;

namespace Truelch.Data
{
    public class PeriodData
    {
        [SerializeField] private string _name;
        public Period Period;
        public Color Color;
        public Sprite Icon;
    }
}