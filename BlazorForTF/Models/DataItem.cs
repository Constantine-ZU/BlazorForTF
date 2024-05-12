public class DataItem
{
    public DateTime Time { get; set; }
    private double _value;
    public double Value
    {
        get { return _value; }
        set { _value = Math.Round(value, 2); }
    }
    public string FormattedTime => Time.ToString("HH:mm");
}
