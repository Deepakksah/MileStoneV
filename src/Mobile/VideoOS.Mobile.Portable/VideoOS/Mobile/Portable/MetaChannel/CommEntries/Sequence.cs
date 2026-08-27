using System;
using System.Xml.Serialization;
using Newtonsoft.Json;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class Sequence
{
	public const string StartTimeName = "StartTime";

	public const string EndTimeName = "EndTime";

	public const string TimeStampName = "TimeStamp";

	private DateTime _startTime;

	private DateTime _endTime;

	private DateTime _timeStamp;

	[JsonProperty("StartTime")]
	[XmlAttribute(AttributeName = "StartTime")]
	public string StartTime
	{
		get
		{
			return _startTime.ToMilisecString();
		}
		set
		{
			_startTime = TimeConverter.FromString(value);
		}
	}

	[JsonProperty("EndTime")]
	[XmlAttribute(AttributeName = "EndTime")]
	public string EndTime
	{
		get
		{
			return _endTime.ToMilisecString();
		}
		set
		{
			_endTime = TimeConverter.FromString(value);
		}
	}

	[JsonProperty("TimeStamp")]
	[XmlAttribute(AttributeName = "TimeStamp")]
	public string TimeStamp
	{
		get
		{
			return _timeStamp.ToMilisecString();
		}
		set
		{
			_timeStamp = TimeConverter.FromString(value);
		}
	}

	public DateTime GetStartTime()
	{
		return _startTime;
	}

	public void SetStartTime(DateTime startTime)
	{
		_startTime = startTime;
	}

	public DateTime GetEndTime()
	{
		return _endTime;
	}

	public void SetEndTime(DateTime endTime)
	{
		_endTime = endTime;
	}

	public DateTime GetTimeStamp()
	{
		return _timeStamp;
	}

	public void SetTimeStamp(DateTime timeStamp)
	{
		_timeStamp = timeStamp;
	}

	public Sequence()
	{
		InternalInit();
	}

	public Sequence(DateTime startTime, DateTime endTime, DateTime timeStamp)
	{
		InitFrom(startTime, endTime, timeStamp);
	}

	private void InternalInit()
	{
		_startTime = (_endTime = (_timeStamp = DateTime.MinValue));
	}

	public void InitFrom(DateTime startTime, DateTime endTime, DateTime timeStamp)
	{
		_startTime = startTime;
		_endTime = endTime;
		_timeStamp = timeStamp;
	}
}
