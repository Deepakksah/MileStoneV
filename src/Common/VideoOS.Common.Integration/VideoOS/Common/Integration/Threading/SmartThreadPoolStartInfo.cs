using System.Threading;

namespace VideoOS.Common.Integration.Threading;

public class SmartThreadPoolStartInfo : WIGStartInfo
{
	private int _idleTimeout = 60000;

	private int _minWorkerThreads;

	private int _maxWorkerThreads = 25;

	private int _threadMaxStackSize;

	private ThreadPriority _threadPriority = ThreadPriority.Normal;

	private string _performanceCounterInstanceName = SmartThreadPool.DefaultPerformanceCounterInstanceName;

	private bool _enableLocalPerformanceCounters;

	public virtual int IdleTimeout
	{
		get
		{
			return _idleTimeout;
		}
		set
		{
			ThrowIfReadOnly();
			_idleTimeout = value;
		}
	}

	public virtual int MinWorkerThreads
	{
		get
		{
			return _minWorkerThreads;
		}
		set
		{
			ThrowIfReadOnly();
			_minWorkerThreads = value;
		}
	}

	public virtual int MaxWorkerThreads
	{
		get
		{
			return _maxWorkerThreads;
		}
		set
		{
			ThrowIfReadOnly();
			_maxWorkerThreads = value;
		}
	}

	public virtual ThreadPriority ThreadPriority
	{
		get
		{
			return _threadPriority;
		}
		set
		{
			ThrowIfReadOnly();
			_threadPriority = value;
		}
	}

	public int ThreadMaxStackSize
	{
		get
		{
			return _threadMaxStackSize;
		}
		set
		{
			ThrowIfReadOnly();
			_threadMaxStackSize = value;
		}
	}

	public virtual string PerformanceCounterInstanceName
	{
		get
		{
			return _performanceCounterInstanceName;
		}
		set
		{
			ThrowIfReadOnly();
			_performanceCounterInstanceName = value;
		}
	}

	public virtual bool EnableLocalPerformanceCounters
	{
		get
		{
			return _enableLocalPerformanceCounters;
		}
		set
		{
			ThrowIfReadOnly();
			_enableLocalPerformanceCounters = value;
		}
	}

	public SmartThreadPoolStartInfo()
	{
		_performanceCounterInstanceName = SmartThreadPool.DefaultPerformanceCounterInstanceName;
		_threadPriority = ThreadPriority.Normal;
		_threadMaxStackSize = 0;
		_maxWorkerThreads = 25;
		_idleTimeout = 60000;
		_minWorkerThreads = 0;
	}

	public SmartThreadPoolStartInfo(SmartThreadPoolStartInfo smartThreadPoolStartInfo)
		: base(smartThreadPoolStartInfo)
	{
		_idleTimeout = smartThreadPoolStartInfo.IdleTimeout;
		_minWorkerThreads = smartThreadPoolStartInfo.MinWorkerThreads;
		_maxWorkerThreads = smartThreadPoolStartInfo.MaxWorkerThreads;
		_threadMaxStackSize = 0;
		_threadPriority = smartThreadPoolStartInfo.ThreadPriority;
		_performanceCounterInstanceName = smartThreadPoolStartInfo.PerformanceCounterInstanceName;
		_enableLocalPerformanceCounters = smartThreadPoolStartInfo._enableLocalPerformanceCounters;
	}

	public new SmartThreadPoolStartInfo AsReadOnly()
	{
		return new SmartThreadPoolStartInfo(this)
		{
			_readOnly = true
		};
	}
}
