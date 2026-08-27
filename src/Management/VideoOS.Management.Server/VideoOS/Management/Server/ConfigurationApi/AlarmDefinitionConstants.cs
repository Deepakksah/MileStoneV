using System;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class AlarmDefinitionConstants
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

	public static readonly Guid UnknownEventGroup = Guid.Empty;

	public static readonly Guid AccessControlCategoryEventGroup = new Guid("921F36DC-8D7A-4895-BB9B-265D44F9E784");

	public static readonly Guid AccessControlEventGroup = new Guid("D29800D6-2FE9-4410-90F3-A88BB342AB32");

	public static readonly Guid SystemMonitorEventsGroup = new Guid("2FB8E979-188E-44CC-B1B7-A7ED9C91E5C0");

	public static readonly Guid AnalyticsEventsGroup = new Guid("a96692c8-51b1-4f87-b12c-0d3d9cbfc5a4");

	public static readonly Guid LprEventsGroup = new Guid("6530AFE1-BD9D-447C-A796-380CBAD2D11C");

	public static readonly Guid TransactEventsGroup = new Guid("69d5a520-576c-4040-9fcc-f4442aa21438");

	public static readonly Guid ExternalEventType = new Guid("0FCB1955-0E80-4FD4-A78A-DB47EE89700C");

	public const string SystemEventType = "System Event";

	public const string SystemAlarmType = "System Alarm";

	public static readonly Guid DefaultRetentionGroupId = new Guid("60CB1591-1424-4A0F-BF5A-A75112DCAED1");
}
