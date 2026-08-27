using System;

namespace VideoOS.Central.Util;

public static class Constants
{
	public static readonly Guid AlarmRuleType_AlarmDefinition = new Guid("4547f085-8b68-4639-826f-8722afb0ffc5");

	public static readonly Guid SoundNotification_Asterisk = new Guid("7369ec94-aebd-461c-98c7-f316875b4b46");

	public static readonly Guid SoundNotification_Beep = new Guid("e5e42849-5095-4e35-9ae9-745316cc0789");

	public static readonly Guid SoundNotification_Exclamation = new Guid("35f31ee4-b145-40c9-8df4-e094502bda36");

	public static readonly Guid SoundNotification_Hand = new Guid("10037f60-987d-4632-a70a-867dd78d060c");

	public static readonly Guid SoundNotification_Question = new Guid("8125f132-6af2-4c77-a6dc-0b3607e66d18");

	public static readonly Guid PriorityHigh = new Guid("8188FF24-B5DA-4C19-9EBF-C1D8FC2CAA75");

	public static readonly Guid PriorityMedium = new Guid("9AD9338B-22BA-4F2E-BF62-E6948AE99BBF");

	public static readonly Guid PriorityLow = new Guid("34F1F987-6854-44FB-88A5-DAA0ADD1E38A");

	public static readonly Guid StateNew = new Guid("60AB3977-090A-4381-A644-8BB51AF2FDAA");

	public static readonly Guid StateInProgress = new Guid("D9D8E200-B335-476B-BD8B-B85B8EFC41B6");

	public static readonly Guid StateOnHold = new Guid("A6B840A5-B48A-49CF-A83C-B2A0B010BEED");

	public static readonly Guid StateClosed = new Guid("E9DEEC24-E845-4946-AEA2-C7C43D916D40");

	public static readonly Guid SystemEventsGroup = new Guid("B1CA6710-F244-4CE2-8DAF-662E342C405A");

	public static readonly Guid DeviceEventsGroup = new Guid("1EACBCAD-D566-4375-834B-CFBE3D937CAA");

	public static readonly Guid HardwareEventsGroup = new Guid("6B90AEE7-E6A5-4B5C-82AA-1686C19AFE19");

	public static readonly Guid RecordingServerEventsGroup = new Guid("64D2C24B-A92F-48A0-B9CA-707AF7828D67");

	public static readonly Guid ExternalEventsGroup = new Guid("5946B6FA-44D9-4F4C-82BB-46A17B924265");

	public static readonly Guid SystemMonitorEventsGroup = new Guid("2FB8E979-188E-44CC-B1B7-A7ED9C91E5C0");

	public static readonly Guid UnknownEventGroup = Guid.Empty;

	public const string SystemEventType = "System Event";

	public const string SystemAlarmType = "System Alarm";

	public static readonly Guid InternalAccessControlAdminUserName = new Guid("45052A73-09B4-4744-B95A-3128D948675E");

	public static readonly Guid LprServerKind = new Guid("6b1845a7-851f-4f0b-9644-940463a5c2e7");

	public static readonly Guid LprServiceType = new Guid("ba177cb7-5a9b-42b8-988d-cfba91ca44e3");

	public static readonly Guid DefaultRetentionGroupId = new Guid("60CB1591-1424-4A0F-BF5A-A75112DCAED1");

	public const string DefaultRetentionGroupName = "Default";

	public const char Underscore = '_';

	public const char WhiteSpace = ' ';
}
