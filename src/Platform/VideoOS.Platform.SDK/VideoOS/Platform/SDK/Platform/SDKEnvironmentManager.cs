#define TRACE
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using Microsoft.Extensions.Logging;
using VideoOS.Platform.Client;
using VideoOS.Platform.Common.MessageClient;
using VideoOS.Platform.Common.Telemetry;
using VideoOS.Platform.Messaging;
using VideoOS.Platform.Messaging.Internal;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Server;
using VideoOS.Platform.SDK.Telemetry;
using VideoOS.Platform.UI;
using VideoOS.Platform.Util;
using VideoOS.Telemetry;

namespace VideoOS.Platform.SDK.Platform;

public class SDKEnvironmentManager : EnvironmentManager, ISDKEnvironmentManager
{
	private delegate object ExecuteMessageDelegate(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender, string logMessage);

	private List<RegistreredReceiver> _messageFilter = new List<RegistreredReceiver>();

	private MessageManager _messageManager;

	private bool _applicationLoggedOn;

	private Mode _mode = Mode.ClientLive;

	private SiteInfoManager _siteInfoManager = new SiteInfoManager();

	private ConfigurationChangedService _configurationChangedService;

	private bool _hasTopLevelErrorBeenLogged;

	public static ClientControl ClientControl => ClientControl.Instance;

	bool ISDKEnvironmentManager.ApploggedState
	{
		set
		{
			_applicationLoggedOn = value;
		}
	}

	public Control TopLevelControl
	{
		get
		{
			if (EnvironmentOptions["UseControlForMessaging"] == "Yes" && Application.OpenForms != null && Application.OpenForms.Count > 0)
			{
				return Application.OpenForms[0];
			}
			if (!_hasTopLevelErrorBeenLogged)
			{
				_hasTopLevelErrorBeenLogged = true;
			}
			return null;
		}
	}

	public override Mode Mode
	{
		get
		{
			return _mode;
		}
		set
		{
			if (_mode != value)
			{
				_mode = value;
				PostMessage(new VideoOS.Platform.Messaging.Message(MessageId.System.ModeChangedIndication, value));
			}
		}
	}

	public override bool ApplicationLoggedOn => _applicationLoggedOn;

	public override EnvironmentType EnvironmentType
	{
		get
		{
			if (EnvironmentOptions.ContainsKey("TESTENVIRONMENT") && Enum.TryParse<EnvironmentType>(EnvironmentOptions["TESTENVIRONMENT"], ignoreCase: true, out var result))
			{
				return result;
			}
			return EnvironmentType.Standalone;
		}
	}

	public override string EnvironmentProduct => string.Format(CultureInfo.InvariantCulture, "MIP SDK v{0}.{1}", "25", "3");

	public override bool TraceFunctionCalls { get; set; }

	public override List<PluginDefinition> AllPluginDefinitions => PlatformManager.PlatformContentTypes();

	public override bool EnableConfigurationChangedService
	{
		get
		{
			return _configurationChangedService != null;
		}
		set
		{
			if (value)
			{
				if (_configurationChangedService == null)
				{
					_configurationChangedService = new ConfigurationChangedService();
					_configurationChangedService.CheckInterval = 60L;
					_configurationChangedService.Init();
				}
			}
			else if (_configurationChangedService != null)
			{
				_configurationChangedService.Close();
				_configurationChangedService = null;
			}
		}
	}

	public override FQID CurrentSite => Configuration.Instance.ServerFQID;

	public override FQID MasterSite => Configuration.Instance.ServerFQID;

	public override List<string> MessageIdList
	{
		get
		{
			List<string> list = new List<string>();
			foreach (PluginDefinition item in PlatformManager.PlatformContentTypes())
			{
				List<string> pluginDefinedMessageIds = item.PluginDefinedMessageIds;
				if (pluginDefinedMessageIds != null)
				{
					list.AddRange(pluginDefinedMessageIds);
				}
			}
			Type[] nestedTypes = new MessageId().GetType().GetNestedTypes();
			list.AddRange(FindMessageIds(nestedTypes, ""));
			return list;
		}
	}

