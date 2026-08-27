using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VideoOS.Mobile.AuditLog;

public static class AuditLogConstants
{
	public static class Categories
	{
		public const string AccessControlCommand = "Access control command";

		public const string Bookmark = "Bookmark";

		public const string Camera = "Camera";

		public const string Export = "Export";

		public const string IoAndEvents = "IO and events";

		public const string Live = "Live";

		public const string LogRead = "Log read";

		public const string Management = "Management";

		public const string ManualRecording = "Manual recording";

		public const string MapControl = "Map Control";

		public const string MapViewing = "Map viewing";

		public const string MetadataSearch = "Metadata search";

		public const string Playback = "Playback";

		public const string Print = "Print";

		public const string PTZ = "PTZ";

		public const string SaveSearch = "Save search";

		public const string Security = "Security";

		public const string SmartWallControl = "Smart Wall control";

		public const string Unknown = "Unknown";
	}

	public static class MessageIDs
	{
		public const string ConfigExport = "ConfigExport";

		public const string DeviceNotificationsEnabledChanged = "DeviceNotificationsEnabledChanged";

		public const string DeviceDeleted = "DeviceDeleted";

		public const string ExportCreated = "ExportCreated";

		public const string ExportDeleted = "ExportDeleted";

		public const string ExportDownloaded = "ExportDownloaded";

		public const string InvestigationCreated = "InvestigationCreated";

		public const string InvestigationUpdated = "InvestigationUpdated";

		public const string InvestigationDeleted = "InvestigationDeleted";

		public const string ItemAdded = "ItemAdded";

		public const string ItemRemoved = "ItemRemoved";

		public const string ItemUpdated = "ItemUpdated";

		public const string LoggedOutByClient = "LoggedOutByClient";

		public const string LoggedOutByServer = "LoggedOutByServer";

		public const string SettingChanged = "SettingChanged";

		public const string TwoStepCodeSent = "TwoStepCodeSent";

		public const string TwoStepCorrectCodeEntered = "TwoStepCorrectCodeEntered";

		public const string TwoStepIncorrectCodeEntered = "TwoStepIncorrectCodeEntered";

		public const string TwoStepUserNotAllowed = "TwoStepUserNotAllowed";

		public const string VideoPlayback = "VideoPlayback";

		public const string VideoPushStarted = "VideoPushStarted";

		public const string VideoPushStopped = "VideoPushStopped";

		public static IEnumerable<string> EnumerateAll => from fi in typeof(MessageIDs).GetFields(BindingFlags.Static | BindingFlags.Public)
			where fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string)
			select fi.GetValue(null) as string;
	}
}
