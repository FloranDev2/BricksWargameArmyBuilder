using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.Localization
{
    [CreateAssetMenu(fileName = "Text Localization", menuName = "ScriptableObjects/TextLocSO")]
    public class TextLocSO : ScriptableObject
    {
        public List<TextLocData> Data;
    }
}