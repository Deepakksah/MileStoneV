using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Util.Queue;

public class MultiLevelResourceQueue<TOutput> : IDisposable
{
	public delegate ResourceProcessor<TOutput> IResourceProcessorFactory(MultiLevelResourceQueueInput<TOutput> input);

	private class MultiLevelResourceQueueDiagnostics : IMultiLevelResourceQueueDiagnostics
	{
		public int MaxConcurrentRequests { get; }

		public int MaxIdleTimeMillisBeforeResourceProcessorCleanup { get; }

		public int ConcurrentRequests { get; }

		public int ActiveResourceProcessors { get; }

		public int IdleResourceProcessors { get; }

		public int ResourceProcessors { get; }

		public int RequestsInQueue { get; }

		public MultiLevelResourceQueueDiagnostics(MultiLevelResourceQueue<TOutput> queue)
		{
			MaxConcurrentRequests = queue._limitManager.Configuration.MaxConcurrentRequests;
			MaxIdleTimeMillisBeforeResourceProcessorCleanup = queue._limitManager.Configuration.MaxIdleTimeMillisBeforeResourceProcessorCleanup;
			ConcurrentRequests = queue._limitManager.ConcurrentRequests;
			List<ResourceProcessor<TOutput>> resourceProcessors = queue.ResourceProcessors;
			ActiveResourceProcessors = resourceProcessors.Count((ResourceProcessor<TOutput> p) => !p.Idle);
			IdleResourceProcessors = resourceProcessors.Count - ActiveResourceProcessors;
			ResourceProcessors = resourceProcessors.Count;
			RequestsInQueue = queue.Count;
		}
	}

	public readonly LimitManager _limitManager;

	private readonly LevelQueue<TOutput> _rootQueue;

	private readonly object _disposedLock = new object();

	private bool _disposed;

	public int Count => _rootQueue.Count;

	public bool Inactive => !_rootQueue.HasChildren;

	public IMultiLevelResourceQueueDiagnostics Diagnostics => new MultiLevelResourceQueueDiagnostics(this);

	public List<ResourceProcessor<TOutput>> ResourceProcessors => _rootQueue.ResourceProcessors;

	public event EventHandler<MultiLevelResourceQueueErrorOccurredEventArgs<TOutput>> ErrorOccurred;

	public MultiLevelResourceQueue(IResourceProcessorFactory resourceProcessorFactory, QueueConfiguration configuration)
	{
		if (configuration == null)
		{
			throw new ArgumentNullException("configuration");
		}
		if (configuration.LevelConfigurations == null)
		{
			throw new ArgumentNullException("LevelConfigurations");
		}
		if (configuration.LevelConfigurations.Count < 1)
		{
			throw new ArgumentException("At least one level configuration must be supplied", "LevelConfigurations");
		}
		if (resourceProcessorFactory == null)
		{
			throw new ArgumentNullException("resourceProcessorFactory");
		}
		_limitManager = new LimitManager(configuration);
		_rootQueue = new LevelQueue<TOutput>(this, _limitManager, resourceProcessorFactory, configuration, QueueConfiguration.RootLevelConfiguration, null, keepAlive: true);
	}

	public void Enqueue(MultiLevelResourceQueueInput<TOutput> input)
	{
		_rootQueue.Enqueue(input);
	}

	public void Clear()
	{
		_rootQueue.Clear();
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
		_rootQueue.Dispose();
		_limitManager.Dispose();
	}

	public void NotifyError(MultiLevelResourceQueueInput<TOutput> input, string errorMessage, Exception exception)
	{
		this.ErrorOccurred?.Invoke(this, new MultiLevelResourceQueueErrorOccurredEventArgs<TOutput>(input, errorMessage, exception));
	}
}
