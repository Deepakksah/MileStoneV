using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.SharedTimeProfile;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPITimeProfile")]
[EntityName("ConfigAPITimeProfileFolder")]
public class TimeProfileConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string TimeProfileTypeKey = "TimeProfileType";

	public const string AllDayEventKey = "AllDayEvent";

	public const string StartDateTimeKey = "StartDateTime";

	public const string OriginalStartDateTimeKey = "OriginalStartDateTime";

	public const string EndDateTimeKey = "EndDateTime";

	public const string SubjectKey = "Subject";

	public const string AppointmentRootIdKey = "AppointmentRootId";

	public const string VarianceSubjectKey = "VarianceSubject";

	public const string VarianceStartDateTimeKey = "VarianceStartDateTime";

	public const string VarianceEndDateTimeKey = "VarianceEndDateTime";

	public const string VarianceOriginalStartDateTimeKey = "VarianceOriginalStartDateTime";

	public const string RecurrenceDescriptionKey = "RecurrenceDescription";

	public const string RecurrenceOccurrenceDurationKey = "RecurrenceOccurrenceDuration";

	public const string RecurrenceOccurrenceStartTimeKey = "RecurrenceOccurrenceStartTime";

	public const string RecurrencePatternDayOfMonthKey = "RecurrencePatternDayOfMonth";

	public const string RecurrencePatternDaysOfWeekKey = "RecurrencePatternDaysOfWeek";

	public const string RecurrencePatternFrequencyKey = "RecurrencePatternFrequency";

	public const string RecurrencePatternMonthOfYearKey = "RecurrencePatternMonthOfYear";

	public const string RecurrencePatternOccurrenceOfDayInMonthKey = "RecurrencePatternOccurrenceOfDayInMonth";

	public const string RecurrencePatternIntervalKey = "RecurrencePatternInterval";

	public const string RecurrencePatternTypeKey = "RecurrencePatternType";

	public const string RecurrenceRangeEndDateKey = "RecurrenceRangeEndDate";

	public const string RecurrenceRangeStartDateKey = "RecurrenceRangeStartDate";

	public const string RecurrenceRangeLimitKey = "RecurrenceRangeLimit";

	public const string RecurrenceRangeMaxOccurrencesKey = "RecurrenceRangeMaxOccurrences";

	public const string RecurrenceDurationKey = "RecurrenceDuration";

	public const string VarianceDateKey = "VarianceDate";

	public const string SunclockSunriseKey = "SunclockSunRise";

	public const string SunclockSunsetKey = "SunclockSunSet";

	public const string SunclockTimeZoneKey = "SunclockTimeZone";

	public const string SunclockGPSCoordinateKey = "SunclockGPSCoordinate";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description, out var profileType);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		if (profileType == TimeProfileType.Calendar)
		{
			AppointmentDefinitions appointments = AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml);
			timeProfile.Name = name;
			timeProfile.Description = description;
			if (item.Children != null)
			{
				ConfigurationItem[] children = item.Children;
				foreach (ConfigurationItem configurationItem in children)
				{
					switch (configurationItem.ItemType)
					{
					case "TimeProfileAppointmentRoot":
						UpdateAppointment(appointments, configurationItem);
						break;
					case "TimeProfileAppointmentRecur":
						UpdateRecurringAppointment(appointments, configurationItem);
						break;
					case "TimeProfileAppointmentVariance":
						UpdateAppointmentVariance(appointments, configurationItem);
						break;
					default:
						throw new ArgumentException("Unknown child itemtype:" + configurationItem.ItemType);
					}
				}
			}
			timeProfile.DefinitionXml = AppointmentConversions.Serialize(appointments);
		}
		else
		{
			SunClockTimeProfileDefinition definition;
			ValidateResult validateResult2 = ValidateSunClockProperties(item, out definition);
			if (!validateResult2.ValidatedOk)
			{
				return validateResult2;
			}
			timeProfile.Name = name;
			timeProfile.Description = description;
			timeProfile.DefinitionXml = definition.ToXml();
		}
		TimeProfile.UpdateTimeProfile(requestContext, timeProfile);
		ConfigurationItem resultItem = ConvertToTimeProfileConfigurationItem(parentPath, timeProfile);
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = resultItem;
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		GetTimeProfile(requestContext, path);
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var _, out var _, out var profileType);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		if (profileType == TimeProfileType.Calendar)
		{
			if (item.Children != null)
			{
				ConfigurationItem[] children = item.Children;
				foreach (ConfigurationItem configurationItem in children)
				{
					try
					{
						DateTime startTime;
						DateTime endTime;
						string subject;
						Guid recurrenceId;
						switch (configurationItem.ItemType)
						{
						case "TimeProfileAppointmentRoot":
						{
							FetchAppointment(configurationItem, out var _, out startTime, out endTime, out subject);
							break;
						}
						case "TimeProfileAppointmentRecur":
						{
							FetchRecurringAppointment(configurationItem, out recurrenceId, out subject, out var _, out var _, out var _, out var _, out var _, out var _, out var _, out var _, out var _, out var _, out var _, out var _, out var _);
							break;
						}
						case "TimeProfileAppointmentVariance":
						{
							FetchAppointmentVarience(configurationItem, out var _, out recurrenceId, out startTime, out endTime, out subject);
							break;
						}
						default:
							throw new ArgumentException("Unknown child itemtype:" + configurationItem.ItemType);
						}
					}
					catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException)
					{
						validateResult = ErrorHelper.CreateValidateResult(ex, configurationItem.Properties);
						if (validateResult != null)
						{
							return validateResult;
						}
						throw;
					}
				}
			}
		}
		else
		{
			SunClockTimeProfileDefinition definition;
			ValidateResult validateResult2 = ValidateSunClockProperties(item, out definition);
			if (!validateResult2.ValidatedOk)
			{
				return validateResult2;
			}
		}
		return validateResult;
	}

	private ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out TimeProfileType profileType)
	{
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (item.Properties == null)
		{
			throw new ArgumentNullException("item.Properties", "Cannot be null");
		}
		name = "";
		description = "";
		try
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TimeProfileType", out string value);
			if (value == TimeProfileType.Calendar.ToString())
			{
				profileType = TimeProfileType.Calendar;
			}
			else
			{
				if (!(value == TimeProfileType.Sunclock.ToString()))
				{
					throw new ArgumentException("Invalid TimeProfileType");
				}
				profileType = TimeProfileType.Sunclock;
			}
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			profileType = TimeProfileType.Calendar;
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string[] ids = TimeProfile.QueryTimeProfileIds(requestContext);
			TimeProfile[] array = TimeProfile.QueryTimeProfiles(requestContext, ids);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			TimeProfile[] array2 = array;
			foreach (TimeProfile timeProfile in array2)
			{
				list.Add(ConvertToTimeProfileConfigurationItem(path, timeProfile));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("TimeProfileFolder", ConfigurationItemPath.CreateSystemPath());
		}
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		return ConvertToTimeProfileConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "TimeProfileFolder").CreatePath(), timeProfile);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!flag && path.ItemType == "TimeProfile")
		{
			GetTimeProfile(requestContext, path);
		}
		switch (method)
		{
		case "AddTimeProfile":
			if (!flag)
			{
				return CreateInvokeInfoAddTimeProfile(item, method);
			}
			return AddTimeProfile(requestContext, item, path);
		case "RemoveTimeProfile":
			if (!flag)
			{
				return CreateInvokeInfoRemoveTimeProfile(item, GetChildItems(requestContext, path), method);
			}
			return RemoveTimeProfile(requestContext, item, path);
		case "AddAppointment":
			if (!flag)
			{
				return CreateInvokeInfoAddAppointment(item, method);
			}
			return AddAppointment(requestContext, item, path);
		case "RemoveAppointment":
			if (!flag)
			{
				return CreateInvokeInfoRemoveAppointment(requestContext, item, path, method);
			}
			return RemoveAppointment(requestContext, item, path);
		case "AddRecurringAppointment":
			if (!flag)
			{
				return CreateInvokeInfoAddRecurringAppointment(item, method);
			}
			return AddRecurringAppointment(requestContext, item, path);
		case "RemoveRecurringAppointment":
			if (!flag)
			{
				return CreateInvokeInfoRemoveRecurringAppointment(requestContext, item, path, method);
			}
			return RemoveRecurringAppointment(requestContext, item, path);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	private static ConfigurationItem CreateInvokeInfoAddTimeProfile(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddTimeProfile;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[3]
		{
			GeneralPropertyDefinitions.GetNameProperty("", isSettable: true),
			GeneralPropertyDefinitions.GetDescriptionProperty("", isSettable: true),
			TimeProfilePropertyDefinition.TimeProfileTypeProperty(TimeProfileType.Calendar, isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoAddAppointment(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddAppointment;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[4]
		{
			TimeProfilePropertyDefinition.AllDayEventProperty(isAllDay: false, isSettable: true),
			TimeProfilePropertyDefinition.StartDateTimeProperty(DateTime.Now, isSettable: true),
			TimeProfilePropertyDefinition.EndDateTimeProperty(DateTime.Now, isSettable: true),
			TimeProfilePropertyDefinition.SubjectProperty("Selected time", isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoAddRecurringAppointment(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddRecurringAppointment;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		DateTime now = DateTime.Now;
		configurationItem.Properties = new List<Property>
		{
			TimeProfilePropertyDefinition.AppointmentRootIdProperty(Guid.NewGuid()),
			TimeProfilePropertyDefinition.SubjectProperty("Selected time", isSettable: true),
			TimeProfilePropertyDefinition.RecurrenceOccurrenceStartTimeProperty(now),
			TimeProfilePropertyDefinition.RecurrenceOccurrenceDurationProperty(TimeSpan.FromHours(1.0)),
			TimeProfilePropertyDefinition.RecurrencePatternFrequencyProperty(RecurrencePatternFrequencyType.Daily),
			TimeProfilePropertyDefinition.RecurrencePatternIntervalProperty(1),
			TimeProfilePropertyDefinition.RecurrencePatternDaysOfWeekProperty(31),
			TimeProfilePropertyDefinition.RecurrencePatternDayOfMonthProperty(now.Day),
			TimeProfilePropertyDefinition.RecurrencePatternMonthOfYearProperty(now.Month),
			TimeProfilePropertyDefinition.RecurrencePatternOccurrenceOfDayInMonthProperty(PatternOccurrenceOfDayInMonthType.None),
			TimeProfilePropertyDefinition.RecurrencePatternTypeProperty(PatternType.Calculated),
			TimeProfilePropertyDefinition.RecurrenceRangeStartDateProperty(now),
			TimeProfilePropertyDefinition.RecurrenceRangeEndDateProperty(now),
			TimeProfilePropertyDefinition.RecurrenceRangeLimitProperty(RangeLimit.LimitByDate),
			TimeProfilePropertyDefinition.RecurrenceRangeMaxOccurrencesProperty(10)
		}.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveTimeProfile(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveTimeProfile, "PropertyRemoveTimeProfile", ConfigurationApiResources.PropertyRemoveTimeProfile, item, members);
	}

	private static ConfigurationItem CreateInvokeInfoRemoveAppointment(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		AppointmentDefinitions appointmentDefinitions = AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml);
		int num = 1;
		foreach (AppointmentDefinition appointment in appointmentDefinitions.Appointments)
		{
			if (appointment.AppointmentRecurrence == null && appointment.Variance == null)
			{
				string text = appointment.StartDateTime.Ticks.ToString(CultureInfo.InvariantCulture);
				string text2 = text;
				text2 = FormattableString.Invariant($"{text}-{num++}");
				dictionary.Add(text2, appointment.Subject);
			}
		}
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveAppointment, "WizardRemoveAppointment", ConfigurationApiResources.PropertyRemoveAppointment, item, dictionary);
	}

	private static ConfigurationItem CreateInvokeInfoRemoveRecurringAppointment(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (AppointmentDefinition appointment in AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml).Appointments)
		{
			if (appointment.AppointmentRecurrence != null && appointment.Variance == null)
			{
				dictionary.Add(appointment.AppointmentRecurrence.RecurrenceId.ToString(), appointment.Subject);
			}
		}
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveRecurringAppointment, "WizardRemoveRecurringAppointment", ConfigurationApiResources.PropertyRemoveRecurringAppointment, item, dictionary);
	}

	private static ConfigurationItem AddTimeProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		if (item.Properties == null)
		{
			throw new ArgumentNullException("Properties");
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TimeProfileType", out string value3);
		if (!Enum.TryParse<TimeProfileType>(value3, out var result))
		{
			throw new ArgumentException("Invalid TimeProfileType");
		}
		if (string.IsNullOrEmpty(value))
		{
			throw new ArgumentException("name is blank");
		}
		string definitionXml = ((result != TimeProfileType.Calendar) ? new SunClockTimeProfileDefinition().ToXml() : AppointmentConversions.Serialize(new AppointmentDefinitions()));
		TimeProfile timeProfile = new TimeProfile
		{
			CreatedDate = DateTime.UtcNow,
			DefinitionXml = definitionXml,
			Description = value2,
			Id = Guid.NewGuid().ToString(),
			Name = value,
			Type = result
		};
		TimeProfile timeProfile2 = TimeProfile.CreateTimeProfile(requestContext, timeProfile);
		ConfigurationItem configurationItem = ConvertToTimeProfileConfigurationItem(path, timeProfile2);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem AddAppointment(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "TimeProfile", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid item. Path: " + path.Text);
		}
		if (item.Properties == null)
		{
			throw new ArgumentNullException("Properties");
		}
		FetchAppointment(item, out var allDay, out var startTime, out var endTime, out var subject);
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		AppointmentDefinitions appointmentDefinitions = AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml);
		appointmentDefinitions.Appointments.Add(new AppointmentDefinition
		{
			StartDateTime = startTime,
			EndDateTime = endTime,
			AllDayEvent = allDay,
			Subject = subject
		});
		string definitionXml = AppointmentConversions.Serialize(appointmentDefinitions);
		timeProfile.LastModified = DateTime.UtcNow;
		timeProfile.DefinitionXml = definitionXml;
		TimeProfile.UpdateTimeProfile(requestContext, timeProfile);
		ConfigurationItem configurationItem = ConvertToTimeProfileConfigurationItem(path, timeProfile);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem AddRecurringAppointment(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "TimeProfile", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid item. Path: " + path.Text);
		}
		if (item.Properties == null)
		{
			throw new ArgumentNullException("Properties");
		}
		FetchRecurringAppointment(item, out var recurrenceId, out var subject, out var occurrenceDuration, out var occurrenceStartTime, out var patternFrequency, out var patternDaysOfWeek, out var patternDayOfMonth, out var patternMonthOfYear, out var patternOccurrenceOfDayInMonth, out var patternInterval, out var patternType, out var rangeStartTime, out var rangeEndTime, out var rangeLimit, out var maxOccurrences);
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		AppointmentDefinitions appointmentDefinitions = AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml);
		appointmentDefinitions.Appointments.Add(new AppointmentDefinition
		{
			AllDayEvent = false,
			StartDateTime = occurrenceStartTime,
			EndDateTime = occurrenceStartTime + occurrenceDuration,
			Subject = subject,
			AppointmentRecurrence = new AppointmentRecurrenceDefinition
			{
				Description = "",
				Duration = occurrenceDuration,
				OccurrenceDuration = occurrenceDuration,
				OccurrenceStartTime = occurrenceStartTime,
				PatternDayOfMonth = patternDayOfMonth,
				PatternDaysOfWeek = patternDaysOfWeek,
				PatternFrequency = patternFrequency,
				PatternInterval = patternInterval,
				PatternMonthOfYear = patternMonthOfYear,
				PatternOccurrenceOfDayInMonth = patternOccurrenceOfDayInMonth,
				PatternType = patternType,
				RangeEndDate = rangeEndTime,
				RangeLimit = rangeLimit,
				RangeMaxOccurrences = maxOccurrences,
				RangeStartDate = rangeStartTime,
				RecurrenceId = recurrenceId
			}
		});
		string definitionXml = AppointmentConversions.Serialize(appointmentDefinitions);
		timeProfile.LastModified = DateTime.UtcNow;
		timeProfile.DefinitionXml = definitionXml;
		TimeProfile.UpdateTimeProfile(requestContext, timeProfile);
		ConfigurationItem configurationItem = ConvertToTimeProfileConfigurationItem(path, timeProfile);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem RemoveTimeProfile(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			TimeProfile.DeleteTimeProfile(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (RemoveNonExistingTimeProfileException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem RemoveAppointment(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "TimeProfile", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string[] array = MethodTypesHelper.GetItemSelectionPropertyValue(item).Split('-');
		int num = 0;
		num = ((array.Length != 1) ? int.Parse(array[1], CultureInfo.InvariantCulture) : 0);
		if (!long.TryParse(array[0], out var result))
		{
			throw new ArgumentException("Invalid selection");
		}
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		bool flag = false;
		int num2 = 0;
		AppointmentDefinitions appointmentDefinitions = AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml);
		foreach (AppointmentDefinition appointment in appointmentDefinitions.Appointments)
		{
			num2++;
			if (appointment.StartDateTime.Ticks == result && (num == 0 || num == num2))
			{
				appointmentDefinitions.Appointments.Remove(appointment);
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			throw new ArgumentException("Invalid selection");
		}
		timeProfile.DefinitionXml = AppointmentConversions.Serialize(appointmentDefinitions);
		TimeProfile.UpdateTimeProfile(requestContext, timeProfile);
		return MethodTypesHelper.CreateInvokeResult(item, path.Text, "Result", "Success", true);
	}

	private static ConfigurationItem RemoveRecurringAppointment(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "TimeProfile", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		if (!Guid.TryParse(MethodTypesHelper.GetItemSelectionPropertyValue(item), out var result))
		{
			throw new ArgumentException("Invalid selection");
		}
		TimeProfile timeProfile = GetTimeProfile(requestContext, path);
		AppointmentDefinitions appointmentDefinitions = AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml);
		Guid guid = Guid.Empty;
		foreach (AppointmentDefinition appointment in appointmentDefinitions.Appointments)
		{
			if (appointment.AppointmentRecurrence != null && appointment.AppointmentRecurrence.RecurrenceId == result)
			{
				guid = appointment.AppointmentRecurrence.RecurrenceId;
				appointmentDefinitions.Appointments.Remove(appointment);
				break;
			}
		}
		foreach (AppointmentDefinition appointment2 in appointmentDefinitions.Appointments)
		{
			if (appointment2.Variance != null && appointment2.Variance.RecurrenceId == guid)
			{
				appointmentDefinitions.Appointments.Remove(appointment2);
				break;
			}
		}
		timeProfile.DefinitionXml = AppointmentConversions.Serialize(appointmentDefinitions);
		TimeProfile.UpdateTimeProfile(requestContext, timeProfile);
		return MethodTypesHelper.CreateInvokeResult(item, path.Text, "Result", "Success", false);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToTimeProfileConfigurationItem(ConfigurationItemPath parentPath, TimeProfile timeProfile)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "TimeProfile";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = timeProfile.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, timeProfile.Id).Text;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(timeProfile.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(timeProfile.Name, isSettable: true));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(timeProfile.Description, isSettable: true));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(timeProfile.LastModified));
		List<ConfigurationItem> list2 = new List<ConfigurationItem>();
		if (timeProfile.Type == TimeProfileType.Calendar)
		{
			list.Add(TimeProfilePropertyDefinition.TimeProfileTypeProperty(TimeProfileType.Calendar, isSettable: false));
			foreach (AppointmentDefinition appointment in AppointmentConversions.ConvertToAppointments(timeProfile.DefinitionXml).Appointments)
			{
				if (appointment.AppointmentRecurrence == null && appointment.Variance == null)
				{
					list2.Add(ConvertToRootAppointment(configurationItem, timeProfile.Id, appointment));
				}
				else if (appointment.AppointmentRecurrence != null)
				{
					list2.Add(ConvertToRecurrenceAppointment(configurationItem, timeProfile.Id, appointment));
				}
				else
				{
					list2.Add(ConvertToVarianceAppointment(configurationItem, timeProfile.Id, appointment));
				}
			}
			List<string> list3 = new List<string> { "AddAppointment", "RemoveAppointment", "AddRecurringAppointment", "RemoveRecurringAppointment" };
			configurationItem.MethodIds = list3.ToArray();
		}
		else
		{
			list.Add(TimeProfilePropertyDefinition.TimeProfileTypeProperty(TimeProfileType.Sunclock, isSettable: false));
			TimeProfileSunClockDefinition definition = TimeProfileSunClockDefinition.FromXml(timeProfile.DefinitionXml);
			list.AddRange(ConvertToSunclock(definition));
			configurationItem.MethodIds = new string[0];
		}
		configurationItem.Properties = list.ToArray();
		configurationItem.Children = list2.ToArray();
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	private static void UpdateAppointment(AppointmentDefinitions appointments, ConfigurationItem child)
	{
		DateTime dateTime = GetDateTime(child, "OriginalStartDateTime");
		foreach (AppointmentDefinition appointment in appointments.Appointments)
		{
			if (appointment.AppointmentRecurrence == null && appointment.StartDateTime.Ticks == dateTime.Ticks)
			{
				appointment.AllDayEvent = GetBool(child, "AllDayEvent");
				appointment.StartDateTime = GetDateTime(child, "StartDateTime");
				appointment.EndDateTime = GetDateTime(child, "EndDateTime");
				appointment.Subject = GetString(child, "Subject");
				break;
			}
		}
	}

	private static void UpdateRecurringAppointment(AppointmentDefinitions appointments, ConfigurationItem item)
	{
		Guid guid = GetGuid(item, "AppointmentRootId");
		foreach (AppointmentDefinition appointment in appointments.Appointments)
		{
			if (appointment.AppointmentRecurrence != null && appointment.AppointmentRecurrence.RecurrenceId == guid)
			{
				appointment.Subject = GetString(item, "Subject");
				appointment.AppointmentRecurrence.OccurrenceDuration = GetTimeSpan(item, "RecurrenceOccurrenceDuration");
				appointment.AppointmentRecurrence.OccurrenceStartTime = GetDateTime(item, "RecurrenceOccurrenceStartTime");
				appointment.AppointmentRecurrence.PatternFrequency = GetEnum<RecurrencePatternFrequencyType>(item, "RecurrencePatternFrequency");
				appointment.AppointmentRecurrence.PatternDaysOfWeek = GetInt(item, "RecurrencePatternDaysOfWeek");
				appointment.AppointmentRecurrence.PatternDayOfMonth = GetInt(item, "RecurrencePatternDayOfMonth");
				appointment.AppointmentRecurrence.PatternMonthOfYear = GetInt(item, "RecurrencePatternMonthOfYear");
				appointment.AppointmentRecurrence.PatternOccurrenceOfDayInMonth = GetEnum<PatternOccurrenceOfDayInMonthType>(item, "RecurrencePatternOccurrenceOfDayInMonth");
				appointment.AppointmentRecurrence.PatternInterval = GetInt(item, "RecurrencePatternInterval");
				appointment.AppointmentRecurrence.PatternType = GetEnum<PatternType>(item, "RecurrencePatternType");
				appointment.AppointmentRecurrence.RangeStartDate = GetDateTime(item, "RecurrenceRangeStartDate");
				appointment.AppointmentRecurrence.RangeEndDate = GetDateTime(item, "RecurrenceRangeEndDate");
				appointment.AppointmentRecurrence.RangeLimit = GetEnum<RangeLimit>(item, "RecurrenceRangeLimit");
				appointment.AppointmentRecurrence.RangeMaxOccurrences = GetInt(item, "RecurrenceRangeMaxOccurrences");
				break;
			}
		}
	}

	private static void UpdateAppointmentVariance(AppointmentDefinitions appointments, ConfigurationItem child)
	{
		DateTime dateTime = GetDateTime(child, "VarianceOriginalStartDateTime");
		foreach (AppointmentDefinition appointment in appointments.Appointments)
		{
			if (appointment.AppointmentRecurrence == null && appointment.Variance != null && appointment.Variance.OriginalStartDate.Ticks == dateTime.Ticks)
			{
				appointment.Variance.StartDate = GetDateTime(child, "VarianceStartDateTime");
				appointment.Variance.EndDate = GetDateTime(child, "VarianceEndDateTime");
				appointment.Variance.Subject = GetString(child, "VarianceSubject");
				break;
			}
		}
	}

	private static void FetchAppointment(ConfigurationItem item, out bool allDay, out DateTime startTime, out DateTime endTime, out string subject)
	{
		allDay = GetBool(item, "AllDayEvent");
		startTime = GetDateTime(item, "StartDateTime");
		endTime = GetDateTime(item, "EndDateTime");
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Subject", out subject);
		if (startTime > endTime)
		{
			throw new ArgumentException("StartTime is after EndTime", "StartDateTime");
		}
	}

	private static void FetchAppointmentVarience(ConfigurationItem item, out DateTime originalDate, out Guid recurrenceId, out DateTime startTime, out DateTime endTime, out string subject)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ItemSelection", out string value) && !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AppointmentRootId", out value))
		{
			throw new ArgumentException("Missing property to recurring appointment");
		}
		recurrenceId = new Guid(value);
		originalDate = GetDateTime(item, "VarianceOriginalStartDateTime");
		startTime = GetDateTime(item, "VarianceStartDateTime");
		endTime = GetDateTime(item, "VarianceEndDateTime");
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("VarianceSubject", out subject);
		if (startTime > endTime)
		{
			throw new ArgumentException("StartTime is after EndTime", "VarianceStartDateTime");
		}
	}

	private static void FetchRecurringAppointment(ConfigurationItem item, out Guid recurrenceId, out string subject, out TimeSpan occurrenceDuration, out DateTime occurrenceStartTime, out RecurrencePatternFrequencyType patternFrequency, out int patternDaysOfWeek, out int patternDayOfMonth, out int patternMonthOfYear, out PatternOccurrenceOfDayInMonthType patternOccurrenceOfDayInMonth, out int patternInterval, out PatternType patternType, out DateTime rangeStartTime, out DateTime rangeEndTime, out RangeLimit rangeLimit, out int maxOccurrences)
	{
		recurrenceId = GetGuid(item, "AppointmentRootId");
		subject = GetString(item, "Subject");
		occurrenceDuration = GetTimeSpan(item, "RecurrenceOccurrenceDuration");
		occurrenceStartTime = GetDateTime(item, "RecurrenceOccurrenceStartTime");
		patternFrequency = GetEnum<RecurrencePatternFrequencyType>(item, "RecurrencePatternFrequency");
		patternDaysOfWeek = GetInt(item, "RecurrencePatternDaysOfWeek");
		patternDayOfMonth = GetInt(item, "RecurrencePatternDayOfMonth");
		patternMonthOfYear = GetInt(item, "RecurrencePatternMonthOfYear");
		patternOccurrenceOfDayInMonth = GetEnum<PatternOccurrenceOfDayInMonthType>(item, "RecurrencePatternOccurrenceOfDayInMonth");
		patternInterval = GetInt(item, "RecurrencePatternInterval");
		patternType = GetEnum<PatternType>(item, "RecurrencePatternType");
		rangeStartTime = GetDateTime(item, "RecurrenceRangeStartDate");
		rangeEndTime = GetDateTime(item, "RecurrenceRangeEndDate");
		rangeLimit = GetEnum<RangeLimit>(item, "RecurrenceRangeLimit");
		maxOccurrences = GetInt(item, "RecurrenceRangeMaxOccurrences");
		if (rangeStartTime > rangeEndTime)
		{
			throw new ArgumentException("RangeStartTime is after RangeEndTime", "RecurrenceRangeStartDate");
		}
	}

	private static ValidateResult ValidateSunClockProperties(ConfigurationItem item, out SunClockTimeProfileDefinition definition)
	{
		string gpsCoordinateDisplayName = GetString(item, "SunclockGPSCoordinate");
		int sunriseOffset = GetInt(item, "SunclockSunRise");
		int sunsetOffset = GetInt(item, "SunclockSunSet");
		string text = GetString(item, "SunclockTimeZone");
		definition = null;
		bool flag = false;
		foreach (TimeZoneInfo systemTimeZone in TimeZoneInfo.GetSystemTimeZones())
		{
			if (systemTimeZone.Id == text)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = false;
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = "SunclockTimeZone",
					ErrorText = "Invalid timezoneid"
				}
			};
			return validateResult;
		}
		try
		{
			definition = new SunClockTimeProfileDefinition(sunriseOffset, sunsetOffset, gpsCoordinateDisplayName, text);
		}
		catch (Exception ex) when (ex is ArgumentException || ex is ArgumentNullException || ex is FormatException)
		{
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = false;
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = "SunClock",
					ErrorText = ex.Message
				}
			};
			return validateResult;
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static string GetString(ConfigurationItem item, string key)
	{
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			return value;
		}
		throw new ArgumentException("Missing property:" + key);
	}

	private static Guid GetGuid(ConfigurationItem item, string key)
	{
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			if (Guid.TryParse(value, out var result))
			{
				return result;
			}
			throw new ArgumentException("Invalid guid format for:" + key);
		}
		throw new ArgumentException("Missing property:" + key, key);
	}

	private static TEnum GetEnum<TEnum>(ConfigurationItem item, string key)
	{
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			if (value == null)
			{
				throw new ArgumentNullException("Invalid enum value as null" + key);
			}
			try
			{
				return (TEnum)Enum.Parse(typeof(TEnum), value);
			}
			catch (ArgumentException)
			{
				throw new ArgumentException("Invalid enum format for:" + key);
			}
		}
		throw new ArgumentException("Missing property:" + key, key);
	}

	private static TimeProfile GetTimeProfile(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		return TimeProfile.QueryTimeProfile(requestContext, path.Id) ?? throw new PathNotFoundException(path.Text);
	}

	private static DateTime GetDateTime(ConfigurationItem item, string key)
	{
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			if (DateTime.TryParse(value, out var result))
			{
				return result;
			}
			throw new ArgumentException("Invalid datetime format for:" + key);
		}
		throw new ArgumentException("Missing property:" + key, key);
	}

	private static TimeSpan GetTimeSpan(ConfigurationItem item, string key)
	{
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			if (TimeSpan.TryParse(value, out var result))
			{
				return result;
			}
			throw new ArgumentException("Invalid timespan format for:" + key);
		}
		throw new ArgumentException("Missing property:" + key, key);
	}

	private static int GetInt(ConfigurationItem item, string key)
	{
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			if (int.TryParse(value, out var result))
			{
				return result;
			}
			throw new ArgumentException("Invalid int format for:" + key);
		}
		throw new ArgumentException("Missing property:" + key, key);
	}

	private static bool GetBool(ConfigurationItem item, string key)
	{
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue(key, out string value))
		{
			if (string.CompareOrdinal(value.ToUpper(CultureInfo.InvariantCulture), "TRUE") == 0 || string.CompareOrdinal(value.ToUpper(CultureInfo.InvariantCulture), "YES") == 0)
			{
				return true;
			}
			return false;
		}
		throw new ArgumentException("Missing property:" + key, key);
	}

	private static ConfigurationItem ConvertToRootAppointment(ConfigurationItem parent, string parentId, AppointmentDefinition appointment)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "TimeProfileAppointmentRoot",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.ChildItem,
			DisplayName = appointment.Subject,
			ParentPath = parent.Path
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, parentId).Text;
		obj.Properties = new List<Property>
		{
			TimeProfilePropertyDefinition.AllDayEventProperty(appointment.AllDayEvent, isSettable: true),
			TimeProfilePropertyDefinition.OriginalStartDateTimeProperty(appointment.StartDateTime),
			TimeProfilePropertyDefinition.StartDateTimeProperty(appointment.StartDateTime, isSettable: true),
			TimeProfilePropertyDefinition.EndDateTimeProperty(appointment.EndDateTime, isSettable: true),
			TimeProfilePropertyDefinition.SubjectProperty(appointment.Subject, isSettable: true)
		}.ToArray();
		obj.Children = new ConfigurationItem[0];
		obj.ChildrenFilled = true;
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}

	private static ConfigurationItem ConvertToRecurrenceAppointment(ConfigurationItem parent, string parentId, AppointmentDefinition appointment)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "TimeProfileAppointmentRecur",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.ChildItem,
			DisplayName = appointment.Subject,
			ParentPath = parent.Path
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, parentId).Text;
		obj.Properties = new List<Property>
		{
			TimeProfilePropertyDefinition.AppointmentRootIdProperty(appointment.AppointmentRecurrence.RecurrenceId),
			TimeProfilePropertyDefinition.AllDayEventProperty(appointment.AllDayEvent, isSettable: false),
			TimeProfilePropertyDefinition.StartDateTimeProperty(appointment.StartDateTime, isSettable: false),
			TimeProfilePropertyDefinition.EndDateTimeProperty(appointment.EndDateTime, isSettable: false),
			TimeProfilePropertyDefinition.SubjectProperty(appointment.Subject, isSettable: true),
			TimeProfilePropertyDefinition.RecurrenceDescriptionProperty(appointment.AppointmentRecurrence.Description),
			TimeProfilePropertyDefinition.RecurrenceOccurrenceStartTimeProperty(appointment.AppointmentRecurrence.OccurrenceStartTime),
			TimeProfilePropertyDefinition.RecurrenceOccurrenceDurationProperty(appointment.AppointmentRecurrence.OccurrenceDuration),
			TimeProfilePropertyDefinition.RecurrencePatternTypeProperty(appointment.AppointmentRecurrence.PatternType),
			TimeProfilePropertyDefinition.RecurrencePatternFrequencyProperty(appointment.AppointmentRecurrence.PatternFrequency),
			TimeProfilePropertyDefinition.RecurrencePatternIntervalProperty(appointment.AppointmentRecurrence.PatternInterval),
			TimeProfilePropertyDefinition.RecurrencePatternDaysOfWeekProperty(appointment.AppointmentRecurrence.PatternDaysOfWeek),
			TimeProfilePropertyDefinition.RecurrencePatternDayOfMonthProperty(appointment.AppointmentRecurrence.PatternDayOfMonth),
			TimeProfilePropertyDefinition.RecurrencePatternMonthOfYearProperty(appointment.AppointmentRecurrence.PatternMonthOfYear),
			TimeProfilePropertyDefinition.RecurrencePatternOccurrenceOfDayInMonthProperty(appointment.AppointmentRecurrence.PatternOccurrenceOfDayInMonth),
			TimeProfilePropertyDefinition.RecurrenceRangeStartDateProperty(appointment.AppointmentRecurrence.RangeStartDate),
			TimeProfilePropertyDefinition.RecurrenceRangeEndDateProperty(appointment.AppointmentRecurrence.RangeEndDate),
			TimeProfilePropertyDefinition.RecurrenceRangeLimitProperty(appointment.AppointmentRecurrence.RangeLimit),
			TimeProfilePropertyDefinition.RecurrenceRangeMaxOccurrencesProperty(appointment.AppointmentRecurrence.RangeMaxOccurrences)
		}.ToArray();
		obj.Children = new ConfigurationItem[0];
		obj.ChildrenFilled = true;
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}

	private static ConfigurationItem ConvertToVarianceAppointment(ConfigurationItem parent, string parentId, AppointmentDefinition appointment)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "TimeProfileAppointmentVariance",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.ChildItem,
			DisplayName = appointment.Subject,
			ParentPath = parent.Path
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, parentId).Text;
		obj.Properties = new List<Property>
		{
			TimeProfilePropertyDefinition.AppointmentRootIdProperty(appointment.Variance.RecurrenceId),
			TimeProfilePropertyDefinition.VarianceOriginalStartDateTimeProperty(appointment.Variance.OriginalStartDate, isSettable: false),
			TimeProfilePropertyDefinition.VarianceSubjectProperty(appointment.Variance.Subject),
			TimeProfilePropertyDefinition.VarianceStartDateTimeProperty(appointment.Variance.StartDate),
			TimeProfilePropertyDefinition.VarianceEndDateTimeProperty(appointment.Variance.EndDate)
		}.ToArray();
		obj.Children = new ConfigurationItem[0];
		obj.ChildrenFilled = true;
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}

	private static List<Property> ConvertToSunclock(TimeProfileSunClockDefinition definition)
	{
		return new List<Property>
		{
			TimeProfilePropertyDefinition.SunClockGpsCoordinateProperty(definition.GPSCoordinate),
			TimeProfilePropertyDefinition.SunClockSunriseOffsetProperty(definition.SunRiseOffset),
			TimeProfilePropertyDefinition.SunClockSunsetOffsetProperty(definition.SunSetOffset),
			TimeProfilePropertyDefinition.SunClockTimeZoneProperty(definition.TimeZone)
		};
	}
}
