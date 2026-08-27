using System;
using System.Collections.Generic;
using System.Globalization;
using Grpc.Core;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Communication;

namespace VideoOS.Reporting.Web.Plugin.Business;

public class LiveData
{
	private static Dictionary<Tuple<string, string, string, string>, int> _maxValue = new Dictionary<Tuple<string, string, string, string>, int>();

	private static LiveDataCache<Tuple<string, string, string, string>, float> _liveDataCache = new LiveDataCache<Tuple<string, string, string, string>, float>();

	public static void ResetCache()
	{
		_maxValue = new Dictionary<Tuple<string, string, string, string>, int>();
		_liveDataCache = new LiveDataCache<Tuple<string, string, string, string>, float>();
	}

	public static LiveDataResult GetPerformanceCounterSample(string hostName, string categoryName, string counterName, string instanceName, int defaultMaxValue, IServiceLogger serviceLogger)
	{
		LiveDataResult liveDataResult = new LiveDataResult();
		Tuple<string, string, string, string> key = new Tuple<string, string, string, string>(hostName, categoryName, counterName, instanceName);
		lock (_maxValue)
		{
			if (!_maxValue.ContainsKey(key))
			{
				_maxValue.Add(key, defaultMaxValue);
			}
		}
		LiveDataCache<Tuple<string, string, string, string>, float>.QueryCounterValue queryCounterValue = QueryPerformanceCounter;
		CacheEntry<float> value = _liveDataCache.GetValue(key, queryCounterValue, serviceLogger);
		if (!value.Error)
		{
			if (!value.Empty)
			{
				liveDataResult.Value = value.Entry;
				float entry = value.Entry;
				liveDataResult.Error = false;
				lock (_maxValue)
				{
					if (entry > (float)_maxValue[key])
					{
						_maxValue[key] = (int)Math.Ceiling(entry);
					}
					liveDataResult.MaxValue = _maxValue[key];
					return liveDataResult;
				}
			}
			liveDataResult.Empty = true;
			return liveDataResult;
		}
		liveDataResult.Error = true;
		return liveDataResult;
	}

	private static void QueryPerformanceCounter(Tuple<string, string, string, string> key, IServiceLogger serviceLogger, params string[] args)
	{
		try
		{
			serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "LiveData, query counter: '{0}/{1}/{2}/{3}'", key.Item1, key.Item2, key.Item3, key.Item4), LogType.Debug);
			float performanceCounterInstanceValue = LiveDataAgent.GetPerformanceCounterInstanceValue(key.Item1, key.Item2, key.Item3, key.Item4);
			_liveDataCache.UpdateCache(key, performanceCounterInstanceValue);
		}
		catch (Exception ex)
		{
			string arg = string.Format(CultureInfo.InvariantCulture, "Error occurred while querying performance counter '{0}/{1}/{2}/{3}'", key.Item1, key.Item2, key.Item3, key.Item4);
			if (ex is RpcException ex2)
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", arg, ex2.Status.Detail), LogType.Error);
			}
			else
			{
				serviceLogger.LogToServerLog(ex, null, LogType.Error);
			}
			_liveDataCache.ResetPendingUpdate(key);
		}
	}
}
