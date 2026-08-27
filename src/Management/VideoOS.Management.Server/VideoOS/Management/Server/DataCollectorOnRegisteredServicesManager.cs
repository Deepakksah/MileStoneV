using System;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

public class DataCollectorOnRegisteredServicesManager : IDisposable
{
	private static DataCollectorOnRegisteredServicesManager _instance = null;

	private static readonly object initLocker = new object();

	private static readonly object queueLocker = new object();

	private bool _disposed;

	private IDataCollectorRegisteredServicesResolver Resolver { get; set; }

	public static DataCollectorOnRegisteredServicesManager Instance
	{
		get
		{
			if (_instance == null)
			{
				lock (initLocker)
				{
					if (_instance == null)
					{
						_instance = new DataCollectorOnRegisteredServicesManager();
					}
				}
			}
			return _instance;
		}
	}

	public System.Threading.Tasks.Task MainRunningResolveTask { get; private set; }

	public System.Threading.Tasks.Task SecondaryRunningResolveTask { get; private set; }

	public DataCollectorOnRegisteredServicesManager()
		: this(new DataCollectorRegisteredServicesResolver())
	{
	}

	public DataCollectorOnRegisteredServicesManager(IDataCollectorRegisteredServicesResolver resolver)
	{
		if (resolver == null)
		{
			throw new ArgumentNullException("resolver");
		}
		Resolver = resolver;
	}

	public void QueueResolve()
	{
		lock (queueLocker)
		{
			if (MainRunningResolveTask == null)
			{
				MainRunningResolveTask = new System.Threading.Tasks.Task(delegate
				{
					Resolver.DoResolve();
				});
				MainRunningResolveTask.Start();
			}
			else if (MainRunningResolveTask.IsCompleted)
			{
				MainRunningResolveTask.Dispose();
				MainRunningResolveTask = new System.Threading.Tasks.Task(delegate
				{
					Resolver.DoResolve();
				});
				MainRunningResolveTask.Start();
			}
			else if (SecondaryRunningResolveTask == null)
			{
				SecondaryRunningResolveTask = new System.Threading.Tasks.Task(delegate
				{
					Resolver.DoResolve();
				});
				SecondaryRunningResolveTask.Start();
			}
			else if (SecondaryRunningResolveTask.IsCompleted)
			{
				SecondaryRunningResolveTask.Dispose();
				SecondaryRunningResolveTask = new System.Threading.Tasks.Task(delegate
				{
					Resolver.DoResolve();
				});
				SecondaryRunningResolveTask.Start();
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		lock (this)
		{
			if (!_disposed && disposing)
			{
				if (MainRunningResolveTask != null)
				{
					MainRunningResolveTask.Dispose();
				}
				if (SecondaryRunningResolveTask != null)
				{
					SecondaryRunningResolveTask.Dispose();
				}
			}
			_disposed = true;
		}
	}

	~DataCollectorOnRegisteredServicesManager()
	{
		Dispose(disposing: false);
	}
}
