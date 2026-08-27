using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Platform.Util.Queue;

public class LevelQueue<TOutput> : IDisposable
{
	private readonly List<LevelQueue<TOutput>> _subLevelQueues;

	private readonly List<ResourceProcessor<TOutput>> _resourceProcessors;

	private readonly MultiLevelResourceQueue<TOutput> _queue;

	private readonly LimitManager _limitManager;

	private readonly MultiLevelResourceQueue<TOutput>.IResourceProcessorFactory _resourceProcessorFactory;

	private readonly QueueConfiguration _configuration;

	protected readonly LevelConfiguration _levelConfiguration;

	private readonly bool _keepAlive;

	private readonly ConcurrentQueueSlim<MultiLevelResourceQueueInput<TOutput>> _levelQueue = new ConcurrentQueueSlim<MultiLevelResourceQueueInput<TOutput>>();

	private readonly ManualResetEventSlim _queueConsumerFinishedEvent = new ManualResetEventSlim(initialState: false);

	private bool _disposed;

	private readonly object _disposedLock = new object();

	private readonly bool _hasSubLevelQueues;

	private readonly object _subLevelLock = new object();

	private bool _stopRequested;

	public object LevelIdentifier { get; }

	public bool Stopped { get; private set; }

	public bool Idle
	{
		get
		{
			lock (_subLevelLock)
			{
				bool num = _levelQueue.Count == 0;
				bool flag = _subLevelQueues == null || _subLevelQueues.All((LevelQueue<TOutput> q) => q.Idle);
				bool flag2 = _resourceProcessors == null || _resourceProcessors.All((ResourceProcessor<TOutput> p) => p.Idle);
				return num && flag && flag2;
			}
		}
	}

	public int Count
	{
		get
		{
			int num = _levelQueue.Count;
			if (_hasSubLevelQueues)
			{
				lock (_subLevelLock)
				{
					num += _subLevelQueues.Sum((LevelQueue<TOutput> q) => q.Count);
				}
			}
			return num;
		}
	}

	public List<ResourceProcessor<TOutput>> ResourceProcessors
	{
		get
		{
			lock (_subLevelLock)
			{
				IEnumerable<ResourceProcessor<TOutput>> resourceProcessors = _resourceProcessors;
				List<ResourceProcessor<TOutput>> list = new List<ResourceProcessor<TOutput>>(resourceProcessors ?? Enumerable.Empty<ResourceProcessor<TOutput>>());
				if (_hasSubLevelQueues)
				{
					list.AddRange(_subLevelQueues.SelectMany((LevelQueue<TOutput> q) => q.ResourceProcessors));
				}
				return list;
			}
		}
	}

	public bool HasChildren
	{
		get
		{
			lock (_subLevelLock)
			{
				return _hasSubLevelQueues ? (_subLevelQueues.Count > 0) : (_resourceProcessors.Count > 0);
			}
		}
	}

	public LevelQueue(MultiLevelResourceQueue<TOutput> queue, LimitManager limitManager, MultiLevelResourceQueue<TOutput>.IResourceProcessorFactory resourceProcessorFactory, QueueConfiguration configuration, LevelConfiguration levelConfiguration, object identifier, bool keepAlive)
	{
		_queue = queue;
		_limitManager = limitManager;
		_resourceProcessorFactory = resourceProcessorFactory;
		_configuration = configuration;
		_levelConfiguration = levelConfiguration;
		_keepAlive = keepAlive;
		LevelIdentifier = identifier;
		_hasSubLevelQueues = configuration.LevelConfigurations.Last() != levelConfiguration;
		if (_hasSubLevelQueues)
		{
			_subLevelQueues = new List<LevelQueue<TOutput>>();
		}
		else
		{
			_resourceProcessors = new List<ResourceProcessor<TOutput>>();
		}
		Task.Factory.StartNew(QueueConsumer, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default).ContinueWith(delegate
		{
			_queueConsumerFinishedEvent.Set();
			Stop();
		}, TaskScheduler.Default);
	}

