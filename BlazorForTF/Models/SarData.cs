public class SarData
{
    public DateTime Time { get; set; }
    public string CPU { get; set; }

    private double _user;
    public double User
    {
        get { return _user; }
        set { _user = Math.Round(value, 2); }
    }

    private double _nice;
    public double Nice
    {
        get { return _nice; }
        set { _nice = Math.Round(value, 2); }
    }

    private double _system;
    public double System
    {
        get { return _system; }
        set { _system = Math.Round(value, 2); }
    }

    private double _ioWait;
    public double IOWait
    {
        get { return _ioWait; }
        set { _ioWait = Math.Round(value, 2); }
    }

    private double _steal;
    public double Steal
    {
        get { return _steal; }
        set { _steal = Math.Round(value, 2); }
    }

    private double _idle;
    public double Idle
    {
        get { return _idle; }
        set { _idle = Math.Round(value, 2); }
    }
}
