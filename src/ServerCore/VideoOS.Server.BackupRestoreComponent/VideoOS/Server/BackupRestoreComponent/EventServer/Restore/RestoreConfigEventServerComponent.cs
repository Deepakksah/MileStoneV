using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Xml;
using VideoOS.Common.Services;
using VideoOS.Management.Common;
using VideoOS.Server.BackupRestoreComponent.EventServer.Intercommunication;
using VideoOS.Utilities.Network;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Restore;

public class RestoreConfigEventServerComponent : IConfigRestore
{
	private const string _componentName = "EventServer";

	private RegisteredService _registeredService;

	private IntercommunicationCommandAgent _intercommunicationCommandAgent;

	private string[] _serviceUsers;

	public string ComponentName => "EventServer";

	private bool IsEventServerNotRegisteredOrOnLocalMachine
	{
		get
		{
			if (_registeredService != null)
			{
				return _registeredService.Addresses.Any((ServerAddress address) => IsAddressOnThisMachine(address.URL));
			}
			return true;
		}
	}

	public RestoreConfigEventServerComponent()
	{
		_intercommunicationCommandAgent = new IntercommunicationCommandAgent(ComponentName);
	}

	private static string ExtractVersionFromCustomData(string customData)
	{
		string result = null;
		if (!string.IsNullOrEmpty(customData))
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(customData);
				XmlNode xmlNode = xmlDocument["EventServer"];
				if (xmlNode != null && xmlNode.HasChildNodes)
				{
					XmlNode xmlNode2 = xmlNode["Version"];
					if (xmlNode2 != null && xmlNode2.HasChildNodes)
					{
						result = xmlNode2.InnerText;
					}
				}
			}
			catch (XmlException)
			{
			}
		}
		return result;
	}

	private static RegisteredService GetRegisteredService(IList<RegisteredService> registeredServices)
	{
		if (registeredServices != null)
		{
			foreach (RegisteredService registeredService in registeredServices)
			{
				if (registeredService.TypeId.Equals(ServiceTypes.ServiceType_EventServerService) && registeredService.Enabled)
				{
					return registeredService;
				}
			}
		}
		return null;
	}

	public RestoreValidationResult ValidateForRestore(string configData, IList<RegisteredService> registeredServices, ReportComponentStatus reportStatus, AbortRequested abortRequested)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		reportStatus(ComponentName, 0);
		_registeredService = GetRegisteredService(registeredServices);
		if (_registeredService != null)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			string text = ExtractVersionFromCustomData(_registeredService.CustomData);
			string text2 = ExtractVersionFromCustomData(configData);
			Regex regex = new Regex("(?<majorversion>\\d*).(?<minorversion>\\d*)(?<revision>[A-Za-z])");
			if (!string.IsNullOrEmpty(text))
			{
				Match match = regex.Match(text);
				num = int.Parse(match.Groups["majorversion"].Value.Trim(), CultureInfo.InvariantCulture);
				num2 = int.Parse(match.Groups["minorversion"].Value.Trim(), CultureInfo.InvariantCulture);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				Match match2 = regex.Match(text2);
				num3 = int.Parse(match2.Groups["majorversion"].Value.Trim(), CultureInfo.InvariantCulture);
				num4 = int.Parse(match2.Groups["minorversion"].Value.Trim(), CultureInfo.InvariantCulture);
			}
			if (num3 == 0 || num3 > num || (num3 == num && num4 > num2))
			{
				return RestoreValidationResult.CreateErrorResult(RestoreConfigError.EventServerDataIncompatible, "Event server validation failed because the backup data is incompatible.", null);
			}
			reportStatus(ComponentName, 50);
			string serviceAccount = _intercommunicationCommandAgent.GetServiceAccount(_registeredService.Addresses);
			if (string.IsNullOrEmpty(serviceAccount))
			{
				return RestoreValidationResult.CreateErrorResult(RestoreConfigError.EventServerNotRunning, "Error communicating with the event server, make sure it is running.", null);
			}
			_serviceUsers = new string[1] { serviceAccount };
		}
		reportStatus(ComponentName, 100);
		return RestoreValidationResult.CreateSuccessResult();
	}

	public RestoreComponentResult Restore(string path, ReportComponentStatus reportStatus, AbortRequested abortRequested)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		reportStatus(ComponentName, 100);
		return RestoreComponentResult.CreateSuccessResult();
	}

	public bool StopServices(ReportComponentStatus reportStatus)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		reportStatus(ComponentName, 100);
		return true;
	}

	public bool StartServices(ReportComponentStatus reportStatus)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		reportStatus(ComponentName, 0);
		bool result = IsEventServerNotRegisteredOrOnLocalMachine || _intercommunicationCommandAgent.SendReloadConfiguration(_registeredService.Addresses, reportStatus);
		reportStatus(ComponentName, 100);
		return result;
	}

	private static bool IsAddressOnThisMachine(string url)
	{
		return IPHelper.DnsGetHostEntry(new Uri(url).Host).HostName.Equals(Dns.GetHostEntry("").HostName, StringComparison.OrdinalIgnoreCase);
	}

	public string[] GetServiceAccounts(IList<RegisteredService> registeredServices)
	{
		return _serviceUsers;
	}
}
