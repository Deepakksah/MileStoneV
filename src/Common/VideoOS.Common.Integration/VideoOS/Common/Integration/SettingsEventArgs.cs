using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration;

public class SettingsEventArgs : EventArgs
{
	private SettingChangedEventData[] _events;

	public SettingChangedEventData[] SettingsEvents
	{
		[DebuggerStepThrough]
		get
		{
			return _events;
		}
	}

	public SettingsEventArgs(SettingChangedEventData settingChangedEventData)
	{
		_events = new SettingChangedEventData[1] { settingChangedEventData };
	}

	public SettingsEventArgs(SettingChangedEventData[] settingChangedEventDataItems)
	{
		_events = settingChangedEventDataItems;
	}
}
