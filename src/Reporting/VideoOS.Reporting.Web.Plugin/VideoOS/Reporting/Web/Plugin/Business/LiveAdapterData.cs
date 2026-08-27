using System;
using System.Collections.Generic;
using System.Globalization;
using Grpc.Core;
using VideoOS.Common.Crypto;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business.Model;
using VideoOS.Reporting.Web.Plugin.Communication;
using VideoOS.Reporting.Web.Plugin.Live;
using VideoOS.Reporting.Web.Plugin.Settings;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Reporting.Web.Plugin.Business;

public static class LiveAdapterData
{
	private class AdapterResultHost : Dictionary<string, LiveDataAdapterResult>
	{
	}

	private const string AdapterCategoryName = "Network Interface";

	private const string AdapterSentCounterName = "Bytes Sent/sec";

	private const string AdapterReceivedCounterName = "Bytes Received/sec";

	private const string AdapterBandwidthCounterName = "Current Bandwidth";

	private static ITokenCache _tokenCache = null;

	private static readonly object _locker = new object();

	private static LiveDataCache<string, AdapterResultHost> _liveDataAdapterHostCache = new LiveDataCache<string, AdapterResultHost>();

	public static ITokenCache DataCollectorTokenCache
	{
		get
		{
			if (_tokenCache == null)
			{
				lock (_locker)
				{
					if (_tokenCache == null)
					{
						using SecretHandler secretHandler = new SecretHandler(new FileSecretProvider(ConfigSettings.ServerLogFolder, "idp.tmp", "idp.sec"));
						secretHandler.EnsureUserEncryptedSecret();
						string secret = new string(secretHandler.ReadSecret());
						_tokenCache = new TokenCache(new IdentityServerTokenHandler(new IdentityServerClientSettings(ConfigSettings.AuthorizationServerUri, ConfigSettings.ClientRegistrationId.ToString(), "client_credentials"), "datacollector", secret));
						_tokenCache.Start();
					}
				}
			}
			return _tokenCache;
		}
	}

	public static void ClearCache()
	{
		_liveDataAdapterHostCache = new LiveDataCache<string, AdapterResultHost>();
	}

	public static LiveDataAdapterResult[] GetAdaptersLiveData(Guid recorderId, string hostname, IServiceLogger serviceLogger)
	{
		LiveDataCache<string, AdapterResultHost>.QueryCounterValue queryCounterValue = QueryAdapterCounters;
		CacheEntry<AdapterResultHost> value = _liveDataAdapterHostCache.GetValue(hostname, queryCounterValue, QueryDatabase, recorderId, serviceLogger);
		List<LiveDataAdapterResult> list = new List<LiveDataAdapterResult>();
		if (value.Entry != null)
		{
			list.AddRange(value.Entry.Values);
		}
		return list.ToArray();
	}

	private static AdapterResultHost QueryDatabase(Guid recorderId)
	{
		AdapterOverview[] array = Database.AdapterOverview(recorderId);
		AdapterResultHost adapterResultHost = new AdapterResultHost();
		if (array != null)
		{
			AdapterOverview[] array2 = array;
			foreach (AdapterOverview adapterOverview in array2)
			{
				adapterResultHost.Add(adapterOverview.AdapterName, LiveDataAdapterResult.Convert(adapterOverview));
			}
		}
		return adapterResultHost;
	}

