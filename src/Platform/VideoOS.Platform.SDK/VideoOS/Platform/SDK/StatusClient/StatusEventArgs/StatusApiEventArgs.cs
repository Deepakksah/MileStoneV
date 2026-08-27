using System;
using System.Globalization;

namespace VideoOS.Platform.SDK.StatusClient.StatusEventArgs;

public class StatusApiEventArgs : EventArgs
{
	private readonly DateTime _time;

	public DateTime Time => _time;

	public StatusApiEventArgs(DateTime time)
	{
		_time = time;
	}

	public virtual string GetUniqueKey()
	{
		return GetType().FullName + _time.ToString(CultureInfo.InvariantCulture);
	}
}
