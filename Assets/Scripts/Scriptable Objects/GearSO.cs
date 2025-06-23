using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using UnityEngine;

namespace Truelch.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Gear", menuName = "ScriptableObjects/Gear")]
    public class GearSO : ScriptableObject
    {
        #region ATTRIBUTES
        public GearData Data;
        #endregion ATTRIBUTES


        #region METHODS
        [ContextMenu("Assign SO")]
        void CM_AssignSO()
        {
            Debug.Log("Assign SO");
            Data.SO = this;
        }
        #endregion METHODS
    }
}