	public bool Enqueue(MultiLevelResourceQueueInput<TOutput> item)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (!item.Canceled)
		{
			return _levelQueue.Enqueue(item);
		}
		return false;
	}

	public void Stop()
	{
		lock (_subLevelLock)
		{
			if (_stopRequested)
			{
				return;
			}
			_stopRequested = true;
			_levelQueue.Stop();
			if (_hasSubLevelQueues)
			{
				foreach (LevelQueue<TOutput> subLevelQueue in _subLevelQueues)
				{
					subLevelQueue.Stop();
					subLevelQueue.Dispose();
				}
				_subLevelQueues.Clear();
			}
			else
			{
				foreach (ResourceProcessor<TOutput> resourceProcessor in _resourceProcessors)
				{
					resourceProcessor.Stop();
					resourceProcessor.Dispose();
				}
				_resourceProcessors.Clear();
			}
		}
		_queueConsumerFinishedEvent.Wait();
		Stopped = true;
	}

	public void Clear()
	{
		lock (_subLevelLock)
		{
			_levelQueue.Clear();
			if (!_hasSubLevelQueues)
			{
				return;
			}
			foreach (LevelQueue<TOutput> subLevelQueue in _subLevelQueues)
			{
				subLevelQueue.Clear();
			}
		}
	}

	public void QueueConsumer()
	{
		while (!_levelQueue.IsStopped && _levelQueue.Peek(20) == null)
		{
		}
		while (!_levelQueue.IsStopped && !_disposed)
		{
			MultiLevelResourceQueueInput<TOutput> multiLevelResourceQueueInput = _levelQueue.Peek(20);
			Cleanup();
			if (multiLevelResourceQueueInput != null)
			{
				if (_hasSubLevelQueues)
				{
					PassItemToSubLevelQueue(multiLevelResourceQueueInput);
				}
				else
				{
					PassItemToResourceProcessor(multiLevelResourceQueueInput);
				}
			}
			Cleanup();
			if (!_keepAlive && _levelQueue.Count == 0 && !HasActiveChildren())
			{
				_levelQueue.Stop();
			}
		}
	}

	private void Cleanup()
	{
		if (_hasSubLevelQueues)
		{
			lock (_subLevelLock)
			{
				_subLevelQueues.Where((LevelQueue<TOutput> q) => q.Stopped).ToList().ForEach(delegate(LevelQueue<TOutput> q)
				{
					_subLevelQueues.Remove(q);
					q.Dispose();
				});
				return;
			}
		}
		lock (_subLevelLock)
		{
			_resourceProcessors.Where((ResourceProcessor<TOutput> p) => p.Stopped).ToList().ForEach(delegate(ResourceProcessor<TOutput> p)
			{
				_resourceProcessors.Remove(p);
				p.Dispose();
			});
		}
	}

	private bool HasActiveChildren()
	{
		lock (_subLevelLock)
		{
			return _hasSubLevelQueues ? _subLevelQueues.Any((LevelQueue<TOutput> q) => !q.Stopped) : _resourceProcessors.Any((ResourceProcessor<TOutput> p) => !p.Stopped);
		}
	}

	private void PassItemToResourceProcessor(MultiLevelResourceQueueInput<TOutput> item)
	{
		if (item.Canceled)
		{
			_levelQueue.Remove(item);
			return;
		}
		ResourceProcessor<TOutput> orCreateResourceProcessor = GetOrCreateResourceProcessor(item);
		if (orCreateResourceProcessor != null)
		{
			WaitForRequestToBeAllowed();
			if (orCreateResourceProcessor.Process(item))
			{
				_levelQueue.Remove(item);
			}
			else
			{
				CancelRequestProcessing();
			}
		}
	}

	private ResourceProcessor<TOutput> GetOrCreateResourceProcessor(MultiLevelResourceQueueInput<TOutput> item)
	{
		lock (_subLevelLock)
		{
			if (_stopRequested || _disposed)
			{
				return null;
			}
			ResourceProcessor<TOutput> resourceProcessor = _resourceProcessors.FirstOrDefault((ResourceProcessor<TOutput> p) => p.Idle && !p.Stopped);
			if (resourceProcessor != null)
			{
				return resourceProcessor;
			}
			if (_resourceProcessors.Count == _configuration.MaxResourceProcessorsPerLevel)
			{
				return null;
			}
			resourceProcessor = _resourceProcessorFactory(item);
			resourceProcessor.Init(_queue, _limitManager, _configuration.MaxIdleTimeMillisBeforeResourceProcessorCleanup);
			_resourceProcessors.Add(resourceProcessor);
			return resourceProcessor;
		}
	}

	private void WaitForRequestToBeAllowed()
	{
		while (!_limitManager.AllowRequestToBeProcessed())
		{
			_limitManager.AllowRequestNotification.Wait();
		}
	}

	private void CancelRequestProcessing()
	{
		_limitManager.RequestProcessingDone();
	}

	private void PassItemToSubLevelQueue(MultiLevelResourceQueueInput<TOutput> item)
	{
		if (item.Canceled)
		{
			_levelQueue.Remove(item);
			return;
		}
		LevelQueue<TOutput> orCreateSubLevelQueue = GetOrCreateSubLevelQueue(item);
		if (orCreateSubLevelQueue != null && orCreateSubLevelQueue.Enqueue(item))
		{
			_levelQueue.Remove(item);
		}
	}

	private LevelConfiguration GetSubLevelConfiguration()
	{
		if (_levelConfiguration == QueueConfiguration.RootLevelConfiguration)
		{
			return _configuration.LevelConfigurations.First();
		}
		return _configuration.LevelConfigurations[_configuration.LevelConfigurations.IndexOf(_levelConfiguration) + 1];
	}

	private LevelQueue<TOutput> GetOrCreateSubLevelQueue(MultiLevelResourceQueueInput<TOutput> item)
	{
		LevelConfiguration subLevelConfiguration = GetSubLevelConfiguration();
		object subLevelIdentifier;
		try
		{
			subLevelIdentifier = _configuration.LevelIdentifierFunc(subLevelConfiguration, item.Payload);
		}
		catch (Exception exception)
		{
			_queue.NotifyError(item, "Unable to identify level for input", exception);
			_levelQueue.Remove(item);
			return null;
		}
		lock (_subLevelLock)
		{
			if (_stopRequested || _disposed)
			{
				return null;
			}
			LevelQueue<TOutput> levelQueue = _subLevelQueues.FirstOrDefault((LevelQueue<TOutput> q) => object.Equals(q.LevelIdentifier, subLevelIdentifier));
			int count = _subLevelQueues.Count;
			if (levelQueue != null)
			{
				return levelQueue;
			}
			if (count == subLevelConfiguration.MaxConcurrentNodes)
			{
				LevelQueue<TOutput> levelQueue2 = _subLevelQueues.FirstOrDefault((LevelQueue<TOutput> q) => q.Idle);
				if (levelQueue2 != null)
				{
					_subLevelQueues.Remove(levelQueue2);
					levelQueue2.Stop();
					levelQueue2.Dispose();
					return GetOrCreateSubLevelQueue(item);
				}
				return null;
			}
			levelQueue = new LevelQueue<TOutput>(_queue, _limitManager, _resourceProcessorFactory, _configuration, subLevelConfiguration, subLevelIdentifier, keepAlive: false);
			_subLevelQueues.Add(levelQueue);
			return levelQueue;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed || !disposing)
		{
			return;
		}
		lock (_disposedLock)
		{
			if (_disposed)
			{
				return;
			}
			_disposed = true;
		}
		if (!Stopped)
		{
			Stop();
		}
		_queueConsumerFinishedEvent.Dispose();
	}
}
