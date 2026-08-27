using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace VideoOS.Platform.SDK.StatusClient;

public static class KnownStatusEvents
{
	private static readonly Lazy<Dictionary<Guid, string>> EventIdsAndEventNames = new Lazy<Dictionary<Guid, string>>(CreateFieldNameDictionary);

	public static readonly Guid PtzManualSessionStarted = new Guid("2EEB582A-1506-4D73-96C9-AD356ACF85FB");

	public static readonly Guid PtzManualSessionStopped = new Guid("EED47283-694F-4683-9E4D-18E7D93CF796");

	public static readonly Guid LiveClientFeedRequested = new Guid("EEDA47FF-4F3D-459E-8143-69896C7C74AD");

	public static readonly Guid LiveClientFeedTerminated = new Guid("66BDD008-B856-44B6-B385-B1F6C3F76F1B");

	public static readonly Guid MotionStarted = new Guid("6EB95DD6-7CCC-4BCE-99F8-AF0D0B582D77");

	public static readonly Guid MotionStopped = new Guid("6F55A7A7-D21C-4629-AC18-AF1975E395A2");

	public static readonly Guid BookmarkReferenceRequested = new Guid("7CFC6709-2C16-405f-9182-0BDB6F9F7E43");

	public static readonly Guid MarkedDataReferenceRequested = new Guid("3A82E9B3-9FFF-4BA1-9593-9927B0821076");

	public static readonly Guid RecordingsEvidenceLocked = new Guid("54DC549C-A007-47BD-8060-87BBAE9497B0");

	public static readonly Guid RecordingsEvidenceUnlocked = new Guid("3334B3E9-A135-497A-A0B9-9036533C2173");

	public static readonly Guid EvidenceLockChanged = new Guid("6EAB8BA5-6CAC-4636-BB2E-0A03875DF41A");

	public static readonly Guid DatabaseDeletingRecordingsBeforeSetRetentionTime = new Guid("1DD4F1AB-3CDA-4F5C-93D7-B0C78D2BC6AD");

	public static readonly Guid DatabaseDeletingRecordingsBeforeSetRetentionSize = new Guid("023D4D8D-5848-43CF-8F94-4F7F230966EF");

	public static readonly Guid DatabaseDiskFullAutoArchiving = new Guid("59ED52FD-A07D-4EDD-8BD6-24D0420CEA34");

	public static readonly Guid RelativeTimerEvent = new Guid("6A00FD9A-A7F1-446C-ABC2-29639CECD595");

	public static readonly Guid PeriodicTimerEvent = new Guid("F07AC0E3-9662-4781-9176-EC9E488AA494");

	public static readonly Guid RecordingStarted = new Guid("4577F552-765A-438c-BC7D-E5FF1F754BC3");

	public static readonly Guid RecordingStopped = new Guid("79A94F89-92DE-4fca-8A43-5561D407423D");

	public static readonly Guid DatabaseRepair = new Guid("B6C4B695-7404-414D-A229-33BF83FFE255");

	public static readonly Guid DatabaseDiskFull = new Guid("A4935AA3-C4B6-42A3-B4C5-6D4C20B043CF");

	public static readonly Guid DatabaseDiskUnavailable = new Guid("29986AA1-924B-4c8d-AAC7-A876883DC82C");

	public static readonly Guid DatabaseDiskAvailable = new Guid("FAB03C4D-0589-48c8-9328-E28B76C95AF0");

	public static readonly Guid ArchiveDiskUnavailable = new Guid("72C29AAE-DDB2-46df-B6FD-7F728A85F292");

	public static readonly Guid ArchiveDiskAvailable = new Guid("B7F0003B-1392-450e-B6CC-9BBE6E7DF065");

	public static readonly Guid DatabaseFullAutoArchive = new Guid("1FDE8231-F0E4-4919-AED1-B2B142D11E54");

	public static readonly Guid ArchiveNotFinished = new Guid("4DA7CFF5-BF0D-46D9-8B35-85F3288904F4");

	public static readonly Guid ManualRecordingStarted = new Guid("D740951A-984D-4049-99B5-25F5CBABD21F");

	public static readonly Guid ManualRecordingStopped = new Guid("A9C1F6FA-ADD6-4BF4-BA2D-A0CEDDB73C51");

	public static readonly Guid HardwareSettingsChanged = new Guid("A600C492-11C8-4D65-92DB-6D46209CE9EA");

	public static readonly Guid HardwareSettingsChangedError = new Guid("829A93AB-1AC0-4AEB-BDA0-ED8F6D8F514B");

	public static readonly Guid InputActivated = new Guid("836CA458-A833-4742-8EE0-64B2380984BD");

	public static readonly Guid InputDeactivated = new Guid("8666E3DC-57A7-4F38-9611-51D774EE7358");

