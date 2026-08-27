using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules;

public static class ConfigurationItemConditionsHelper
{
	public static readonly Dictionary<DayOfWeek, string> _daysOfWeekNamesByValue = new Dictionary<DayOfWeek, string>
	{
		{
			DayOfWeek.Monday,
			ConfigurationApiResources.PropertyDayTypeEnumMonday
		},
		{
			DayOfWeek.Tuesday,
			ConfigurationApiResources.PropertyDayTypeEnumTuesday
		},
		{
			DayOfWeek.Wednesday,
			ConfigurationApiResources.PropertyDayTypeEnumWednesday
		},
		{
			DayOfWeek.Thursday,
			ConfigurationApiResources.PropertyDayTypeEnumThursday
		},
		{
			DayOfWeek.Friday,
			ConfigurationApiResources.PropertyDayTypeEnumFriday
		},
		{
			DayOfWeek.Saturday,
			ConfigurationApiResources.PropertyDayTypeEnumSaturday
		},
		{
			DayOfWeek.Sunday,
			ConfigurationApiResources.PropertyDayTypeEnumSunday
		}
	};

	public static Property GetDaysOfWeekConditionProperty(string propertyName, string displayName, string translationId, string values)
	{
		Dictionary<string, string> valueTypeInfos = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToDictionary((DayOfWeek t) => t.ToString(), (DayOfWeek t) => _daysOfWeekNamesByValue[t]);
		return GeneralPropertyDefinitions.GetListProperty(values, propertyName, displayName, translationId, isSettable: true, valueTypeInfos);
	}
}
