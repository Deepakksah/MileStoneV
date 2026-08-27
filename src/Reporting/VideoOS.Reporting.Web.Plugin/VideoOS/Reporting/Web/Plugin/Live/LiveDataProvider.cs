using System;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business;

namespace VideoOS.Reporting.Web.Plugin.Live;

public static class LiveDataProvider
{
	public static ILiveResult GetLiveCpu(string hostName)
	{
		return GetLiveCpu(hostName, ServiceLogger.Instance);
	}

	public static ILiveResult GetLiveCpu(string hostName, IServiceLogger serviceLogger)
	{
		LiveDataResult performanceCounterSample = LiveData.GetPerformanceCounterSample(hostName, "Processor", "% Processor Time", "_Total", 100, serviceLogger);
		LiveResult liveResult = new LiveResult();
		liveResult.DisplayUnit = Unit.Percent;
		if (performanceCounterSample.Error)
		{
			liveResult.Error = true;
		}
		if (performanceCounterSample.Empty)
		{
			liveResult.DisplayUnit = Unit.Unknown;
		}
		else
		{
			liveResult.DisplayValue = performanceCounterSample.Value;
			liveResult.ValueInPercent = (int)performanceCounterSample.Value;
		}
		return liveResult;
	}

	public static ILiveResult GetLiveMemory(string hostName, int maxValue)
	{
		return GetLiveMemory(hostName, maxValue, ServiceLogger.Instance);
	}

	public static ILiveResult GetLiveMemory(string hostName, int maxValue, IServiceLogger serviceLogger)
	{
		LiveDataResult performanceCounterSample = LiveData.GetPerformanceCounterSample(hostName, "Memory", "Available MBytes", "", 100, serviceLogger);
		LiveResult liveResult = new LiveResult();
		if (performanceCounterSample.Error)
		{
			liveResult.Error = true;
		}
		if (performanceCounterSample.Empty)
		{
			liveResult.DisplayUnit = Unit.Unknown;
		}
		else
		{
			if (performanceCounterSample.Value < 1024f)
			{
				liveResult.DisplayValue = performanceCounterSample.Value;
				liveResult.DisplayUnit = Unit.MB;
			}
			else
			{
				liveResult.DisplayValue = performanceCounterSample.Value / 1024f;
				liveResult.DisplayUnit = Unit.GB;
			}
			liveResult.ValueInPercent = performanceCounterSample.Value * 100f / (float)maxValue;
		}
		return liveResult;
	}

	public static ILiveResult GetDiskReadSpeed(string hostName, string instanceName)
	{
		return GetDiskReadSpeed(hostName, instanceName, ServiceLogger.Instance);
	}

	public static ILiveResult GetDiskReadSpeed(string hostName, string instanceName, IServiceLogger serviceLogger)
	{
		LiveDataResult performanceCounterSample = LiveData.GetPerformanceCounterSample(hostName, "LogicalDisk", "Disk Read Bytes/sec", instanceName, 20971520, serviceLogger);
		LiveResult liveResult = new LiveResult();
		if (performanceCounterSample.Error)
		{
			liveResult.Error = true;
		}
		if (performanceCounterSample.Empty)
		{
			liveResult.DisplayUnit = Unit.Unknown;
		}
		else
		{
			(liveResult.DisplayValue, liveResult.DisplayUnit) = DataUnitUtility.GetCalculatedUnit(performanceCounterSample.Value, new Unit[4]
			{
				Unit.BytesSec,
				Unit.KBSec,
				Unit.MBSec,
				Unit.GBSec
			}, 1024f, 1024u);
			if (performanceCounterSample.MaxValue > 0)
			{
				liveResult.ValueInPercent = performanceCounterSample.Value * 100f / (float)performanceCounterSample.MaxValue;
			}
			else
			{
				liveResult.ValueInPercent = 0f;
			}
			if (liveResult.ValueInPercent > 100f)
			{
				liveResult.ValueInPercent = 100f;
			}
		}
		return liveResult;
	}

	public static ILiveResult GetDiskWriteSpeed(string hostName, string instanceName)
	{
		return GetDiskWriteSpeed(hostName, instanceName, ServiceLogger.Instance);
	}

	public static ILiveResult GetDiskWriteSpeed(string hostName, string instanceName, IServiceLogger serviceLogger)
	{
		LiveDataResult performanceCounterSample = LiveData.GetPerformanceCounterSample(hostName, "LogicalDisk", "Disk Write Bytes/sec", instanceName, 20971520, serviceLogger);
		LiveResult liveResult = new LiveResult();
		if (performanceCounterSample.Error)
		{
			liveResult.Error = true;
		}
		if (performanceCounterSample.Empty)
		{
			liveResult.DisplayUnit = Unit.Unknown;
		}
		else
		{
			(float, Unit) calculatedUnit = DataUnitUtility.GetCalculatedUnit(performanceCounterSample.Value, new Unit[4]
			{
				Unit.BytesSec,
				Unit.KBSec,
				Unit.MBSec,
				Unit.GBSec
			}, 1024f, 1024u);
			liveResult.DisplayValue = calculatedUnit.Item1;
			liveResult.DisplayUnit = calculatedUnit.Item2;
			liveResult.ValueInPercent = performanceCounterSample.Value * 100f / (float)performanceCounterSample.MaxValue;
			if (liveResult.ValueInPercent > 100f)
			{
				liveResult.ValueInPercent = 100f;
			}
		}
		return liveResult;
	}

	public static LiveDataAdapterResult[] GetAdaptersLiveData(Guid recorderId, string hostName)
	{
		return GetAdaptersLiveData(recorderId, hostName, ServiceLogger.Instance);
	}

	public static LiveDataAdapterResult[] GetAdaptersLiveData(Guid recorderId, string hostName, IServiceLogger serviceLogger)
	{
		return LiveAdapterData.GetAdaptersLiveData(recorderId, hostName, serviceLogger);
	}
}
