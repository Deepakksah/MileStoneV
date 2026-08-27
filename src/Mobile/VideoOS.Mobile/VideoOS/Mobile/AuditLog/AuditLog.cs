using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Log;

namespace VideoOS.Mobile.AuditLog;

public class AuditLog : IAuditLog
{
	private const string Application = "MobileServer";

	private const string Component = "All";

	private const string Version = "v.0.8";

	private const string DummyMessageId = "DummyMessage";

	private static readonly IReadOnlyList<CultureInfo> _supportedServerCultures;

	private static readonly Dictionary<CultureInfo, Dictionary<string, LogMessage>> _messagesByCulture;

	private static readonly Item _relatedItem;

	private LogClient _logClient;

	static AuditLog()
	{
		_supportedServerCultures = new string[14]
		{
			"da-DK", "de-DE", "en-US", "es-ES", "fr-FR", "it-IT", "ja-JP", "ko-KR", "pt-BR", "ru-RU",
			"sv-SE", "tr-TR", "zh-CN", "zh-TW"
		}.Select((string name) => new CultureInfo(name)).ToList();
		_messagesByCulture = new Dictionary<CultureInfo, Dictionary<string, LogMessage>>();
		_relatedItem = VideoOS.Platform.Configuration.Instance.GetItemsByKind(Kind.Server).FirstOrDefault();
	}

	public AuditLog(LogClient logClient)
	{
		_logClient = logClient;
		foreach (CultureInfo supportedServerCulture in _supportedServerCultures)
		{
			Dictionary<string, LogMessage> messages = GetMessages(supportedServerCulture);
			if (!messages.ContainsKey("DummyMessage"))
			{
				messages["DummyMessage"] = GetDummyMessage();
			}
			LogMessageDictionary logMessageDictionary = new LogMessageDictionary(supportedServerCulture.Name, "v.0.8", "MobileServer", "All", messages, "Server");
			_logClient.RegisterDictionary(logMessageDictionary);
		}
		DummyMessage();
	}

