using System.Collections.Generic;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules;

public static class Constants
{
	public static class Properties
	{
		public static class Frequency
		{
			public static class Daily
			{
				public static readonly string Prefix = "Schedule.Frequency.Daily";

				public static readonly string Every = Prefix + ".Every";
			}

			public static class Weekly
			{
				public static readonly string Prefix = "Schedule.Frequency.Weekly";

				public static readonly string Every = Prefix + ".Every";

				public static readonly string Days = Prefix + ".Days";
			}

			public static class Monthly
			{
				public static readonly string Prefix = "Schedule.Frequency.Monthly";

				public static readonly string Every = Prefix + ".Every";

				public static readonly string Day = Prefix + ".Day";
			}

			public static class MonthlyRelative
			{
				public static readonly string Prefix = "Schedule.Frequency.MonthlyRelative";

				public static readonly string Every = Prefix + ".Every";

				public static readonly string Day = Prefix + ".Day";

				public static readonly string Offset = Prefix + ".Offset";
			}

			public const string Type = "Schedule.Frequency";

			public const string EveryPostfix = "Every";
		}

		public static class DailyFrequency
		{
			public static class Once
			{
				public static readonly string Prefix = "Schedule.DailyFrequency.Once";

				public static readonly string StartTime = Prefix + ".StartTime";
			}

			public static class Recurring
			{
				public static readonly string Prefix = "Schedule.DailyFrequency.Recurring";

				public static readonly string StartTime = Prefix + ".StartTime";

				public static readonly string EndTime = Prefix + ".EndTime";

				public static readonly string Every = Prefix + ".Every";
			}

			public const string Type = "Schedule.DailyFrequency";

			public const string StartTimePostfix = "StartTime";

			public const string EndTimePostFix = "EndTime";

			public const string DefaultStartTime = "00:00";

			public const string DefaultEndTime = "23:59";
		}

		public const string Id = "Id";

		public const string Name = "Name";

		public const string RuleToRemovePath = "RemoveRulePath";

		public const string StartRuleType = "StartRuleType";

		public const string StopRuleType = "StopRuleType";

		public const string StartEventGroup = "StartEventGroup";

		public const string StartEventType = "StartEventType";

		public const string StartEventSources = "StartEventSources";

		public const string StopEventGroup = "StopEventGroup";

		public const string StopEventType = "StopEventType";

		public const string StopEventSources = "StopEventSources";

		public const string StartActions = "StartActions";

		public const string StopActions = "StopActions";

		public const string CameraSelection = "ItemSelectionCamera";

		public const string WithinTimeProfile = "WithinTimeProfile";

		public const string OutsideTimeProfile = "OutsideTimeProfile";

		public const string TimeBetween = "TimeOfDayBetween";

		public const string DaysOfWeek = "DaysOfWeek";

		public const string StartTimePostfix = "StartTime";

		public const string EndTimePostfix = "EndTime";

		public const string Always = "Always";

		public const string TimeProfilePostfix = "TimeProfile";

		public const string TimeBetweenDefaultTime = "08:00:00";

		public const string DaysOfWeekPostfix = "Days";

		public const string DelayedStopTriggerTime = "TimerTime";

		public const string WithinWindow = "WithinWindow";

		public const string WithinWindowPostfix = "Window";

		public const string FailoverActive = "FailoverActive";

		public const string FailoverInactive = "FailoverInactive";

		public const string DaysOfWeekParameterPropertyKey = "DaysOfWeek.Days";

		public const string WithinTimeProfileParameterPropertyKey = "WithinTimeProfile.TimeProfile";

		public const string OutsideTimeProfileParameterPropertyKey = "OutsideTimeProfile.TimeProfile";

		public const string TimeBetweenStartParameterPropertyKey = "TimeOfDayBetween.StartTime";

		public const string TimeBetweenStopParameterPropertyKey = "TimeOfDayBetween.EndTime";

		public const string WithinWindowParameterPropertyKey = "WithinWindow.Window";
	}

	public const string ItemType = "Rule";

	public const string FolderType = "RuleFolder";

	public const string StartActionPrefix = "Start";

	public const string StopActionPrefix = "Stop";

	public const string InvalidValueFormat = "Invalid {0}: '{1}'";

	public const string MissingValueFormat = "{0} is empty";

	public const string MissingPropertyFormat = "The key: {0} is missing";

	public static Dictionary<string, string> RuleTypeEnumValues = new Dictionary<string, string>
	{
		{
			"TimeInterval",
			ConfigurationApiResources.RuleTypeInterval
		},
		{
			"Event",
			ConfigurationApiResources.RuleTypeEvent
		}
	};
}
