using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Dism;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework.IISSetup;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

public class IISSetupPageActions : PageSetupActionsBase
{
	private const string _WWWPUBLISHRegKeyName = "SYSTEM\\CurrentControlSet\\Services\\W3SVC";

	private const string _WWWPUBLISHServiceName = "W3SVC";

	private const string SERVICE_DISABLED = "4";

	private const string SERVICE_AUTOSTART = "2";

	private const string _systemProcess = "System";

	private bool _isCustomMode;

	protected IEnumerable<SiteInfo> Websites { get; private set; }

	public override string PageName => "IISSetupPage";

	public IISSetupPageActions()
	{
		GlobalData.SetValue("IISManagementConsoleInstalled", IsIISManagementConsoleInstalled().ToString(), "IISSetupPageActions()");
	}

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (!parameters.ContainsKey("WebSiteId"))
		{
			parameters["WebSiteId"] = "1";
		}
		if (!parameters.ContainsKey("WebSiteName"))
		{
			throw new ArgumentNullException("Params.IISValues.WebSiteName");
		}
		if (!parameters.ContainsKey("WebSiteAddress"))
		{
			throw new ArgumentNullException("Params.IISValues.WebSiteAddressName");
		}
		if (!parameters.ContainsKey("WebSitePort"))
		{
			throw new ArgumentNullException("Params.IISValues.WebSitePortName");
		}
		if (GlobalData.GetValueAsBoolean("IISManagementConsoleInstalled"))
		{
			Websites = WebSiteExplorer.GetWebSites();
		}
		else
		{
			Websites = new List<SiteInfo>();
		}
		if (Websites.Any())
		{
			LogManager.Write("Some websites were found.");
			SiteInfo defaultWebSite = GetDefaultWebSite();
			if (defaultWebSite != null)
			{
				LogManager.Write("Default website was found.");
				parameters["WebSiteId"] = defaultWebSite.Id;
				parameters["WebSiteName"] = defaultWebSite.Name;
				parameters["WebSiteAddress"] = defaultWebSite.Address;
				parameters["WebSitePort"] = defaultWebSite.HttpPort.ToString();
			}
			string value = null;
			if (GlobalData.TryGetValue("INSTALLATION_MODE", out value))
			{
				_isCustomMode = value == "CustomMode";
			}
			else
			{
				_isCustomMode = true;
			}
			SiteInfo websiteToUse = null;
			try
			{
				websiteToUse = SelectWebsiteToUse();
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
				if (WebSiteExplorer.IisInstalled())
				{
					throw new UIFrameworkErrorException(Resources.IISSetup_ErrorConfiguring, enablePreviousButtong: false);
				}
			}
			if (websiteToUse != null)
			{
				return AfterWebSiteSelected(ref websiteToUse);
			}
			return websiteToUse == null;
		}
		SiteInfo websiteToUse2 = CreateFakeSiteInfo(parameters);
		return AfterWebSiteSelected(ref websiteToUse2);
	}

	protected static SiteInfo CreateFakeSiteInfo(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		return new SiteInfo(parameters["WebSiteId"], parameters["WebSiteName"], parameters["WebSiteAddress"], Convert.ToInt32(parameters["WebSitePort"], CultureInfo.InvariantCulture), Convert.ToInt32(parameters["WebSiteSecurePort"], CultureInfo.InvariantCulture), 2);
	}

	private bool IsIISManagementConsoleInstalled()
	{
		DismSession dismSession = null;
		try
		{
			LogManager.Write("IsIISManagementConsoleInstalled(): Checking if IIS-ManagementConsole is installed...", TraceEventType.Information);
			dismSession = new DismSession((ILogger)(object)new LogWrapper());
			dismSession.Initialize(null);
			dismSession.Open();
			FeatureInfo featureInfo = dismSession.QueryFeatureInfo(Feature.IISManagementConsole);
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "IsIISManagementConsoleInstalled(): IIS-ManagementConsole status is {0}", featureInfo.FeatureState), TraceEventType.Information);
			return featureInfo.FeatureState == PackageFeatureState.Installed;
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "IsIISManagementConsoleInstalled(): Failed with exception {0}", arg), TraceEventType.Error);
			throw;
		}
		finally
		{
			dismSession?.Close();
			dismSession?.Shutdown();
		}
	}

	private void CheckPortsAvailable(string id, string port)
	{
		int httpsPort = 443;
		bool flag = true;
		if (GlobalData.GetValueAsBoolean("IISManagementConsoleInstalled"))
		{
			flag = WebSiteExplorer.GetWebsiteHttpsBinding(id, out httpsPort);
		}
		LogManager.Write("Selected website httpsport: " + httpsPort + " needToCheck= " + flag);
		if (flag)
		{
			CheckPort(httpsPort);
		}
		if (int.TryParse(port, out var result))
		{
			CheckPort(result);
		}
	}

	private void CheckPort(int port)
	{
		if (!PortHelper.IsPortAvailable(port, out var processName))
		{
			if (string.Compare(processName, "System", StringComparison.InvariantCultureIgnoreCase) == 0)
			{
				LogManager.Write($"'System' process is using port {port}. Proceed.");
			}
			else if (!string.IsNullOrEmpty(processName))
			{
				throw new UIFrameworkErrorException(string.Format(Resources.IISSetup_ErrorPortInUse, port, processName), enablePreviousButtong: true);
			}
		}
	}

	protected virtual SiteInfo SelectWebsiteToUse()
	{
		SiteInfo siteInfo = null;
		if ((siteInfo = GetDefaultWebSite()) == null)
		{
			LogManager.Write("IISPageSetupAction.BeforeActivatePage() Default web site was not found");
			siteInfo = ((Websites.Count() > 1) ? null : Websites.First());
		}
		if (_isCustomMode)
		{
			if (Websites.Count() > 1)
			{
				LogManager.Write("IISPageSetupAction.BeforeActivatePage() More than one website was found");
				siteInfo = null;
			}
			else
			{
				LogManager.Write("IISPageSetupAction.BeforeActivatePage() First website on the list was selected");
				siteInfo = Websites.First();
			}
		}
		return siteInfo;
	}

	private bool StartServiceIfNotRunning(string serviceName)
	{
		try
		{
			ServiceController serviceController = new ServiceController(serviceName);
			if (serviceController.Status == ServiceControllerStatus.Running || serviceController.Status == ServiceControllerStatus.StartPending)
			{
				LogManager.Write($"The service {serviceName} is running.", TraceEventType.Information);
				return true;
			}
			serviceController.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 0, 0, 250));
			serviceController.Start();
			LogManager.Write($"Start of the service {serviceName} requested.", TraceEventType.Information);
			return serviceController.Status == ServiceControllerStatus.Running || serviceController.Status == ServiceControllerStatus.StartPending;
		}
		catch (InvalidOperationException ex)
		{
			string message = string.Format(Resources.IisService_CantStart, serviceName, ex.Message);
			LogManager.Write($"The service {serviceName} cannot start. Details: {ex.TypeAndMessage()}.", TraceEventType.Error);
			throw new UIFrameworkErrorException(message, enablePreviousButtong: false);
		}
		catch (System.ServiceProcess.TimeoutException)
		{
			string message2 = string.Format(Resources.IisService_CantStartTimeOut, serviceName);
			LogManager.Write($"The service {serviceName} cannot start. Timed out.", TraceEventType.Error);
			throw new UIFrameworkErrorException(message2, enablePreviousButtong: false);
		}
		catch (Exception ex3)
		{
			string message3 = string.Format(Resources.IisService_ErrorOccured, ex3.Message);
			LogManager.Write($"Could not start the service. Details: {ex3.TypeAndMessage()}.", TraceEventType.Error);
			throw new UIFrameworkErrorException(message3, enablePreviousButtong: false);
		}
	}

	private bool CheckIISServicesStartupSetup()
	{
		if (IsIISServicesSet())
		{
			return StartServiceIfNotRunning("W3SVC");
		}
		return false;
	}

	private bool IsIISServicesSet()
	{
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services\\W3SVC", writable: false);
		if (registryKey == null)
		{
			return false;
		}
		object value = registryKey.GetValue("Start");
		if (value == null)
		{
			return false;
		}
		if (value.ToString() == "4")
		{
			throw new UIFrameworkErrorException(Resources.IisService_ErrorDisabled, enablePreviousButtong: true);
		}
		if (value.ToString() != "2")
		{
			throw new UIFrameworkErrorException(Resources.IisService_ErrorNotAutoStart, enablePreviousButtong: true);
		}
		return true;
	}

	private SiteInfo GetDefaultWebSite()
	{
		SiteInfo result = null;
		foreach (SiteInfo website in Websites)
		{
			if (website.Name.Equals("default web site", StringComparison.InvariantCultureIgnoreCase) || website.HttpPort == 80)
			{
				result = website;
				break;
			}
		}
		return result;
	}

	private static bool StartWebSite(SiteInfo siteInfo)
	{
		try
		{
			using (new ShowWaitCursor())
			{
				Process process = Process.Start(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ".\\VideoOS.Installer.StartIIS7.exe"), "\"" + siteInfo.Name + "\"");
				if (process.WaitForExit(60000))
				{
					LogManager.Write($"Start web site {siteInfo.Name} exit code: {process.ExitCode}");
					return process.ExitCode == 0 || siteInfo.State.State == SiteInfo.SiteState.STATETYPE.started;
				}
			}
			return siteInfo.State.State == SiteInfo.SiteState.STATETYPE.started;
		}
		catch (TargetInvocationException ex)
		{
			bool flag = false;
			TCPUDPConnections tCPUDPConnections = new TCPUDPConnections();
			tCPUDPConnections.GetTcpConnections();
			for (int i = 0; i < tCPUDPConnections.Count; i++)
			{
				if (tCPUDPConnections[i].Local != null && tCPUDPConnections[i].Local.Port == siteInfo.HttpPort && Utils.GetProcessNameByPID(tCPUDPConnections[i].PID) != "inetinfo")
				{
					flag = true;
					throw new UIFrameworkErrorException($"{Resources.IISSetup_ErrorUnableStartWebSite + Environment.NewLine}{Resources.IISSetup_MessageUnableStartServiceSubLine + Environment.NewLine}{string.Format(Resources.IISSetup_MessageProcessName, Utils.GetProcessNameByPID(tCPUDPConnections[i].PID)) + Environment.NewLine + string.Format(Resources.IISSetup_MessageFilename, Utils.GetProcessMainModuleFileNameByPID(tCPUDPConnections[i].PID)) + Environment.NewLine + Resources.IISSetup_MessageResolveStartProcess}", enablePreviousButtong: true);
				}
			}
			if (!flag)
			{
				throw new UIFrameworkErrorException($"{Resources.IISSSetup_SetupErrorHeadLine + Environment.NewLine}{Resources.IISSetup_ErrorUnableStartWebSite + Environment.NewLine}{ex.Message}", enablePreviousButtong: true);
			}
			return false;
		}
		catch (Exception ex2)
		{
			throw new UIFrameworkErrorException($"{Resources.IISSSetup_SetupErrorHeadLine + Environment.NewLine}{Resources.IISSetup_ErrorUnableStartWebSite + Environment.NewLine}{ex2.Message}", enablePreviousButtong: true);
		}
	}

	public virtual bool AfterWebSiteSelected(ref SiteInfo websiteToUse)
	{
		CheckPortsAvailable(websiteToUse.Id, websiteToUse.HttpPort.ToString());
		if (websiteToUse.State.State != SiteInfo.SiteState.STATETYPE.started && !StartWebSite(websiteToUse))
		{
			LogManager.Write("IISPageSetupAction.AfterWebsiteSelected() Failed to start the website " + websiteToUse.Id);
			return true;
		}
		if (websiteToUse.HttpsPort == 0)
		{
			websiteToUse = new SiteInfo(websiteToUse.Id, websiteToUse.Name, websiteToUse.Address, websiteToUse.HttpPort, 443, (int)websiteToUse.State.State);
		}
		if (websiteToUse.HttpPort == 0)
		{
			websiteToUse = new SiteInfo(websiteToUse.Id, websiteToUse.Name, websiteToUse.Address, 80, websiteToUse.HttpsPort, (int)websiteToUse.State.State);
		}
		UpdateIisValues(websiteToUse, "IISPageSetup.AfterWebSiteSelected()");
		return false;
	}

	protected static void UpdateIisValues(SiteInfo websiteToUse, string caller)
	{
		if (websiteToUse == null)
		{
			throw new ArgumentNullException("websiteToUse");
		}
		GlobalData.SetValue("WebSiteId", websiteToUse.Id, caller);
		GlobalData.SetValue("WebSiteName", websiteToUse.Name, caller);
		GlobalData.SetValue("WebSiteAddress", websiteToUse.Address, caller);
		GlobalData.SetValue("WebSitePort", websiteToUse.HttpPort.ToString(CultureInfo.InvariantCulture), caller);
		GlobalData.SetValue("WebSiteSecurePort", websiteToUse.HttpsPort.ToString(CultureInfo.InvariantCulture), caller);
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		CheckIISServicesStartupSetup();
		Dictionary<string, string> dictionary = GlobalData.ToDict();
		if (ValidateIISParameters(dictionary, parameters))
		{
			dictionary = GlobalData.ToDict();
		}
		CheckPortsAvailable(dictionary["WebSiteId"], dictionary["WebSitePort"]);
		SiteInfo siteInfo = new SiteInfo(dictionary["WebSiteId"], dictionary["WebSiteName"], dictionary["WebSiteAddress"], int.Parse(dictionary["WebSitePort"]), int.Parse(dictionary["WebSiteSecurePort"]), int.Parse(dictionary["ServerState"]));
		if (siteInfo.State.State != SiteInfo.SiteState.STATETYPE.started && !StartWebSite(siteInfo))
		{
			return false;
		}
		return true;
	}

	private bool ValidateIISParameters(Dictionary<string, string> settings, Dictionary<string, string> parameters)
	{
		bool flag = false;
		flag = ValidateParameter(settings, parameters, "WebSiteId") || flag;
		flag = ValidateParameter(settings, parameters, "WebSiteName") || flag;
		flag = ValidateParameter(settings, parameters, "WebSiteAddress") || flag;
		flag = ValidateParameter(settings, parameters, "WebSitePort") || flag;
		return (!parameters.ContainsKey("ServerState")) ? (ValidateParameter(settings, parameters, "ServerState", 2.ToString()) || flag) : (ValidateParameter(settings, parameters, "ServerState") || flag);
	}

	private bool ValidateParameter(Dictionary<string, string> settings, Dictionary<string, string> parameters, string paramName)
	{
		return ValidateParameter(settings, parameters, paramName, null);
	}

	private bool ValidateParameter(Dictionary<string, string> settings, Dictionary<string, string> parameters, string paramName, string paramValue)
	{
		if (!settings.ContainsKey(paramName))
		{
			if (parameters.ContainsKey(paramName))
			{
				SetParameter(paramName, parameters[paramName]);
				return true;
			}
			if (paramValue != null)
			{
				SetParameter(paramName, paramValue);
				return true;
			}
			throw new ArgumentNullException(paramName);
		}
		return false;
	}

	private bool SetParameter(string paramName, string paramValue)
	{
		GlobalData.SetValue(paramName, paramValue, "IISSetupPageAction.BeforeLeavePage()");
		return true;
	}
}