	public static readonly Guid InputChanged = new Guid("FDA121B3-2070-4F31-A086-9F74E16580C0");

	public static readonly Guid InputSensor = new Guid("F84C8443-BFE3-4ECA-B69B-07DAEBC63A02");

	public static readonly Guid OutputActivated = new Guid("7A78F5BB-D8C3-4997-89B7-CAE72713B7DB");

	public static readonly Guid OutputDeactivated = new Guid("35742498-BCC5-4F0A-9800-827C9388D1CD");

	public static readonly Guid OutputChanged = new Guid("672F9F80-55FE-496F-BEE4-6C870BAB5064");

	public static readonly Guid FeedOverflowStarted = new Guid("536AD730-05AE-42CD-B14C-07B07C293E79");

	public static readonly Guid FeedOverflowStopped = new Guid("247FEF11-15DB-4C99-99DB-DE5475EAE443");

	public static readonly Guid CommunicationError = new Guid("A334AF1C-4B4B-4957-9E5F-AB8CA07FEAB6");

	public static readonly Guid CommunicationStarted = new Guid("DD3E6464-7DC0-405A-A92F-6150587563E8");

	public static readonly Guid CommunicationStopped = new Guid("0EE90664-2924-42A0-A816-4129D0ECABDC");

	public static readonly Guid DeviceLicenseRemoved = new Guid("65D84F0C-F671-4F99-A502-C0F470F72131");

	public static readonly Guid DeviceSettingsChanged = new Guid("4C8BE5ED-80FA-43E7-BF5B-4E7570AD0025");

	public static readonly Guid DeviceSettingsChangedError = new Guid("87BE2A0D-C33B-4B5F-AE1F-AFA6770A2497");

	public static readonly Guid MotionStartedDriver = new Guid("A7BD4B94-6EB0-4B5D-92D9-23F69BD23824");

	public static readonly Guid MotionStoppedDriver = new Guid("E3DD8ED6-CF93-410D-818A-6C474414D885");

	public static readonly Guid ArchivingFailedDriver = new Guid("036EE274-0E74-49CE-AC16-7410D4A6D9C5");

	public static readonly Guid DiskSpaceLowDriver = new Guid("37517225-05C8-44A2-A478-5B28B6BE0E07");

	public static readonly Guid DeviceWriteFailedDriver = new Guid("28666A13-4181-4DDC-B63D-F677A60566C6");

	public static readonly Guid FeedOverflowStartedDriver = new Guid("3F437517-EE1B-4BF3-AEC0-2A6C890C1F31");

	public static readonly Guid FeedOverflowStoppedDriver = new Guid("1CF31257-D98F-4854-9396-34BC61EFCA9C");

	public static readonly Guid DeviceSettingsChangedDriver = new Guid("DDEF0EEB-DD6E-49F4-A37D-1314877D3BCE");

	public static readonly Guid HardwareCommunicationError = new Guid("6BAAD64B-C395-4F52-B6A3-DD1B64AA2F0F");

	public static readonly Guid HardwareCommunicationStarted = new Guid("0553C396-5E16-4C22-B3D1-F548E42DFBB4");

	public static readonly Guid HardwareCommunicationStopped = new Guid("63EA1F06-5A83-4F39-9FAB-49959FDE7B66");

	public static readonly Guid DatabaseDiskAvailableDriver = new Guid("6B64E7EB-CC17-4309-95A1-5A75C441DF61");

	public static readonly Guid DatabaseDiskUnavailableDriver = new Guid("9D4EC252-152B-4050-A6D5-BF2499DD32A1");

	public static readonly Guid ArchiveDiskAvailableDriver = new Guid("B9AB9D96-8AB2-4A92-B502-37FE7C229A73");

	public static readonly Guid ArchiveDiskUnavailableDriver = new Guid("68C7E212-DC02-4F4A-B193-32CBAE96C182");

	public static readonly Guid DatabaseDiskFullDriver = new Guid("9A57EBBE-16DD-4D0E-82A7-AAAB302FB027");

	public static readonly Guid DatabaseFullAutoArchiveDriver = new Guid("03C07623-2064-4A18-882E-F839071CC0B3");

	public static readonly Guid DatabaseDeleteBeforeSetRetentionTimeDriver = new Guid("6A59A380-B037-43FA-A783-A57C80BCE4A6");

	public static readonly Guid DatabaseDeleteBeforeSetRetentionSizeDriver = new Guid("60CA79ED-D482-4EA1-AC62-DBC12526E734");

	public static readonly Guid FailoverStarted = new Guid("F951B1F0-2FED-48F7-88D3-49EB5999C923");

	public static readonly Guid FailoverStopped = new Guid("C7B9227A-8E11-451B-BC9C-8EA659731012");

