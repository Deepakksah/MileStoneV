using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.SDK.StatusClient;

public class EventMapper
{
	private static readonly Lazy<Dictionary<Guid, string>> StatusApiEventsToBaseMessageIds = new Lazy<Dictionary<Guid, string>>(CreateStatusApiEventsToBaseEventMessageIdDictionary);

	private static readonly Lazy<Dictionary<string, Guid>> BaseMessageIdsToStatusApiEvents = new Lazy<Dictionary<string, Guid>>(CreateBaseEventMessageIdToStatusApiEventsDictionary);

	public static bool TryConvertStatusEventGuidToBaseEventMessageId(Guid statusApiEventId, out string eventName)
	{
		return StatusApiEventsToBaseMessageIds.Value.TryGetValue(statusApiEventId, out eventName);
	}

	public static string StatusEventGuidToBaseEventMessageId(Guid statusApiEventId, string defaultName)
	{
		if (TryConvertStatusEventGuidToBaseEventMessageId(statusApiEventId, out var eventName))
		{
			return eventName;
		}
		return defaultName;
	}

	public static bool TryConvertBaseEventMessageIdToStatusEventGuid(string eventName, out Guid statusApiEventId)
	{
		return BaseMessageIdsToStatusApiEvents.Value.TryGetValue(eventName, out statusApiEventId);
	}

	private static Dictionary<string, Guid> CreateBaseEventMessageIdToStatusApiEventsDictionary()
	{
		return CreateStatusApiEventsToBaseEventMessageIdDictionary().ToDictionary((KeyValuePair<Guid, string> kvp) => kvp.Value, (KeyValuePair<Guid, string> kvp) => kvp.Key);
	}

