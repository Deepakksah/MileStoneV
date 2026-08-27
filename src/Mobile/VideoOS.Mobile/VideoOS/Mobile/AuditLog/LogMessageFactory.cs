using System.Globalization;
using System.Reflection;
using System.Resources;
using VideoOS.Platform;
using VideoOS.Platform.Log;

namespace VideoOS.Mobile.AuditLog;

public static class LogMessageFactory
{
	private static readonly ResourceManager _resourceManager = new ResourceManager(typeof(Resources).FullName, Assembly.GetExecutingAssembly());

	public static LogMessage CreateLogMessage(string messageId, CultureInfo culture)
	{
		LogMessage logMessage = new LogMessage
		{
			Id = messageId,
			Message = _resourceManager.GetString(messageId, culture),
			Group = "Audit",
			Severity = "Info",
			Status = "Success"
		};
		switch (messageId)
		{
		case "ExportCreated":
		case "ExportDeleted":
		case "ExportDownloaded":
		case "InvestigationCreated":
		case "InvestigationDeleted":
		case "InvestigationUpdated":
		case "ConfigExport":
			logMessage.Category = "Export";
			logMessage.RelatedObjectKind = Kind.Server;
			break;
		case "DeviceDeleted":
		case "ItemRemoved":
		case "ItemUpdated":
		case "DeviceNotificationsEnabledChanged":
		case "ItemAdded":
		case "SettingChanged":
			logMessage.Category = "Management";
			logMessage.RelatedObjectKind = Kind.Server;
			break;
		case "LoggedOutByClient":
		case "LoggedOutByServer":
		case "TwoStepCodeSent":
		case "TwoStepCorrectCodeEntered":
		case "TwoStepIncorrectCodeEntered":
		case "TwoStepUserNotAllowed":
			logMessage.Category = "Security";
			logMessage.RelatedObjectKind = Kind.Server;
			break;
		case "VideoPlayback":
			logMessage.Category = "Playback";
			logMessage.RelatedObjectKind = Kind.Camera;
			break;
		case "VideoPushStopped":
		case "VideoPushStarted":
			logMessage.Category = "Live";
			logMessage.RelatedObjectKind = Kind.Camera;
			break;
		default:
			return null;
		}
		return logMessage;
	}
}
