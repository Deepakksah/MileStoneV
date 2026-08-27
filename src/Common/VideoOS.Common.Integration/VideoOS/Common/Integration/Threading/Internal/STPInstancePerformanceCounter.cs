using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Threading.Internal;

public class STPInstancePerformanceCounter : IDisposable
{
	private bool _isDisposed;

	private PerformanceCounter _pcs;

	protected STPInstancePerformanceCounter()
	{
		_isDisposed = false;
	}

	public STPInstancePerformanceCounter(string instance, STPPerformanceCounterType spcType)
		: this()
	{
		STPPerformanceCounters instance2 = STPPerformanceCounters.Instance;
		_pcs = new PerformanceCounter("SmartThreadPool", instance2._stpPerformanceCounters[(int)spcType].Name, instance, readOnly: false);
		_pcs.RawValue = _pcs.RawValue;
	}

	public void Close()
	{
		if (_pcs != null)
		{
			_pcs.RemoveInstance();
			_pcs.Close();
			_pcs = null;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
	}

	public virtual void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			Close();
		}
		_isDisposed = true;
	}

	public virtual void Increment()
	{
		_pcs.Increment();
	}

	public virtual void IncrementBy(long val)
	{
		_pcs.IncrementBy(val);
	}

	public virtual void Set(long val)
	{
		_pcs.RawValue = val;
	}
}
