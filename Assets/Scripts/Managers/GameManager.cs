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

        public delegate void OnGearChanged(int unitIndex, int gearIndex, GearData newGear, GearData oldGear);
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
            Debug.Log("ComputeArmySpecialization()");

            //Step 1: prepare spe army data
            Debug.Log("--- Step 1: prepare spe army data ---");
            List<SpecializationGearData> speList = new List<SpecializationGearData>();
            foreach (var gearSO in GearSOs)
            {
                if (gearSO.Data.SlotSize > 1 && gearSO.Data.UnitType == UnitType.Minifig)
                {
                    SpecializationGearData spe = new SpecializationGearData(gearSO.Data/*.GetClone()*/);
                    speList.Add(spe);
                }
            }


            //Step 2: go through all units
            Debug.Log("--- Step 2: go through all units ---");
            foreach (var unit in ArmyUnits)
            {
                Debug.Log(" -> unit: " + unit.CurrentName + "...");

                //Megafig -> no need to check (spe is still ok)
                if (unit.Type == UnitType.Minifig)
                {
                    //A: reset is ok (for spe)
                    foreach (var spe in speList)
                    {
                        spe.IsOk = false;

                        //B: go through all gears and set is ok to true if we find the gear
                        foreach (var gear in unit.GearList)
                        {
                            if (gear != null && gear.IsReal && gear.SO == spe.Gear.SO)
                            {
                                spe.IsOk = true;
                                break;
                            }
                        }

                        Debug.Log("... is spe (" + spe.Gear.LocNames[0].Txt + ") ok: " + spe.IsOk);
                    }
                }
                else
                {
                    Debug.Log("It's a megafig, let's ignore it!");
                }
            }

            //Step 3: apply spe (or NON spe)
            Debug.Log("--- Step 3: apply spe (or NON spe) ---");
            foreach (var unit in ArmyUnits)
            {
                Debug.Log(" -> unit: " + unit.CurrentName);
                for (int i = 0; i < unit.GearList.Count; i++)
                {
                    Debug.Log(" --> i: " + i);
                    var gear = unit.GearList[i];
                    foreach (SpecializationGearData spe in speList)
                    {
                        if (gear != null && gear.IsReal && spe.Gear.SO == gear.SO)
                        {
                            if (spe.IsOk)
                            {
                                Debug.Log("[A] Spe ok! (" + gear.LocNames[0].Txt + ")");
                                spe.Occ++;
                                if (spe.Occ > 1)
                                {
                                    Debug.Log("Need to remove!");
                                    //i--;
                                }
                            }
                            else
                            {
                                Debug.Log("[B] Spe NOT ok! (" + gear.LocNames[0].Txt + ")");

                                for (int j = 1; j < gear.SlotSize; j++)
                                {
                                    int index = i + j;
                                    Debug.Log("index: " + index);
                                    //If we can't fit the gear, let's remove it.
                                    //I might do a "auto-re-arrange" later...
                                    if (unit.GearList[index] == null || !unit.GearList[index].IsReal)
                                    {
                                        Debug.Log("-> Filling the slot!");
                                        spe.Occ++;
                                        if (spe.Occ < gear.SlotSize)
                                        {
                                            unit.GearList[index] = gear.GetClone();
                                            Debug.Log(" ---> Added: " + gear.LocNames[0].Txt + " at: " + index);
                                        }
                                    }
                                    else
                                    {
                                        Debug.Log("-> Can't fill, slot is occupied!");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CheckForGear(GearData changedGear)
        {
            Debug.Log("CheckForGear(changedGear: " + changedGear + ")");

            if (changedGear == null || !changedGear.IsReal)
            {
                Debug.Log("WTF! ChangedGear is null or not real!");
                return;
            }

            SpecializationGearData spe = new SpecializationGearData(changedGear);

            foreach (var unit in ArmyUnits)
            {
                spe.IsOk = false;
                foreach (var gear in unit.GearList)
                {
                    if (gear != null && gear.IsReal && gear.SO == changedGear.SO)
                    {
                        Debug.Log("Here!");
                        spe.IsOk = true;
                    }
                }
            }

            //2nd loop
            foreach (var unit in ArmyUnits)
            {
                for (int i = 0; i < unit.GearList.Count; i++)
                {
                    var gear = unit.GearList[i];
                    int count = 0;
                    if (gear != null && gear.IsReal && gear.SO == changedGear.SO)
                    {
                        Debug.Log("[OK] gear != null && gear.IsReal && gear.SO == changedGear.SO");
                        if (spe.IsOk)
                        {
                            Debug.Log("[A] Is Army Spe");
                            //IS army spe
                            count++;
                            if (count > 1)
                            {
                                //Remove this Gear
                                unit.GearList[i] = null; //should be ok?
                                Debug.Log(">>>>> Yay!");
                            }                            
                        }
                        else
                        {
                            Debug.Log("[B] Is NOT Army Spe");
                            //Not army spe
                            List<GearData> newList = new List<GearData>();

                            int index = 0;
                            while (newList.Count < unit.MaxGear && index < unit.GearList.Count)
                            {
                                Debug.Log("index: " + index + " / " + unit.GearList.Count);
                                if (gear != null && gear.IsReal)
                                {
                                    if (newList.Count + gear.SlotSize <= unit.MaxGear)
                                    {
                                        for (int j = 0; j < gear.SlotSize; j++)
                                        {
                                            if (newList.Count < unit.MaxGear)
                                            {
                                                newList.Add(gear.GetClone());
                                                Debug.Log("j: " + j + " -> add: " + gear.LocNames[0].Txt);
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
                                        for (int j = newList.Count; j < unit.MaxGear; j++)
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
                        }
                    }
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

        //public void ChangeGear(int unitIndex, int gearIndex, GearDataSO newGearSO, GearDataSO oldGearSO)
        public void ChangeGear(int unitIndex, int gearIndex, GearData newGear, GearData oldGear)
        {
            //Debug.Log("ChangeGear(unitIndex: " + unitIndex + ", gearIndex : " + gearIndex + ", newGear: " + newGear.LocNames[0].Txt + ")");
            ComputeArmySpecialization();
            //CheckForGear(newGear);
            //CheckForGear(oldGear);
            onGearChanged?.Invoke(unitIndex, gearIndex, newGear, oldGear);
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