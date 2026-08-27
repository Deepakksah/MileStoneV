namespace VideoOS.Platform.Client.Hid;

public sealed class HidButtonValueChangedEventArgs : HidValueChangedEventArgs<HidButtonValueData>
{
	public HidButtonValueChangedEventArgs(HidInput source, HidButtonValueData value)
		: base(source, value)
	{
	}
}
