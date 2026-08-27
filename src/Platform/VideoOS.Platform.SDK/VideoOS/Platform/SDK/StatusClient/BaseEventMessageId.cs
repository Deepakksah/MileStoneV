using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VideoOS.Platform.SDK.StatusClient;

public static class BaseEventMessageId
{
	private static readonly Lazy<List<string>> EventNames = new Lazy<List<string>>(CreateEventList);

	public const string PtzManualSessionStarted = "Ptz Manual Session Started";

	public const string PtzManualSessionStopped = "Ptz Manual Session Stopped";

	public const string LiveClientFeedRequested = "Live Client Feed Requested";

	public const string LiveClientFeedTerminated = "Live Client Feed Terminated";

	public const string MotionStarted = "Motion Detected";

	public const string MotionStopped = "Motion Stopped";

	public const string BookmarkReferenceRequested = "Bookmark Reference Requested";

	public const string MarkedDataReferenceRequested = "Marked Data Reference Requested";

	public static readonly string RecordingsEvidenceLocked = "Recordings Evidence Locked";

	public static readonly string RecordingsEvidenceUnlocked = "Recordings Evidence Unlocked";

	public static readonly string EvidenceLockChanged = "Evidence Lock Changed";

	public const string RelativeTimerEvent = "Relative Timer Event";

	public const string PeriodicTimerEvent = "Periodic Timer Event";

	public const string RecordingStarted = "Recording Started";

	public const string RecordingStopped = "Recording Stopped";

	public const string DatabaseRepair = "Database being repaired";

	public const string DatabaseDiskFull = "Running Out of Disk Space";

	public const string DatabaseDiskUnavailable = "Database Failure: Disk Unavailable";

	public const string DatabaseDiskAvailable = "Database Disk Available";

	public const string ArchiveDiskUnavailable = "Archive Failure: Disk Unavailable";

	public const string ArchiveDiskAvailable = "Archive Disk Available";

	public const string DatabaseFullAutoArchive = "Database Full Auto Archive";

	public const string ArchiveNotFinished = "Archive Not Finished";

	public const string ArchiveFailure = "Archive Failure";

	public static readonly string ManualRecordingStarted = "Manual Recording Started";

	public static readonly string ManualRecordingStopped = "Manual Recording Stopped";

	public static readonly string DatabaseDiskFullAutoArchive = "Database Disk Full Auto Archive";

	public static readonly string DatabaseDeleteBeforeSetRetentionTime = "DatabaseDeleteBeforeSetRetentionTime";

	public static readonly string DatabaseDeleteBeforeSetRetentionSize = "DatabaseDeleteBeforeSetRetentionSize";

	public const string HardwareSettingsChanged = "Hardware Settings Changed";

	public const string HardwareSettingsChangedError = "Hardware Settings Changed Error";

	public const string InputActivated = "Input Activated";

	public const string InputDeactivated = "Input Deactivated";

	public const string InputChanged = "Input Changed";

	public const string InputSensor = "Input Sensor";

	public const string OutputActivated = "Output Activated";

	public const string OutputDeactivated = "Output Deactivated";

	public const string OutputChanged = "Output Changed";

	public const string FeedOverflowStarted = "Feed Overflow Started";

	public const string FeedOverflowStopped = "Feed Overflow Stopped";

	public const string CommunicationError = "Not Responding";

	public const string CommunicationStarted = "Responding";

	public const string CommunicationStopped = "Communication Stopped";

	public const string DeviceSettingsChanged = "Settings Changed";

	public const string DeviceSettingsChangedError = "Settings Changed Error";

	public const string MotionStartedDriver = "Motion Started Driver";

	public const string MotionStoppedDriver = "Motion Stopped Driver";

	public const string ArchivingFailedDriver = "Archiving Failed Driver";

	public const string DiskSpaceLowDriver = "Disk Space Low Driver";

	public const string DeviceWriteFailedDriver = "Device Write Failed Driver";

	public const string FeedOverflowStartedDriver = "Feed Overflow Started Driver";

	public const string FeedOverflowStoppedDriver = "Feed Overflow Stopped Driver";

	public const string DeviceSettingsChangedDriver = "Device Settings Changed Driver";

	public const string HardwareCommunicationError = "Not Responding";

	public const string HardwareCommunicationStarted = "Responding";

	public const string HardwareCommunicationStopped = "Hardware Communication Stopped";

