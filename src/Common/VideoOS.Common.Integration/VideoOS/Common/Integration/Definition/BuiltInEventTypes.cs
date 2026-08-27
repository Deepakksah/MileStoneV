using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration.Definition;

public sealed class BuiltInEventTypes
{
	public class SNMPMapInformation
	{
		public string ID { get; set; }

		public bool Recorder { get; set; }

		public bool Server { get; set; }

		public SNMPMapInformation(string SNMPId, bool Recorder = false, bool Server = false)
		{
			ID = SNMPId;
			this.Recorder = Recorder;
			this.Server = Server;
		}
	}

	public class SNMPMappingAttribute : Attribute
	{
		public SNMPMapInformation Info;

		public SNMPMappingAttribute(bool SNMPEnabled, string msg = "")
		{
			Info = new SNMPMapInformation(string.Empty);
		}

		public SNMPMappingAttribute(string SNMPId, bool Recorder = false, bool Server = false, string msg = "")
		{
			Info = new SNMPMapInformation(SNMPId, Recorder, Server);
		}
	}

	public static class EventIDs
	{
		[SNMPMapping("19", true, false, "")]
		public static readonly Guid PtzManualSessionStarted = new Guid("2EEB582A-1506-4D73-96C9-AD356ACF85FB");

		[SNMPMapping("20", true, false, "")]
		public static readonly Guid PtzManualSessionStopped = new Guid("EED47283-694F-4683-9E4D-18E7D93CF796");

		[SNMPMapping("14", true, false, "")]
		public static readonly Guid LiveClientFeedRequested = new Guid("EEDA47FF-4F3D-459E-8143-69896C7C74AD");

		[SNMPMapping("15", true, false, "")]
		public static readonly Guid LiveClientFeedTerminated = new Guid("66BDD008-B856-44B6-B385-B1F6C3F76F1B");

		[SNMPMapping("2", true, false, "")]
		public static readonly Guid MotionStarted = new Guid("6EB95DD6-7CCC-4BCE-99F8-AF0D0B582D77");

		[SNMPMapping("3", true, false, "")]
		public static readonly Guid MotionStopped = new Guid("6F55A7A7-D21C-4629-AC18-AF1975E395A2");

		[SNMPMapping("35", true, false, "")]
		public static readonly Guid BookmarkReferenceRequested = new Guid("7CFC6709-2C16-405f-9182-0BDB6F9F7E43");

		[SNMPMapping("53", true, false, "")]
		public static readonly Guid MarkedDataReferenceRequested = new Guid("3A82E9B3-9FFF-4BA1-9593-9927B0821076");

		[SNMPMapping("54", true, false, "")]
		public static readonly Guid RecordingsEvidenceLocked = new Guid("54DC549C-A007-47BD-8060-87BBAE9497B0");

		[SNMPMapping("55", true, false, "")]
		public static readonly Guid RecordingsEvidenceUnlocked = new Guid("3334B3E9-A135-497A-A0B9-9036533C2173");

		[SNMPMapping("56", true, false, "")]
		public static readonly Guid EvidenceLockChanged = new Guid("6EAB8BA5-6CAC-4636-BB2E-0A03875DF41A");

		[SNMPMapping("21", true, false, "")]
		public static readonly Guid RecordingStarted = new Guid("4577F552-765A-438c-BC7D-E5FF1F754BC3");

		[SNMPMapping("22", true, false, "")]
		public static readonly Guid RecordingStopped = new Guid("79A94F89-92DE-4fca-8A43-5561D407423D");

		[SNMPMapping("25", true, false, "")]
		public static readonly Guid DatabaseRepair = new Guid("B6C4B695-7404-414D-A229-33BF83FFE255");

		[SNMPMapping("29", true, false, "")]
		public static readonly Guid DatabaseDiskFull = new Guid("A4935AA3-C4B6-42A3-B4C5-6D4C20B043CF");

		[SNMPMapping("31", true, false, "")]
		public static readonly Guid DatabaseDiskUnavailable = new Guid("29986AA1-924B-4c8d-AAC7-A876883DC82C");

		[SNMPMapping("30", true, false, "")]
		public static readonly Guid DatabaseDiskAvailable = new Guid("FAB03C4D-0589-48c8-9328-E28B76C95AF0");

		[SNMPMapping("27", true, false, "")]
		public static readonly Guid ArchiveDiskUnavailable = new Guid("72C29AAE-DDB2-46df-B6FD-7F728A85F292");

