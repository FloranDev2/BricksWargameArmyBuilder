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

        //Public (Will certainly be moved to a DataManager)
        // - Constant Data (units stats, gear, ...)
        [Header("Const")]
        public List<MinifigSO> MinifigSOs;
        //I don't need MegafigSOs because Megafigs are charectarized by their options and category.
        public List<GearSO> MinifigGearSOs;
        public List<GearSO> MegafigGearSOs;

        [Header("Period")]
        [SerializeField] private int _periodIndex; //Dynamic (+ saved / loaded)
        public List<PeriodData> PeriodDataList; //Constant

        //Inspector
        [Header("Language")]
        [SerializeField] private int _languageIndex;
        //[SerializeField] private Language _currLanguage = Language.French;
        public List<LanguageData> LanguageDataList;

        [Header("Unit data")] //Will be hidden, but I'm showing it in the inspector for debug purpose
        [SerializeField] private List<MinifigData> _armyMinifigs = new List<MinifigData>();

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
            if (unitIndex < _armyMinifigs.Count)
            {
                _armyMinifigs[unitIndex].CurrentName = newName;
            }
            else
            {

            }

            //TODO: save
        }

        //Getters

        /// <summary>
        /// Get the authorized minifigs for the current period.
        /// </summary>
        /// <returns></returns>
        public List<MinifigSO> GetMinifigs()
        {
            List<MinifigSO> minifigs = new List<MinifigSO>();
            Period currPeriod = GetCurrentPeriod();

            foreach (MinifigSO miniSO in MinifigSOs)
            {
                if (miniSO.Data.Period == currPeriod)
                {
                    minifigs.Add(miniSO);
                }
            }

            return minifigs;
        }

        //Dynamic Data
        public void ChangeUnitClass(int unitIndex/*,*/)
        {

        }

        public void AddUnit()
        {

        }
        #endregion Public

        #endregion METHODS
    }
}