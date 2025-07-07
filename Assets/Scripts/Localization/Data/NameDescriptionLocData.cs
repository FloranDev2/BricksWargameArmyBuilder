using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.Localization
{
    [System.Serializable]
    public class NameDescriptionLocData
    {
        public string Name;
        [TextArea(1, 10)] public string Description;
        public Language Language;
    }
}