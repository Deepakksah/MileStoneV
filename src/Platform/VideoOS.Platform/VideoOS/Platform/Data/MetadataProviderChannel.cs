using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using VideoOS.Platform.Data.MediaProviderInternal;
using VideoOS.Platform.Metadata;

namespace VideoOS.Platform.Data;

public class MetadataProviderChannel
{
	private readonly int _maxNumberOfItemsInQueue;

	private readonly int _channel;

	private readonly ConcurrentDictionary<RequestHeader, BlockingCollection<MediaData>> _transmissionQueues = new ConcurrentDictionary<RequestHeader, BlockingCollection<MediaData>>();

	private readonly Dictionary<string, string> _parameterDictionary = new Dictionary<string, string>();

	private readonly Dictionary<string, string> _serverParameterDictionary = new Dictionary<string, string>();

	private readonly MetadataSerializer _serializer = new MetadataSerializer();

	private bool _toBeDisconnected;

	public MetadataDeviceDefintion MetadataDeviceDefintion { get; set; }

	public int Channel => _channel;

	public Dictionary<string, string> ParameterDictionary => _parameterDictionary;

	public Dictionary<string, string> ServerParameterDictionary => _serverParameterDictionary;

	public bool IsConnected { get; private set; }

	public int ActiveSessions => _transmissionQueues.Count;

	public bool ToBeDisconnnected => _toBeDisconnected;

	public event Action<MediaProviderSession> SessionOpening;

	public event Action<MediaProviderSession> SessionClosed;

	public event Action<MediaProviderSession, MetadataProviderChannel> ParametersChanged;

	public MetadataProviderChannel(int channel, int queueLength)
	{
		_channel = channel;
		_maxNumberOfItemsInQueue = queueLength;
	}

	public bool Connect()
	{
		return false;
	}

	public void Disconnect()
	{
		_toBeDisconnected = true;
	}

	public bool QueueMetadata(MetadataStream metadata, DateTime startTime, TimeSpan? timeSpan = null)
	{
		using MemoryStream memoryStream = new MemoryStream();
		_serializer.WriteMetadataXml(memoryStream, metadata);
		return QueueBinaryMetadata(memoryStream.ToArray(), startTime, timeSpan);
	}

	public bool QueueMetadata(string metadata, DateTime startTime, TimeSpan? timeSpan = null)
	{
		return QueueBinaryMetadata(MetadataSerializer.MetadataEncoding.GetBytes(metadata), startTime, timeSpan);
	}

	public BlockingCollection<MediaData> GetMetadataQueue(RequestHeader requestHeader)
	{
		if (_transmissionQueues.TryGetValue(requestHeader, out var value))
		{
			return value;
		}
		throw new MIPException("There is no transmission queue for this request header on channel " + requestHeader.Channel.ToString(CultureInfo.InvariantCulture));
	}

	public void FireSessionOpeningEvent(RequestHeader requestHeader)
	{
		BlockingCollection<MediaData> value = new BlockingCollection<MediaData>(_maxNumberOfItemsInQueue);
		if (!_transmissionQueues.TryAdd(requestHeader, value))
		{
			EnvironmentManager.Instance.Log(error: false, "MetadataProviderChannel:FireSessionOpeningEvent", "Blocking queue for session already exists");
		}
		IsConnected = true;
		this.SessionOpening?.Invoke(requestHeader.MediaProviderSession);
	}

	public void FireSessionClosedEvent(RequestHeader requestHeader)
	{
		if (!_transmissionQueues.TryRemove(requestHeader, out var _))
		{
			EnvironmentManager.Instance.Log(error: false, "MetadataProviderChannel:FireSessionClosedEvent", "Blocking collection not found");
		}
		if (_transmissionQueues.Count == 0)
		{
			IsConnected = false;
		}
		this.SessionClosed?.Invoke(requestHeader.MediaProviderSession);
	}

	public void FireParametersChanged(RequestHeader requestHeader)
	{
		this.ParametersChanged?.Invoke(requestHeader.MediaProviderSession, this);
	}

	public void UpdateRelevantSettings(RequestHeader requestHeader)
	{
		string value = "MDStream." + (Channel - 1).ToString(CultureInfo.InvariantCulture) + ".";
		foreach (string key in requestHeader.SetParameters.Keys)
		{
			if (!key.StartsWith(value, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			string[] array = key.Split('.');
			int result = 0;
			int result2 = 0;
			int result3 = 0;
			string text = key;
			if (array.Length <= 3 || !int.TryParse(array[1], out result) || !int.TryParse(array[2], out result2) || !int.TryParse(array[3], out result3))
			{
				continue;
			}
			int num = result + 1;
			if (Channel == num && MetadataDeviceDefintion != null && MetadataDeviceDefintion.MetadataTypes.Count >= result2)
			{
				MetadataTypeDefinition metadataTypeDefinition = MetadataDeviceDefintion.MetadataTypes[result2];
				if (metadataTypeDefinition.Capabilities.Count >= result3)
				{
					text = metadataTypeDefinition.Capabilities[result3].Name;
					ServerParameterDictionary[text] = requestHeader.SetParameters[key];
				}
			}
		}
	}

	private bool QueueBinaryMetadata(byte[] metadata, DateTime startTime, TimeSpan? timeSpan)
	{
		MediaData item = new MediaData
		{
			DateTime = startTime,
			TimeSpan = (timeSpan ?? new TimeSpan(0L)),
			Bytes = metadata,
			MediaContent = MediaContent.MetaData
		};
		bool flag = false;
		foreach (BlockingCollection<MediaData> value in _transmissionQueues.Values)
		{
			bool flag2 = value.TryAdd(item, TimeSpan.FromMilliseconds(20.0));
			flag = flag || flag2;
		}
		return flag;
	}
}
