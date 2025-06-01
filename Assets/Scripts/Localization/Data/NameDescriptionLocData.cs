using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Truelch.Localization
{
    [System.Serializable]
    public class NameDescriptionLocData
    {
        public string Name;
        [TextArea] public string Description;
        public Language Language;
    }
}