using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using VideoOS.Log;
using VideoOS.Log.LPRService_en_US;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.LPR.Common.Loggers;

public class Logger : ILogger
{
	private string _locationId;

	private string _locationName;

	private ServerId _managementServerId;

	private string _logServerAddress;

	private object _lock = new object();

	private LPRClient.Client _lprServerLog;

	private readonly string _fullHostName = Dns.GetHostEntry("").HostName;

	public Logger(ServerId managementServerId)
	{
		_locationName = "";
		try
		{
			List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, managementServerId);
			if (registeredServiceUriInfo != null && registeredServiceUriInfo.Count > 0 && registeredServiceUriInfo[0].UriArray != null && registeredServiceUriInfo[0].UriArray.Count > 0 && Uri.TryCreate(registeredServiceUriInfo[0].UriArray[0], UriKind.Absolute, out var result))
			{
				_locationName = result.Host;
				_locationId = registeredServiceUriInfo[0].Instance.ToString();
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "LPR.Logger", string.Format(CultureInfo.InvariantCulture, "Failed to get registered LPR server service {0}. Exception: {1}", managementServerId, ex.Message));
		}
		_managementServerId = managementServerId;
	}

	public void LogAddLicensePlate(string listName, string licensePlate)
	{
		lock (_lock)
		{
			GetLogger()?.LPRListsAdd(GetToken(), string.Empty, _fullHostName, _locationId, _locationName, DateTime.Now, ueSourceType.LPRClient, ueListType.Edit_LPR_license_plate_list, licensePlate, listName);
		}
	}

	public void LogDeleteLicensePlate(string listName, string licensePlate)
	{
		lock (_lock)
		{
			GetLogger()?.LPRListsDelete(GetToken(), string.Empty, _fullHostName, _locationId, _locationName, DateTime.Now, ueSourceType.LPRClient, ueListType.Edit_LPR_license_plate_list, licensePlate, listName);
		}
	}

	public void LogClearLicensePlates(string listName)
	{
		lock (_lock)
		{
			GetLogger()?.LPRListsDelete(GetToken(), string.Empty, _fullHostName, _locationId, _locationName, DateTime.Now, ueSourceType.LPRClient, ueListType.Edit_LPR_license_plate_list, "[All plates]", listName);
		}
	}

	public void LogSearchLicensePlateList(string listName, string licensePlate)
	{
		lock (_lock)
		{
			GetLogger()?.LPRListsSearch(GetToken(), string.Empty, _fullHostName, _locationId, _locationName, DateTime.Now, ueSourceType.LPRClient, ueListType.Edit_LPR_license_plate_list, licensePlate, listName);
		}
	}

	public void LogSearchLicensePlateEvents(string filter)
	{
		lock (_lock)
		{
			GetLogger()?.LPREventSearch(GetToken(), string.Empty, _fullHostName, _locationId, _locationName, DateTime.Now, ueSourceType.LPRClient, ueListType.Search_LPR_events, filter);
		}
	}

	public void Initialize()
	{
		GetLogger();
	}

	public void Close()
	{
		lock (_lock)
		{
			_lprServerLog = null;
		}
	}

	private static bool IsOAuthIdentity(LoginSettings ls)
	{
		PropertyInfo propertyInfo = ls.GetType().GetProperties().FirstOrDefault((PropertyInfo p) => p.Name == "IsOAuthIdentity");
		if (!(propertyInfo != null))
		{
			return false;
		}
		return (bool)propertyInfo.GetValue(ls);
	}

	private LPRClient.Client GetLogger()
	{
		lock (_lock)
		{
			if (_lprServerLog == null)
			{
				string logServerAddress = GetLogServerAddress();
				if (string.IsNullOrEmpty(logServerAddress))
				{
					return null;
				}
				LPRClient.Client client = new LPRClient.Client();
				client.SetServer(logServerAddress);
				LoginSettings loginSettings = LoginSettings.GetLoginSettings(EnvironmentManager.Instance.MasterSite.ServerId);
				if (IsOAuthIdentity(loginSettings))
				{
					client.SetUserId(loginSettings.FullyQualifiedUserName);
					client.SetAuthentication(Authentication.NoAuthentication);
				}
				else if (EnvironmentManager.Instance.LoginNetworkCredential.Equals(CredentialCache.DefaultNetworkCredentials))
				{
					client.SetUserId(string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", Environment.UserDomainName, Environment.UserName));
					client.SetAuthentication(Authentication.NTLMAuthentication);
				}
				else if (!string.IsNullOrEmpty(EnvironmentManager.Instance.LoginNetworkCredential.Domain))
				{
					string text = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", EnvironmentManager.Instance.LoginNetworkCredential.Domain, EnvironmentManager.Instance.LoginNetworkCredential.UserName);
					client.SetUserId(text);
					client.SetAuthentication(Authentication.NTLMAuthentication);
					client.SetCredentials(text, EnvironmentManager.Instance.LoginNetworkCredential.Password);
				}
				else
				{
					client.SetUserId(EnvironmentManager.Instance.LoginNetworkCredential.UserName);
					client.SetAuthentication(Authentication.BasicAuthentication);
					client.SetCredentials(EnvironmentManager.Instance.LoginNetworkCredential.UserName, EnvironmentManager.Instance.LoginNetworkCredential.Password);
				}
				string path = "Milestone\\XProtect LPR";
				string path2 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), path);
				client.SetFileQueuePath(Path.Combine(path2, "LogServiceFileQueue"));
				DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LogServiceMsgs"));
				if (directoryInfo.Exists)
				{
					client.UploadCultureFiles(directoryInfo.FullName, recursive: true, forceupload: true, asynchronous: true);
				}
				else
				{
					client.TrustCultureFiles();
				}
				_lprServerLog = client;
			}
		}
		return _lprServerLog;
	}

	private string GetLogServerAddress()
	{
		lock (_lock)
		{
			if (_logServerAddress == null)
			{
				try
				{
					List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_LogService, _managementServerId);
					if (registeredServiceUriInfo.Count > 0 && registeredServiceUriInfo[0].UriArray.Count > 0)
					{
						_logServerAddress = registeredServiceUriInfo[0].UriArray[0];
					}
				}
				catch (CommunicationMIPException ex)
				{
					EnvironmentManager.Instance.Log(error: true, "LPR.Logger", string.Format(CultureInfo.InvariantCulture, "Failed to get registered log service {0}. Exception: {1}", _managementServerId, ex.Message));
				}
			}
		}
		return _logServerAddress;
	}

	private string GetToken()
	{
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(_managementServerId);
		if (loginSettings != null)
		{
			return loginSettings.Token;
		}
		return "";
	}
}
