namespace VideoOS.Platform.Client.Hid;

public sealed class HidAxisValueChangedEventArgs : HidValueChangedEventArgs<double>
{
	public HidAxisValueChangedEventArgs(HidInput source, double value)
		: base(source, value)
	{
	}
}