		[SNMPMapping("26", true, false, "")]
		public static readonly Guid ArchiveDiskAvailable = new Guid("B7F0003B-1392-450e-B6CC-9BBE6E7DF065");

		[SNMPMapping("36", true, false, "")]
		public static readonly Guid DatabaseFullAutoArchive = new Guid("1FDE8231-F0E4-4919-AED1-B2B142D11E54");

		[SNMPMapping("37", true, false, "")]
		public static readonly Guid ArchiveNotFinished = new Guid("4DA7CFF5-BF0D-46D9-8B35-85F3288904F4");

		[SNMPMapping("57", true, false, "")]
		public static readonly Guid ManualRecordingStarted = new Guid("D740951A-984D-4049-99B5-25F5CBABD21F");

		[SNMPMapping("58", true, false, "")]
		public static readonly Guid ManualRecordingStopped = new Guid("A9C1F6FA-ADD6-4BF4-BA2D-A0CEDDB73C51");

		[SNMPMapping("61", true, false, "")]
		public static readonly Guid DatabaseDiskFullAutoArchive = new Guid("59ED52FD-A07D-4EDD-8BD6-24D0420CEA34");

		[SNMPMapping("60", true, false, "")]
		public static readonly Guid DatabaseDeleteBeforeSetRetentionTime = new Guid("1DD4F1AB-3CDA-4F5C-93D7-B0C78D2BC6AD");

		[SNMPMapping("59", true, false, "")]
		public static readonly Guid DatabaseDeleteBeforeSetRetentionSize = new Guid("023D4D8D-5848-43CF-8F94-4F7F230966EF");

		[SNMPMapping("105", true, false, "")]
		public static readonly Guid DatabaseArchivingSessionStarted = new Guid("9EAF1770-42ED-4C5B-A06F-95722721883D");

		[SNMPMapping("106", true, false, "")]
		public static readonly Guid DatabaseArchivingSessionEnded = new Guid("B73F8241-3320-45CF-89F3-80E4A090E487");

		[SNMPMapping("107", true, false, "")]
		public static readonly Guid DatabaseRepairSessionStarted = new Guid("54D7CE97-2343-40D5-92D2-350F5DFB3029");

		[SNMPMapping("108", true, false, "")]
		public static readonly Guid DatabaseRepairSessionEnded = new Guid("2D7FBAD3-54DB-4155-B471-3BA9ECB5CBAB");

		[SNMPMapping(false, "Not accessible from UI yet")]
		public static readonly Guid HardwareSettingsChanged = new Guid("A600C492-11C8-4D65-92DB-6D46209CE9EA");

		[SNMPMapping(false, "Not accessible from UI yet")]
		public static readonly Guid HardwareSettingsChangedError = new Guid("829A93AB-1AC0-4AEB-BDA0-ED8F6D8F514B");

		[SNMPMapping("4", true, false, "")]
		public static readonly Guid InputActivated = new Guid("836CA458-A833-4742-8EE0-64B2380984BD");

		[SNMPMapping("6", true, false, "")]
		public static readonly Guid InputDeactivated = new Guid("8666E3DC-57A7-4F38-9611-51D774EE7358");

		[SNMPMapping("5", true, false, "")]
		public static readonly Guid InputChanged = new Guid("FDA121B3-2070-4F31-A086-9F74E16580C0");

		[SNMPMapping(false, "Not accessible from UI yet")]
		public static readonly Guid InputSensor = new Guid("F84C8443-BFE3-4ECA-B69B-07DAEBC63A02");

		[SNMPMapping("16", true, false, "")]
		public static readonly Guid OutputActivated = new Guid("7A78F5BB-D8C3-4997-89B7-CAE72713B7DB");

		[SNMPMapping("18", true, false, "")]
		public static readonly Guid OutputDeactivated = new Guid("35742498-BCC5-4F0A-9800-827C9388D1CD");

		[SNMPMapping("17", true, false, "")]
		public static readonly Guid OutputChanged = new Guid("672F9F80-55FE-496F-BEE4-6C870BAB5064");

		[SNMPMapping("12", true, false, "")]
		public static readonly Guid FeedOverflowStarted = new Guid("536AD730-05AE-42CD-B14C-07B07C293E79");

		[SNMPMapping("13", true, false, "")]
		public static readonly Guid FeedOverflowStopped = new Guid("247FEF11-15DB-4C99-99DB-DE5475EAE443");

