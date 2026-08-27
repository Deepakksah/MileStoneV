using System;

namespace VideoOS.Platform.Client.Hid;

public abstract class HidValueChangedEventArgs : EventArgs
{
	public HidInput Source { get; }

	public object Value { get; }

	public HidValueChangedEventArgs(HidInput source, object value)
	{
		Source = source;
		Value = value;
	}
}
public abstract class HidValueChangedEventArgs<T> : HidValueChangedEventArgs
{
	public new T Value => (T)base.Value;

	public HidValueChangedEventArgs(HidInput source, T value)
		: base(source, value)
	{
	}
}
