using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Login;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Metadata;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class MetadataPlaybackSource
{
	private Item _item;

	private object _sessionId;

	private DateTime _lastReceived;

	private LoginSettings _loginSettings;

	private readonly object _syncObject = new object();

	private readonly MetadataCache _metadataCache;

	private object _regObj1;

	public object SessionId => _sessionId;

	public Item Item => _item;

	public MetadataPlaybackSource(Item item)
		: this(item, doTelemetry: true)
	{
	}

	public MetadataPlaybackSource(Item item, bool doTelemetry)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (item.FQID.Kind != Kind.Metadata)
		{
			throw new ArgumentException("Only Metadata Items are supported by RawDataSource");
		}
		_item = item;
		_lastReceived = DateTime.Now;
		_metadataCache = new MetadataCache();
		if (doTelemetry)
		{
			new TrackPageViewData(typeof(MetadataPlaybackSource)).Send();
		}
	}

	public void Init()
	{
		lock (_syncObject)
		{
			if (_sessionId != null)
			{
				Close();
			}
			_sessionId = InternalCommandService.Instance.ImageExporterConnect(Item.FQID, "METADATA", requestSignatures: false);
			if (_sessionId == null)
			{
				throw new NotAuthorizedMIPException("Unable to connect to requested metadata device:" + Item.Name);
			}
			_loginSettings = LoginSettingsCache.GetLoginSettings(Item.FQID);
			if (_loginSettings != null)
			{
				_loginSettings.TokenChangedEvent += TokenChangedEvent;
			}
			if (_regObj1 == null)
			{
				_regObj1 = EnvironmentManager.Instance.RegisterReceiver(SystemConfigChangedEvent, new MessageIdFilter(MessageId.System.LocalConfigurationChangedIndication));
			}
		}
	}

	public void Close()
	{
		lock (_syncObject)
		{
			if (_regObj1 != null)
			{
				EnvironmentManager.Instance.UnRegisterReceiver(_regObj1);
			}
			_regObj1 = null;
			if (_sessionId != null)
			{
				InternalCommandService.Instance.ImageExporterDisconnect(_sessionId);
				_sessionId = null;
				if (_loginSettings != null)
				{
					_loginSettings.TokenChangedEvent -= TokenChangedEvent;
				}
			}
		}
	}

	public MetadataPlaybackData Get(DateTime timestamp)
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:Get() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			MetadataPlaybackData atOrBefore = _metadataCache.GetAtOrBefore(timestamp);
			if (atOrBefore != null)
			{
				return atOrBefore;
			}
			List<MetadataPlaybackData> list = Get(timestamp, TimeSpan.FromDays(1.0), 1);
			if (list == null || !list.Any())
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(list);
			MetadataPlaybackData atOrBefore2 = _metadataCache.GetAtOrBefore(timestamp);
			if (atOrBefore2 != null)
			{
				return atOrBefore2;
			}
			MetadataPlaybackData firstEntry = _metadataCache.FirstEntry;
			if (firstEntry != null)
			{
				_lastReceived = firstEntry.DateTime;
			}
			return firstEntry;
		}
	}

	public List<MetadataPlaybackData> Get(DateTime timestamp, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		bool flag = false;
		List<MetadataPlaybackData> list = new List<MetadataPlaybackData>();
		lock (_syncObject)
		{
			_metadataCache.Clear();
		}
		DateTime dateTime = timestamp;
		while (!flag)
		{
			lock (_syncObject)
			{
				RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetAt(_sessionId, Item.FQID, dateTime, withHeader: false);
				if (rawVideoSourceDataList != null && rawVideoSourceDataList.List.Count > 0)
				{
					list.AddRange(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
					int index = rawVideoSourceDataList.List.Count - 1;
					_lastReceived = rawVideoSourceDataList.List[index].DateTime;
					dateTime = rawVideoSourceDataList.List[index].DateTime + TimeSpan.FromMilliseconds(1.0);
				}
				else
				{
					flag = true;
				}
				if (list.Count >= maxCountAfter || dateTime - timestamp > maxTimeAfter)
				{
					flag = true;
				}
			}
		}
		return list;
	}

	public MetadataPlaybackData GetNearest(DateTime timestamp)
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetNearest() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			MetadataPlaybackData nearest = _metadataCache.GetNearest(timestamp);
			if (nearest != null)
			{
				return nearest;
			}
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetNearest(_sessionId, Item.FQID, timestamp, withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			if (timestamp <= _metadataCache.FirstEntry.DateTime)
			{
				return _metadataCache.FirstEntry;
			}
			if (timestamp >= _metadataCache.LastEntry.DateTime)
			{
				return _metadataCache.LastEntry;
			}
			return _metadataCache.GetNearest(timestamp);
		}
	}

	public MetadataPlaybackData GetAtOrBefore(DateTime timestamp)
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetAtOrBefore() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			MetadataPlaybackData atOrBefore = _metadataCache.GetAtOrBefore(timestamp);
			if (atOrBefore != null)
			{
				return atOrBefore;
			}
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetAtOrBefore(_sessionId, Item.FQID, timestamp, withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			if (timestamp >= _metadataCache.LastEntry.DateTime)
			{
				return _metadataCache.LastEntry;
			}
			return _metadataCache.GetAtOrBefore(timestamp);
		}
	}

	public bool GoToWithResult(DateTime timestamp, string playbackPlayModeData)
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GoToWithResult() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			bool result = InternalCommandService.Instance.ImageExporterGoTo2(_sessionId, timestamp, playbackPlayModeData);
			_metadataCache.Clear();
			return result;
		}
	}

	public void GoTo(DateTime timestamp, string playbackPlayModeData)
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GoTo() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			InternalCommandService.Instance.ImageExporterGoTo(_sessionId, timestamp, playbackPlayModeData);
			_metadataCache.Clear();
		}
	}

	public MetadataPlaybackData GetNext()
	{
		MetadataPlaybackData nextViaLowerLevel = GetNextViaLowerLevel();
		if (nextViaLowerLevel != null)
		{
			_lastReceived = nextViaLowerLevel.DateTime;
			return nextViaLowerLevel;
		}
		return null;
	}

	public MetadataPlaybackData GetNext(DateTime timestamp)
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetNext() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			MetadataPlaybackData metadataPlaybackData = _metadataCache.FindCacheEntryAfter(timestamp);
			if (metadataPlaybackData != null)
			{
				return metadataPlaybackData;
			}
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetAfter(_sessionId, Item.FQID, timestamp, withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.FirstEntry;
		}
	}

	public MetadataPlaybackData GetPrevious()
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetPrevious() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			MetadataPlaybackData previous = _metadataCache.GetPrevious();
			if (previous != null)
			{
				return previous;
			}
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "Previous", withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null || !rawVideoSourceDataList.List.Any())
			{
				_ = _metadataCache.FirstEntry;
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.LastEntry;
		}
	}

	public MetadataPlaybackData GetPrevious(DateTime timestamp)
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetPrevious() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			MetadataPlaybackData metadataPlaybackData = _metadataCache.FindCacheEntryBefore(timestamp);
			if (metadataPlaybackData != null)
			{
				return metadataPlaybackData;
			}
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawGetBefore(_sessionId, Item.FQID, timestamp, withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.LastEntry;
		}
	}

	private MetadataPlaybackData GetNextViaLowerLevel()
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetNextViaLowerLevel() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			MetadataPlaybackData next = _metadataCache.GetNext();
			if (next != null)
			{
				return next;
			}
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "Next", withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null || !rawVideoSourceDataList.List.Any())
			{
				_ = _metadataCache.LastEntry;
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.FirstEntry;
		}
	}

	public MetadataPlaybackData GetPreviousSequence()
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetPreviousSequence() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "PreviousSequence", withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.FirstEntry;
		}
	}

	public MetadataPlaybackData GetNextSequence()
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("RawideoSource:GetNextSequence() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "NextSequence", withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.FirstEntry;
		}
	}

	public MetadataPlaybackData GetBegin()
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetBegin() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "Begin", withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.FirstEntry;
		}
	}

	public MetadataPlaybackData GetEnd()
	{
		if (_sessionId == null)
		{
			throw new InvalidOperationException("MetadataPlaybackSource:GetEnd() - State error, must call Init() first. ");
		}
		lock (_syncObject)
		{
			RawVideoSourceDataList rawVideoSourceDataList = InternalCommandService.Instance.ImageExporterRawCommand(_sessionId, "End", withHeader: false);
			if (rawVideoSourceDataList == null || rawVideoSourceDataList.List == null)
			{
				_metadataCache.Clear();
				return null;
			}
			_metadataCache.SetCacheContents(rawVideoSourceDataList.List.Select(ConvertToMetadataPlaybackContent));
			return _metadataCache.LastEntry;
		}
	}

	private void TokenChangedEvent(object sender, EventArgs e)
	{
		lock (_syncObject)
		{
			if (_sessionId == null)
			{
				return;
			}
			int num = 3;
			bool flag = false;
			while (!flag && num-- > 0)
			{
				try
				{
					InternalCommandService.Instance.SetNewToken(_sessionId, ((LoginSettings)sender).Token);
					flag = true;
				}
				catch (Exception ex)
				{
					EnvironmentManager.Instance.Log(error: true, "MetadataPlaybackSource:TokenChangedEvent", ex.Message, new object[1] { ex });
				}
			}
		}
	}

	private MetadataPlaybackData ConvertToMetadataPlaybackContent(RawVideoSourceData input)
	{
		MetadataContent content = ((input.Content == null || !input.Content.Any()) ? null : new MetadataContent(input.Content));
		return new MetadataPlaybackData
		{
			SequenceNumber = input.SequenceNumber,
			DateTime = input.DateTime,
			NextDateTime = (input.IsNextAvailable ? new DateTime?(input.NextDateTime) : ((DateTime?)null)),
			PreviousDateTime = (input.IsPreviousAvailable ? new DateTime?(input.PreviousDateTime) : ((DateTime?)null)),
			Content = content
		};
	}

	private object SystemConfigChangedEvent(Message message, FQID dest, FQID sender)
	{
		if (message.Data is List<FQID> list && _item != null)
		{
			foreach (FQID item2 in list)
			{
				if (item2.ObjectId == _item.FQID.ObjectId)
				{
					Item item = ((item2.ServerId.UserContext == null) ? Configuration.Instance.GetItem(item2) : item2.ServerId.UserContext.Configuration.GetItem(item2));
					if (item != null)
					{
						_item.FQID = item.FQID;
					}
					break;
				}
			}
		}
		return null;
	}
}
