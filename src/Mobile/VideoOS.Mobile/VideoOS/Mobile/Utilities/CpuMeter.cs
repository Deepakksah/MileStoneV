using System;
using System.Diagnostics;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Utilities;

public class CpuMeter
{
	private readonly Process _process;

	private long _lastLogTime;

	private long _lastCpuUsageTime;

	private readonly CycleTaskMember _cpuMeterTimer;

	public long CpuUsagePercentage { get; private set; }

	public long UsedThreadsCount { get; private set; }

	public static CpuMeter CpuMeterS { get; private set; }

	public CpuMeter(TimeSpan timeout)
	{
		_process = System.Diagnostics.Process.GetCurrentProcess();
		_lastLogTime = DateTime.MinValue.Ticks;
		_cpuMeterTimer = new CycleTaskMember((Action)Process, (Action)null, (Action)null, (int)timeout.TotalMilliseconds);
		CpuMeterS = this;
	}

	public void Start()
	{
		_cpuMeterTimer.StartTask();
	}

	public void Stop()
	{
		_cpuMeterTimer.SignalTaskToStop();
	}

	public void Process()
	{
		try
		{
			long ticks = DateTime.UtcNow.Ticks;
			long num = ticks - _lastLogTime;
			long ticks2 = _process.TotalProcessorTime.Ticks;
			if (num != 0L)
			{
				CpuUsagePercentage = (long)((float)((ticks2 - _lastCpuUsageTime) * 100) / ((float)num * (float)Environment.ProcessorCount));
			}
			_lastLogTime = ticks;
			_lastCpuUsageTime = ticks2;
			UsedThreadsCount = _process.Threads.Count;
		}
		catch (Exception ex)
		{
			Log.Error("CpuMeter.Process", ex.Message);
		}
	}
}
