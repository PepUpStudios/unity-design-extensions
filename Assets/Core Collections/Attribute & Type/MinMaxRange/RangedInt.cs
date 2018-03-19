
[System.Serializable]
public class RangedInt {
    public int minValue;
    public int maxValue;

    public RangedInt(int min, int max) {
        minValue = min;
        maxValue = max;
    }
}