	public void SettingChanged(string serverName, string name, string oldValue, string newValue)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = name,
			["2"] = oldValue,
			["3"] = newValue
		};
		AuditEntry("SettingChanged", stringParams);
	}

	public void RegisteredDeviceNotificationsEnabledChanged(string serverName, string deviceName, string enabledState)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = enabledState,
			["2"] = deviceName
		};
		AuditEntry("DeviceNotificationsEnabledChanged", stringParams);
	}

	public void RegisteredDeviceDeleted(string serverName, string deviceName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = deviceName
		};
		AuditEntry("DeviceDeleted", stringParams);
	}

	public void ExportLocationChanged(string serverName, string oldValue, string newValue)
	{
		SettingChanged(serverName, Resources.ExportLocation, oldValue, newValue);
	}

	public void ConfigExported(string serverName, string fileName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = fileName
		};
		AuditEntry("ConfigExport", stringParams);
	}

	public void AddedItem(string serverName, string itemName, string newItem)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = itemName,
			["2"] = newItem
		};
		AuditEntry("ItemAdded", stringParams);
	}

	public void RemovedItem(string serverName, string itemName, string oldItem)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = itemName,
			["2"] = oldItem
		};
		AuditEntry("ItemRemoved", stringParams);
	}

	public void UpdatedItem(string serverName, string itemName, string oldItem, string newItem)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = itemName,
			["2"] = oldItem,
			["3"] = newItem
		};
		AuditEntry("ItemUpdated", stringParams);
	}

	public void InvestigationCreated(string serverName, InvestigationAuditLogData details, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = details.Name,
			["2"] = details.Start.Format(),
			["3"] = details.End.Format(),
			["4"] = camera.Name
		};
		AuditEntry("InvestigationCreated", stringParams, camera);
	}

	public void InvestigationUpdated(string serverName, InvestigationAuditLogData oldDetails, InvestigationAuditLogData newDetails, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = oldDetails.Name,
			["2"] = newDetails.Name,
			["3"] = oldDetails.Start.Format(),
			["4"] = oldDetails.End.Format(),
			["5"] = newDetails.Start.Format(),
			["6"] = newDetails.End.Format(),
			["7"] = camera.Name,
			["8"] = newDetails.State.Format()
		};
		AuditEntry("InvestigationUpdated", stringParams, camera);
	}

	public void InvestigationDeleted(string serverName, InvestigationAuditLogData details, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = details.Name,
			["2"] = details.Start.Format(),
			["3"] = details.End.Format(),
			["4"] = camera.Name
		};
		AuditEntry("InvestigationDeleted", stringParams, camera);
	}

	public void ExportCreated(string serverName, InvestigationAuditLogData investigation, ExportAuditLogData export, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = investigation.Name,
			["2"] = investigation.Start.Format(),
			["3"] = investigation.End.Format(),
			["4"] = camera.Name,
			["5"] = export.IsPasswordProtected.Format(),
			["6"] = export.IncludeAudio.Format(),
			["7"] = export.Type.Format()
		};
		AuditEntry("ExportCreated", stringParams, camera);
	}

	public void ExportDownloaded(string serverName, InvestigationAuditLogData investigation, ExportAuditLogData export, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = investigation.Name,
			["2"] = investigation.Start.Format(),
			["3"] = investigation.End.Format(),
			["4"] = camera.Name,
			["5"] = export.IsPasswordProtected.Format(),
			["6"] = export.IncludeAudio.Format(),
			["7"] = export.Type.Format()
		};
		AuditEntry("ExportDownloaded", stringParams, camera);
	}

	public void ExportDeleted(string serverName, InvestigationAuditLogData investigation, ExportAuditLogData export, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = investigation.Name,
			["2"] = investigation.Start.Format(),
			["3"] = investigation.End.Format(),
			["4"] = camera.Name,
			["5"] = export.Type.Format()
		};
		AuditEntry("ExportDeleted", stringParams, camera);
	}

	public void VideoPushStarted(string serverName, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("VideoPushStarted", stringParams, camera);
	}

	public void VideoPushStopped(string serverName, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("VideoPushStopped", stringParams, camera);
	}

	public void TwoStepVerificationUserNotAllowed(string serverName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("TwoStepUserNotAllowed", stringParams);
	}

	public void TwoStepVerificationCodeSent(string serverName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("TwoStepCodeSent", stringParams);
	}

	public void TwoStepVerificationCorrectCodeEntered(string serverName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("TwoStepCorrectCodeEntered", stringParams);
	}

	public void TwoStepVerificationIncorrectCodeEntered(string serverName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("TwoStepIncorrectCodeEntered", stringParams);
	}

	public void VideoPlayback(string serverName, DateTime from, DateTime to, Item camera)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string>
		{
			["0"] = serverName,
			["1"] = from.ToString(),
			["2"] = to.ToString()
		};
		AuditEntry("VideoPlayback", stringParams, camera);
	}

	public void ClientInitiatedLogout(string serverName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("LoggedOutByClient", stringParams);
	}

	public void ServerInitiatedLogout(string serverName)
	{
		Dictionary<string, string> stringParams = new Dictionary<string, string> { ["0"] = serverName };
		AuditEntry("LoggedOutByServer", stringParams);
	}

	private static Dictionary<string, LogMessage> GetMessages(CultureInfo culture)
	{
		if (!_messagesByCulture.ContainsKey(culture))
		{
			_messagesByCulture[culture] = AuditLogConstants.MessageIDs.EnumerateAll.ToDictionary((string id) => id, (string id) => LogMessageFactory.CreateLogMessage(id, culture));
		}
		return _messagesByCulture[culture];
	}

	private static LogMessage GetDummyMessage()
	{
		return new LogMessage
		{
			Id = "DummyMessage",
			Message = "Audit log initializing message",
			Group = "Audit",
			Severity = "Info",
			Status = "Success",
			RelatedObjectKind = Kind.Server,
			Category = "Unknown"
		};
	}

	private void AuditEntry(string messageID, Dictionary<string, string> stringParams, Item item = null)
	{
		_logClient.AuditEntry("MobileServer", "All", messageID, item ?? _relatedItem, stringParams, "Granted");
	}

	private void DummyMessage()
	{
		_logClient.AuditEntry("MobileServer", "All", "DummyMessage", null, new Dictionary<string, string>(), "Default");
	}
}
