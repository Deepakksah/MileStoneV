using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.ReportMachineConfiguration;

namespace VideoOS.DataCollector.Service.PerfCounterConfiguration.Refresh.EventHandlers.RefreshInstalledServices;

public class InstallerServicesService : IInstallerServicesService
{
	private readonly ILogger<InstallerServicesService> _logger;

	private readonly IServiceControlManager _serviceControlManager;

	public InstallerServicesService(IServiceControlManager serviceControlManager, ILogger<InstallerServicesService> logger)
	{
		_serviceControlManager = serviceControlManager;
		_logger = logger;
	}

	public List<PerformanceCounterValue> UpdateInstallerServices(ConcurrentDictionary<string, string> installedServices)
	{
		List<PerformanceCounterValue> list = new List<PerformanceCounterValue>();
		Dictionary<string, string> installedServicesList = GetInstalledServicesList();
		IEnumerable<KeyValuePair<string, string>> enumerable = from _003C_003Eh__TransparentIdentifier0 in Enumerable.GroupJoin(installedServices, installedServicesList, delegate(KeyValuePair<string, string> o)
			{
				KeyValuePair<string, string> keyValuePair = o;
				return keyValuePair.Key;
			}, delegate(KeyValuePair<string, string> p)
			{
				KeyValuePair<string, string> keyValuePair = p;
				return keyValuePair.Key;
			}, (KeyValuePair<string, string> o, IEnumerable<KeyValuePair<string, string>> t) => new { o, t })
			from od in t.DefaultIfEmpty()
			where od.Key == null
			select o;
		foreach (KeyValuePair<string, string> item in enumerable)
		{
			installedServices.TryRemove(item);
			_logger.LogInformation("Service removed: " + item.Key + " for " + item.Value);
		}
		foreach (KeyValuePair<string, string> item2 in installedServicesList)
		{
			if (!installedServices.ContainsKey(item2.Key))
			{
				int num = ((!_serviceControlManager.IsRunning(item2.Value)) ? 1 : 0);
				list.Add(new PerformanceCounterValue(item2.Key, item2.Value, "Windows Services")
				{
					CalculatedValue = num,
					CollectionTime = DateTime.UtcNow,
					RawValue = num
				});
				installedServices.TryAdd(item2.Key, item2.Value);
				_logger.LogInformation("Service added: " + item2.Key + " for " + item2.Value);
			}
		}
		return list;
	}

	private Dictionary<string, string> GetInstalledServicesList()
	{
		Guid guid = new Guid("E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80");
		try
		{
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
			using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\\\VideoOS\\\\Installation");
			if (registryKey2 != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				string[] valueNames = registryKey2.GetValueNames();
				string[] array = valueNames;
				foreach (string text in array)
				{
					if (guid.Equals(new Guid(text)))
					{
						dictionary.Add(text, GetAPIGatewayURI());
					}
					else if (registryKey2.GetValue(text) is string value)
					{
						dictionary.Add(text, value);
					}
				}
				return dictionary;
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			_logger.LogWarning(ex.Message);
		}
		catch (SecurityException ex2)
		{
			_logger.LogInformation(ex2.Message);
		}
		return new Dictionary<string, string>();
	}

	private string GetAPIGatewayURI()
	{
		try
		{
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
			using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\\\VideoOS\\\\GatewayService");
			if (registryKey2 != null)
			{
				string text = registryKey2.GetValue("ServiceUrl")?.ToString();
				if (text != null)
				{
					return text + ".well-known/uris";
				}
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			_logger.LogWarning(ex.Message);
		}
		catch (SecurityException ex2)
		{
			_logger.LogInformation(ex2.Message);
		}
		return string.Empty;
	}
}
