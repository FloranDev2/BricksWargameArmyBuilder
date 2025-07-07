namespace Truelch.Data
{
    [System.Serializable]
    public class SpecializationGearData
    {
        #region ATTRIBUTES
        public GearData Gear;
        public bool IsOk;
        public int Occ;
        #endregion ATTRIBUTES


        #region METHODS
        public SpecializationGearData(GearData gear)
        {
            Gear = gear;

            IsOk = true;
            Occ = 0;
        }
        #endregion METHODS
    }
}