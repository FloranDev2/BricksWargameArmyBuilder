using System.Collections;
using System.Collections.Generic;
using Truelch.Data;
using Truelch.Enums;
using Truelch.Localization;
using Truelch.ScriptableObjects;
using Truelch.UI;
using UnityEngine;

namespace Truelch.Managers
{
    /// <summary>
    /// For simplicity sake, for now, all data will be regrouped here. (no data manager)
    /// TODO: stuff for Megafig
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        #region ATTRIBUTES
        //Event delegates
        public delegate void OnLanguageChanged(Language newLanguage);
        public static OnLanguageChanged onLanguageChanged;

        public delegate void OnUnitAdded(UnitSO unitSO);
        public static OnUnitAdded onUnitAdded;

        //Public (Will certainly be moved to a DataManager)
        // - Constant Data (units stats, gear, ...)
        [Header("Const")]
        // --- OLD ---
        //public List<MinifigSO> MinifigSOs;
        //I don't need MegafigSOs because Megafigs are charectarized by their options and category.
        //public List<GearSO> MinifigGearSOs;
        //public List<GearSO> MegafigGearSOs;

        // --- NEW ---
        public List<UnitSO> UnitSOs;
        public List<GearSO> GearSOs;

        [Header("Period")]
        [SerializeField] private int _periodIndex; //Dynamic (+ saved / loaded)
        public List<PeriodData> PeriodDataList; //Constant

        //Inspector
        [Header("Language")]
        [SerializeField] private int _languageIndex;
        public List<LanguageData> LanguageDataList;

        [Header("Unit data")] //Will be hidden, but I'm showing it in the inspector for debug purpose
        [SerializeField] private List<UnitData> _armyUnits = new List<UnitData>();

        //Hidden
        // - Managers
        //private SaveManager _saveMgr;

        // - Period
        private bool _medFanOn = true; //Medieval / Fantasy
        private bool _modFutOn = true; //Modern / Sci-Fi
        #endregion ATTRIBUTES


        #region PROPERTIES
        public bool MedFanOn => _modFutOn;
        public bool ModFutOn => _modFutOn;
        #endregion PROPERTIES


        #region METHODS

        #region Initialization
        /*
        IEnumerator Start()
        {
            yield return new WaitUntil(() => SaveManager.Instance != null);
            _saveMgr = SaveManager.Instance;
        }
        */
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

        public Period GetCurrentPeriod()
        {
            if (_periodIndex >= 0 && _periodIndex < PeriodDataList.Count)
            {
                return PeriodDataList[_periodIndex].Period;
            }

            return Period.Any;
        }

        public void SetPeriod(Period newPeriod)
        {
            for (int i = 0; i < PeriodDataList.Count; i++)
            {
                var data = PeriodDataList[i];
                if (data.Period == newPeriod)
                {
                    //We found the appropriate period!
                    _periodIndex = i;

                    //TODO: save here?
                }
            }

            Debug.Log("We couldn't find the period!");
        }

        public Language GetCurrentLanguage()
        {
            if (_languageIndex >= 0 && _languageIndex < LanguageDataList.Count)
            {
                return LanguageDataList[_languageIndex].Language;
            }

            return Language.French;
        }

        public void SetLanguage(Language newLanguage)
        {
            _languageIndex = (int)newLanguage;
            onLanguageChanged?.Invoke(newLanguage);
        }

        public void ChangeUnitName(int unitIndex, string newName)
        {
            if (unitIndex < _armyUnits.Count)
            {
                _armyUnits[unitIndex].CurrentName = newName;
            }

            //TODO: save
        }

        //Dynamic Data
        public void ChangeUnitClass(int unitIndex, UnitData newClass)
        {
            //Keep some stuff? (Name, Gear, ...)

            _armyUnits[unitIndex] = newClass;
        }

        //public UnitData AddUnit(UnitData data)
        public void /*UnitData*/ AddUnit(UnitSO so)
        {
            //Look for the default name:
            //foreach (var locName in data.LocNames)
            foreach (var locName in so.Data.LocNames)
            {
                if (locName.Language == GetCurrentLanguage())
                {
                    //data.CurrentName = locName.Txt;
                    so.Data.CurrentName = locName.Txt;
                }
            }
            
            //_armyUnits.Add(data);
            _armyUnits.Add(so.Data);

            //Event
            onUnitAdded?.Invoke(so);

            //return data;
        }

        public void RemoveUnit(UnitData data)
        {
            _armyUnits.Remove(data);
        }
        #endregion Public

        #endregion METHODS
    }
}