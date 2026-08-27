using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util.Queue;

namespace VideoOS.Platform.Data;

public sealed class ItemRequestQueue : IDisposable
{
	private class QueueResourceProcessor : ResourceProcessor<bool>
	{
		protected override bool ShouldReinitialize(MultiLevelResourceQueueInput<bool> input)
		{
			return false;
		}

		protected override void Initialize(MultiLevelResourceQueueInput<bool> input)
		{
		}

		protected override void ProcessItem(MultiLevelResourceQueueInput<bool> input)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (!input.Canceled)
			{
				input.OutputAction(obj: false);
			}
		}
	}

	private readonly MultiLevelResourceQueue<bool> _queue;

	private bool _disposed;

	public ItemRequestQueue(int totalRequestsInParallel, int sitesInParallel, int serversPerSiteInParallel, int requestsPerServerInParallel)
	{
		LevelConfiguration siteLevel = new LevelConfiguration(sitesInParallel);
		LevelConfiguration serverLevel = new LevelConfiguration(serversPerSiteInParallel);
		QueueConfiguration configuration = new QueueConfiguration(totalRequestsInParallel, requestsPerServerInParallel, int.MaxValue, delegate(LevelConfiguration levelConfiguration, object o)
		{
			if (levelConfiguration == siteLevel)
			{
				FQID fQID = ((Item)o).FQID;
				if (fQID.ServerId.ServerType == "BLK")
				{
					return "BLK";
				}
				while (fQID.ServerId.ServerType != "XPCO")
				{
					fQID = fQID.GetParent();
				}
				return fQID.ObjectId;
			}
			return (levelConfiguration == serverLevel) ? ((object)((Item)o).FQID.ServerId.Id) : null;
		})
		{
			LevelConfigurations = { siteLevel, serverLevel }
		};
		_queue = new MultiLevelResourceQueue<bool>(CreateResourceProcessor, configuration);
		_queue.ErrorOccurred += delegate(object sender, MultiLevelResourceQueueErrorOccurredEventArgs<bool> args)
		{
			EnvironmentManager.Instance.Log(error: true, GetType().Name + "-MultiLevelResourceQueue-ErrorOccurred", "MultiLevelResourceQueue signaled ErrorOccurred: {0}", new object[1] { args.ErrorMessage });
		};
		new TrackPageViewData(typeof(ItemRequestQueue)).Send();
	}

	public Task Enqueue(Item item, Action action, CancellationToken cancellationToken)
	{
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (action == null)
		{
			throw new ArgumentNullException("action");
		}
		if (item.FQID.Kind != Kind.Camera && item.FQID.Kind != Kind.Microphone && item.FQID.Kind != Kind.Speaker && item.FQID.Kind != Kind.Metadata)
		{
			throw new ArgumentException("Item kind must be of type Camera, Microphone, Speaker or Metadata/>", "item");
		}
		Task task = new Task(action);
		MultiLevelResourceQueueInput<bool> input = null;
		Action<bool> outputAction = delegate
		{
			if (!input.Canceled)
			{
				task.RunSynchronously();
			}
		};
		input = new MultiLevelResourceQueueInput<bool>(item, outputAction);
		if (cancellationToken != CancellationToken.None)
		{
			cancellationToken.Register(delegate
			{
				input.Canceled = true;
			});
			input.Canceled = cancellationToken.IsCancellationRequested;
		}
		_queue.Enqueue(input);
		return task;
	}

	public void Dispose()
	{
		if (!_disposed)
		{
			_disposed = true;
			_queue.Dispose();
		}
	}

	private static ResourceProcessor<bool> CreateResourceProcessor(MultiLevelResourceQueueInput<bool> input)
	{
		return new QueueResourceProcessor();
	}
}
