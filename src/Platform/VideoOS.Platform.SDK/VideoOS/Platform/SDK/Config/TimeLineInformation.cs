using System;

namespace VideoOS.Platform.SDK.Config;

public class TimeLineInformation
{
	private Guid _deviceId;

	private Guid _type;

	private DateTime _timeBegin;

	private long _interval;

	private int _count;

	private string _data;

	public Guid DeviceId => _deviceId;

	public Guid Type => _type;

	public DateTime BeginTime => _timeBegin;

	public long Interval => _interval;

	public int Count => _count;

	public string Data => _data;

	public TimeLineInformation()
	{
	}

	public TimeLineInformation(Guid deviceId, Guid type, DateTime begin, long interval, int count, string data)
	{
		_deviceId = deviceId;
		_type = type;
		_timeBegin = begin;
		_interval = interval;
		_count = count;
		_data = data;
	}
}
