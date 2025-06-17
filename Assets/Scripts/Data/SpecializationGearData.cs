namespace Truelch.Data
{
    [System.Serializable]
    public class SpecializationGearData
    {
        public GearData Gear;
        public bool IsOk;

        public SpecializationGearData(GearData gear)
        {
            Gear = gear;
            IsOk = true;
        }
    }
}