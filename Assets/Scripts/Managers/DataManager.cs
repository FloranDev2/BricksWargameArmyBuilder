using BayatGames.SaveGameFree;
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

        public delegate void OnArmyAdded(ArmyData armyData);
        public static OnArmyAdded onArmyAdded;

        public delegate void OnArmyRemoved(ArmyData armyData);
        public static OnArmyRemoved onArmyRemoved;

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
        [Header("App")]
        public bool IsPublicVersion = true; //true: public version (hidden texts) / false: "briqueux" version (full content)
        
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

        [Header("Armies")]
        private ArmyData _currArmy;//IN A FUTURE VERSION: rather _tmpArmy; we need to confirm a save to change the data
        public List<ArmyData> Armies = new List<ArmyData>();

        //Hidden
        // - Managers
        //private CanvasManager _canvasMgr;

        // - Misc
        private bool _isReady = false;
        #endregion ATTRIBUTES


        #region PROPERTIES
        public bool IsReady
        {
            get
            {
                return _isReady;
            }
        }

        public ArmyData CurrArmy
        {
            get
            {
                return _currArmy;
            }
            set
            {
                _currArmy = value;
            }
        }

        public List<UnitData> ArmyUnits
        {
            get
            {
                return CurrArmy.Units;
            }
        }
        #endregion PROPERTIES


        #region METHODS

        #region Initialization
        /*IEnumerator*/ void Start()
        {
            //yield return new WaitUntil(() => CanvasManager.Instance);
            //_canvasMgr = CanvasManager.Instance;
            Load();
            _isReady = true;
        }
        #endregion Initialization

        #region Misc
        private GearSO FindGearSO(string id)
        {
            foreach (GearSO gearSO in GearSOs)
            {
                if (gearSO.Data.Id == id)
                {
                    return gearSO;
                }
            }
            Debug.Log("Couldn't find gear's scriptable object for this id: " + id);
            return null;
        }

        private UnitSO FindUnitSO(string id)
        {
            foreach (UnitSO unitSO in UnitSOs)
            {
                if (unitSO.Data.Id == id)
                {
                    return unitSO;
                }
            }
            Debug.Log("Couldn't find unit's scriptable object for this id: " + id);
            return null;
        }
        #endregion Misc

        #region Save / Load
        [ContextMenu("Load")]
        private void Load()
        {
            //Debug.Log("Load"); //TODO: feedback pop up!
            if (SaveGame.Exists("armies"))
            {
                Armies = SaveGame.Load<List<ArmyData>>("armies");

                //Fix data
                foreach (ArmyData army in Armies)
                {
                    foreach (UnitData unit in army.Units)
                    {
                        UnitSO unitSO = FindUnitSO(unit.Id);
                        if (unitSO != null) //this one shouldn't be null
                        {
                            unit.Icon = unitSO.Data.Icon;
                        }
                        foreach (GearData gear in unit.GearList)
                        {
                            GearSO gearSO = FindGearSO(gear.Id);
                            if (gearSO != null) //this one can be null if the gear slot is empty
                            {
                                gear.Icon = gearSO.Data.Icon;
                            }
                        }
                    }
                }
            }
        }

        [ContextMenu("Save")]
        private void Save()
        {
            Debug.Log("Save"); //TODO: feedback pop up!
            SaveGame.Save("armies", Armies);
        }

        [ContextMenu("Clear Save Data")]
        private void ClearSaveData()
        {
            SaveGame.DeleteAll();
        }
        #endregion Save / Load

        #region Misc
        //Why did I do that?
        //I should have named it "CleanAllGears"
        //Ok, I think I did that so that I could handle multi slot gears that should be removed (I guess).
        private void ClearUnfitGears(UnitData unit)
        {
            if (unit.GearList != null)
            {
                for (int gearIndex = 0; gearIndex < unit.GearList.Count; gearIndex++)
                {
                    GearData gear = unit.GearList[gearIndex];
                    if (gear != null && !string.IsNullOrEmpty(gear.Id))
                    {
                        if (!IsGearOk(unit, gear, false, true))
                        {
                            unit.GearList[gearIndex].ClearMe();
                        }

                        if (gear.IsTurret)
                        {
                            //Debug.Log("Turret!");
                            int prevIndex = gearIndex - 1;
                            if (prevIndex >= 0 && prevIndex < unit.GearList.Count)
                            {
                                var prevGear = unit.GearList[prevIndex];
                                if (!prevGear.IsReal || !prevGear.TurretPossible)
                                {
                                    //Debug.Log("Previous gear isn't fitting for a turret!");
                                    unit.GearList[gearIndex].ClearMe();
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.Log("unit.GearList is null!");
            }
        }

        //We are using it only if the gear isn't a singleton.
        private void ClearSimilarGears(UnitData unitData, GearData gearData)
        {
            if (gearData == null) //should NOT happen
            {
                Debug.Log("WTF");
                return;
            }

            if (!gearData.IsSingleton) return;

            //Oooh, the issue is that the gear is clear in the mean time.
            //Let's save the Id

            string id = gearData.Id;

            for (int i = 0; i < unitData.GearList.Count; i++)
            {
                var gear2 = unitData.GearList[i];
                if (gear2 != null && gear2.Id == id)
                {
                    gear2.ClearMe();
                }
                else if (gear2 == null)
                {
                    //Debug.Log("gear2 == null => new data and clear");
                    gear2 = new GearData();
                    gear2.ClearMe();
                }
            }
        }
        #endregion Misc

        #region Public
        // --- UI CALLBACKS ---
        public void OnSaveClick()
        {
            Save();
        }

        private bool IsTurretOk(UnitData unitData, int turretGearIndex)
        {
            int prevIndex = turretGearIndex - 1;
            if (prevIndex >= 0 && prevIndex < unitData.GearList.Count) //second condition shouldn't be necessary, but let's stay safe...
            {
                GearData prevGear = unitData.GearList[prevIndex];
                return prevGear.TurretPossible;
            }
            //Debug.Log("WTF");
            return false;
        }

        public bool IsGearOk(UnitData unitData, GearData gearData, bool checkSingleton, bool isDebug)
        {
            if (gearData.Id == "megafig_transport") isDebug = true;

            //if (isDebug) Debug.Log("IsGearOk(unitData: " + unitData.Id + ", unit range type: " + unitData.RangeType + ", gearData: " + gearData.Id + ")");

            //gearSO can be null
            if (gearData == null || string.IsNullOrEmpty(gearData.Id))
            {
                if (isDebug) Debug.Log("gearData / SO == null ===> RETURN FALSE");
                return false;
            }

            bool isOk = true;

            //Minifig vs Megafig
            if (gearData.UnitType != unitData.Type)
            {
                //if (isDebug) Debug.Log("gearSO.Data.UnitType != unitData.Type ===> isOk = false");
                isOk = false;
            }

            //MF Category (Ground, Levitation, Walker, Flying, Creature, Support)
            bool isMegaCatOk = gearData.RestrictedMegaCategories.Count == 0;
            foreach (MegafigCategory megaType in gearData.RestrictedMegaCategories)
            {
                if (unitData.MegaCategory == megaType)
                {
                    isMegaCatOk = true;
                    break;
                }
            }
            isOk = isOk && isMegaCatOk;

            //Light, Medium, Heavy
            bool isMegaSizeOk = gearData.RestrictedMegaSizes.Count == 0;

            foreach (MegafigSize megaSize in gearData.RestrictedMegaSizes)
            {
                if (unitData.MegaSize == megaSize)
                {
                    isMegaSizeOk = true;
                    break;
                }
            }
            isOk = isOk && isMegaSizeOk;

            //Singleton (TODO)
            if (checkSingleton)
            {
                //foreach (var gear in unitData.GearList)
                for (int i = 0; i < unitData.GearList.Count; i++)
                {
                    var gear = unitData.GearList[i];
                    //Debug.Log("[bug] gear: " + GearData.GetId(gear)); //this one happens to be null when changing class
                    if (gear == null)
                    {
                        gear = new GearData();
                    }
                    //Debug.Log("[bug] gearData: " + GearData.GetId(gearData));
                    if (gear.IsSingleton && gear.Id == gearData.Id)
                    {
                        //if (isDebug) Debug.Log("Here! (singleton)");
                        isOk = false;
                        break;
                    }
                }
            }

            //Incompatible Gears (flying vs mounted)
            foreach (string s in gearData.IncompatibleGears)
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
            //if (isDebug) Debug.Log("gearData.RestrictedRangeTypes.Count: " + gearData.AuthorizedRangeTypes.Count);
            if (gearData.AuthorizedRangeTypes.Count > 0)
            {
                bool isRangeTypeOk = false;
                foreach (RangeType rt in gearData.AuthorizedRangeTypes)
                {
                    //if (isDebug) Debug.Log(" -> range type: " + rt + " / unitData.RangeType: " + unitData.RangeType);
                    if (rt == unitData.RangeType)
                    {
                        //if (isDebug) Debug.Log(" ---> YES!");
                        isRangeTypeOk = true;
                    }
                    //else
                    //{
                    //    if (isDebug) Debug.Log(" ---> NO!");
                    //}
                }
                //if (isDebug) Debug.Log(" [END] ===> isRangeTypeOk: " + isRangeTypeOk);
                isOk = isOk && isRangeTypeOk;
            }

            //Minifig type (Companion for heroes / Captain and heavy weapon for troops)
            if (gearData.MiniType != MinifigType.Both && gearData.MiniType != unitData.MiniType)
            {
                if (isDebug) Debug.Log("Minifig type ===> isOk = false");
                isOk = false;
            }

            return isOk;
        }

        //New: index is for the turret option
        public List<GearSO> GetGearSOs(UnitData unitData, int index)
        {
            List<GearSO> availableGears = new List<GearSO>();
            foreach (GearSO gearSO in GearSOs)
            {
                bool isOk = IsGearOk(unitData, gearSO.Data, true, false /*true*/);

                //Additional check for turret
                if (gearSO.Data.IsTurret)
                {
                    isOk = isOk && IsTurretOk(unitData, index);
                }

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
            if (changingUnit == null)
            {
                Debug.Log("ChangeUnitClass -> WTF changingUnit == null");
            }

            //Debug.Log("===== ChangeUnitClass(changingUnit: " + changingUnit?.CurrentName + ", newClassSO: " + newClassSO.name + ") =====");

            string currName = changingUnit.CurrentName;
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
            if (changingUnit.MaxGear > newClassSO.Data.MaxGear)
            {
                for (int i = newClassSO.Data.MaxGear; i < changingUnit.MaxGear; i++)
                {
                    ClearSimilarGears(changingUnit, changingUnit.GearList[i]);
                }
            }

            //Keep gears when applicable
            //This was an issue when we used BindedGears, but we are giving up on them
            //Maybe I could use gears.RemoveRange() instead of creating a new list and assigning it
            List<GearData> gears = new List<GearData>();
            if (changingUnit.Type == newClassSO.Data.Type)
            {
                for (int i = 0; i < newClassSO.Data.MaxGear; i++)
                {
                    GearData gear = new GearData();

                    if (i < changingUnit.GearList.Count && changingUnit.GearList[i] != null)
                    {
                        //PROBLEM: DOING THIS WILL LOSE THE BINDED GEARS FOR MULTI SLOT GEARS!!!
                        gear = changingUnit.GearList[i].GetClone();
                    }
                    gears.Add(gear);
                }
            }
            newUnit.GearList = gears;

            //Give back the things saved, IF they are relevant
            if (!string.IsNullOrEmpty(currName))
            {
                newUnit.CurrentName = currName;
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
                bool cond3 = IsGearOk(changingUnit, changingUnit.GearList[i], false, true);
                if (cond1 && cond2 && cond3)
                {
                    gear = changingUnit.GearList[i].GetClone();
                }
                gears.Add(gear);
            }
            changingUnit.GearList = gears;

            //int unitIndex, MegafigCategory newCat
            onUnitMegaCatChanged?.Invoke(unitIndex, newCat);
        }

        //This can be unsuccessful
        public void TryToChangeGear(int unitIndex, int gearIndex, GearData newGear, GearData oldGear)
        {
            //Debug.Log("TryToChangeGear(unitIndex: " + unitIndex + ", gearIndex: " + gearIndex + ", newGear: " + GearData.GetId(newGear) + ", oldGear: " + GearData.GetId(oldGear) + ")");

            var unit = ArmyUnits[unitIndex];

            if (oldGear != null && oldGear.SlotSize > 1)
            {
                ClearSimilarGears(unit, unit.GearList[gearIndex]); //I hope this will work
            }

            if (gearIndex + newGear.SlotSize - 1 < unit.GearList.Count)
            {
                for (int index = gearIndex; index < gearIndex + newGear.SlotSize; index++)
                {
                    GearData clonedGear = newGear.GetClone();
                    unit.GearList[index] = clonedGear;
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

            // --- NEW ---
            // -> WILL IT WORK?
            ClearUnfitGears(unit); //test to remove turrets

            //Event
            onRefreshed?.Invoke();
        }

        public void RemoveGear(int unitIndex, int gearIndex)
        {
            UnitData unit = ArmyUnits[unitIndex];
            GearData gear = unit.GearList[gearIndex];
            gear.ClearMe(); //for transport, since clear similar gears WON'T clear self!
            ClearSimilarGears(unit, gear);

            // --- NEW ---
            // -> IT WORKS!!!
            ClearUnfitGears(unit); //test to remove turrets

            onRefreshed?.Invoke();
        }

        /// <summary>
        /// No need to clone the data, this method will do it already
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public UnitData AddUnit(UnitData src)
        {
            //Debug.Log("AddUnit");

            UnitData unitData = src.GetClone();

            //Look for the default name:
            foreach (var locName in src.LocNames)
            {
                if (locName.Language == GetCurrentLanguage())
                {
                    //unitData.CurrentName = locName.Txt; //NO
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

        public void AddArmy(ArmyData src)
        {
            //var army = new ArmyData();
            var army = src.GetClone();
            Armies.Add(army);

            //Event
            onArmyAdded?.Invoke(army);
        }

        public void RemoveArmy(ArmyData army)
        {
            if (Armies.Contains(army))
            {
                Armies.Remove(army);
            }
            else
            {
                Debug.Log("The army we want to remove isn't in the army list!");
            }

            //Event
            onArmyRemoved?.Invoke(army);
        }
        #endregion Public

        #endregion METHODS
    }
}