	public const string DatabaseDiskAvailableDriver = "Database Disk Available Driver";

	public const string DatabaseDiskUnavailableDriver = "Database Disk Unavailable Driver";

	public const string ArchiveDiskAvailableDriver = "Archive Disk Available Driver";

	public const string ArchiveDiskUnavailableDriver = "Archive Disk Unavailable Driver";

	public const string DatabaseDiskFullDriver = "Database Disk Full Driver";

	public const string DatabaseFullAutoArchiveDriver = "Database Full Auto Archive Driver";

	public static readonly string DatabaseDeleteBeforeSetRetentionTimeDriver = "DatabaseDeleteBeforeSetRetentionTimeDriver";

	public static readonly string DatabaseDeleteBeforeSetRetentionSizeDriver = "DatabaseDeleteBeforeSetRetentionSizeDriver";

	public const string FailoverStarted = "Failover Started";

	public const string FailoverStopped = "Failover Stopped";

	public static readonly string ResetDeviceConnection = "ResetDeviceConnection";

	public static readonly string SystemMonitorCpuUsageCritical = "SystemMonitorCpuUsageCritical";

	public static readonly string SystemMonitorCpuUsageWarning = "SystemMonitorCpuUsageWarning";

	public static readonly string SystemMonitorCpuUsageNormal = "SystemMonitorCpuUsageNormal";

	public static readonly string SystemMonitorMemoryUsageCritical = "SystemMonitorMemoryUsageCritical";

	public static readonly string SystemMonitorMemoryUsageWarning = "SystemMonitorMemoryUsageWarning";

	public static readonly string SystemMonitorMemoryUsageNormal = "SystemMonitorMemoryUsageNormal";

	public static readonly string SystemMonitorUsedSpaceCritical = "SystemMonitorUsedSpaceCritical";

	public static readonly string SystemMonitorUsedSpaceWarning = "SystemMonitorUsedSpaceWarning";

	public static readonly string SystemMonitorUsedSpaceNormal = "SystemMonitorUsedSpaceNormal";

	public static readonly string SystemMonitorLiveFpsCritical = "SystemMonitorLiveFpsCritical";

	public static readonly string SystemMonitorLiveFpsWarning = "SystemMonitorLiveFpsWarning";

	public static readonly string SystemMonitorLiveFpsNormal = "SystemMonitorLiveFpsNormal";

	public static readonly string SystemMonitorRecordingFpsCritical = "SystemMonitorRecordingFpsCritical";

	public static readonly string SystemMonitorRecordingFpsWarning = "SystemMonitorRecordingFpsWarning";

	public static readonly string SystemMonitorRecordingFpsNormal = "SystemMonitorRecordingFpsNormal";

	public static readonly string SystemMonitorFreeSpaceCritical = "SystemMonitorFreeSpaceCritical";

	public static readonly string SystemMonitorFreeSpaceWarning = "SystemMonitorFreeSpaceWarning";

	public static readonly string SystemMonitorFreeSpaceNormal = "SystemMonitorFreeSpaceNormal";

	public static readonly string SystemMonitorRetentionTimeCritical = "SystemMonitorRetentionTimeCritical";

	public static readonly string SystemMonitorRetentionTimeWarning = "SystemMonitorRetentionTimeWarning";

	public static readonly string SystemMonitorRetentionTimeNormal = "SystemMonitorRetentionTimeNormal";

	public static readonly string SystemMonitorServiceAvailableCritical = "SystemMonitorServiceAvailableCritical";

	public static readonly string SystemMonitorServiceAvailableNormal = "SystemMonitorServiceAvailableNormal";

	public static readonly string AutomaticActivationFailed = "AutomaticActivationFailed";

	public const string ExternalEvent = "External Event";

	public const string Enabled = "Enabled";

	public const string Disabled = "Disabled";

	public const string ServerNotResponding = "Server Not Responding";

	public const string ServerResponding = "Server Responding";

	public static IEnumerable<string> GetAllEvents()
	{
		return EventNames.Value;
	}

	private static List<string> CreateEventList()
	{
		return (from fi in typeof(BaseEventMessageId).GetFields(BindingFlags.Static | BindingFlags.Public)
			where fi.IsLiteral && !fi.IsInitOnly
			where fi.FieldType == typeof(string)
			select fi.GetValue(null)).Cast<string>().ToList();
	}
}
