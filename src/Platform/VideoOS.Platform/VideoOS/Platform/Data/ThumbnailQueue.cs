using System;
using System.Configuration;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using VideoOS.Platform.Util;
using VideoOS.Platform.Util.Queue;

namespace VideoOS.Platform.Data;

public sealed class ThumbnailQueue : IDisposable
{
	public enum ThumbnailGetStrategy
	{
		Get,
		GetAtOrBefore,
		GetNearest,
		GetAtOrBeforeOrAfterIfBeforeNotFound
	}

	public class ThumbnailCacheKey
	{
		public Guid Guid { get; }

		public DateTime Timestamp { get; }

		public Size DesiredSize { get; }

		public bool PrivacyMaskLifted { get; }

		public ThumbnailCacheKey(Guid guid, DateTime timestamp, Size desiredSize)
		{
			Guid = guid;
			Timestamp = timestamp;
			DesiredSize = desiredSize;
			PrivacyMaskLifted = EnvironmentManager.Instance.MasterSite.ServerId.UserContext.PrivacyMaskLifted;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ThumbnailCacheKey thumbnailCacheKey))
			{
				return false;
			}
			if (object.Equals(Guid, thumbnailCacheKey.Guid) && object.Equals(Timestamp, thumbnailCacheKey.Timestamp) && object.Equals(DesiredSize, thumbnailCacheKey.DesiredSize))
			{
				return object.Equals(PrivacyMaskLifted, thumbnailCacheKey.PrivacyMaskLifted);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return Guid.GetHashCode();
		}
	}

	public class ThumbnailQueueDiagnosticsAdapter : IThumbnailQueueDiagnostics
	{
		public int MaxConcurrentRequests { get; }

		public int MaxIdleTimeMillisBeforeVideoSourceCleanup { get; }

		public int ConcurrentRequests { get; }

		public int ActiveVideoSources { get; }

		public int IdleVideoSources { get; }

		public int VideoSources { get; }

		public int RequestsInQueue { get; }

		public ThumbnailQueueDiagnosticsAdapter(IMultiLevelResourceQueueDiagnostics d)
		{
			MaxConcurrentRequests = d.MaxConcurrentRequests;
			MaxIdleTimeMillisBeforeVideoSourceCleanup = d.MaxIdleTimeMillisBeforeResourceProcessorCleanup;
			ConcurrentRequests = d.ConcurrentRequests;
			ActiveVideoSources = d.ActiveResourceProcessors;
			IdleVideoSources = d.IdleResourceProcessors;
			VideoSources = d.ResourceProcessors;
			RequestsInQueue = d.RequestsInQueue;
		}
	}

	public class ThumbnailRequestAdapter : ICancelableRequest
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

		public ThumbnailRequestAdapter(ICancelable cancelable)
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

		public Size DesiredSize { get; set; }

		public ThumbnailGetStrategy Strategy { get; set; }
	}

	public class BitmapVideoSourceResourceProcessor : ResourceProcessor<BitmapDataResult>
	{
		private BitmapVideoSource _bitmapVideoSource;

		private QueueInputPayload _currentPayload;

		protected override bool ShouldReinitialize(MultiLevelResourceQueueInput<BitmapDataResult> item)
		{
			if (_bitmapVideoSource == null)
			{
				return true;
			}
			QueueInputPayload queueInputPayload = (QueueInputPayload)item.Payload;
			if (_currentPayload != null && object.Equals(_currentPayload.Item.FQID.ObjectId, queueInputPayload.Item.FQID.ObjectId))
			{
				return _currentPayload.DesiredSize != queueInputPayload.DesiredSize;
			}
			return true;
		}

		protected override void Initialize(MultiLevelResourceQueueInput<BitmapDataResult> item)
		{
			try
			{
				QueueInputPayload queueInputPayload = (_currentPayload = (QueueInputPayload)item.Payload);
				if (!item.Canceled)
				{
					_bitmapVideoSource?.Close();
					_bitmapVideoSource = new BitmapVideoSource(queueInputPayload.Item);
					if (queueInputPayload.DesiredSize == default(Size))
					{
						_bitmapVideoSource.Init();
						return;
					}
					_bitmapVideoSource.SetKeepAspectRatio(keepAspectRatio: true, fillSpace: false);
					_bitmapVideoSource.Init((int)queueInputPayload.DesiredSize.Width, (int)queueInputPayload.DesiredSize.Height);
				}
			}
			catch
			{
				_bitmapVideoSource?.Close();
				_bitmapVideoSource = null;
				BitmapDataResult obj2 = new BitmapDataResult(failedToRetrieveImage: true, noImageAvailable: false);
				if (!item.Canceled)
				{
					item.OutputAction?.Invoke(obj2);
				}
			}
		}

		protected override void ProcessItem(MultiLevelResourceQueueInput<BitmapDataResult> item)
		{
			QueueInputPayload queueInputPayload = (QueueInputPayload)item.Payload;
			DateTime timestamp = queueInputPayload.Timestamp;
			Action<BitmapDataResult> outputAction = item.OutputAction;
			if (item.Canceled)
			{
				return;
			}
			BitmapData bitmapData = null;
			BitmapDataResult obj;
			try
			{
				switch (queueInputPayload.Strategy)
				{
				case ThumbnailGetStrategy.Get:
					bitmapData = (BitmapData)_bitmapVideoSource.Get(timestamp);
					break;
				case ThumbnailGetStrategy.GetAtOrBefore:
					bitmapData = (BitmapData)_bitmapVideoSource.GetAtOrBefore(timestamp);
					break;
				case ThumbnailGetStrategy.GetNearest:
					bitmapData = (BitmapData)_bitmapVideoSource.GetNearest(timestamp);
					break;
				case ThumbnailGetStrategy.GetAtOrBeforeOrAfterIfBeforeNotFound:
					bitmapData = (BitmapData)_bitmapVideoSource.GetAtOrBefore(timestamp);
					if (bitmapData == null)
					{
						bitmapData = (BitmapData)_bitmapVideoSource.GetNearest(timestamp);
					}
					break;
				}
				obj = ((bitmapData == null) ? new BitmapDataResult(failedToRetrieveImage: false, noImageAvailable: true) : new BitmapDataResult(bitmapData));
			}
			catch (Exception exception)
			{
				_bitmapVideoSource?.Close();
				_bitmapVideoSource = null;
				obj = new BitmapDataResult(failedToRetrieveImage: true, noImageAvailable: false, exception);
			}
			if (!item.Canceled)
			{
				outputAction?.Invoke(obj);
			}
		}

		public override void Stop()
		{
			_bitmapVideoSource?.Close();
			_bitmapVideoSource = null;
			base.Stop();
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if (disposing)
			{
				_bitmapVideoSource?.Close();
				_bitmapVideoSource = null;
			}
		}
	}

	private static readonly Lazy<ThumbnailQueue> _lazyInstance = new Lazy<ThumbnailQueue>(() => new ThumbnailQueue());

	private const string KeyPrefix = "ThumbnailQueue.";

	private static readonly string MaxConcurrentRecordingServersKey = "ThumbnailQueue.MaxConcurrentRecordingServers";

	private static readonly string MaxVideoSourcesPerRecordingServerKey = "ThumbnailQueue.MaxVideoSourcesPerRecordingServer";

	private static readonly string MaxConcurrentRequestsInTotalKey = "ThumbnailQueue.MaxConcurrentRequestsInTotal";

	private static readonly string MaxResourceProcessorsPerLevelKey = "ThumbnailQueue.MaxResourceProcessorsPerLevel";

	private static readonly string MaxVideoSourceIdleTimeMillisKey = "ThumbnailQueue.MaxVideoSourceIdleTimeMillis";

	private static readonly string ThumbnailCacheEnabledKey = "ThumbnailQueue.ThumbnailCacheEnabled";

	private static readonly string MaxThumbnailCacheCapacityKey = "ThumbnailQueue.MaxThumbnailCacheCapacity";

	private static readonly string ThumbnailCacheExpirationTimeMillisKey = "ThumbnailQueue.ThumbnailCacheExpirationTimeMillis";

	private static readonly string ThumbnailCreationThreadPoolSizeKey = "ThumbnailQueue.ThumbnailCreationThreadPoolSize";

	private readonly MultiLevelResourceQueue<BitmapDataResult> _queue;

	private bool _disposed;

	public readonly LRUCache<ThumbnailCacheKey, BitmapData> _cache;

	private readonly WorkStealingTaskScheduler _imageSourceCreationScheduler;

	public static ThumbnailQueue Instance => _lazyInstance.Value;

	public IThumbnailQueueDiagnostics Diagnostics => new ThumbnailQueueDiagnosticsAdapter(_queue.Diagnostics);

	public int CacheSize => _cache?.CacheSize ?? (-1);

	public bool UsingCache { get; }

	private ThumbnailQueue()
	{
		int intValueOrDefault = GetIntValueOrDefault(MaxConcurrentRecordingServersKey, 5);
		int intValueOrDefault2 = GetIntValueOrDefault(MaxVideoSourcesPerRecordingServerKey, 5);
		int intValueOrDefault3 = GetIntValueOrDefault(MaxResourceProcessorsPerLevelKey, intValueOrDefault2);
		int intValueOrDefault4 = GetIntValueOrDefault(MaxConcurrentRequestsInTotalKey, 100);
		int intValueOrDefault5 = GetIntValueOrDefault(MaxVideoSourceIdleTimeMillisKey, 60000);
		int intValueOrDefault6 = GetIntValueOrDefault(MaxThumbnailCacheCapacityKey, 500);
		int intValueOrDefault7 = GetIntValueOrDefault(ThumbnailCacheExpirationTimeMillisKey, 300000);
		int intValueOrDefault8 = GetIntValueOrDefault(ThumbnailCreationThreadPoolSizeKey, 4);
		_imageSourceCreationScheduler = new WorkStealingTaskScheduler(intValueOrDefault8);
		TimeSpan? itemLifeSpan = null;
		if (intValueOrDefault7 > 0)
		{
			itemLifeSpan = TimeSpan.FromMilliseconds(intValueOrDefault7);
		}
		string b = ConfigurationManager.AppSettings[ThumbnailCacheEnabledKey];
		UsingCache = !string.Equals("false", b, StringComparison.OrdinalIgnoreCase) && !string.Equals("no", b, StringComparison.OrdinalIgnoreCase);
		if (UsingCache)
		{
			_cache = new LRUCache<ThumbnailCacheKey, BitmapData>(intValueOrDefault6, itemLifeSpan);
		}
		LevelConfiguration recordingServerLevel = new LevelConfiguration(intValueOrDefault);
		LevelConfiguration item = new LevelConfiguration(intValueOrDefault2);
		QueueConfiguration configuration = new QueueConfiguration(intValueOrDefault4, intValueOrDefault3, intValueOrDefault5, (LevelConfiguration c, object i) => (recordingServerLevel == c) ? ((object)((QueueInputPayload)i).Item.FQID.ServerId.Id) : ((object)((QueueInputPayload)i).Item.FQID.ObjectId))
		{
			LevelConfigurations = { recordingServerLevel, item }
		};
		_queue = new MultiLevelResourceQueue<BitmapDataResult>(CreateResourceProcessor, configuration);
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

	private static ResourceProcessor<BitmapDataResult> CreateResourceProcessor(MultiLevelResourceQueueInput<BitmapDataResult> input)
	{
		return new BitmapVideoSourceResourceProcessor();
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
	[Obsolete("Has potential race-condition in calling code, thus obsoleted. Please use the overload where the outputAction also includes the request.")]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, Size desiredSize, Action<BitmapDataResult> outputAction)
	{
		return Enqueue(item, timestamp, ThumbnailGetStrategy.GetAtOrBefore, desiredSize, delegate(ICancelableRequest request, BitmapDataResult result)
		{
			outputAction(result);
		}, forceOutputActionInWorkerThread: false);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	[Obsolete("Has potential race-condition in calling code, thus obsoleted. Please use the overload where the outputAction also includes the request.")]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, ThumbnailGetStrategy strategy, Size desiredSize, Action<BitmapDataResult> outputAction)
	{
		return Enqueue(item, timestamp, strategy, desiredSize, delegate(ICancelableRequest request, BitmapDataResult result)
		{
			outputAction(result);
		}, forceOutputActionInWorkerThread: false);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, Size desiredSize, Action<ICancelableRequest, BitmapDataResult> outputAction)
	{
		return Enqueue(item, timestamp, ThumbnailGetStrategy.GetAtOrBefore, desiredSize, outputAction, forceOutputActionInWorkerThread: false);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, ThumbnailGetStrategy strategy, Size desiredSize, Action<ICancelableRequest, BitmapDataResult> outputAction)
	{
		return Enqueue(item, timestamp, strategy, desiredSize, outputAction, forceOutputActionInWorkerThread: false);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public ICancelableRequest Enqueue(Item item, DateTime timestamp, ThumbnailGetStrategy strategy, Size desiredSize, Action<ICancelableRequest, BitmapDataResult> outputAction, bool forceOutputActionInWorkerThread)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (outputAction == null)
		{
			throw new ArgumentNullException("outputAction");
		}
		ThumbnailRequestAdapter request = null;
		if (UsingCache)
		{
			ThumbnailCacheKey key = CreateCacheKey(item, timestamp, desiredSize);
			BitmapData cachedResult = _cache.Get(key);
			if (cachedResult != null)
			{
				request = new ThumbnailRequestAdapter(new CachedResultInput());
				if (!forceOutputActionInWorkerThread)
				{
					outputAction(request, new BitmapDataResult(cachedResult));
				}
				else
				{
					Task.Factory.StartNew(delegate
					{
						outputAction(request, new BitmapDataResult(cachedResult));
					}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default);
				}
				return request;
			}
		}
		Action<BitmapDataResult> outputAction2 = delegate(BitmapDataResult data)
		{
			if (data.BitmapData != null && UsingCache)
			{
				ThumbnailCacheKey key2 = CreateCacheKey(item, timestamp, desiredSize);
				_cache.Add(key2, data.BitmapData);
			}
			outputAction(request, data);
		};
		MultiLevelResourceQueueInput<BitmapDataResult> multiLevelResourceQueueInput = new MultiLevelResourceQueueInput<BitmapDataResult>(new QueueInputPayload
		{
			Item = item,
			Timestamp = timestamp,
			DesiredSize = desiredSize,
			Strategy = strategy
		}, outputAction2);
		request = new ThumbnailRequestAdapter(multiLevelResourceQueueInput);
		_queue.Enqueue(multiLevelResourceQueueInput);
		return request;
	}

	public Task<BitmapSource> ConvertToBitmapSource(BitmapData bitmapData)
	{
		return Task.Factory.StartNew(() => bitmapData?.ConvertToBitmapSource(), CancellationToken.None, TaskCreationOptions.None, _imageSourceCreationScheduler);
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			_queue.Dispose();
			if (_cache != null)
			{
				_cache.Dispose();
			}
			_imageSourceCreationScheduler.Dispose();
		}
	}

	public static ThumbnailCacheKey CreateCacheKey(Item item, DateTime timestamp, Size desiredSize)
	{
		return new ThumbnailCacheKey(item.FQID.ObjectId, timestamp, desiredSize);
	}
}