		[SNMPMapping("9", true, false, "")]
		public static readonly Guid CommunicationError = new Guid("A334AF1C-4B4B-4957-9E5F-AB8CA07FEAB6");

		[SNMPMapping("10", true, false, "")]
		public static readonly Guid CommunicationStarted = new Guid("DD3E6464-7DC0-405A-A92F-6150587563E8");

		[SNMPMapping("11", true, false, "")]
		public static readonly Guid CommunicationStopped = new Guid("0EE90664-2924-42A0-A816-4129D0ECABDC");

		[SNMPMapping("23", true, false, "")]
		public static readonly Guid DeviceSettingsChanged = new Guid("4C8BE5ED-80FA-43E7-BF5B-4E7570AD0025");

		[SNMPMapping("24", true, false, "")]
		public static readonly Guid DeviceSettingsChangedError = new Guid("87BE2A0D-C33B-4B5F-AE1F-AFA6770A2497");

		[SNMPMapping("7", true, false, "")]
		public static readonly Guid MotionStartedDriver = new Guid("A7BD4B94-6EB0-4B5D-92D9-23F69BD23824");

		[SNMPMapping("8", true, false, "")]
		public static readonly Guid MotionStoppedDriver = new Guid("E3DD8ED6-CF93-410D-818A-6C474414D885");

		[SNMPMapping("48", true, false, "")]
		public static readonly Guid ArchivingFailedDriver = new Guid("036EE274-0E74-49CE-AC16-7410D4A6D9C5");

		[SNMPMapping("41", true, false, "")]
		public static readonly Guid DiskSpaceLowDriver = new Guid("37517225-05C8-44A2-A478-5B28B6BE0E07");

		[SNMPMapping("49", true, false, "")]
		public static readonly Guid DeviceWriteFailedDriver = new Guid("28666A13-4181-4DDC-B63D-F677A60566C6");

		[SNMPMapping("50", true, false, "")]
		public static readonly Guid FeedOverflowStartedDriver = new Guid("3F437517-EE1B-4BF3-AEC0-2A6C890C1F31");

		[SNMPMapping("51", true, false, "")]
		public static readonly Guid FeedOverflowStoppedDriver = new Guid("1CF31257-D98F-4854-9396-34BC61EFCA9C");

		[SNMPMapping("52", true, false, "")]
		public static readonly Guid DeviceSettingsChangedDriver = new Guid("DDEF0EEB-DD6E-49F4-A37D-1314877D3BCE");

		[SNMPMapping(false, "")]
		public static readonly Guid DeviceLicenseRemoved = new Guid("65D84F0C-F671-4F99-A502-C0F470F72131");

		[SNMPMapping(false, "")]
		public static readonly Guid CommunicationStartedDriver = new Guid("13D423D4-22FE-48C5-B02E-0EBE87A58075");

		[SNMPMapping(false, "")]
		public static readonly Guid CommunicationStoppedDriver = new Guid("21CD9F40-B0C6-4BE3-9DBE-BD920BAD84EC");

		[SNMPMapping(false, "")]
		public static readonly Guid CommunicationErrorDriver = new Guid("90068CF3-EE96-448C-977B-38770DEC8DCC");

		[SNMPMapping("38", true, false, "")]
		public static readonly Guid HardwareCommunicationError = new Guid("6BAAD64B-C395-4F52-B6A3-DD1B64AA2F0F");

		[SNMPMapping("39", true, false, "")]
		public static readonly Guid HardwareCommunicationStarted = new Guid("0553C396-5E16-4C22-B3D1-F548E42DFBB4");

		[SNMPMapping("40", true, false, "")]
		public static readonly Guid HardwareCommunicationStopped = new Guid("63EA1F06-5A83-4F39-9FAB-49959FDE7B66");

		[SNMPMapping("42", true, false, "")]
		public static readonly Guid DatabaseDiskAvailableDriver = new Guid("6B64E7EB-CC17-4309-95A1-5A75C441DF61");

		[SNMPMapping("43", true, false, "")]
		public static readonly Guid DatabaseDiskUnavailableDriver = new Guid("9D4EC252-152B-4050-A6D5-BF2499DD32A1");

		[SNMPMapping("44", true, false, "")]
		public static readonly Guid ArchiveDiskAvailableDriver = new Guid("B9AB9D96-8AB2-4A92-B502-37FE7C229A73");

		[SNMPMapping("45", true, false, "")]
		public static readonly Guid ArchiveDiskUnavailableDriver = new Guid("68C7E212-DC02-4F4A-B193-32CBAE96C182");

