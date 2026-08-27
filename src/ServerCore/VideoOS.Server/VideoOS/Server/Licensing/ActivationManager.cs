#define TRACE
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;
using VideoOS.Management.Common;
using VideoOS.Server.DataComponents;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Timers;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server.Licensing;

public class ActivationManager : IDisposable, IActivationManager
{
	public sealed class ActivationResult
	{
		private string _statusCode;

		private string _errorCode;

		public string StatusCode
		{
			[DebuggerStepThrough]
			get
			{
				return _statusCode;
			}
			[DebuggerStepThrough]
			set
			{
				_statusCode = value;
			}
		}

		public string ErrorCode
		{
			[DebuggerStepThrough]
			get
			{
				return _errorCode;
			}
			[DebuggerStepThrough]
			set
			{
				_errorCode = value;
			}
		}

		public ActivationResult()
		{
			_statusCode = "Success";
		}

		public void LoadXml(string xml)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("/ActivationResult/@status");
			if (xmlNode == null)
			{
				throw new FormatException("status attribute is missing.");
			}
			_statusCode = xmlNode.InnerText;
			XmlNode xmlNode2 = xmlDocument.SelectSingleNode("/ActivationResult/@error");
			if (xmlNode2 == null || xmlNode2.Value.Length == 0)
			{
				_errorCode = null;
			}
			else
			{
				_errorCode = xmlNode2.Value;
			}
		}
	}

	private readonly ILicenseManager _manager;

	private IServerEnvironment _serverEnvironment;

	private readonly Action _automaticActivation;

	private readonly Action<string, string> _manualActivation;

	private readonly TimeSpan _activationDelay;

	private readonly TimeSpan _activationRetryDelay;

	private ITimer _timer;

	private readonly object _lock = new object();

	private bool _automaticActivationFailed;

	protected volatile LicenseException _lastAutomaticActivationException;

	private volatile bool _systemActivation;

	public bool SystemActivation
	{
		get
		{
			return _systemActivation;
		}
		set
		{
			_systemActivation = value;
		}
	}

	public bool IsActivationQueued { get; private set; }

	public DateTime NextActivation { get; private set; }

	public Exception LastAutomaticActivationError
	{
		get
		{
			return _lastAutomaticActivationException;
		}
		protected set
		{
			_lastAutomaticActivationException = value as LicenseException;
		}
	}

	public event EventHandler AutomaticActivationFailedEventHandler;

	public ActivationManager(ILicenseManager manager, IServerConfig serverConfig, IServerEnvironment serverEnvironment)
		: this(manager, null, null, serverConfig.AutomaticActivationDelaySec * 1000, serverConfig.AutomaticActivationRetryDelaySec * 1000, new ThreadingTimerWrapper(), serverEnvironment)
	{
		_automaticActivation = DoAutomaticLicenseActivation;
		_manualActivation = ManualActivateLicense;
	}

	public ActivationManager(ILicenseManager manager, Action automaticActivation, Action<string, string> manualActivation, int activationDelayMs, int activationRetryDelayMs, ITimer timer, IServerEnvironment serverEnvironment)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("manager");
		}
		_manager = manager;
		_serverEnvironment = serverEnvironment;
		_automaticActivation = automaticActivation;
		_manualActivation = manualActivation;
		_activationDelay = TimeSpan.FromMilliseconds(activationDelayMs);
		_activationRetryDelay = TimeSpan.FromMilliseconds(activationRetryDelayMs);
		_timer = timer;
		_timer.Elapsed += TimerCallback;
		_systemActivation = _manager.IsFeatureLicensed("SystemActivated");
	}

	public void ActivateLicense(string userName, string password)
	{
		_manualActivation(userName, password);
		LastAutomaticActivationError = null;
		lock (_lock)
		{
			_automaticActivationFailed = false;
			IsActivationQueued = false;
			_timer.StopTimer();
			NextActivation = DateTime.MinValue;
		}
	}

	private void ManualActivateLicense(string userName, string password)
	{
		ILicenseActivationService licenseActivationService = LicenseFactory.RetrieveLicenseActivationService(_serverEnvironment.Server.Logger, GetOnlineActivationUrl(), ServerConfig.Instance.OnlineActivationRequestTimeout);
		byte[] array;
		try
		{
			EventTracer.Log.TraceMethodStart("CreateServerLicenseRequest");
			array = _manager.CreateServerLicenseRequest(ServerConfig.Instance.IncludeMacAddressInLicenseRequest);
			EventTracer.Log.TraceMethodStop("CreateServerLicenseRequest");
		}
		catch (Exception ex)
		{
			_serverEnvironment.Server.Logger.LogWarning("Online activation - GetLicenseRequestFromConfiguration failed. Exception {ExceptionMessage}", ex.Message);
			throw new LicenseException("Creating license request failed.", LicenseErrorNumber.GeneralLicenseError, ex);
		}
		_serverEnvironment.Server.Logger.LogInformation("Online activation - Created Service License Request, Data Size={DataSize}", array.Length);
		byte[] array2;
		try
		{
			_serverEnvironment.Server.Logger.LogInformation("Online activation - Acquire License Start");
			EventTracer.Log.TraceMethodStart("Acquire License");
			array2 = licenseActivationService.AcquireLicense(userName, password, array);
			EventTracer.Log.TraceMethodStop("Acquire License");
		}
		catch (LicenseException ex2)
		{
			_serverEnvironment.Server.Logger.LogError("Online activation - CreateLicense failed. Exception {Exception}", ex2);
			throw;
		}
		catch (WebException ex3)
		{
			_serverEnvironment.Server.Logger.LogError("Online activation - CreateLicense failed. Exception {Exception}", ex3);
			throw new LicenseException("Unable to communicate with the license activation server.", LicenseErrorNumber.LicenseActivationServerCommunicationFailed);
		}
		catch (Exception ex4)
		{
			_serverEnvironment.Server.Logger.LogError("Online activation - CreateLicense failed. Exception {Exception}", ex4);
			throw new LicenseException("Could not obtain a new license.", LicenseErrorNumber.GeneralLicenseError);
		}
		finally
		{
			_serverEnvironment.Server.Logger.LogInformation("Online activation - Acquire License End");
		}
		_serverEnvironment.Server.Logger.LogInformation("Import License - Start");
		ILicenseLoader licenseLoader = LicenseFactory.CreateLicenseLoader();
		ILicenseDocument licenseDocument = licenseLoader.ImportLicenseFile(array2);
		_serverEnvironment.Server.Logger.LogInformation("Update License by Configuration Manager");
		LicensingEventTracer.Logger.TraceMethodStart("Import License");
		Server.Instance.UpdateLicenseThroughUpdateConfigurationManagerCommand(array2, licenseDocument.CustomerData, licenseDocument.Sites);
		LicensingEventTracer.Logger.TraceMethodStop("Import License");
		_serverEnvironment.Server.Logger.LogInformation("Import License - End");
	}

	public void QueueAutomaticActivation()
	{
		try
		{
			if (!QueryAutomaticLicenseActivationEnabled())
			{
				return;
			}
			lock (_lock)
			{
				if (!IsActivationQueued)
				{
					IsActivationQueued = true;
					_timer.StartTimer(_activationDelay, Timeout.InfiniteTimeSpan);
					NextActivation = DateTime.UtcNow + _activationDelay;
				}
			}
		}
		catch (DatabaseException ex)
		{
			_serverEnvironment.Server.Logger.LogWarning("AutomaticLicenseActivation: Database lookup failed while queuing automatic license activation; Exception: \n{Exception}", ex);
		}
	}

	public static string FormatSystemInformationXml(string slc, string encryptedSlc)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		TextWriter textWriter = null;
		try
		{
			textWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(textWriter);
			textWriter = null;
			xmlTextWriter.WriteStartDocument(standalone: true);
			xmlTextWriter.WriteStartElement("SystemInformation");
			xmlTextWriter.WriteElementString("SLC", slc);
			xmlTextWriter.WriteElementString("EncryptedSLC", encryptedSlc);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndDocument();
		}
		finally
		{
			textWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}

	private Uri GetOnlineActivationUrl()
	{
		Uri uri = null;
		string localMachineRegistryPath = OemResources.Instance.Server.LocalMachineRegistryPath;
		_serverEnvironment.Server.Logger.LogDebug("GetOnlineActivationUrl() - keyName: {KeyName}", localMachineRegistryPath);
		try
		{
			uri = new Uri((string)Registry.GetValue(localMachineRegistryPath, "OnlineActivationUrl", null));
			_serverEnvironment.Server.Logger.LogInformation("GetOnlineActivationUrl() - url: {Url}", uri);
			return uri;
		}
		catch (SecurityException)
		{
		}
		catch (IOException)
		{
		}
		catch (ArgumentException)
		{
		}
		catch (UriFormatException)
		{
			Trace.WriteLine("License server url read from the registry has an incorrect format.");
		}
		uri = OemResources.Instance.Server.OnlineActivationUrl;
		_serverEnvironment.Server.Logger.LogInformation("GetOnlineActivationUrl() - url: {Url}", uri);
		return uri;
	}

	public bool QueryAutomaticLicenseActivationEnabled()
	{
		if (_systemActivation)
		{
			return true;
		}
		using ISystemSettingsComponent systemSettingsComponent = _serverEnvironment.DataComponentFactory.CreateSystemSettingsComponent();
		string value = systemSettingsComponent.QuerySystemSettingByName(SettingList.EnableAutomaticActivation);
		return bool.Parse(value);
	}

	private void TimerCallback(object objectState, EventArgs eventArgs)
	{
		try
		{
			if (!QueryAutomaticLicenseActivationEnabled())
			{
				return;
			}
			try
			{
				_automaticActivation();
				lock (_lock)
				{
					IsActivationQueued = false;
					NextActivation = DateTime.MinValue;
					_automaticActivationFailed = false;
				}
				LastAutomaticActivationError = null;
				_serverEnvironment.ServerLogManager.AddLogEntry(LogEntryId.AutomaticActivationSuccess);
			}
			catch (LicenseException lastAutomaticActivationError)
			{
				LastAutomaticActivationError = lastAutomaticActivationError;
				lock (_lock)
				{
					if (!_automaticActivationFailed && this.AutomaticActivationFailedEventHandler != null)
					{
						this.AutomaticActivationFailedEventHandler(this, new EventArgs());
					}
					_automaticActivationFailed = true;
					_serverEnvironment.ServerLogManager.AddLogEntry(LogEntryId.AutomaticActivationFailed);
					IsActivationQueued = true;
					_timer.StartTimer(_activationRetryDelay, Timeout.InfiniteTimeSpan);
					NextActivation = DateTime.UtcNow + _activationRetryDelay;
				}
			}
		}
		catch (DatabaseException ex)
		{
			_serverEnvironment.Server.Logger.LogWarning("AutomaticLicenseActivation: Database lookup failed while executing automatic license activation; Exception: \n{Exception}", ex);
		}
	}

	private void DoAutomaticLicenseActivation()
	{
		string text = null;
		string text2 = null;
		if (_systemActivation)
		{
			text = "MilestonePBUUser";
			text2 = "ekyo91dlekyQQbp1zT";
		}
		else
		{
			string value = null;
			using (ISystemSettingsComponent systemSettingsComponent = _serverEnvironment.DataComponentFactory.CreateSystemSettingsComponent())
			{
				value = systemSettingsComponent.QuerySystemSettingByName(SettingList.ActivationCredentials);
			}
			string xml = Server.Instance.CorporateEncryptionSymmetric.DecryptFromBase64String(value);
			ActivationCredentials activationCredentials = ActivationCredentials.Parse(xml);
			if (!string.IsNullOrEmpty(activationCredentials.UserName) && !string.IsNullOrEmpty(activationCredentials.Password))
			{
				text = activationCredentials.UserName;
				text2 = activationCredentials.Password;
			}
		}
		if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text2))
		{
			ActivateLicense(text, text2);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (disposing && _timer != null)
		{
			_timer.Elapsed -= TimerCallback;
			_timer.Dispose();
			_timer = null;
		}
	}
}
