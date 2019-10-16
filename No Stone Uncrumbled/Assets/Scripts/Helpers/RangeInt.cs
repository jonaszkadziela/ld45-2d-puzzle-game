[System.Serializable]
public class RangeInt
{
    public int min;
    public int max;

    public RangeInt(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
    
    public static RangeInt operator+(RangeInt range, int number)
    {
        range.min += number;
        range.max += number;

        return range;
    }

    public static RangeInt operator-(RangeInt range, int number)
    {
        range.min -= number;
        range.max -= number;

        return range;
    }
}
