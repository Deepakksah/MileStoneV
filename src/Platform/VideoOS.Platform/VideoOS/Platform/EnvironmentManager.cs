using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Media;
using System.Xml;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.License;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.UI;
using VideoOS.Platform.Util;

namespace VideoOS.Platform;

public abstract class EnvironmentManager
{
	public enum MessageReceiverHint
	{
		None,
		AvoidAsync
	}

	private static EnvironmentManager _instance;

	private ICredentials _credentialCache;

	private string _authType = "Negotiate";

	private NetworkCredential _networkCredential = CredentialCache.DefaultNetworkCredentials;

	private Dictionary<string, string> _environmentOptions = new Dictionary<string, string>();

	private bool _hasMipSettingsBeenLoaded;

	private VideoOS.Platform.License.LicenseManager _licenseManager;

	public static EnvironmentManager Instance
	{
		[DebuggerStepThrough]
		get
		{
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public virtual Mode Mode { get; set; }

	public virtual bool SmartClientInOfflineMode { get; set; }

	public virtual string SmartClientOfflineDirectory { get; set; }

	public abstract bool ApplicationLoggedOn { get; }

	public virtual EnvironmentType EnvironmentType => EnvironmentType.Administration;

	public static bool UsingUserContext { get; set; }

	public virtual string EnvironmentProduct => "Not Implemented yet";

	public virtual SystemLicense SystemLicense => new SystemLicense
	{
		SLC = "AAA-BBBB-CCCC",
		Expire = DateTime.MaxValue,
		GracePeriod = 30,
		Issued = DateTime.MinValue
	};

	public virtual VideoOS.Platform.License.LicenseManager LicenseManager
	{
		get
		{
			if (_licenseManager == null)
			{
				_licenseManager = new VideoOS.Platform.License.LicenseManager();
			}
			return _licenseManager;
		}
	}

	public virtual Dictionary<string, string> EnvironmentOptions
	{
		get
		{
			if (!_hasMipSettingsBeenLoaded)
			{
				InitializeMIPSettings();
				_hasMipSettingsBeenLoaded = true;
			}
			return _environmentOptions;
		}
	}

	public virtual FQID CurrentSite => Configuration.Instance.ServerFQID;

	public virtual FQID MasterSite => Configuration.Instance.ServerFQID;

	[Obsolete("From 2022 R1, this property may be null when the user is using OAuth 2.0 tokens. Try always to use the LoginSettings class instead and check the IsOAuthIdentity == false, before using the NetworkCredential there.")]
	public virtual NetworkCredential LoginNetworkCredential
	{
		get
		{
			return _networkCredential;
		}
		set
		{
			SecurityAccess.Logoff();
			_networkCredential = value;
			_credentialCache = null;
		}
	}

	public virtual string LoginAuthType
	{
		get
		{
			return _authType;
		}
		set
		{
			_authType = value;
		}
	}

	public virtual ICredentials LoginCredentialCache
	{
		get
		{
			return _credentialCache;
		}
		set
		{
			_credentialCache = value;
		}
	}

	public abstract bool TraceFunctionCalls { get; set; }

	public bool TraceMessageCommunication { get; set; }

	public bool TraceSendDetails { get; set; }

	public bool TracePlaybackDetails { get; set; }

	public bool MeasureSendPostHandlers { get; set; }

	public bool DebugLoggingEnabled { get; set; }

	public bool DebugTelemetryLoggingEnabled { get; set; }

	public virtual bool EnableConfigurationChangedService
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual List<PluginDefinition> AllPluginDefinitions
	{
		get
		{
			ArrayList pluginDefinitions = PluginManager.GetPluginDefinitions();
			List<PluginDefinition> list = new List<PluginDefinition>();
			foreach (PluginDefinition item in pluginDefinitions)
			{
				list.Add(item);
			}
			return list;
		}
	}

	public abstract List<string> MessageIdList { get; }

	public bool PluginInitializationInProgress { get; set; }

	public event EventHandler EnvironmentOptionsChangedEvent;

	public void FireEnvironmentOptionsChangedEvent()
	{
		if (this.EnvironmentOptionsChangedEvent != null)
		{
			this.EnvironmentOptionsChangedEvent(this, new EventArgs());
		}
	}

	protected virtual void Init()
	{
		Init(null);
	}

	protected virtual void Init(IDictionary<string, Color> brandColors)
	{
		if (EnvironmentType == EnvironmentType.SmartClient || EnvironmentType == EnvironmentType.Administration)
		{
			ThemeManager.Init(brandColors);
		}
	}

	public virtual bool HasCapability(string capability)
	{
		return false;
	}

	public virtual Item GetSiteItem(Guid id)
	{
		return Configuration.Instance.GetItem(new FQID(new ServerId("XP", "localhost", 80, id), FolderType.No, Kind.Server));
	}

	public virtual Item GetSiteItem(FQID fqid)
	{
		return Configuration.Instance.GetItem(fqid);
	}

	public virtual void Log(bool error, string where, [Localizable(false)] string text)
	{
		Log("", error, where, text, null);
	}

	public virtual void Log(bool error, string where, [Localizable(false)] string text, object[] args)
	{
		Log("", error, where, text, args);
	}

	public virtual void Log(string nameOfClass, bool error, string where, [Localizable(false)] string text, object[] args)
	{
	}

	public abstract void ExceptionHandler(string where, Exception exception);

	public abstract void ExceptionHandler(string nameOfClass, string where, Exception exception);

	public abstract void ExceptionDialog(string where, Exception exception);

	public abstract void ExceptionDialog(string nameOfClass, string where, Exception exception);

	public abstract object RegisterReceiver(MessageReceiver messageReceiver, MessageFilter messageFilter);

	public virtual object RegisterReceiver(MessageReceiver messageReceiver, MessageFilter messageFilter, MessageReceiverHint hint)
	{
		return RegisterReceiver(messageReceiver, messageFilter);
	}

	public abstract void UnRegisterReceiver(object registeredReceiver);

	public abstract Collection<object> SendMessage(Message message, FQID destination, FQID sender);

	public virtual Collection<object> SendMessage(Message message, FQID destination)
	{
		return SendMessage(message, destination, null);
	}

	public virtual Collection<object> SendMessage(Message message)
	{
		return SendMessage(message, null, null);
	}

	public abstract void PostMessage(Message message, FQID destination, FQID sender);

	public virtual void PostMessage(Message message, FQID destination)
	{
		PostMessage(message, destination, null);
	}

	public virtual void PostMessage(Message message)
	{
		PostMessage(message, null, null);
	}

	private void InitializeMIPSettings()
	{
		try
		{
			string text = Path.Combine(Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().Location).Path)), "MIPSettings.xml");
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				if (SetBoolSetting(xmlDocument, "Settings/TraceFunctionCalls", out var result))
				{
					TraceFunctionCalls = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/TraceMessageCommunication", out result))
				{
					TraceMessageCommunication = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/TraceSendDetails", out result))
				{
					TraceSendDetails = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/TracePlaybackDetails", out result))
				{
					TracePlaybackDetails = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/TraceItemConfigurationCache", out result))
				{
					Configuration.TraceItemConfigurationCache = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/TraceConfigurationApi", out result))
				{
					Factory.TraceCommunication = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/MeasureSendPostHandlers", out result))
				{
					MeasureSendPostHandlers = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/RestAPIDebugLogging", out result))
				{
					ConfigurationApiManager.EnableDebugLogging = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/DebugLogging", out result))
				{
					DebugLoggingEnabled = result;
				}
				if (SetBoolSetting(xmlDocument, "Settings/TelemetryDebugLogging", out result))
				{
					DebugTelemetryLoggingEnabled = result;
				}
				if (SetIntSetting(xmlDocument, "Settings/ConnectionCheckTimeout", out var result2))
				{
					_environmentOptions["ConnectionCheckTimeout"] = result2.ToString();
				}
			}
		}
		catch (Exception)
		{
		}
	}

	private static bool SetBoolSetting(XmlDocument doc, string nodePath, out bool result)
	{
		XmlNode xmlNode = doc.SelectSingleNode(nodePath);
		if (xmlNode != null && bool.TryParse(xmlNode.InnerText, out result))
		{
			return true;
		}
		result = false;
		return false;
	}

	private static bool SetIntSetting(XmlDocument doc, string nodePath, out int result)
	{
		XmlNode xmlNode = doc.SelectSingleNode(nodePath);
		if (xmlNode != null && int.TryParse(xmlNode.InnerText, out result))
		{
			return true;
		}
		result = 0;
		return false;
	}
}
