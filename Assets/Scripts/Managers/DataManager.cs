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
    public class DataManager : Singleton<DataManager>
    {
        #region ATTRIBUTES
        //Event delegates
        public delegate void OnLanguageChanged(Language newLanguage);
        public static OnLanguageChanged onLanguageChanged;

        public delegate void OnArmyAdded(/*ArmyData armyData*/);
        public static OnArmyAdded onArmyAdded;

        public delegate void OnUnitAdded(UnitData unitData);
        public static OnUnitAdded onUnitAdded;

        public delegate void OnUnitRemoved(UnitData unitData);
        public static OnUnitRemoved onUnitRemoved;

        public delegate void OnUnitClassChanged(int unitIndex, UnitData newClass);
        public static OnUnitClassChanged onUnitClassChanged;

        public delegate void OnUnitMegaCatChanged(int unitIndex, MegafigCategory newCat);
        public static OnUnitMegaCatChanged onUnitMegaCatChanged;

        public delegate void OnGearsRefreshed();
        public static OnGearsRefreshed onRefreshed;

        //Public (Will certainly be moved to a DataManager)
        // - Constant Data (units stats, gear, ...)
        [Header("Const")]
        public List<UnitSO> UnitSOs;
        public List<GearSO> GearSOs;

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
        private SaveManager   _saveMgr;
        #endregion ATTRIBUTES


        #region METHODS

        #region Initialization
        IEnumerator Start()
        {
            yield return new WaitUntil(() =>
                CanvasManager.Instance &&
                SaveManager.Instance != null
            );

            _canvasMgr = CanvasManager.Instance;
            _saveMgr   = SaveManager.Instance;
        }
        #endregion Initialization

        #region Debug
        [ContextMenu("Debug Gears")]
        void DebugGears()
        {
            Debug.Log("DebugGears()");
            foreach (var unit in ArmyUnits)
            {
                Debug.Log(" -> unit: " + unit.CurrentName);
                foreach (var gear in unit.GearList)
                {
                    Debug.Log(" ----- gear: " + GearData.GetId(gear));
                    if (gear != null && gear.BindedGears != null)
                    {
                        Debug.Log(" ----- gear.BindedGears.Count: " + gear.BindedGears.Count);
                        foreach (var bindedGear in gear.BindedGears)
                        {
                            Debug.Log(" --------> bindedGear: " + GearData.GetId(bindedGear));
                        }
                    }
                }
            }
        }
        #endregion Debug

        #region Misc
        //Why did I do that?
        //I should have named it "CleanAllGears"
        //Ok, I think I did that so that I could handle multi slot gears that should be removed (I guess).
        private void ClearUnfitGears(UnitData unit)
        {
            Debug.Log("===== ClearUnfitGears() =====");

            if (unit.GearList != null)
            {
                //Debug.Log("unit.GearList.Count: " + unit.GearList.Count);
                for (int gearIndex = 0; gearIndex < unit.GearList.Count; gearIndex++)
                {
                    GearData gear = unit.GearList[gearIndex];
                    if (gear != null && gear.SO != null)
                    {
                        //Debug.Log("unit: " + unit);
                        //Debug.Log("gear.SO: " + gear.SO);

                        //if (gear.SO != null && !IsGearOk(unit, gear.SO))
                        //{
                        //    gear.ClearMe();
                        //}

                        //Another thing to consider: when we change Megafig class (from Heavy to Medium for example), slots will reduce.
                        //And since some
                        //Visualization:
                        //Heavy : ABCDEE
                        //Medium: ABCDE-

                        //Debug.Log("gearIndex: " + gearIndex + ", gear.SlotSize: " + gear.SlotSize + ", unit.GearList.Count: " + unit.GearList.Count);
                        //if (gearIndex + gear.SlotSize > unit.GearList.Count)
                        //{
                        //    Debug.Log("HERE!!! gearIndex: " + gearIndex);
                        //    unit.GearList[gearIndex].ClearMe();
                        //}

                        if (!IsGearOk(unit, gear.SO, true))
                        {
                            Debug.Log("HERE! :D :D :D");
                            unit.GearList[gearIndex].ClearMe();
                        }
                    }
                }
            }
            else
            {
                Debug.Log("unit.GearList is null!");
            }
        }
        #endregion Misc

        #region Public
        // --- UI CALLBACKS ---
        public void OnSaveClick()
        {
            //_saveMgr.
        }

        public bool IsGearOk(UnitData unitData, GearSO gearSO, bool isDebug = false)
        {
            if (isDebug)
            {
                Debug.Log("IsGearOk(unitData: " + unitData + ", gearSO: " + gearSO + ")");
                if (gearSO != null) Debug.Log("--- gearSO.Data: " + gearSO.Data);
                if (unitData != null) Debug.Log("--- unitData.Type: " + unitData.Type);
            }

            //gearSO can be null
            if (gearSO == null)
            {
                if (isDebug) Debug.Log("gearSO == null ===> RETURN FALSE");
                return false;
            }

            bool isOk = true;

            //Minifig vs Megafig
            if (gearSO.Data.UnitType != unitData.Type)
            {
                if (isDebug) Debug.Log("gearSO.Data.UnitType != unitData.Type ===> isOk = false");
                isOk = false;
            }

            //MF Category (Ground, Levitation, Walker, Flying, Creature, Support)
            bool isMegaCatOk = gearSO.Data.RestrictedMegaCategories.Count == 0;
            foreach (MegafigCategory megaType in gearSO.Data.RestrictedMegaCategories)
            {
                if (unitData.MegaCategory == megaType)
                {
                    isMegaCatOk = true;
                    break;
                }
            }
            isOk = isOk && isMegaCatOk;

            //Light, Medium, Heavy
            bool isMegaSizeOk = gearSO.Data.RestrictedMegaSizes.Count == 0;
            foreach (MegafigSize megaSize in gearSO.Data.RestrictedMegaSizes)
            {
                if (unitData.MegaSize == megaSize)
                {
                    isMegaSizeOk = true;
                    break;
                }
            }
            isOk = isOk && isMegaSizeOk;

            //Singleton
            //New: don't allow a gear that's already equipped on the unit (if the gear is singleton, which is almost all gears?)
            //Woops, it'll detect itself, making it ALWAYS false?
            foreach (var unitGear in unitData.GearList)
            {
                if (unitGear != null && unitGear.IsReal)
                {
                    if (unitGear.SO == gearSO && unitGear.IsSingleton)
                    {
                        if (isDebug) Debug.Log("(unitGear.SO == gearSO && unitGear.IsSingleton ===> isOk = false");
                        isOk = false;
                    }
                }
            }

            //Incompatible Gears (flying vs mounted)
            foreach (string s in gearSO.Data.IncompatibleGears)
            {
                foreach (var g in unitData.GearList)
                {
                    if (g != null && g.Id == s)
                    {
                        if (isDebug) Debug.Log("Incompatible Gears ===> isOk = false");
                        isOk = false;
                    }
                }
            }

            //Attack type (melee weapon is reserved for ranged units)
            if (gearSO.Data.AuthorizedRangeTypes.Count > 0)
            {
                bool isRangeTypeOk = false;
                foreach (var rt in gearSO.Data.AuthorizedRangeTypes)
                {
                    if (rt == unitData.RangeType)
                    {
                        isRangeTypeOk = true;
                    }
                }
                if (isDebug) Debug.Log("Incompatible Gears ===> isOk = false");
                isOk = isOk && isRangeTypeOk;
            }

            //Minifig type (Companion for heroes / Captain and heavy weapon for troops)
            if (gearSO.Data.MiniType != MinifigType.Both && gearSO.Data.MiniType != unitData.MiniType)
            {
                if (isDebug) Debug.Log("Minifig type ===> isOk = false");
                isOk = false;
            }

            return isOk;
        }

        public List<GearSO> GetGearSOs(UnitData unitData)
        {
            List<GearSO> availableGears = new List<GearSO>();
            foreach (GearSO gearSO in GearSOs)
            {
                bool isOk = IsGearOk(unitData, gearSO);

                //End: Is Ok -> add
                if (isOk)
                {
                    availableGears.Add(gearSO);
                }
            }
            return availableGears;
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

        public void ChangeUnitClass(UnitData changingUnit, UnitSO newClassSO)
        {
            Debug.Log("===== ChangeUnitClass(changingUnit: " + changingUnit.CurrentName + ", newClassSO: " + newClassSO.name + ") =====");

            string name = changingUnit.CurrentName;
            var newUnit = newClassSO.Data.GetClone();

            if (!ArmyUnits.Contains(changingUnit))
            {
                Debug.Log("WTF -> changingUnit: " + changingUnit);

                foreach (var unit in ArmyUnits)
                {
                    Debug.Log(" -> army unit: " + unit);
                }

                return;
            }

            int index = ArmyUnits.IndexOf(changingUnit);

            //Check if we need to remove a multi slot item if one of them is removed
            Debug.Log("changingUnit.MaxGear: " + changingUnit.MaxGear + ", newClassSO.Data.MaxGear: " + newClassSO.Data.MaxGear);
            if (changingUnit.MaxGear > newClassSO.Data.MaxGear)
            {
                Debug.Log("Here");
                //for (int i = changingUnit.MaxGear; i <= newClassSO.Data.MaxGear; i++)
                for (int i = newClassSO.Data.MaxGear; i < changingUnit.MaxGear; i++)
                {
                    Debug.Log("i: " + i);
                    //changingUnit.GearList[i].ClearMe();

                    var gear = changingUnit.GearList[i];
                    foreach (var gear2 in gear.BindedGears)
                    {
                        //Debug.Log("gear2 index: " + gear.BindedGears.IndexOf(gear2));
                        Debug.Log("gear2 index: " + changingUnit.GearList.IndexOf(gear2));
                        gear2.ClearMe();
                    }
                }
            }

            //Keep gears when applicable
            List<GearData> gears = new List<GearData>();
            if (changingUnit.Type == newClassSO.Data.Type)
            {
                for (int i = 0; i < newClassSO.Data.MaxGear; i++)
                {
                    GearData gear = new GearData();

                    if (i < changingUnit.GearList.Count && changingUnit.GearList[i] != null)
                    {
                        gear = changingUnit.GearList[i].GetClone();
                    }
                    gears.Add(gear);
                }
            }
            newUnit.GearList = gears;

            //Give back the things saved, IF they are relevant
            if (!string.IsNullOrEmpty(name))
            {
                newUnit.CurrentName = name;
            }

            ArmyUnits[index] = newUnit;

            //New
            ClearUnfitGears(newUnit);

            onUnitClassChanged?.Invoke(index, newUnit);
        }
            
        public void ChangeUnitMegaCategory(int unitIndex, MegafigCategory newCat)
        {
            Debug.Log("ChangeUnitMegaCategory(unitIndex: " + unitIndex + ", newCat: " + newCat + ")");

            var changingUnit = ArmyUnits[unitIndex];
            changingUnit.MegaCategory = newCat;

            //Keep gears when applicable
            List<GearData> gears = new List<GearData>();
            for (int i = 0; i < changingUnit.MaxGear; i++)
            {
                GearData gear = new GearData();
                bool cond1 = i < changingUnit.GearList.Count;
                bool cond2 = changingUnit.GearList[i] != null;
                bool cond3 = IsGearOk(changingUnit, changingUnit.GearList[i].SO/*, true*/);
                if (cond1 && cond2 && cond3)
                {
                    gear = changingUnit.GearList[i].GetClone();
                }
                gears.Add(gear);
            }
            changingUnit.GearList = gears;

            //New
            //ClearUnfitGears(changingUnit);

            //int unitIndex, MegafigCategory newCat
            onUnitMegaCatChanged?.Invoke(unitIndex, newCat);
        }

        //This can be unsuccessful
        public void TryToChangeGear(int unitIndex, int gearIndex, GearData newGear, GearData oldGear)
        {
            Debug.Log("TryToChangeGear(unitIndex: " + unitIndex + ", gearIndex: " + gearIndex + ", newGear: " + GearData.GetId(newGear) + ", oldGear: " + GearData.GetId(oldGear) + ")");

            var unit = ArmyUnits[unitIndex];

            if (oldGear != null && oldGear.SlotSize > 1)
            {
                foreach (var gear in oldGear.BindedGears)
                {
                    gear.ClearMe();
                }
            }

            //If we have space, fill with the needed.
            //If there's no space, don't do it and display a feedback.
            //+ Apply new gear!
            //Debug.Log("Apply new gear:");
            List<GearData> bindedGears = new List<GearData>();
            if (gearIndex + newGear.SlotSize - 1 < unit.GearList.Count)
            {
                for (int index = gearIndex; index < gearIndex + newGear.SlotSize; index++)
                {
                    GearData clonedGear = newGear.GetClone();
                    unit.GearList[index] = clonedGear;
                    bindedGears.Add(clonedGear);
                }                
            }
            else
            {
                //Debug.Log("YOU CANNOT ADD THIS GEAR HERE!");
                //if (_canvasMgr != null && _canvasMgr.FeedbackUI != null)
                //{
                //    _canvasMgr.FeedbackUI.ShowPopUp("Tu ne peux pas ajouter cet équipement ici !");
                //}
            }

            //Link gears together
            //Debug.Log("Link gears together:");
            //for (int index = gearIndex; index < unit.GearList.Count; index++)
            for (int index = gearIndex; index < gearIndex + newGear.SlotSize; index++)
            {
                //Debug.Log("index: " + index);
                var gear2 = unit.GearList[index];

                if (gear2 == null)
                {
                    Debug.Log("===> HERE gear2 == null (bandaid fix applied)");
                    unit.GearList[index] = new GearData(); //bandaid fix
                }

                gear2.BindedGears = new List<GearData>();
                foreach (var gear in bindedGears)
                {
                    //Debug.Log("-> here -> gear2: " + GearData.DebugMe(gear2));
                    gear2.BindedGears.Add(gear);
                }
            }

            //DebugGears();

            //Event
            onRefreshed?.Invoke();
        }

        public void RemoveGear(int unitIndex, int gearIndex, GearData removedGear)
        {
            //Debug.Log("RemoveGear(unitIndex: " + unitIndex + ", gearIndex: " + gearIndex + ", removedGear: " + GearData.GetId(removedGear) + ")");

            var gear1 = ArmyUnits[unitIndex].GearList[gearIndex];
            //Debug.Log("gear1: " + GearData.DebugMe(gear1));
            if (gear1 != null)
            {
                //Debug.Log("gear1.BindedGears: " + gear1.BindedGears);
                if (gear1.BindedGears != null)
                {
                    //Debug.Log("gear1.BindedGears.Count: " + gear1.BindedGears.Count);
                }
                foreach (GearData gear2 in gear1.BindedGears)
                {
                    //Debug.Log(" -> binded gear: " + gear2);
                    if (gear2 != null)
                    {
                        gear2.ClearMe();
                    }
                }
            }
            else
            {
                //Debug.Log("gear1 == null");
            }

            //Event (should I still use it?)
            onRefreshed?.Invoke();
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

            //unitData.DebugGears();

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