	void ISDKEnvironmentManager.SDKEnvironmentManagerInitialize()
	{
		SDKEnvironmentManagerInitialize(multiuser: false);
	}

	public void SDKEnvironmentManagerInitialize(bool multiuser)
	{
		Init();
		EnvironmentManager.UsingUserContext = multiuser;
		Migration.NewEventMigrate = false;
		InternalConfigService.Instance = new SDKInternalConfigService();
		InternalMessagesClientFactory.Instance = new SdkMessagesClientFactory();
		_messageManager = new MessageManager();
		EnvironmentOptions["DualItemHierarchy"] = "Yes";
		EnvironmentOptions["ToolkitFork"] = "Yes";
		EnvironmentOptions["UsePing"] = "No";
		EnvironmentOptions["UseControlForMessaging"] = "Yes";
		EnvironmentOptions["CompanyNameFolder"] = "Milestone";
		EnvironmentOptions["SoftwareDecodingThreads"] = GetDecoderThreadsDefault();
		if (!EnvironmentOptions.ContainsKey("ConnectionCheckTimeout"))
		{
			EnvironmentOptions["ConnectionCheckTimeout"] = ConnectionCheck.DefaultTimeout.ToString();
		}
		EnvironmentOptions["UseRestApiCaching"] = "Yes";
		if (ClientControl.Instance == null)
		{
			TelemetryKeyProvider telemetryKeyProvider = new TelemetryKeyProvider();
			MIPTelemetryEnabler telemetryEnabler = new MIPTelemetryEnabler();
			TelemetryManager telemetryManager = new TelemetryManager((ILogger)(object)new SDKTelemetryLogger(EnvironmentManager.Instance.DebugTelemetryLoggingEnabled), telemetryKeyProvider, telemetryEnabler);
			ClientControl.Instance = new TelemetryClientControl("SDK", telemetryManager);
		}
	}

	public static string GetDecoderThreadsDefault()
	{
		if (System.Environment.Is64BitProcess)
		{
			return "2,2";
		}
		return "2,1";
	}

	void ISDKEnvironmentManager.Logout(ServerId serverId)
	{
		_messageManager.Logout(serverId);
		SecurityAccess.Logoff();
	}

	void ISDKEnvironmentManager.LogoutAll()
	{
		_messageManager.LogoutAll();
		SecurityAccess.Logoff();
	}

	public void InitializeUIFramework(IDictionary<string, Color> brandColors)
	{
		ThemeManager.Init(brandColors);
	}

	public void TelemetryFlush()
	{
		ClientControl.TelemetryClient.FlushWithMaximumWait(600);
	}

	public Item GetSiteItem(Guid id, UserContext userContext)
	{
		SiteInfo site = _siteInfoManager.GetSite(id, userContext);
		if (site != null)
		{
			return new SiteServerItem(new ServerId(site.ServerType, site.ServerUri.DnsSafeHost, site.ServerUri.Port, site.Id), site, _siteInfoManager);
		}
		return null;
	}

	public override Item GetSiteItem(Guid id)
	{
		return GetSiteItem(id, null);
	}

	public override Item GetSiteItem(FQID fqid)
	{
		return GetSiteItem(fqid.ServerId.Id, fqid.ServerId.UserContext);
	}

	public override void Log(bool error, string where, string text, object[] args)
	{
		Log("", error, where, text, args);
	}

	public override void Log(string nameOfClass, bool error, string where, string text, object[] args)
	{
		if (!error && !base.DebugLoggingEnabled)
		{
			return;
		}
		try
		{
			string text2 = string.Format("{0}: {1} ({2})", error ? "Error" : "Debug", where, nameOfClass);
			if (args == null)
			{
				Logger.DebugLog(text2 + ": " + text);
			}
			else
			{
				Logger.DebugLog(string.Format(text2 + ":" + text, args));
			}
		}
		catch (Exception ex)
		{
			Trace.WriteLine("Exception in Log:" + ex.Message);
			if (nameOfClass == null)
			{
				Trace.WriteLine("nameOfClass=null");
			}
			else
			{
				Trace.WriteLine("nameOfClass=" + nameOfClass);
			}
			if (where == null)
			{
				Trace.WriteLine("where=null");
			}
			else
			{
				Trace.WriteLine("where=" + where);
			}
			if (text == null)
			{
				Trace.WriteLine("text=null");
			}
			else
			{
				Trace.WriteLine("text=" + text);
			}
		}
	}

