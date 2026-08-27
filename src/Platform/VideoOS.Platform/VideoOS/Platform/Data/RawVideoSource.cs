using System;
using System.Collections.Generic;
using VideoOS.Platform.Login;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class RawVideoSource : VideoSource
{
	private object _sessionId;

	private DateTime _lastReceived;

	private LoginSettings _loginSettings;

	private readonly object _syncObject = new object();

	public object SessionId => _sessionId;

	public RawVideoSource(Item item)
		: base(item)
	{
		_lastReceived = DateTime.UtcNow;
		new TrackPageViewData(typeof(RawVideoSource)).Send();
	}

	public override void Init(int width, int height)
	{
		EnvironmentManager.Instance.Log(error: false, "RawVideoSource:Init", "Width and Height ignored");
		Init();
	}

	public override void Init()
	{
		lock (_syncObject)
		{
			if (_sessionId != null)
			{
				Close();
			}
		}
		base.Init();
		if (Item == null)
		{
			throw new ImplementationMIPException("You need to initialize Item property before calling Init()");
		}
		if (Item.FQID.Kind != Kind.Camera && Item.FQID.Kind != Kind.Microphone && Item.FQID.Kind != Kind.Speaker && Item.FQID.Kind != Kind.Metadata)
		{
			throw new ImplementationMIPException("Only Cameras, Microphones, Speakers and Meta Datas are supported by RawDataSource");
		}
		lock (_syncObject)
		{
			_sessionId = InternalCommandService.Instance.ImageExporterConnect(Item.FQID, GetMediaType(), requestSignatures: false);
			if (_sessionId == null)
			{
				throw new NotAuthorizedMIPException("Unable to connect to requested camera:" + (Item.Name ?? string.Empty));
			}
			_loginSettings = LoginSettingsCache.GetLoginSettings(Item.FQID);
			if (_loginSettings != null)
			{
				_loginSettings.TokenChangedEvent += TokenChangedEvent;
			}
		}
	}

	private string GetMediaType()
	{
		if (Item.FQID.Kind == Kind.Camera)
		{
			return "VIDEO";
		}
		if (Item.FQID.Kind == Kind.Microphone || Item.FQID.Kind == Kind.Speaker)
		{
			return "AUDIO";
		}
		if (Item.FQID.Kind == Kind.Metadata)
		{
			return "METADATA";
		}
		return "VIDEO";
	}

	public override void Close()
	{
		base.Close();
		if (_sessionId == null)
		{
			return;
		}
		lock (_syncObject)
		{
			InternalCommandService.Instance.ImageExporterDisconnect(_sessionId);
			_sessionId = null;
			if (_loginSettings != null)
			{
				_loginSettings.TokenChangedEvent -= TokenChangedEvent;
			}
		}
	}

	public override object Get(DateTime dateTime)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:Get() - State error, must call Init() first. ");
		}
		List<object> list = Get(dateTime, TimeSpan.FromDays(1.0), 1);
		if (list != null && list.Count > 0)
		{
			if (list[0] is RawVideoSourceDataList rawVideoSourceDataList && rawVideoSourceDataList.List.Count > 0)
			{
				_lastReceived = rawVideoSourceDataList.List[rawVideoSourceDataList.List.Count - 1].DateTime;
			}
			else
			{
				_lastReceived = DateTime.MaxValue;
			}
			return list[0];
		}
		return null;
	}

	public override List<object> Get(DateTime dateTime, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		bool flag = false;
		List<object> list = new List<object>();
		DateTime dateTime2 = dateTime;
		while (!flag)
		{
			lock (_syncObject)
			{
				RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetAt(_sessionId, Item.FQID, dateTime2);
				if (rawVideoSourceDataList != null && rawVideoSourceDataList.List.Count > 0)
				{
					list.Add(rawVideoSourceDataList);
					int index = rawVideoSourceDataList.List.Count - 1;
					_lastReceived = rawVideoSourceDataList.List[index].DateTime;
					dateTime2 = rawVideoSourceDataList.List[index].DateTime + TimeSpan.FromMilliseconds(1.0);
				}
				else
				{
					flag = true;
					_lastReceived = DateTime.MaxValue;
				}
				if (list.Count >= maxCountAfter || dateTime2 - dateTime > maxTimeAfter)
				{
					flag = true;
				}
			}
		}
		return list;
	}

	public override object GetNearest(DateTime dateTime)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetNearest() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			return InternalCommandService.Instance.ImageExporterRawGetNearest(_sessionId, Item.FQID, dateTime);
		}
	}

	public override object GetAtOrBefore(DateTime dateTime)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetAtOrBefore() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			return InternalCommandService.Instance.ImageExporterRawGetAtOrBefore(_sessionId, Item.FQID, dateTime);
		}
	}

	public bool GoToWithResult(DateTime timestamp, string playbackPlayModeData)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GoToWithResult() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			return InternalCommandService.Instance.ImageExporterGoTo2(_sessionId, timestamp, playbackPlayModeData);
		}
	}

	public void GoTo(DateTime timestamp, string playbackPlayModeData)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GoTo() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			InternalCommandService.Instance.ImageExporterGoTo(_sessionId, timestamp, playbackPlayModeData);
		}
	}

	public override object GetNext()
	{
		object nextViaLowerLevel = GetNextViaLowerLevel();
		if (nextViaLowerLevel != null)
		{
			return nextViaLowerLevel;
		}
		if (_lastReceived != DateTime.MaxValue)
		{
			DateTime dateTime = _lastReceived + TimeSpan.FromMilliseconds(1.0);
			return Get(dateTime);
		}
		return null;
	}

	private void FillLastDateTime(RawVideoSourceDataList rawVideoSourceData)
	{
		if (rawVideoSourceData != null && rawVideoSourceData.List.Count > 0)
		{
			_lastReceived = rawVideoSourceData.List[rawVideoSourceData.List.Count - 1].DateTime;
		}
		else
		{
			_lastReceived = DateTime.MaxValue;
		}
	}

	public RawVideoSourceDataList GetNext(DateTime dateTime)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetNext() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetAfter(_sessionId, Item.FQID, dateTime);
			FillLastDateTime(rawVideoSourceDataList);
			return rawVideoSourceDataList;
		}
	}

	public RawVideoSourceDataList GetPrevious()
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetPrevious() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "Previous");
			FillLastDateTime(rawVideoSourceDataList);
			return rawVideoSourceDataList;
		}
	}

	public RawVideoSourceDataList GetPrevious(DateTime dateTime)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetPrevious() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetBefore(_sessionId, Item.FQID, dateTime);
			FillLastDateTime(rawVideoSourceDataList);
			return rawVideoSourceDataList;
		}
	}

	private RawVideoSourceDataList GetNextViaLowerLevel()
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetNextViaLowerLevel() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "Next");
			FillLastDateTime(rawVideoSourceDataList);
			return rawVideoSourceDataList;
		}
	}

	public RawVideoSourceDataList GetPreviousSequence()
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetPreviousSequence() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "PreviousSequence");
			FillLastDateTime(rawVideoSourceDataList);
			return rawVideoSourceDataList;
		}
	}

	public RawVideoSourceDataList GetNextSequence()
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawideoSource:GetNextSequence() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			return InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "NextSequence");
		}
	}

	public RawVideoSourceDataList GetBegin()
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetBegin() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "Begin");
			FillLastDateTime(rawVideoSourceDataList);
			return rawVideoSourceDataList;
		}
	}

	public RawVideoSourceDataList GetEnd()
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("RawVideoSource:GetEnd() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "End");
			FillLastDateTime(rawVideoSourceDataList);
			return rawVideoSourceDataList;
		}
	}

	private void TokenChangedEvent(object sender, EventArgs e)
	{
		if (_sessionId == null)
		{
			return;
		}
		lock (_syncObject)
		{
			int num = 3;
			bool flag = false;
			while (!flag && num-- > 0)
			{
				try
				{
					InternalCommandService.Instance.SetNewToken(_sessionId, ((LoginSettings)sender).Token);
					flag = true;
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
