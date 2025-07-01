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

        public delegate void OnUnitRemoved(UnitData unitData);
        public static OnUnitRemoved onUnitRemoved;

        public delegate void OnUnitClassChanged(int unitIndex, UnitData newClass);
        public static OnUnitClassChanged onUnitClassChanged;

        public delegate void OnUnitMegaCatChanged(int unitIndex, MegafigCategory newCat);
        public static OnUnitMegaCatChanged onUnitMegaCatChanged;

        public delegate void OnGearChanged(int unitIndex, int gearIndex, GearData newGear/*, GearData oldGear*/);
        public static OnGearChanged onGearChanged;

        public delegate void OnGearRemoved(int unitIndex, int gearIndex);
        public static OnGearRemoved onGearRemoved;

        //Public (Will certainly be moved to a DataManager)
        // - Constant Data (units stats, gear, ...)
        [Header("Const")]
        public List<UnitSO> UnitSOs;
        public List<GearSO> GearSOs;

        [Header("Period")]
        [SerializeField] private int _periodIndex; //Dynamic (+ saved / loaded)
        public List<PeriodData> PeriodDataList; //Constant

        [Header("Megafig")]
        public List<MegafigCategoryLocData> MegafigCategoryLocDataList;

        //Inspector
        [Header("Language")]
        [SerializeField] private int _languageIndex;
        public List<LanguageData> LanguageDataList;

        [Header("Unit data")] //Will be hidden, but I'm showing it in the inspector for debug purpose
        public List<UnitData> ArmyUnits = new List<UnitData>();

        //Hidden
        // - Managers
        private CanvasManager _canvasMgr;
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
        IEnumerator Start()
        {
            yield return new WaitUntil(() =>
                CanvasManager.Instance != null);
            //SaveManager.Instance != null);

            _canvasMgr = CanvasManager.Instance;
            //_saveMgr = SaveManager.Instance;
        }
        #endregion Initialization

        #region Misc
        private void ComputeArmySpecialization()
        {
            //Debug.Log("ComputeArmySpecialization()");

            //Prepare data
            List<SpecializationGearData> speList = new List<SpecializationGearData>();
            foreach (GearSO gearSO in GearSOs)
            {
                if (gearSO.Data.SlotSize == 2 && gearSO.Data.UnitType == UnitType.Minifig)
                {
                    speList.Add(new SpecializationGearData(gearSO.Data));
                }
            }

            //Check for army specialization
            foreach (UnitData unit in ArmyUnits)
            {
                //Set to false again...
                foreach (SpecializationGearData spe in speList)
                {
                    spe.IsOk = false;
                }

                foreach (GearData gear in unit.GearList)
                {
                    //...until we meet the gear and set it to true again!
                    if (gear != null && gear.IsReal)
                    {
                        //Debug.Log("-> gear: " + gear.LocNames[0].Txt + ", gear.SlotSize: " + gear.SlotSize);
                        if (gear.SlotSize == 2)
                        {
                            //Debug.Log(" -> here!");
                            foreach (SpecializationGearData spe in speList)
                            {
                                if (spe.Gear.SO == gear.SO) //new
                                {
                                    //Debug.Log("Spe ok");
                                    spe.IsOk = true;
                                }
                            }
                        }
                    }
                }

                //2nd loop: update specialized gear on units
                //List<GearData> gearToClean = new List<GearData>();
                List<GearData> newList = new List<GearData>();

                int index = 0;
                while (newList.Count < unit.MaxGear && index < unit.GearList.Count)
                {
                    //Debug.Log("index: " + index + " / " + unit.GearList.Count);
                    GearData gear = unit.GearList[index];
                    if (gear != null && gear.IsReal)
                    {
                        if (newList.Count + gear.SlotSize <= unit.MaxGear)
                        {
                            for (int j = 0; j < gear.SlotSize; j++)
                            {
                                if (newList.Count < unit.MaxGear)
                                {
                                    newList.Add(gear.GetClone());
                                    //Debug.Log("j: " + j + " -> add: " + gear.LocNames[0].Txt);
                                }
                                else
                                {
                                    //And tell the UI
                                    //Debug.Log("Maybe I should remove this Gear!");
                                    _canvasMgr.FeedbackUI.ShowTempMsg("Maybe I should remove this Gear!");

                                    break;
                                }
                            }
                        }
                        else
                        {
                            //Instead, fill with empty stuff
                            for (int i = newList.Count; i < unit.MaxGear; i++)
                            {
                                newList.Add(null);
                            }
                            _canvasMgr.FeedbackUI.ShowTempMsg("Maybe I should remove this Gear!");
                        }                        
                    }
                    else
                    {
                        newList.Add(null);
                    }
                    index++;
                }

                unit.GearList = newList; //test

                //Debug
                foreach (var gear in unit.GearList)
                {
                    if (gear != null && gear.IsReal)
                    {
                        //Debug.Log(" -> gear: " + gear.LocNames[0].Txt);
                    }
                }

                //Clear extra
                for (int i = unit.MaxGear; i < unit.GearList.Count; i++)
                {

                }
            }
        }
        #endregion Misc

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
            List<GearSO> availableGears = new List<GearSO>();
            foreach (GearSO gear in GearSOs)
            {
                bool isOk = true;

                if (gear.Data.UnitType != unitData.Type)
                {
                    isOk = false;
                }

                bool isMegaCatOk = gear.Data.RestrictedMegaCategories.Count == 0;
                foreach (MegafigCategory megaType in gear.Data.RestrictedMegaCategories)
                {
                    if (unitData.MegaCategory == megaType)
                    {
                        isMegaCatOk = true;
                        break;
                    }
                }
                isOk = isOk && isMegaCatOk;

                bool isMegaSizeOk = gear.Data.RestrictedMegaSizes.Count == 0;
                foreach (MegafigSize megaSize in gear.Data.RestrictedMegaSizes)
                {
                    if (unitData.MegaSize == megaSize)
                    {
                        isMegaSizeOk = true;
                        break;
                    }
                }
                isOk = isOk && isMegaSizeOk;

                //New: don't allow a gear that's already eqquiped on the unit (if the gear is singleton, which is almost all gears?)
                foreach (var unitGear in unitData.GearList)
                {                    
                    if (unitGear != null && unitGear.IsReal)
                    {
                        if (unitGear.SO == gear && unitGear.IsSingleton)
                        {
                            isOk = false;
                        }
                    }
                }

                //End: Is Ok -> add
                if (isOk)
                {
                    availableGears.Add(gear);
                }
            }
            return availableGears;
        }

        public Period GetCurrentPeriod()
        {
            if (_periodIndex >= 0 && _periodIndex < PeriodDataList.Count)
            {
                return PeriodDataList[_periodIndex].Period;
            }

            return Period.Any;
        }

        public void SetCurrentPeriod(int index)
        {
            _periodIndex = index;
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

        public void ChangeUnitMegaCategory(int unitIndex, MegafigCategory newCat)
        {
            Debug.Log("ChangeUnitMegaCategory(unitIndex: " + unitIndex + ", newCat: " + newCat + ")");
            ArmyUnits[unitIndex].MegaCategory = newCat;
            onUnitMegaCatChanged?.Invoke(unitIndex, newCat);
        }

        public void ChangeGear(int unitIndex, int gearIndex, GearData newGear/*, GearData oldGear*/)
        {
            //Debug.Log("ChangeGear(unitIndex: " + unitIndex + ", gearIndex : " + gearIndex + ", newGear: " + newGear.LocNames[0].Txt + ")");
            ComputeArmySpecialization();
            onGearChanged?.Invoke(unitIndex, gearIndex, newGear/*, oldGear*/);
        }

        public void RemoveGear(int unitIndex, int gearIndex, GearData removedGear)
        {
            for (int i = 0; i < ArmyUnits[unitIndex].GearList.Count; i++)
            {
                var gear = ArmyUnits[unitIndex].GearList[i];
                if (gear != null && gear.IsReal && gear.SO == removedGear.SO)
                {
                    ArmyUnits[unitIndex].GearList[i] = null;
                }
            }

            //ComputeArmySpecialization(); //to update gear icons?
            onGearRemoved?.Invoke(unitIndex, gearIndex);
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
                    unitData.CurrentName = locName.Txt;
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
                onUnitRemoved?.Invoke(data);
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