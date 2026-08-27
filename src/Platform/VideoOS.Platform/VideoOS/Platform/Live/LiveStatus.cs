namespace VideoOS.Platform.Live;

public class LiveStatus
{
	private StatusFlags _currentStatusFlags;

	public StatusFlags CurrentStatus
	{
		get
		{
			return _currentStatusFlags;
		}
		set
		{
			_currentStatusFlags = value;
		}
	}

	public LiveStatus(StatusFlags startValue)
	{
		_currentStatusFlags = startValue;
	}

	public StatusFlags UpdateStatus(LiveSourceStatus lss, StatusFlags initialFlags)
	{
		StatusFlags statusFlags = _currentStatusFlags;
		StatusFlags statusFlags2 = StatusFlags.None;
		foreach (int key in lss.Keys)
		{
			string text = lss.Value(key);
			StatusFlags statusFlags3 = FromImageServerValue(key);
			if (text == "0" && (_currentStatusFlags & statusFlags3) != StatusFlags.None)
			{
				statusFlags2 |= statusFlags3;
				statusFlags &= ~statusFlags3;
			}
			if (text == "1")
			{
				statusFlags |= statusFlags3;
				if ((_currentStatusFlags & statusFlags3) == 0)
				{
					statusFlags2 |= statusFlags3;
				}
			}
		}
		_currentStatusFlags = statusFlags | initialFlags;
		return statusFlags2 | initialFlags;
	}

	public void SetLiveModeStart(bool start)
	{
		if (!start)
		{
			_currentStatusFlags |= StatusFlags.ClientLiveStopped;
		}
		else
		{
			_currentStatusFlags &= ~StatusFlags.ClientLiveStopped;
		}
	}

	private StatusFlags FromImageServerValue(int key)
	{
		return key switch
		{
			1 => StatusFlags.LiveFeed, 
			2 => StatusFlags.Motion, 
			3 => StatusFlags.Recording, 
			4 => StatusFlags.Notification, 
			5 => StatusFlags.CameraConnectionLost, 
			6 => StatusFlags.DatabaseFail, 
			7 => StatusFlags.DiskFull, 
			100 => StatusFlags.ClientLiveStopped, 
			_ => StatusFlags.None, 
		};
	}
}