		[SNMPMapping("46", true, false, "")]
		public static readonly Guid DatabaseDiskFullDriver = new Guid("9A57EBBE-16DD-4D0E-82A7-AAAB302FB027");

		[SNMPMapping("47", true, false, "")]
		public static readonly Guid DatabaseFullAutoArchiveDriver = new Guid("03C07623-2064-4A18-882E-F839071CC0B3");

		[SNMPMapping("64", true, false, "")]
		public static readonly Guid DatabaseDiskFullAutoArchiveDriver = new Guid("BF7FAA15-06E3-47FD-A450-913853D74F5F");

		[SNMPMapping("63", true, false, "")]
		public static readonly Guid DatabaseDeleteBeforeSetRetentionTimeDriver = new Guid("6A59A380-B037-43FA-A783-A57C80BCE4A6");

		[SNMPMapping("62", true, false, "")]
		public static readonly Guid DatabaseDeleteBeforeSetRetentionSizeDriver = new Guid("60CA79ED-D482-4EA1-AC62-DBC12526E734");

		[SNMPMapping("33", true, true, "")]
		public static readonly Guid FailoverStarted = new Guid("F951B1F0-2FED-48F7-88D3-49EB5999C923");

		[SNMPMapping("34", true, true, "")]
		public static readonly Guid FailoverStopped = new Guid("C7B9227A-8E11-451B-BC9C-8EA659731012");

		[SNMPMapping("101", false, true, "")]
		public static readonly Guid FailoverEncryptedCommunicationError = new Guid("9826E6DA-C34E-475E-958F-93925A3324BC");

		[SNMPMapping("102", false, true, "")]
		public static readonly Guid FailoverAuthorizationError = new Guid("D7940922-5CB4-40E4-AC62-7CE361DA8298");

		[SNMPMapping(false, "")]
		public static readonly Guid ResetDeviceConnection = new Guid("6F26B774-1757-4914-A278-3975E5EA4EAB");

		[SNMPMapping("88", false, true, "")]
		public static readonly Guid SystemMonitorCpuUsageCritical = new Guid("1B36A57E-B656-402E-81E2-E4B873201F06");

		[SNMPMapping("67", false, true, "")]
		public static readonly Guid SystemMonitorCpuUsageWarning = new Guid("8E98B3E2-D0EA-4608-AD4D-66FCBB422A62");

		[SNMPMapping("66", false, true, "")]
		public static readonly Guid SystemMonitorCpuUsageNormal = new Guid("727A8D92-75FD-424F-A2D7-EDDBBBC84305");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorCpuUsageUndefined = new Guid("D003BAE9-0E12-413F-A57B-673A83C56D1A");

		[SNMPMapping("77", false, true, "")]
		public static readonly Guid SystemMonitorMemoryUsageCritical = new Guid("363741CF-4D7A-426D-9FE2-A201C4DB51C0");

		[SNMPMapping("79", false, true, "")]
		public static readonly Guid SystemMonitorMemoryUsageWarning = new Guid("0C5B3A2D-F60F-4CF0-A99C-1D62DCAEE5B4");

		[SNMPMapping("78", false, true, "")]
		public static readonly Guid SystemMonitorMemoryUsageNormal = new Guid("2457EF7A-5742-4550-9948-AD5CC51D0645");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorMemoryUsageUndefined = new Guid("2C3F82DC-1D15-4A60-9EAB-EB517592861D");

		[SNMPMapping("83", false, true, "")]
		public static readonly Guid SystemMonitorUsedSpaceCritical = new Guid("766CF688-F3E2-4D2F-8D08-012193783739");

		[SNMPMapping("85", false, true, "")]
		public static readonly Guid SystemMonitorUsedSpaceWarning = new Guid("E68C0857-8D86-4190-B528-96AF21BE0F62");

		[SNMPMapping("84", false, true, "")]
		public static readonly Guid SystemMonitorUsedSpaceNormal = new Guid("C8E0A296-43C7-48CC-825D-74B90B6CBDE6");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorUsedSpaceUndefined = new Guid("0DD65A18-DFD1-493A-BA6A-F15D37FFE037");

		[SNMPMapping("71", false, true, "")]
		public static readonly Guid SystemMonitorLiveFpsCritical = new Guid("A4CC82CE-B91D-4CC3-96B0-75B527C5F8F4");