	private static Dictionary<Guid, string> CreateStatusApiEventsToBaseEventMessageIdDictionary()
	{
		return new Dictionary<Guid, string>
		{
			{
				KnownStatusEvents.PtzManualSessionStarted,
				"Ptz Manual Session Started"
			},
			{
				KnownStatusEvents.PtzManualSessionStopped,
				"Ptz Manual Session Stopped"
			},
			{
				KnownStatusEvents.LiveClientFeedRequested,
				"Live Client Feed Requested"
			},
			{
				KnownStatusEvents.LiveClientFeedTerminated,
				"Live Client Feed Terminated"
			},
			{
				KnownStatusEvents.MotionStarted,
				"Motion Detected"
			},
			{
				KnownStatusEvents.MotionStopped,
				"Motion Stopped"
			},
			{
				KnownStatusEvents.BookmarkReferenceRequested,
				"Bookmark Reference Requested"
			},
			{
				KnownStatusEvents.MarkedDataReferenceRequested,
				"Marked Data Reference Requested"
			},
			{
				KnownStatusEvents.RecordingsEvidenceLocked,
				BaseEventMessageId.RecordingsEvidenceLocked
			},
			{
				KnownStatusEvents.RecordingsEvidenceUnlocked,
				BaseEventMessageId.RecordingsEvidenceUnlocked
			},
			{
				KnownStatusEvents.EvidenceLockChanged,
				BaseEventMessageId.EvidenceLockChanged
			},
			{
				KnownStatusEvents.RelativeTimerEvent,
				"Relative Timer Event"
			},
			{
				KnownStatusEvents.PeriodicTimerEvent,
				"Periodic Timer Event"
			},
			{
				KnownStatusEvents.RecordingStarted,
				"Recording Started"
			},
			{
				KnownStatusEvents.RecordingStopped,
				"Recording Stopped"
			},
			{
				KnownStatusEvents.DatabaseRepair,
				"Database being repaired"
			},
			{
				KnownStatusEvents.DatabaseDiskFull,
				"Running Out of Disk Space"
			},
			{
				KnownStatusEvents.DatabaseDiskUnavailable,
				"Database Failure: Disk Unavailable"
			},
			{
				KnownStatusEvents.DatabaseDiskAvailable,
				"Database Disk Available"
			},
			{
				KnownStatusEvents.ArchiveDiskUnavailable,
				"Archive Failure: Disk Unavailable"
			},
			{
				KnownStatusEvents.ArchiveDiskAvailable,
				"Archive Disk Available"
			},
			{
				KnownStatusEvents.DatabaseFullAutoArchive,
				"Database Full Auto Archive"
			},
			{
				KnownStatusEvents.ArchiveNotFinished,
				"Archive Not Finished"
			},
			{
				KnownStatusEvents.ManualRecordingStarted,
				BaseEventMessageId.ManualRecordingStarted
			},
			{
				KnownStatusEvents.ManualRecordingStopped,
				BaseEventMessageId.ManualRecordingStopped
			},
			{
				KnownStatusEvents.DatabaseDiskFullAutoArchiving,
				BaseEventMessageId.DatabaseDiskFullAutoArchive
			},
			{
				KnownStatusEvents.DatabaseDeletingRecordingsBeforeSetRetentionTime,
				BaseEventMessageId.DatabaseDeleteBeforeSetRetentionTime
			},
			{
				KnownStatusEvents.DatabaseDeletingRecordingsBeforeSetRetentionSize,
				BaseEventMessageId.DatabaseDeleteBeforeSetRetentionSize
			},
			{
				KnownStatusEvents.HardwareSettingsChanged,
				"Hardware Settings Changed"
			},
			{
				KnownStatusEvents.HardwareSettingsChangedError,
				"Hardware Settings Changed Error"
			},
			{
				KnownStatusEvents.InputActivated,
				"Input Activated"
			},
			{
				KnownStatusEvents.InputDeactivated,
				"Input Deactivated"
			},
			{
				KnownStatusEvents.InputChanged,
				"Input Changed"
			},
			{
				KnownStatusEvents.InputSensor,
				"Input Sensor"
			},
			{
				KnownStatusEvents.OutputActivated,
				"Output Activated"
			},
			{
				KnownStatusEvents.OutputDeactivated,
				"Output Deactivated"
			},
			{
				KnownStatusEvents.OutputChanged,
				"Output Changed"
			},
			{
				KnownStatusEvents.FeedOverflowStarted,
				"Feed Overflow Started"
			},
			{
				KnownStatusEvents.FeedOverflowStopped,
				"Feed Overflow Stopped"
			},
			{
				KnownStatusEvents.CommunicationError,
				"Not Responding"
			},
			{
				KnownStatusEvents.CommunicationStarted,
				"Responding"
			},
			{
				KnownStatusEvents.CommunicationStopped,
				"Communication Stopped"
			},
			{
				KnownStatusEvents.DeviceSettingsChanged,
				"Settings Changed"
			},
			{
				KnownStatusEvents.DeviceSettingsChangedError,
				"Settings Changed Error"
			},
			{
				KnownStatusEvents.MotionStartedDriver,
				"Motion Started Driver"
			},
			{
				KnownStatusEvents.MotionStoppedDriver,
				"Motion Stopped Driver"
			},
			{
				KnownStatusEvents.ArchivingFailedDriver,
				"Archiving Failed Driver"
			},
			{
				KnownStatusEvents.DiskSpaceLowDriver,
				"Disk Space Low Driver"
			},
			{
				KnownStatusEvents.DeviceWriteFailedDriver,
				"Device Write Failed Driver"
			},
			{
				KnownStatusEvents.FeedOverflowStartedDriver,
				"Feed Overflow Started Driver"
			},
			{
				KnownStatusEvents.FeedOverflowStoppedDriver,
				"Feed Overflow Stopped Driver"
			},
			{
				KnownStatusEvents.DeviceSettingsChangedDriver,
				"Device Settings Changed Driver"
			},
			{
				KnownStatusEvents.HardwareCommunicationError,
				"Not Responding"
			},
			{
				KnownStatusEvents.HardwareCommunicationStarted,
				"Responding"
			},
			{
				KnownStatusEvents.HardwareCommunicationStopped,
				"Hardware Communication Stopped"
			},
			{
				KnownStatusEvents.DatabaseDiskAvailableDriver,
				"Database Disk Available Driver"
			},
			{
				KnownStatusEvents.DatabaseDiskUnavailableDriver,
				"Database Disk Unavailable Driver"
			},
			{
				KnownStatusEvents.ArchiveDiskAvailableDriver,
				"Archive Disk Available Driver"
			},
			{
				KnownStatusEvents.ArchiveDiskUnavailableDriver,
				"Archive Disk Unavailable Driver"
			},
			{
				KnownStatusEvents.DatabaseDiskFullDriver,
				"Database Disk Full Driver"
			},
			{
				KnownStatusEvents.DatabaseFullAutoArchiveDriver,
				"Database Full Auto Archive Driver"
			},
			{
				KnownStatusEvents.DatabaseDeleteBeforeSetRetentionTimeDriver,
				BaseEventMessageId.DatabaseDeleteBeforeSetRetentionTimeDriver
			},
			{
				KnownStatusEvents.DatabaseDeleteBeforeSetRetentionSizeDriver,
				BaseEventMessageId.DatabaseDeleteBeforeSetRetentionSizeDriver
			},
			{
				KnownStatusEvents.FailoverStarted,
				"Failover Started"
			},
			{
				KnownStatusEvents.FailoverStopped,
				"Failover Stopped"
			},
			{
				KnownStatusEvents.ResetDeviceConnection,
				BaseEventMessageId.ResetDeviceConnection
			},
			{
				KnownStatusEvents.SystemMonitorCpuUsageCritical,
				BaseEventMessageId.SystemMonitorCpuUsageCritical
			},
			{
				KnownStatusEvents.SystemMonitorCpuUsageWarning,
				BaseEventMessageId.SystemMonitorCpuUsageWarning
			},
			{
				KnownStatusEvents.SystemMonitorCpuUsageNormal,
				BaseEventMessageId.SystemMonitorCpuUsageNormal
			},
			{
				KnownStatusEvents.SystemMonitorMemoryUsageCritical,
				BaseEventMessageId.SystemMonitorMemoryUsageCritical
			},
			{
				KnownStatusEvents.SystemMonitorMemoryUsageWarning,
				BaseEventMessageId.SystemMonitorMemoryUsageWarning
			},
			{
				KnownStatusEvents.SystemMonitorMemoryUsageNormal,
				BaseEventMessageId.SystemMonitorMemoryUsageNormal
			},
			{
				KnownStatusEvents.SystemMonitorUsedSpaceCritical,
				BaseEventMessageId.SystemMonitorUsedSpaceCritical
			},
			{
				KnownStatusEvents.SystemMonitorUsedSpaceWarning,
				BaseEventMessageId.SystemMonitorUsedSpaceWarning
			},
			{
				KnownStatusEvents.SystemMonitorUsedSpaceNormal,
				BaseEventMessageId.SystemMonitorUsedSpaceNormal
			},
			{
				KnownStatusEvents.SystemMonitorLiveFpsCritical,
				BaseEventMessageId.SystemMonitorLiveFpsCritical
			},
			{
				KnownStatusEvents.SystemMonitorLiveFpsWarning,
				BaseEventMessageId.SystemMonitorLiveFpsWarning
			},
			{
				KnownStatusEvents.SystemMonitorLiveFpsNormal,
				BaseEventMessageId.SystemMonitorLiveFpsNormal
			},
			{
				KnownStatusEvents.SystemMonitorRecordingFpsCritical,
				BaseEventMessageId.SystemMonitorRecordingFpsCritical
			},
			{
				KnownStatusEvents.SystemMonitorRecordingFpsWarning,
				BaseEventMessageId.SystemMonitorRecordingFpsWarning
			},
			{
				KnownStatusEvents.SystemMonitorRecordingFpsNormal,
				BaseEventMessageId.SystemMonitorRecordingFpsNormal
			},
			{
				KnownStatusEvents.SystemMonitorFreeSpaceCritical,
				BaseEventMessageId.SystemMonitorFreeSpaceCritical
			},
			{
				KnownStatusEvents.SystemMonitorFreeSpaceWarning,
				BaseEventMessageId.SystemMonitorFreeSpaceWarning
			},
			{
				KnownStatusEvents.SystemMonitorFreeSpaceNormal,
				BaseEventMessageId.SystemMonitorFreeSpaceNormal
			},
			{
				KnownStatusEvents.SystemMonitorRetentionTimeCritical,
				BaseEventMessageId.SystemMonitorRetentionTimeCritical
			},
			{
				KnownStatusEvents.SystemMonitorRetentionTimeWarning,
				BaseEventMessageId.SystemMonitorRetentionTimeWarning
			},
			{
				KnownStatusEvents.SystemMonitorRetentionTimeNormal,
				BaseEventMessageId.SystemMonitorRetentionTimeNormal
			},
			{
				KnownStatusEvents.SystemMonitorServiceAvailableCritical,
				BaseEventMessageId.SystemMonitorServiceAvailableCritical
			},
			{
				KnownStatusEvents.SystemMonitorServiceAvailableNormal,
				BaseEventMessageId.SystemMonitorServiceAvailableNormal
			}
		};
	}
}
