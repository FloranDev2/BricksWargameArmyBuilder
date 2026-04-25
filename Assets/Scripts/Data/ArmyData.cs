using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class ArmyData
    {
        #region ATTRIBUTES
        public string Name;
        public List<UnitData> Units;
        #endregion ATTRIBUTES


        #region METHODS
        public ArmyData()
        {
            Units = new List<UnitData>();
        }
        #endregion METHODS
    }
}