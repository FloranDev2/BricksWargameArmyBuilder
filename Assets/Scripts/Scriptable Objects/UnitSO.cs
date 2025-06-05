using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using UnityEngine;

namespace Truelch.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Unit", menuName = "ScriptableObjects/Unit")]
    public class UnitSO : ScriptableObject
    {
        public UnitData Data;
    }
}