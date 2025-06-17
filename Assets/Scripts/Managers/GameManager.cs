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

        public delegate void OnUnitAdded(UnitData unitData);
        public static OnUnitAdded onUnitAdded;

        public delegate void OnUnitClassChanged(int unitIndex, UnitData newClass);
        public static OnUnitClassChanged onUnitClassChanged;

        public delegate void OnGearChanged(int unitIndex, int gearIndex, GearData newGear);
        public static OnGearChanged onGearChanged;

        //Public (Will certainly be moved to a DataManager)
        // - Constant Data (units stats, gear, ...)
        [Header("Const")]
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
        public List<UnitData> ArmyUnits = new List<UnitData>();

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
        // --- UI CALLBACKS ---
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

        public List<GearSO> GetGearSOs(UnitData unitData)
        {
            List<GearSO> gears = new List<GearSO>();
            foreach (GearSO gear in GearSOs)
            {
                bool isOk = true;

                if (gear.Data.UnitType != unitData.Type)
                {
                    isOk = false;
                }

                bool isMegaOk = gear.Data.RestrictedMegaTypes.Count == 0;
                foreach (MegafigType megaType in gear.Data.RestrictedMegaTypes)
                {
                    if (unitData.MegaType == megaType)
                    {
                        isMegaOk = true;
                        break;
                    }
                }
                isOk = isOk && isMegaOk;

                if (isOk)
                {
                    gears.Add(gear);
                }
            }
            return gears;
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

        //Dynamic Data
        public void ChangeUnitClass(int unitIndex, UnitData newClass)
        {
            //Debug.Log("ChangeUnitClass(unitIndex: " + unitIndex + ", newClass: " + newClass + ")");
            ArmyUnits[unitIndex] = newClass;
            onUnitClassChanged?.Invoke(unitIndex, newClass);
        }

        public void ChangeGear(int unitIndex, int gearIndex, GearData newGear)
        {
            onGearChanged?.Invoke(unitIndex, gearIndex, newGear);
        }

        /// <summary>
        /// No need to clone the data, this method will do it already
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public UnitData AddUnit(UnitData src)
        {
            UnitData unitData = src.GetClone();

            //Look for the default name:
            foreach (var locName in src.LocNames)
            {
                if (locName.Language == GetCurrentLanguage())
                {
                    src.CurrentName = locName.Txt;
                }
            }

            ArmyUnits.Add(unitData);

            //Event
            onUnitAdded?.Invoke(unitData);

            return unitData;
        }

        public void RemoveUnit(UnitData data)
        {
            if (ArmyUnits.Contains(data))
            {
                ArmyUnits.Remove(data);
            }
            else
            {
                Debug.Log("Does NOT contain! :(");
            }
        }
        #endregion Public

        #endregion METHODS
    }
}