using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using UnityEngine;

namespace Truelch.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Gear", menuName = "ScriptableObjects/Gear")]
    public class GearSO : ScriptableObject
    {
        public GearData Data;
    }
}