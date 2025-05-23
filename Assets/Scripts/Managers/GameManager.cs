using System.Collections;
using System.Collections.Generic;
using Truelch.Localization;
using Truelch.ScriptableObjects;
using UnityEngine;

namespace Truelch.Managers
{
    /// <summary>
    /// For simplicity sake, for now, all data will be regrouped here. (no data manager)
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        #region ATTRIBUTES
        //Event delegates
        public delegate void OnLanguageChanged(Language newLanguage);
        public static OnLanguageChanged onLanguageChanged;

        //Public (Will certainly be moved to a DataManager)
        // - Constant Data (units stats, gear, ...)
        [Header("Const")]
        public List<MinifigSO> Minifigs;
        public List<GearSO> MinifigGears;

        //TODO: stuff for Megafig

        // - Dynamic Data (army being created)
        //[Header("Dynamic")]

        //Hidden
        // - Period
        private bool _medFanOn = true; //Medieval / Fantasy
        private bool _modFutOn = true; //Modern / Sci-Fi

        // - Language
        private Language _currLanguage = Language.French;
        #endregion ATTRIBUTES


        #region PROPERTIES
        public bool MedFanOn { get { return _medFanOn; } }

        public bool ModFutOn => _modFutOn;

        public Language CurrentLanguage
        {
            get
            {
                return _currLanguage;
            }
            private set
            {
                _currLanguage = value;
                onLanguageChanged?.Invoke(value);
            }
        }
        #endregion PROPERTIES


        #region METHODS

        #region Initialization

        #endregion Initialization

        #region Public
        public void OnSaveClick()
        {

        }

        public void OnMedFanToggleClick()
        {
            _medFanOn = !_medFanOn;
            if (_medFanOn == false && _modFutOn == false)
            {
                _modFutOn = true;
            }
        }

        public void OnModFutToggleClick()
        {
            _modFutOn = !_modFutOn;
            if (_medFanOn == false && _modFutOn == false)
            {
                _medFanOn = true;
            }
        }
        #endregion Public

        #endregion METHODS
    }
}