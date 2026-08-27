namespace VideoOS.Rules.Interfaces;

public static class RuleXmlConstants
{
	public static class Rules
	{
		public const string ElementRule = "Rule";

		public const string ElementStartRule = "StartRule";

		public const string ElementStopRule = "StopRule";

		public const string ElementRuleType = "RuleType";

		public const string ElementEventType = "EventType";

		public const string ElementTimerValue = "TimerValue";

		public const string AttributeType = "type";

		public const string RuleTypeEvent = "Event";

		public const string RuleTypeTimeInterval = "TimeInterval";

		public const string RuleTypeRecurringTime = "RecurringTime";

		public const string ElementSchedule = "Schedule";

		public const string RuleTypeTimer = "Timer";
	}

	public static class Sources
	{
		public const string ElementSources = "Sources";

		public const string ElementSourceList = "SourceList";

		public const string ElementSourceId = "SourceId";

		public const string ElementGroupList = "GroupList";

		public const string ElementGroupId = "GroupId";

		public const string ElementSiteSourceList = "SiteSourceList";

		public const string ElementSiteSourceId = "SiteSourceId";
	}

	public static class Conditions
	{
		public const string ElementConditionsNode = "Conditions";

		public const string ElementTimeConditionNode = "TimeCondition";

		public const string ElementConditionNode = "Condition";

		public const string ElementTokenNode = "Token";

		public const string ElementValueNode = "Value";

		public const string ElementRegionNode = "Region";

		public const string AttributeName = "name";

		public const string AttributeType = "type";

		public const string AttributeDataType = "datatype";

		public const string TokenValue = "Value";

		public const string TokenOperator = "Operator";

		public const string TokenUnaryOperator = "UnaryOperator";

		public const string TokenParenthesis = "Parenthesis";

		public const string TokenFunction = "Function";
	}

	public static class Actions
	{
		public const string ElementActions = "Actions";

		public const string ElementAction = "Action";

		public const string ElementParameter = "Parameter";

		public const string ElementSubRule = "SubRule";

		public const string ElementSubAction = "SubAction";

		public const string AttributeActionName = "name";

		public const string AttributeActionInstanceId = "instanceId";

		public const string AttributeParameterName = "name";

		public const string ActionMissing = "VideoOS.ActionMissing";
	}

	public static class EventGeneratorTypeNames
	{
		public const string System = "System";

		public const string RecorderPlugin = "RecorderPlugin";

		public const string Device = "Device";

		public const string Recorder = "Recorder";

		public const string Server = "Server";

		public const string Timer = "Timer";

		public const string External = "External";

		public const string DriverDevice = "DriverDevice";

		public const string DriverHardware = "DriverHardware";

		public const string Hardware = "Hardware";

		public const string MIPDevice = "MIPDevice";

		public const string MIP = "MIP";

		public const string SystemDashboard = "SystemDashboard";
	}

	public static class SubtypeNames
	{
		public const string Server = "Server";

		public const string Disk = "Disk";

		public const string Device = "Device";

		public const string Storage = "Storage";
	}

	public static class TimeRange
	{
		public const string MetadataKey = "TimeRange";

		public const string XmlRoot = "timeRange";

		public const string XmlStartTime = "startTime";

		public const string XmlEndTime = "endTime";

		public const string UseMetadata = "UseMetadata";
	}

	public static class AudioMessage
	{
		public const string MetadataKey = "AudioMessage";

		public const string UseMetadata = "11111111-1111-1111-1111-111111111111";
	}

	public static class EventType
	{
		public const string ElementId = "Id";

		public const string ElementGenerator = "Generator";

		public const string ElementGroupId = "GroupId";

		public const string ElementGroupName = "GroupName";

		public const string ElementDisplayName = "DisplayName";
	}

	public static class Hardware
	{
		public const string AllHardwareKey = "C5552DB0-A4ED-4BB0-B01C-E96E500D801F";
	}
}
