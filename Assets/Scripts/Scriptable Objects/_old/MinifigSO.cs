using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using UnityEngine;

namespace Truelch.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Minifig", menuName = "ScriptableObjects/Minifig")]
    public class MinifigSO : ScriptableObject
    {
        public MinifigData Data;
    }
}