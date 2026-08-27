namespace VideoOS.Platform.Client.Hid;

public sealed class HidSliderValueChangedEventArgs : HidValueChangedEventArgs<double>
{
	public HidSliderValueChangedEventArgs(HidInput source, double value)
		: base(source, value)
	{
	}
}
