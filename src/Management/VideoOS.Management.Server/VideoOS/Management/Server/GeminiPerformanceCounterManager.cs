using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace VideoOS.Management.Server;

public static class GeminiPerformanceCounterManager
{
	private static readonly ConcurrentDictionary<Guid, GeminiPerformanceCounterSet> CounterSets;

	private static readonly ConcurrentDictionary<string, KeyValuePair<Guid, int>> CounterUris;

	static GeminiPerformanceCounterManager()
	{
		CounterSets = new ConcurrentDictionary<Guid, GeminiPerformanceCounterSet>();
		CounterUris = new ConcurrentDictionary<string, KeyValuePair<Guid, int>>(StringComparer.OrdinalIgnoreCase);
		Initialize();
	}

	private static void Initialize()
	{
		string[] files = Directory.GetFiles(AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory, "*PerfCounters.man");
		foreach (string uri in files)
		{
			try
			{
				XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(new NameTable());
				xmlNamespaceManager.AddNamespace("e", "http://schemas.microsoft.com/win/2004/08/events");
				xmlNamespaceManager.AddNamespace("c", "http://schemas.microsoft.com/win/2005/12/counters");
				foreach (XElement item in XDocument.Load(uri).XPathSelectElements("/e:instrumentationManifest/e:instrumentation/c:counters/c:provider", xmlNamespaceManager))
				{
					Guid providerGuid = Guid.Parse(item.Attribute("providerGuid").Value);
					foreach (XElement item2 in item.XPathSelectElements("c:counterSet", xmlNamespaceManager))
					{
						Guid counterSetGuid = Guid.Parse(item2.Attribute("guid").Value);
						string value = item2.Attribute("name").Value;
						string value2 = item2.Attribute("instances").Value;
						List<GeminiPerformanceCounterInfo> list = new List<GeminiPerformanceCounterInfo>();
						foreach (XElement item3 in item2.XPathSelectElements("c:counter", xmlNamespaceManager))
						{
							int id = int.Parse(item3.Attribute("id").Value, CultureInfo.InvariantCulture);
							string value3 = item3.Attribute("name").Value;
							string value4 = item3.Attribute("uri").Value;
							string value5 = item3.Attribute("type").Value;
							list.Add(new GeminiPerformanceCounterInfo(id, value3, ParseCounterType(value5), value4));
						}
						CounterSetInstanceType counterSetInstanceType = (CounterSetInstanceType)Enum.Parse(typeof(CounterSetInstanceType), value2, ignoreCase: true);
						if (counterSetInstanceType != CounterSetInstanceType.GlobalAggregate && counterSetInstanceType != CounterSetInstanceType.MultipleAggregate)
						{
							throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown counter instance type: {0}", value2));
						}
						if (counterSetInstanceType == CounterSetInstanceType.GlobalAggregate)
						{
							RegisterCounterSet(providerGuid, counterSetGuid, value, list, isMultiInstance: false);
						}
						else
						{
							RegisterCounterSet(providerGuid, counterSetGuid, value, list, isMultiInstance: true);
						}
					}
				}
			}
			catch
			{
				GeminiTrace.Error("Error initializing performance counters");
			}
		}
	}

	private static CounterType ParseCounterType(string counterType)
	{
		return counterType switch
		{
			"perf_counter_counter" => CounterType.RateOfCountPerSecond32, 
			"perf_counter_rawcount" => CounterType.RawData32, 
			"perf_average_base" => CounterType.AverageBase, 
			"perf_average_timer" => CounterType.AverageTimer32, 
			_ => throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown counter type '{0}'", counterType)), 
		};
	}

	private static void RegisterCounterSet(Guid providerGuid, Guid counterSetGuid, string counterSetName, IReadOnlyCollection<GeminiPerformanceCounterInfo> counters, bool isMultiInstance)
	{
		try
		{
			GeminiPerformanceCounterSet counterSet = new GeminiPerformanceCounterSet(providerGuid, counterSetGuid, counterSetName, counters, isMultiInstance);
			CounterSets.AddOrUpdate(counterSetGuid, (Guid id) => counterSet, (Guid id, GeminiPerformanceCounterSet c) => counterSet);
			foreach (GeminiPerformanceCounterInfo counter in counters)
			{
				KeyValuePair<Guid, int> reference = new KeyValuePair<Guid, int>(counterSetGuid, counter.Id);
				CounterUris.AddOrUpdate(counter.Uri, (string s) => reference, (string s, KeyValuePair<Guid, int> id) => reference);
			}
		}
		catch
		{
			GeminiTrace.Error("Error registering performance counter set");
		}
	}

	public static GeminiPerformanceCounter GetPerformanceCounter(string counterUri, string instanceName = null)
	{
		if (CounterUris.TryGetValue(counterUri, out var value) && CounterSets.TryGetValue(value.Key, out var value2))
		{
			if (value2.IsMultiInstance)
			{
				if (string.IsNullOrWhiteSpace(instanceName))
				{
					throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'{0}' is a multi-instance counter. Instance name must be specified", counterUri), "instanceName");
				}
				return value2[value.Value, instanceName];
			}
			if (string.IsNullOrWhiteSpace(instanceName))
			{
				return value2[value.Value];
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "'{0}' is not a multi-instance counter. Instance name cannot be specified", counterUri), "instanceName");
		}
		if (CounterSets.Count != 0)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Counter with uri '{0}' has not been registered", counterUri), "counterUri");
		}
		return default(GeminiPerformanceCounter);
	}
}