		[SNMPMapping("73", false, true, "")]
		public static readonly Guid SystemMonitorLiveFpsWarning = new Guid("54CDA1C8-FB7F-4DAB-B506-B0AD707F2B86");

		[SNMPMapping("72", false, true, "")]
		public static readonly Guid SystemMonitorLiveFpsNormal = new Guid("E14E849F-7355-4D03-97FE-77C292FE01C7");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorLiveFpsUndefined = new Guid("FE3D0779-1F51-486E-AAB3-1D12A4FCB23C");

		[SNMPMapping("80", false, true, "")]
		public static readonly Guid SystemMonitorRecordingFpsCritical = new Guid("7A60A969-E2B9-4F47-84E7-A2F6D07C56FD");

		[SNMPMapping("82", false, true, "")]
		public static readonly Guid SystemMonitorRecordingFpsWarning = new Guid("C93E7128-BD54-43AA-9FB2-971E7926D14B");

		[SNMPMapping("81", false, true, "")]
		public static readonly Guid SystemMonitorRecordingFpsNormal = new Guid("839754E6-82AF-44FC-9E2F-437413D602D6");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorRecordingFpsUndefined = new Guid("C39716BF-9501-4FC4-98D6-CD4CA1EA11BD");

		[SNMPMapping("68", false, true, "")]
		public static readonly Guid SystemMonitorFreeSpaceCritical = new Guid("F80AF822-6A89-4C2F-AB38-B55D0D2CD13A");

		[SNMPMapping("70", false, true, "")]
		public static readonly Guid SystemMonitorFreeSpaceWarning = new Guid("B3862572-A265-4A2D-B847-1BE7AFE5610E");

		[SNMPMapping("69", false, true, "")]
		public static readonly Guid SystemMonitorFreeSpaceNormal = new Guid("C9FFDF1E-3766-49C8-BC84-91E95C2E5225");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorFreeSpaceUndefined = new Guid("07EEFA87-32B9-49B2-883F-05B75B11877A");

		[SNMPMapping("74", false, true, "")]
		public static readonly Guid SystemMonitorRetentionTimeCritical = new Guid("D1603FBE-4BB8-49EB-9928-0E8EB3C736CE");

		[SNMPMapping("76", false, true, "")]
		public static readonly Guid SystemMonitorRetentionTimeWarning = new Guid("5851E907-35CD-480D-9198-669E9BD8610A");

		[SNMPMapping("75", false, true, "")]
		public static readonly Guid SystemMonitorRetentionTimeNormal = new Guid("11C329C1-B36E-4F3F-9AB9-9B3A462DC0DA");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorRetentionTimeUndefined = new Guid("5AA16452-3CB3-4986-BB8C-87BF77C2AC11");

		[SNMPMapping("86", false, true, "")]
		public static readonly Guid SystemMonitorServiceAvailableCritical = new Guid("2FE3A39D-7ACF-436C-9552-67FDF9978A69");

		[SNMPMapping("87", false, true, "")]
		public static readonly Guid SystemMonitorServiceAvailableNormal = new Guid("6EF169EC-4B56-48E4-A8AA-65A07D0C35DC");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemMonitorServiceAvailableUndefined = new Guid("1254477F-6650-491B-A204-DC463FF75F4E");

		[SNMPMapping("89", false, true, "")]
		public static readonly Guid SystemMonitorGPUDecodingCritical = new Guid("B4D94AFC-9EFE-4083-89AF-078E6AB871E4");

		[SNMPMapping("90", false, true, "")]
		public static readonly Guid SystemMonitorGPUDecodingWarning = new Guid("3526491F-9CAC-47FB-BEC2-F9C016C26CD5");

		[SNMPMapping("91", false, true, "")]
		public static readonly Guid SystemMonitorGPUDecodingNormal = new Guid("94831D23-3267-46E2-811F-D1DA115D63CB");

		[SNMPMapping("92", false, true, "")]
		public static readonly Guid SystemMonitorGPUDecodingUndefined = new Guid("E4E414A7-2788-4F76-B2D9-A67B73929453");

		[SNMPMapping("93", false, true, "")]
		public static readonly Guid SystemMonitorGPURenderingCritical = new Guid("CED4736D-B018-4116-A9F6-1E679741CD77");

		[SNMPMapping("94", false, true, "")]
		public static readonly Guid SystemMonitorGPURenderingWarning = new Guid("2FEBE17F-CB67-4A44-B6D5-D3C5828BF770");