	private static void QueryAdapterCounters(string hostname, IServiceLogger serviceLogger, params string[] args)
	{
		try
		{
			serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "LiveAdapterData, query counters for: '{0}'", hostname), LogType.Debug);
			AdapterResultHost adapterCounterAdapterResultHost = GetAdapterCounterAdapterResultHost(LiveDataAgent.GetPerformanceCounterValues(hostname, "Network Interface", new string[3] { "Bytes Sent/sec", "Bytes Received/sec", "Current Bandwidth" }));
			_liveDataAdapterHostCache.UpdateCache(hostname, adapterCounterAdapterResultHost);
		}
		catch (Exception ex)
		{
			string arg = string.Format(CultureInfo.InvariantCulture, "Error occurred while querying adapter performance counters for '{0}'", hostname);
			if (ex is RpcException ex2)
			{
				serviceLogger.LogToServerLog(string.Format(CultureInfo.InvariantCulture, "{0}: {1}", arg, ex2.Status.Detail), LogType.Error);
			}
			else
			{
				serviceLogger.LogToServerLog(ex, null, LogType.Error);
			}
			_liveDataAdapterHostCache.ResetPendingUpdate(hostname);
		}
	}

	private static AdapterResultHost GetAdapterCounterAdapterResultHost(List<PerformanceCounterValue> counterValues)
	{
		counterValues.Sort((PerformanceCounterValue x, PerformanceCounterValue y) => string.Compare(x.InstanceName, y.InstanceName, StringComparison.OrdinalIgnoreCase));
		Dictionary<string, Dictionary<string, float>> dictionary = new Dictionary<string, Dictionary<string, float>>();
		foreach (PerformanceCounterValue counterValue in counterValues)
		{
			if (!dictionary.TryGetValue(counterValue.InstanceName, out var value))
			{
				value = new Dictionary<string, float>();
				dictionary.Add(counterValue.InstanceName, value);
			}
			value.Add(counterValue.CounterName, counterValue.CalculatedValue);
		}
		AdapterResultHost adapterResultHost = new AdapterResultHost();
		foreach (KeyValuePair<string, Dictionary<string, float>> item in dictionary)
		{
			LiveDataAdapterResult liveDataAdapterResult = new LiveDataAdapterResult();
			item.Value.TryGetValue("Bytes Sent/sec", out var value2);
			item.Value.TryGetValue("Bytes Received/sec", out var value3);
			item.Value.TryGetValue("Current Bandwidth", out var value4);
			float num = value2 + value3;
			float num2 = ((value4 != 0f) ? (num * 8f * 100f / value4) : 0f);
			liveDataAdapterResult.InstanceName = item.Key;
			Unit[] units = new Unit[3]
			{
				Unit.BytesSec,
				Unit.KBSec,
				Unit.MBSec
			};
			Unit[] units2 = new Unit[4]
			{
				Unit.bps,
				Unit.Kbps,
				Unit.Mbps,
				Unit.Gbps
			};
			(liveDataAdapterResult.SentSec, liveDataAdapterResult.SentSecUnit) = DataUnitUtility.GetCalculatedUnit(value2, units, 999f, 1024u);
			(liveDataAdapterResult.ReceivedSec, liveDataAdapterResult.ReceivedSecUnit) = DataUnitUtility.GetCalculatedUnit(value3, units, 999f, 1024u);
			(liveDataAdapterResult.Total, liveDataAdapterResult.TotalUnit) = DataUnitUtility.GetCalculatedUnit(num, units, 999f, 1024u);
			if (value4 == 0f)
			{
				liveDataAdapterResult.Utilization = 0f;
				liveDataAdapterResult.UtilizationUnit = Unit.Unknown;
				liveDataAdapterResult.CurrentBandwidth = 0f;
				liveDataAdapterResult.CurrentBandwidthUnit = Unit.Unknown;
			}
			else
			{
				(float, Unit) calculatedUnit = DataUnitUtility.GetCalculatedUnit(value4, units2, 999f, 1000u);
				num2 = ((value4 != 0f) ? (num * 8f * 100f / value4) : 0f);
				liveDataAdapterResult.Utilization = num2;
				liveDataAdapterResult.UtilizationUnit = Unit.Percent;
				(liveDataAdapterResult.CurrentBandwidth, liveDataAdapterResult.CurrentBandwidthUnit) = calculatedUnit;
			}
			adapterResultHost.Add(liveDataAdapterResult.InstanceName, liveDataAdapterResult);
		}
		return adapterResultHost;
	}
}
