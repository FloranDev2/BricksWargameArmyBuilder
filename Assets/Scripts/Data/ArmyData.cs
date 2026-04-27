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
        public List<UnitData> Units = new List<UnitData>();
        #endregion ATTRIBUTES


        #region METHODS
        public ArmyData()
        {
            Name = "";
            Units = new List<UnitData>();
        }

        public ArmyData GetClone()
        {
            Debug.Log("ArmyData.GetClone()");
            ArmyData clone = new ArmyData();

            clone.Name = Name;
            Debug.Log("clone.Name: " + clone.Name);
            clone.Units = new List<UnitData>();
            foreach (var unit in Units)
            {
                Debug.Log(" -> cloned unit: " + unit.CurrentName);
                clone.Units.Add(unit.GetClone());
            }

            return clone;
        }
        #endregion METHODS
    }
}