		[SNMPMapping("95", false, true, "")]
		public static readonly Guid SystemMonitorGPURenderingNormal = new Guid("BDC4E2BB-C011-4094-994A-AAA9861163D4");

		[SNMPMapping("96", false, true, "")]
		public static readonly Guid SystemMonitorGPURenderingUndefined = new Guid("44ECFD80-C265-4545-896A-BAFC0403FA94");

		[SNMPMapping("97", false, true, "")]
		public static readonly Guid SystemMonitorGPUMemoryCritical = new Guid("CBE4E664-8AD6-404F-9EA3-63633EBB1273");

		[SNMPMapping("98", false, true, "")]
		public static readonly Guid SystemMonitorGPUMemoryWarning = new Guid("48248FDA-AE5B-40BA-99EA-A02B7F39235B");

		[SNMPMapping("99", false, true, "")]
		public static readonly Guid SystemMonitorGPUMemoryNormal = new Guid("CB218E23-934F-4E65-A0A6-11F1822C0F99");

		[SNMPMapping("100", false, true, "")]
		public static readonly Guid SystemMonitorGPUMemoryUndefined = new Guid("21704364-3869-4A2C-AED9-C5C99FE6FCAB");

		[SNMPMapping("65", false, true, "")]
		public static readonly Guid AutomaticActivationFailed = new Guid("D85E2670-F19E-4932-B217-D8AAFFAFF4A3");

		[SNMPMapping(false, "")]
		public static readonly Guid ScheduledPasswordChangeStarted = new Guid("B08BFF9D-A4A4-4760-8C99-B3DA8C8B6A1B");

		[SNMPMapping(false, "")]
		public static readonly Guid ScheduledPasswordChangeCompletedSuccessfully = new Guid("C83A58A9-3272-4E30-BC6D-5A8393FB1DCE");

		[SNMPMapping(false, "")]
		public static readonly Guid ScheduledPasswordChangeCompletedWithErrors = new Guid("F0C89E43-4757-423A-A9E9-2426F228A2E5");

		[SNMPMapping(false, "")]
		public static readonly Guid ManagementServerFailoverNodeStarted = new Guid("FB78A562-B1F4-4C39-B68D-05B87488E858");

		[SNMPMapping(false, "")]
		public static readonly Guid SystemConfigurationChanged = new Guid("06C5010D-11F2-4D70-BD79-4CEC3A20D589");

		[SNMPMapping(false, "")]
		public static readonly Guid MipItemPositionChanged = new Guid("A7255B28-D427-4B55-9109-CEEEFC4542B8");

		[SNMPMapping(false, "")]
		public static readonly Guid SmartWallChanged = new Guid("629405da-f906-44d6-a3d1-57d3de1d1eb8");
	}

	private static Lazy<Dictionary<Guid, SNMPMapInformation>> _knownIds = new Lazy<Dictionary<Guid, SNMPMapInformation>>(() => CreateKnownIds());

	private static Dictionary<Guid, SNMPMapInformation> KnownIds => _knownIds.Value;

	private static SNMPMapInformation GetSNMPId(FieldInfo field)
	{
		return ((SNMPMappingAttribute)field.GetCustomAttribute(typeof(SNMPMappingAttribute))).Info;
	}

	private static Dictionary<Guid, SNMPMapInformation> CreateKnownIds()
	{
		return (from fieldinfo in typeof(EventIDs).GetFields(BindingFlags.Static | BindingFlags.Public)
			where fieldinfo.FieldType == typeof(Guid)
			select new
			{
				eventId = (Guid)fieldinfo.GetValue(null),
				snmpInfo = GetSNMPId(fieldinfo)
			}).ToDictionary(a => a.eventId, a => a.snmpInfo);
	}

	public static bool IsKnownId(Guid id)
	{
		return KnownIds.ContainsKey(id);
	}

	public static string GetUnknownEventName(Guid eventID)
	{
		string unknownEvent = Resources.UnknownEvent;
		return string.Format(CultureInfo.InvariantCulture, unknownEvent, eventID.ToString());
	}

	public static string GetRecorderSNMPId(Guid eventID)
	{
		if (KnownIds.TryGetValue(eventID, out var value) && value.Recorder)
		{
			return value.ID;
		}
		return string.Empty;
	}

	public static string GetServerSNMPId(Guid eventID)
	{
		if (KnownIds.TryGetValue(eventID, out var value) && value.Server)
		{
			return value.ID;
		}
		return string.Empty;
	}
}