	public override void ExceptionHandler(string where, Exception exception)
	{
		string text = string.Format("{0} ({1})", where, "");
		Log(error: true, where, ExceptionToString(exception), null);
		Trace.WriteLine(where + text + exception.Message + exception.StackTrace);
	}

	public override void ExceptionHandler(string nameOfClass, string where, Exception exception)
	{
		string text = $"{where} ({nameOfClass})";
		Log(nameOfClass, error: true, where, ExceptionToString(exception), null);
		Trace.WriteLine(where + text + exception.Message + exception.StackTrace);
	}

	public override void ExceptionDialog(string where, Exception exception)
	{
		string text = string.Format("{0} ({1})", where, "");
		MessageBox.Show(text + System.Environment.NewLine + exception.Message + System.Environment.NewLine + exception.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
	}

	public override void ExceptionDialog(string nameOfClass, string where, Exception exception)
	{
		string text = $"{where} ({nameOfClass})";
		MessageBox.Show(text + System.Environment.NewLine + exception.Message + System.Environment.NewLine + exception.StackTrace, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Hand);
	}

	public override object RegisterReceiver(MessageReceiver messageReceiver, MessageFilter messageFilter)
	{
		RegistreredReceiver registreredReceiver = new RegistreredReceiver(messageReceiver, messageFilter);
		lock (_messageFilter)
		{
			_messageFilter.Add(registreredReceiver);
		}
		if (messageFilter is MessageIdFilter messageIdFilter && (messageIdFilter.Match(new VideoOS.Platform.Messaging.Message(MessageId.System.LicenseChangedIndication), null, null) || messageIdFilter.Match(new VideoOS.Platform.Messaging.Message("Server.ConfigurationChangedIndication"), null, null) || messageIdFilter.Match(new VideoOS.Platform.Messaging.Message(MessageId.System.SystemConfigurationChangedIndication), null, null)))
		{
			EnvironmentManager.Instance.EnableConfigurationChangedService = true;
		}
		return registreredReceiver;
	}

	public override void UnRegisterReceiver(object registeredReceiver)
	{
		lock (_messageFilter)
		{
			if (registeredReceiver is RegistreredReceiver registreredReceiver && _messageFilter.Contains(registreredReceiver))
			{
				_messageFilter.Remove((RegistreredReceiver)registeredReceiver);
				registreredReceiver.Unregistered = true;
			}
			else
			{
				Log(error: false, "UnRegisterReceiver", "Unable to find object to UnRegister", null);
			}
		}
	}

	public override Collection<object> SendMessage(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender)
	{
		Collection<object> collection = null;
		if (TopLevelControl != null && !TopLevelControl.IsDisposed && TopLevelControl.InvokeRequired && TopLevelControl.IsHandleCreated && !message.MessageId.EndsWith("Command") && !message.MessageId.EndsWith("Request"))
		{
			return TopLevelControl.Invoke(new ExecuteMessageDelegate(ExecuteMessage), message, destination, sender, "SendMessage") as Collection<object>;
		}
		return ExecuteMessage(message, destination, sender, "SendMessage") as Collection<object>;
	}

	public override void PostMessage(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender)
	{
		if (TopLevelControl != null && !TopLevelControl.IsDisposed && TopLevelControl.IsHandleCreated && !message.MessageId.EndsWith("Command") && !message.MessageId.EndsWith("Request"))
		{
			TopLevelControl.BeginInvoke(new ExecuteMessageDelegate(ExecuteMessage), message, destination, sender, "PostMessage");
		}
		else
		{
			ExecuteMessage(message, destination, sender, "PostMessage");
		}
	}

	private object ExecuteMessage(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender, string logMessage)
	{
		Collection<object> collection = new Collection<object>();
		List<RegistreredReceiver> list = new List<RegistreredReceiver>();
		lock (_messageFilter)
		{
			foreach (RegistreredReceiver item in _messageFilter)
			{
				if (item.MessageFilter == null || item.MessageFilter.Match(message, destination, sender))
				{
					list.Add(item);
				}
			}
		}
		foreach (RegistreredReceiver item2 in list)
		{
			try
			{
				if (!item2.Unregistered)
				{
					if (TraceFunctionCalls)
					{
						Log(error: false, logMessage, message.MessageId + " to " + item2.MessageReceiver.Method.DeclaringType.Name + "." + item2.MessageReceiver.Method.Name, null);
					}
					if (base.TraceSendDetails)
					{
						TraceClassContent(message.Data);
					}
					object obj = item2.MessageReceiver(message, destination, sender);
					if (TraceFunctionCalls)
					{
						Log(error: false, logMessage, message.MessageId + " to (Returned)" + item2.MessageReceiver.Method.DeclaringType.Name + "." + item2.MessageReceiver.Method.Name, null);
					}
					if (obj != null)
					{
						collection.Add(obj);
					}
				}
			}
			catch (Exception exception)
			{
				ExceptionHandler(logMessage, exception);
			}
		}
		return collection;
	}

	private void TraceClassContent(object data)
	{
		if (data == null)
		{
			return;
		}
		if (data is DateTime)
		{
			LogDetail("Data contains DateTime=" + ((DateTime)data).ToString("yyyy-MM-dd HH:mm:ss.fff"));
			return;
		}
		LogDetail("Data contains " + data.GetType().Name);
		FieldInfo[] fields = data.GetType().GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			object value = fieldInfo.GetValue(data);
			if (value is DateTime)
			{
				LogDetail(fieldInfo.Name + " = " + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff"));
			}
			else if (value is FQID)
			{
				LogDetail(fieldInfo.Name + " = " + ((FQID)value).ObjectId.ToString());
			}
			else
			{
				LogDetail(fieldInfo.Name + " = " + value);
			}
		}
	}

	private void LogDetail(string tx)
	{
		Log(error: false, "TraceDetail", "--- " + tx);
	}

	public void CallPluginFunction(ViewItemPlugin clientPlugin, string function)
	{
		if (TraceFunctionCalls)
		{
			Log(error: false, "Calling:" + clientPlugin.Name, function, Array.Empty<object>());
		}
	}

	private string ExceptionToString(Exception e)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (e.InnerException != null)
		{
			stringBuilder.Append("************** Inner Exception **************");
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append(ExceptionToString(e.InnerException));
			stringBuilder.Append(System.Environment.NewLine);
			stringBuilder.Append("************** Outer Exception **************");
			stringBuilder.Append(System.Environment.NewLine);
		}
		stringBuilder.Append("Exception source:      ");
		stringBuilder.Append((e.Source == null) ? e.Message : e.Source);
		stringBuilder.Append(System.Environment.NewLine);
		stringBuilder.Append("Exception type:        ");
		stringBuilder.Append(e.GetType().FullName);
		stringBuilder.Append(System.Environment.NewLine);
		stringBuilder.Append("Exception message:     ");
		stringBuilder.Append(e.Message);
		stringBuilder.Append(System.Environment.NewLine);
		stringBuilder.Append("Exception target site: ");
		stringBuilder.Append((e.TargetSite == null) ? e.Message : e.TargetSite.Name);
		stringBuilder.Append(System.Environment.NewLine);
		if (e.StackTrace != null)
		{
			string value = e.StackTrace.ToString();
			stringBuilder.Append(value);
		}
		stringBuilder.Append(System.Environment.NewLine);
		return stringBuilder.ToString();
	}

	private List<string> FindMessageIds(Type[] types, string preName)
	{
		List<string> list = new List<string>();
		foreach (Type type in types)
		{
			string text = preName + type.Name + ".";
			Type[] nestedTypes = type.GetNestedTypes();
			if (nestedTypes != null)
			{
				list.AddRange(FindMessageIds(nestedTypes, text));
			}
			FieldInfo[] fields = type.GetFields();
			foreach (FieldInfo fieldInfo in fields)
			{
				list.Add(text + fieldInfo.Name);
			}
		}
		return list;
	}
}