	public static readonly Guid ResetDeviceConnection = new Guid("6F26B774-1757-4914-A278-3975E5EA4EAB");

	public static readonly Guid SystemMonitorCpuUsageCritical = new Guid("1B36A57E-B656-402E-81E2-E4B873201F06");

	public static readonly Guid SystemMonitorCpuUsageWarning = new Guid("8E98B3E2-D0EA-4608-AD4D-66FCBB422A62");

	public static readonly Guid SystemMonitorCpuUsageNormal = new Guid("727A8D92-75FD-424F-A2D7-EDDBBBC84305");

	public static readonly Guid SystemMonitorMemoryUsageCritical = new Guid("363741CF-4D7A-426D-9FE2-A201C4DB51C0");

	public static readonly Guid SystemMonitorMemoryUsageWarning = new Guid("0C5B3A2D-F60F-4CF0-A99C-1D62DCAEE5B4");

	public static readonly Guid SystemMonitorMemoryUsageNormal = new Guid("2457EF7A-5742-4550-9948-AD5CC51D0645");

	public static readonly Guid SystemMonitorUsedSpaceCritical = new Guid("766CF688-F3E2-4D2F-8D08-012193783739");

	public static readonly Guid SystemMonitorUsedSpaceWarning = new Guid("E68C0857-8D86-4190-B528-96AF21BE0F62");

	public static readonly Guid SystemMonitorUsedSpaceNormal = new Guid("C8E0A296-43C7-48CC-825D-74B90B6CBDE6");

	public static readonly Guid SystemMonitorLiveFpsCritical = new Guid("A4CC82CE-B91D-4CC3-96B0-75B527C5F8F4");

	public static readonly Guid SystemMonitorLiveFpsWarning = new Guid("54CDA1C8-FB7F-4DAB-B506-B0AD707F2B86");

	public static readonly Guid SystemMonitorLiveFpsNormal = new Guid("E14E849F-7355-4D03-97FE-77C292FE01C7");

	public static readonly Guid SystemMonitorRecordingFpsCritical = new Guid("7A60A969-E2B9-4F47-84E7-A2F6D07C56FD");

	public static readonly Guid SystemMonitorRecordingFpsWarning = new Guid("C93E7128-BD54-43AA-9FB2-971E7926D14B");

	public static readonly Guid SystemMonitorRecordingFpsNormal = new Guid("839754E6-82AF-44FC-9E2F-437413D602D6");

	public static readonly Guid SystemMonitorFreeSpaceCritical = new Guid("F80AF822-6A89-4C2F-AB38-B55D0D2CD13A");

	public static readonly Guid SystemMonitorFreeSpaceWarning = new Guid("B3862572-A265-4A2D-B847-1BE7AFE5610E");

	public static readonly Guid SystemMonitorFreeSpaceNormal = new Guid("C9FFDF1E-3766-49C8-BC84-91E95C2E5225");

	public static readonly Guid SystemMonitorRetentionTimeCritical = new Guid("D1603FBE-4BB8-49EB-9928-0E8EB3C736CE");

	public static readonly Guid SystemMonitorRetentionTimeWarning = new Guid("5851E907-35CD-480D-9198-669E9BD8610A");

	public static readonly Guid SystemMonitorRetentionTimeNormal = new Guid("11C329C1-B36E-4F3F-9AB9-9B3A462DC0DA");

	public static readonly Guid SystemMonitorServiceAvailableCritical = new Guid("2FE3A39D-7ACF-436C-9552-67FDF9978A69");

	public static readonly Guid SystemMonitorServiceAvailableNormal = new Guid("6EF169EC-4B56-48E4-A8AA-65A07D0C35DC");

	public static readonly Guid AutomaticActivationFailed = new Guid("D85E2670-F19E-4932-B217-D8AAFFAFF4A3");

	public static string GetEventName(Guid eventId)
	{
		if (EventMapper.TryConvertStatusEventGuidToBaseEventMessageId(eventId, out var eventName))
		{
			return eventName;
		}
		if (EventIdsAndEventNames.Value.TryGetValue(eventId, out eventName))
		{
			return eventName;
		}
		return eventId.ToString();
	}

	public static IEnumerable<Guid> GetAllEvents()
	{
		return EventIdsAndEventNames.Value.Keys;
	}

	private static Dictionary<Guid, string> CreateFieldNameDictionary()
	{
		return (from fi in typeof(KnownStatusEvents).GetFields(BindingFlags.Static | BindingFlags.Public)
			where fi.FieldType == typeof(Guid)
			select fi).ToDictionary((FieldInfo fi) => (Guid)fi.GetValue(null), (FieldInfo fi) => fi.Name);
	}
}
