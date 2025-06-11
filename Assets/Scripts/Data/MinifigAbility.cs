using System.Collections;
using System.Collections.Generic;
using Truelch.Localization;
using UnityEngine;

namespace Truelch.Data
{
    [System.Serializable]
    public class MinifigAbility
    {
        #region ATTRIBUTES
        public Sprite Icon;
        public List<TextLocData> LocDescriptions;
        #endregion ATTRIBUTES


        #region METHODS
        public MinifigAbility GetClone()
        {
            MinifigAbility clone = new MinifigAbility();

            clone.Icon = Icon;
            clone.LocDescriptions = new List<TextLocData>();
            foreach (var tld in LocDescriptions)
            {
                clone.LocDescriptions.Add(tld.GetClone());
            }
            
            return clone;
        }
        #endregion METHODS
    }
}