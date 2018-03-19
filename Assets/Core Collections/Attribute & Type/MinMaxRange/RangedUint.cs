
[System.Serializable]
public class RangedUint {
    public uint minValue;
    public uint maxValue;

    public RangedUint(uint min, uint max) {
        minValue = min;
        maxValue = max;
    }
}
