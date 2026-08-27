using System;
using System.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util.Queue;

namespace VideoOS.Platform.Data;

public sealed class MetadataQueue : IDisposable
{
	public enum MetadataGetStrategy
	{
		Get,
		GetAtOrBefore,
		GetNearest,
		GetAtOrBeforeOrAfterIfBeforeNotFound
	}

	public class MetadataCacheKey
	{
		public Guid Guid { get; }

		public DateTime Timestamp { get; }

		public MetadataCacheKey(Guid guid, DateTime timestamp)
		{
			Guid = guid;
			Timestamp = timestamp;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is MetadataCacheKey metadataCacheKey))
			{
				return false;
			}
			if (object.Equals(Guid, metadataCacheKey.Guid))
			{
				return object.Equals(Timestamp, metadataCacheKey.Timestamp);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Guid.GetHashCode();
		}
	}

	public class MetadataRequestAdapter : ICancelableRequest
	{
		private readonly ICancelable _cancelable;

		public bool Canceled
		{
			get
			{
				return _cancelable.Canceled;
			}
			set
			{
				_cancelable.Canceled = value;
			}
		}

		public MetadataRequestAdapter(ICancelable cancelable)
		{
			_cancelable = cancelable;
		}
	}

	public class CachedResultInput : ICancelable
	{
		public bool Canceled { get; set; }
	}

	public class QueueInputPayload
	{
		public Item Item { get; set; }

		public DateTime Timestamp { get; set; }

		public MetadataGetStrategy Strategy { get; set; }
	}

	public class MetadataPlaybackSourceResourceProcessor : ResourceProcessor<MetadataQueueResult>
	{
		private MetadataPlaybackSource _metadataPlaybackSource;

		private QueueInputPayload _currentPayload;

		protected override bool ShouldReinitialize(MultiLevelResourceQueueInput<MetadataQueueResult> item)
		{
			if (_metadataPlaybackSource == null)
			{
				return true;
			}
			QueueInputPayload queueInputPayload = (QueueInputPayload)item.Payload;
			if (_currentPayload != null)
			{
				return !object.Equals(_currentPayload.Item.FQID.ObjectId, queueInputPayload.Item.FQID.ObjectId);
			}
			return true;
		}

		protected override void Initialize(MultiLevelResourceQueueInput<MetadataQueueResult> item)
		{
			try
			{
				QueueInputPayload queueInputPayload = (_currentPayload = (QueueInputPayload)item.Payload);
				if (!item.Canceled)
				{
					_metadataPlaybackSource?.Close();
					_metadataPlaybackSource = new MetadataPlaybackSource(queueInputPayload.Item);
					_metadataPlaybackSource.Init();
				}
			}
			catch
			{
				_metadataPlaybackSource?.Close();
				_metadataPlaybackSource = null;
				MetadataQueueResult obj2 = new MetadataQueueResult(failedToRetrieveMetadata: true, noMetadataAvailable: false);
				if (!item.Canceled)
				{
					item.OutputAction?.Invoke(obj2);
				}
			}
		}

		protected override void ProcessItem(MultiLevelResourceQueueInput<MetadataQueueResult> item)
		{
			QueueInputPayload queueInputPayload = (QueueInputPayload)item.Payload;
			DateTime timestamp = queueInputPayload.Timestamp;
			Action<MetadataQueueResult> outputAction = item.OutputAction;
			if (item.Canceled)
			{
				return;
			}
			MetadataPlaybackData metadataPlaybackData = null;
			MetadataQueueResult obj;
			try
			{
				switch (queueInputPayload.Strategy)
				{
				case MetadataGetStrategy.Get:
					metadataPlaybackData = _metadataPlaybackSource.Get(timestamp);
					break;
				case MetadataGetStrategy.GetAtOrBefore:
					metadataPlaybackData = _metadataPlaybackSource.GetAtOrBefore(timestamp);
					break;
				case MetadataGetStrategy.GetNearest:
					metadataPlaybackData = _metadataPlaybackSource.GetNearest(timestamp);
					break;
				case MetadataGetStrategy.GetAtOrBeforeOrAfterIfBeforeNotFound:
					metadataPlaybackData = _metadataPlaybackSource.GetAtOrBefore(timestamp);
					if (metadataPlaybackData == null)
					{
						metadataPlaybackData = _metadataPlaybackSource.GetNearest(timestamp);
					}
					break;
				}
				string text = metadataPlaybackData?.Content?.GetMetadataString();
				obj = ((!string.IsNullOrWhiteSpace(text)) ? new MetadataQueueResult(XDocument.Parse(text).CreateNavigator()) : new MetadataQueueResult(failedToRetrieveMetadata: false, noMetadataAvailable: true));
			}
			catch (Exception exception)
			{
				_metadataPlaybackSource?.Close();
				_metadataPlaybackSource = null;
				obj = new MetadataQueueResult(failedToRetrieveMetadata: true, noMetadataAvailable: false, exception);
			}
			if (!item.Canceled)
			{
				outputAction?.Invoke(obj);
			}
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_metadataPlaybackSource?.Close();
				_metadataPlaybackSource = null;
			}
		}
	}

	private static readonly Lazy<MetadataQueue> _lazyInstance = new Lazy<MetadataQueue>(() => new MetadataQueue());

	private const string KeyPrefix = "MetadataQueue.";

	private const string MaxConcurrentRecordingServersKey = "MetadataQueue.MaxConcurrentRecordingServers";

	private const string MaxMetadataPlaybackSourcesPerRecordingServerKey = "MetadataQueue.MaxMetadataPlaybackSourcesPerRecordingServer";

	private const string MaxMetadataResourceProcessorsPerLevelKey = "MetadataQueue.MaxMetadataResourceProcessorsPerLevelKey";

	private const string MaxConcurrentRequestsInTotalKey = "MetadataQueue.MaxConcurrentRequestsInTotal";

	private const string MaxMetadataPlaybackSourcesIdleTimeMillisKey = "MetadataQueue.MaxMetadataPlaybackSourcesIdleTimeMillis";

	private const string CacheEnabledKey = "MetadataQueue.CacheEnabled";

	private const string MaxCacheCapacityKey = "MetadataQueue.MaxCacheCapacity";

	private const string CacheExpirationTimeMillisKey = "MetadataQueue.CacheExpirationTimeMillis";

	private readonly MultiLevelResourceQueue<MetadataQueueResult> _queue;

	private bool _disposed;

	public readonly LRUCache<MetadataCacheKey, IXPathNavigable> _cache;

	public static MetadataQueue Instance => _lazyInstance.Value;

	public int CacheSize => _cache?.CacheSize ?? (-1);

	public bool UsingCache { get; }

	private MetadataQueue()
	{
		int intValueOrDefault = GetIntValueOrDefault("MetadataQueue.MaxConcurrentRecordingServers", 5);
		int intValueOrDefault2 = GetIntValueOrDefault("MetadataQueue.MaxMetadataPlaybackSourcesPerRecordingServer", 5);
		int intValueOrDefault3 = GetIntValueOrDefault("MetadataQueue.MaxMetadataResourceProcessorsPerLevelKey", intValueOrDefault2);
		int intValueOrDefault4 = GetIntValueOrDefault("MetadataQueue.MaxConcurrentRequestsInTotal", 100);
		int intValueOrDefault5 = GetIntValueOrDefault("MetadataQueue.MaxMetadataPlaybackSourcesIdleTimeMillis", 60000);
		int intValueOrDefault6 = GetIntValueOrDefault("MetadataQueue.MaxCacheCapacity", 500);
		int intValueOrDefault7 = GetIntValueOrDefault("MetadataQueue.CacheExpirationTimeMillis", 300000);
		TimeSpan? itemLifeSpan = null;
		if (intValueOrDefault7 > 0)
		{
			itemLifeSpan = TimeSpan.FromMilliseconds(intValueOrDefault7);
		}
		string b = ConfigurationManager.AppSettings["MetadataQueue.CacheEnabled"];
		UsingCache = !string.Equals("false", b, StringComparison.OrdinalIgnoreCase) && !string.Equals("no", b, StringComparison.OrdinalIgnoreCase);
		if (UsingCache)
		{
			_cache = new LRUCache<MetadataCacheKey, IXPathNavigable>(intValueOrDefault6, itemLifeSpan);
		}
		LevelConfiguration recordingServerLevel = new LevelConfiguration(intValueOrDefault);
		LevelConfiguration item = new LevelConfiguration(intValueOrDefault2);
		QueueConfiguration configuration = new QueueConfiguration(intValueOrDefault4, intValueOrDefault3, intValueOrDefault5, (LevelConfiguration c, object i) => (recordingServerLevel == c) ? ((object)((QueueInputPayload)i).Item.FQID.ServerId.Id) : ((object)((QueueInputPayload)i).Item.FQID.ObjectId))
		{
			LevelConfigurations = { recordingServerLevel, item }
		};
		_queue = new MultiLevelResourceQueue<MetadataQueueResult>(CreateResourceProcessor, configuration);
		new TrackPageViewData(typeof(MetadataQueue)).Send();
	}

	private static int GetIntValueOrDefault(string key, int defaultValue)
	{
		string value = ConfigurationManager.AppSettings[key];
		if (string.IsNullOrEmpty(value))
		{
			return defaultValue;
		}
		try
		{
			return Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}
		catch (OverflowException)
		{
		}
		catch (FormatException)
		{
		}
		return defaultValue;
	}

	private static ResourceProcessor<MetadataQueueResult> CreateResourceProcessor(MultiLevelResourceQueueInput<MetadataQueueResult> input)
	{
		return new MetadataPlaybackSourceResourceProcessor();
	}

	public void ClearCache()
	{
		if (UsingCache)
		{
			_cache.Clear();
		}
	}

	public void Clear()
	{
		Clear(clearCache: true);
	}

	public void Clear(bool clearCache)
	{
		_queue.Clear();
		if (clearCache)
		{
			ClearCache();
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, Action<ICancelableRequest, MetadataQueueResult> outputAction)
	{
		return Enqueue(item, timestamp, MetadataGetStrategy.GetAtOrBefore, outputAction, forceOutputActionInWorkerThread: false);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, MetadataGetStrategy strategy, Action<ICancelableRequest, MetadataQueueResult> outputAction)
	{
		return Enqueue(item, timestamp, strategy, outputAction, forceOutputActionInWorkerThread: false);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, MetadataGetStrategy strategy, Action<ICancelableRequest, MetadataQueueResult> outputAction, bool forceOutputActionInWorkerThread)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (outputAction == null)
		{
			throw new ArgumentNullException("outputAction");
		}
		MetadataRequestAdapter request = null;
		if (UsingCache)
		{
			MetadataCacheKey key = CreateCacheKey(item, timestamp);
			IXPathNavigable cachedResult = _cache.Get(key);
			if (cachedResult != null)
			{
				request = new MetadataRequestAdapter(new CachedResultInput());
				if (!forceOutputActionInWorkerThread)
				{
					outputAction(request, new MetadataQueueResult(cachedResult));
				}
				else
				{
					Task.Factory.StartNew(delegate
					{
						outputAction(request, new MetadataQueueResult(cachedResult));
					}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
				}
				return request;
			}
		}
		Action<MetadataQueueResult> outputAction2 = delegate(MetadataQueueResult data)
		{
			if (data.MetadataXmlDocument != null && UsingCache)
			{
				MetadataCacheKey key2 = CreateCacheKey(item, timestamp);
				_cache.Add(key2, data.MetadataXmlDocument);
			}
			outputAction(request, data);
		};
		MultiLevelResourceQueueInput<MetadataQueueResult> multiLevelResourceQueueInput = new MultiLevelResourceQueueInput<MetadataQueueResult>(new QueueInputPayload
		{
			Item = item,
			Timestamp = timestamp,
			Strategy = strategy
		}, outputAction2);
		request = new MetadataRequestAdapter(multiLevelResourceQueueInput);
		_queue.Enqueue(multiLevelResourceQueueInput);
		return request;
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			_queue.Dispose();
			_cache.Dispose();
		}
	}

	public static MetadataCacheKey CreateCacheKey(Item item, DateTime timestamp)
	{
		return new MetadataCacheKey(item.FQID.ObjectId, timestamp);
	}
}
