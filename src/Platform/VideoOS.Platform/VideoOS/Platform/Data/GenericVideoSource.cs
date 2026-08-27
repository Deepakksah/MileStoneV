using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.ToolkitXmlProviders;

namespace VideoOS.Platform.Data;

public abstract class GenericVideoSource<TDataType> : VideoSource where TDataType : class
{
	private object _sessionId;

	private DateTime _lastReceived;

	private LoginSettings _loginSettings;

	private readonly object _syncObject = new object();

	private readonly Guid[] _supportedKinds;

	private readonly IInternalCommandServiceImplementer<TDataType> _implementer;

	protected object SyncObject => _syncObject;

	public string ItemMediaType => Item.FQID.Kind.ToMedia();

	public object SessionId => _sessionId;

	protected GenericVideoSource(Item item, Guid[] supportedKinds, IInternalCommandServiceImplementer<TDataType> implementer)
		: base(item)
	{
		_lastReceived = DateTime.UtcNow;
		_supportedKinds = supportedKinds;
		_implementer = implementer;
	}

	protected void InitInternal(Func<object> imageExporterConnect, Action onConnected)
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
			throw new ImplementationMIPException("Your need to initialize Item property before calling Init()");
		}
		if (!_supportedKinds.Contains(Item.FQID.Kind))
		{
			throw new ImplementationMIPException("Item of this Kind is not supported: " + Item.FQID.Kind.ToMedia());
		}
		if (imageExporterConnect == null)
		{
			throw new InvalidParameterMIPException("imageExporterConnect delegate is null");
		}
		lock (_syncObject)
		{
			_sessionId = imageExporterConnect();
			if (_sessionId == null)
			{
				throw new NotAuthorizedMIPException("Unable to connect to requested camera:" + (Item.Name ?? string.Empty));
			}
			_loginSettings = LoginSettingsCache.GetLoginSettings(Item.FQID);
			if (_loginSettings != null)
			{
				_loginSettings.TokenChangedEvent += TokenChangedEvent;
			}
			onConnected?.Invoke();
		}
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
			throw new ImplementationMIPException("BaseVideoSource:Get() - State error, must call Init() first. ");
		}
		List<object> list = Get(dateTime, TimeSpan.FromDays(1.0), 1);
		if (list != null && list.Count > 0)
		{
			FillLastDateTime((TDataType)list[0]);
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
				TDataType val = FireCommandInternal(_implementer.GetAt, dateTime2);
				ISourceData sourceData = ((val != null) ? _implementer.Convertor(val) : null);
				if (sourceData != null)
				{
					list.Add(val);
					dateTime2 = sourceData.DateTime + TimeSpan.FromMilliseconds(1.0);
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
		return FireCommandInternal(_implementer.GetNearest, dateTime);
	}

	public override object GetAtOrBefore(DateTime dateTime)
	{
		return FireCommandInternal(_implementer.GetAtOrBefore, dateTime);
	}

	public bool GoToWithResult(DateTime timestamp, string playbackPlayModeData)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("BaseVideoSource:GoToWithResult() - State error, must call Init() first. ");
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
			throw new ImplementationMIPException("BaseVideoSource:GoTo() - State error, must call Init() first. ");
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

	private void FillLastDateTime(TDataType dataType)
	{
		if (dataType != null)
		{
			_lastReceived = _implementer.Convertor(dataType).DateTime;
		}
		else
		{
			_lastReceived = DateTime.MaxValue;
		}
	}

	protected TDataType FireCommandInternal(Func<object, FQID, DateTime, TDataType> command, DateTime dateTime)
	{
		Func<TDataType> command2 = ((command == null) ? null : ((Func<TDataType>)(() => command(_sessionId, Item.FQID, dateTime))));
		return FireCommandInternal(command2);
	}

	protected TDataType FireCommandInternal(Func<object, string, TDataType> command, string commandName)
	{
		Func<TDataType> command2 = ((command == null) ? null : ((Func<TDataType>)(() => command(_sessionId, commandName))));
		return FireCommandInternal(command2);
	}

	protected TDataType FireCommandInternal(Func<TDataType> command)
	{
		if (_sessionId == null)
		{
			throw new ImplementationMIPException("GenericVideoSource:FireCommandInternal() - State error, must call Init() first. ");
		}
		if (command == null)
		{
			throw new NotImplementedException();
		}
		lock (_syncObject)
		{
			TDataType val = command();
			FillLastDateTime(val);
			_implementer.OnData(val);
			return val;
		}
	}

	public TDataType GetNext(DateTime dateTime)
	{
		return FireCommandInternal(_implementer.GetAfter, dateTime);
	}

	public TDataType GetPrevious()
	{
		return FireCommandInternal(_implementer.Command, "Previous");
	}

	public TDataType GetPrevious(DateTime dateTime)
	{
		return FireCommandInternal(_implementer.GetBefore, dateTime);
	}

	private TDataType GetNextViaLowerLevel()
	{
		return FireCommandInternal(_implementer.Command, "Next");
	}

	public TDataType GetPreviousSequence()
	{
		return FireCommandInternal(_implementer.Command, "PreviousSequence");
	}

	public TDataType GetNextSequence()
	{
		return FireCommandInternal(_implementer.Command, "NextSequence");
	}

	public TDataType GetBegin()
	{
		return FireCommandInternal(_implementer.Command, "Begin");
	}

	public TDataType GetEnd()
	{
		return FireCommandInternal(_implementer.Command, "End");
	}

	private void TokenChangedEvent(object sender, EventArgs e)
	{
		if (_sessionId != null)
		{
			Thread thread = new Thread(UpdateToolkitToken);
			thread.Name = "VideoOS.Platform.Data.GenericVideoSource<TDataType> - Token Renew to Toolkit";
			thread.Start(((LoginSettings)sender).Token);
		}
	}

	private void UpdateToolkitToken(object token)
	{
		int num = 3;
		bool flag = false;
		while (!flag && num-- > 0)
		{
			try
			{
				lock (_syncObject)
				{
					InternalCommandService.Instance.SetNewToken(_sessionId, (string)token);
				}
				flag = true;
			}
			catch
			{
				EnvironmentManager.Instance.Log(error: true, GetType().Name + ".UpdateToolkitToken", "Unable to set new token");
			}
		}
	}
}
