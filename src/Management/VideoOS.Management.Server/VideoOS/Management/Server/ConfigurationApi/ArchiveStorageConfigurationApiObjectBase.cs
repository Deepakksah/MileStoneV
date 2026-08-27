using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class ArchiveStorageConfigurationApiObjectBase : ConfigurationApiBase, IConfigurationApiSupport
{
	public class ArchiveScheduleConfigurationApi
	{
		public const string FrequencyType = "FrequencyType";

		public const string FrequencyRecurrenceFactor = "FrequencyRecurrenceFactor";

		public const string FrequencyInterval = "FrequencyInterval";

		public const string FrequencyRelativeInterval = "FrequencyRelativeInterval";

		public const string FrequencySubDayType = "FrequencySubDayType";

		public const string FrequencySubDayInterval = "FrequencySubDayInterval";

		public const string ActiveStartTimeOfDay = "ActiveStartTimeOfDay";

		public const string ActiveEndTimeOfDay = "ActiveEndTimeOfDay";

		public static ConfigurationItem ConvertToScheduleConfigurationItem(string path, string parentPath, string xml)
		{
			Schedule schedule = Schedule.FromXml(xml);
			ConfigurationItem configurationItem = new ConfigurationItem();
			configurationItem.ItemType = "ArchiveSchedule";
			configurationItem.UIHint = "DirectionHorizontal";
			configurationItem.ItemCategory = ItemCategories.ChildItem;
			configurationItem.DisplayName = "ArchiveSchedule";
			configurationItem.ParentPath = parentPath;
			configurationItem.Path = path;
			configurationItem.Properties = new Property[8]
			{
				SchedulePropertyDefinition.FrequencyTypeProperty(schedule.FrequencyType),
				SchedulePropertyDefinition.FrequencyRecurrenceFactorProperty(schedule.FrequencyRecurrenceFactor),
				SchedulePropertyDefinition.FrequencyIntervalProperty(schedule.FrequencyInterval),
				SchedulePropertyDefinition.FrequencyRelativeIntervalProperty(schedule.FrequencyRelativeInterval),
				SchedulePropertyDefinition.FrequencySubDayTypeProperty(schedule.FrequencySubDayType),
				SchedulePropertyDefinition.FrequencySubDayIntervalProperty(schedule.FrequencySubDayInterval),
				SchedulePropertyDefinition.ActiveStartTimeOfDayProperty(schedule.ActiveStartTimeOfDay.TotalSeconds),
				SchedulePropertyDefinition.ActiveEndTimeOfDayProperty(schedule.ActiveEndTimeOfDay.TotalSeconds)
			};
			configurationItem.MethodIds = new string[0];
			return configurationItem;
		}

		public static ValidateResult ValidateItem(ConfigurationItem archiveStorageItem, ArchiveStorageDataContract archiveStorage, out string scheduleXml)
		{
			ConfigurationItem configurationItem = null;
			if (archiveStorageItem.Children != null)
			{
				configurationItem = archiveStorageItem.Children.Where((ConfigurationItem child) => child.ItemType == "ArchiveSchedule").FirstOrDefault();
			}
			if (configurationItem == null)
			{
				scheduleXml = archiveStorage.ArchiveSchedule;
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = archiveStorageItem
				};
			}
			return ValidateItemInternal(configurationItem, archiveStorage.ArchiveSchedule, out scheduleXml);
		}

		public static ValidateResult ValidateItemInternal(ConfigurationItem item, string existingScheduleXml, out string newScheduleXml)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			if (item.Properties == null)
			{
				newScheduleXml = existingScheduleXml;
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = item
				};
			}
			if (string.Equals(item.ItemType, "ArchiveSchedule", StringComparison.Ordinal))
			{
				Schedule schedule = Schedule.FromXml(existingScheduleXml);
				newScheduleXml = null;
				FrequencyType? frequencyType = null;
				int? frequencyRecurrenceFactor = null;
				int? frequencyInterval = null;
				FrequencyRelativeInterval? frequencyRelativeInterval = null;
				FrequencySubDayType? frequencySubDayType = null;
				int? frequencySubDayInterval = null;
				double? activeStartTimeOfDay = null;
				double? activeEndTimeOfDay = null;
				ValidateResult validateResult = ValidateArguments(item, out frequencyType, out frequencyRecurrenceFactor, out frequencyInterval, out frequencyRelativeInterval, out frequencySubDayType, out frequencySubDayInterval, out activeStartTimeOfDay, out activeEndTimeOfDay);
				if (validateResult != null)
				{
					return validateResult;
				}
				try
				{
					frequencyType = frequencyType ?? schedule.FrequencyType;
					frequencyRecurrenceFactor = frequencyRecurrenceFactor ?? schedule.FrequencyRecurrenceFactor;
					frequencyInterval = frequencyInterval ?? schedule.FrequencyInterval;
					frequencyRelativeInterval = frequencyRelativeInterval ?? schedule.FrequencyRelativeInterval;
					frequencySubDayType = frequencySubDayType ?? schedule.FrequencySubDayType;
					frequencySubDayInterval = frequencySubDayInterval ?? schedule.FrequencySubDayInterval;
					activeStartTimeOfDay = activeStartTimeOfDay ?? schedule.ActiveStartTimeOfDay.TotalSeconds;
					activeEndTimeOfDay = activeEndTimeOfDay ?? schedule.ActiveEndTimeOfDay.TotalSeconds;
					if (!frequencyType.HasValue || !frequencyInterval.HasValue || !Schedule.IsFrequencyIntervalValid(frequencyType.Value, frequencyInterval.Value))
					{
						ValidateResult validateResult2 = new ValidateResult();
						validateResult2.ErrorResults = new ErrorResult[1]
						{
							new ErrorResult
							{
								ErrorProperty = "FrequencyInterval",
								ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, ConfigurationApiResources.PropertyScheduleFrequencyInterval),
								ErrorTextId = "ErrorOutOfRange"
							}
						};
						validateResult2.ValidatedOk = false;
						validateResult2.ResultItem = null;
						return validateResult2;
					}
					Schedule schedule2 = new Schedule
					{
						FrequencyType = frequencyType.Value,
						FrequencyRecurrenceFactor = frequencyRecurrenceFactor.Value,
						FrequencyInterval = frequencyInterval.Value,
						FrequencyRelativeInterval = frequencyRelativeInterval.Value,
						FrequencySubDayType = frequencySubDayType.Value,
						FrequencySubDayInterval = frequencySubDayInterval.Value,
						ActiveStartTimeOfDay = TimeSpan.FromSeconds(activeStartTimeOfDay.Value),
						ActiveEndTimeOfDay = TimeSpan.FromSeconds(activeEndTimeOfDay.Value)
					};
					newScheduleXml = schedule2.ToXml();
					return new ValidateResult
					{
						ValidatedOk = true,
						ResultItem = item
					};
				}
				catch (ArgumentNullException exception)
				{
					ValidateResult validateResult3 = ErrorHelper.CreateValidateResult(exception, item.Properties);
					if (validateResult3 != null)
					{
						return validateResult3;
					}
					throw;
				}
				catch (ArgumentException exception2)
				{
					ValidateResult validateResult4 = ErrorHelper.CreateValidateResult(exception2, item.Properties);
					if (validateResult4 != null)
					{
						return validateResult4;
					}
					throw;
				}
			}
			throw new NotSupportedException();
		}

		private static ValidateResult ValidateArguments(ConfigurationItem item, out FrequencyType? frequencyType, out int? frequencyRecurrenceFactor, out int? frequencyInterval, out FrequencyRelativeInterval? frequencyRelativeInterval, out FrequencySubDayType? frequencySubDayType, out int? frequencySubDayInterval, out double? activeStartTimeOfDay, out double? activeEndTimeOfDay)
		{
			frequencyType = null;
			frequencyRecurrenceFactor = null;
			frequencyInterval = null;
			frequencyRelativeInterval = null;
			frequencySubDayType = null;
			frequencySubDayInterval = null;
			activeStartTimeOfDay = null;
			activeEndTimeOfDay = null;
			try
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FrequencyType", out frequencyType);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FrequencyRecurrenceFactor", out frequencyRecurrenceFactor);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FrequencyInterval", out frequencyInterval);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FrequencyRelativeInterval", out frequencyRelativeInterval);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FrequencySubDayType", out frequencySubDayType);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FrequencySubDayInterval", out frequencySubDayInterval);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ActiveStartTimeOfDay", out activeStartTimeOfDay);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ActiveEndTimeOfDay", out activeEndTimeOfDay);
			}
			catch (ArgumentException exception)
			{
				ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			if (frequencyRecurrenceFactor.HasValue && frequencyRecurrenceFactor.Value < 0)
			{
				ValidateResult validateResult2 = new ValidateResult();
				validateResult2.ErrorResults = new ErrorResult[1]
				{
					new ErrorResult
					{
						ErrorProperty = "FrequencyRecurrenceFactor",
						ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, ConfigurationApiResources.PropertyScheduleFrequencyRecurrenceFactor),
						ErrorTextId = "ErrorOutOfRange"
					}
				};
				validateResult2.ValidatedOk = false;
				validateResult2.ResultItem = null;
				return validateResult2;
			}
			if (frequencySubDayInterval.HasValue && frequencySubDayInterval.Value < 0)
			{
				ValidateResult validateResult2 = new ValidateResult();
				validateResult2.ErrorResults = new ErrorResult[1]
				{
					new ErrorResult
					{
						ErrorProperty = "FrequencySubDayInterval",
						ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, ConfigurationApiResources.PropertyScheduleFrequencySubDayInterval),
						ErrorTextId = "ErrorOutOfRange"
					}
				};
				validateResult2.ValidatedOk = false;
				validateResult2.ResultItem = null;
				return validateResult2;
			}
			if (activeStartTimeOfDay.HasValue && (activeStartTimeOfDay.Value < 0.0 || activeStartTimeOfDay.Value > 86399.0))
			{
				ValidateResult validateResult2 = new ValidateResult();
				validateResult2.ErrorResults = new ErrorResult[1]
				{
					new ErrorResult
					{
						ErrorProperty = "ActiveStartTimeOfDay",
						ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, ConfigurationApiResources.PropertyScheduleActiveStartTimeOfDay),
						ErrorTextId = "ErrorOutOfRange"
					}
				};
				validateResult2.ValidatedOk = false;
				validateResult2.ResultItem = null;
				return validateResult2;
			}
			if (activeEndTimeOfDay.HasValue && (activeEndTimeOfDay.Value < 0.0 || activeEndTimeOfDay.Value > 86399.0))
			{
				ValidateResult validateResult2 = new ValidateResult();
				validateResult2.ErrorResults = new ErrorResult[1]
				{
					new ErrorResult
					{
						ErrorProperty = "ActiveEndTimeOfDay",
						ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, ConfigurationApiResources.PropertyScheduleActiveEndTimeOfDay),
						ErrorTextId = "ErrorOutOfRange"
					}
				};
				validateResult2.ValidatedOk = false;
				validateResult2.ResultItem = null;
				return validateResult2;
			}
			return null;
		}
	}

	public abstract ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		ParamNullCheck(requestContext, "requestContext");
		ParamNullCheck(item, "item");
		ParamNullCheck(path, "path");
		ParamNullCheck(parentPath, "parentPath");
		ParamNullCheck(methodId, "methodId");
		ThrowIfPathInvalid(path);
		ThrowIfParentPathInvalid(parentPath);
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return GetMethodInputParameters(requestContext, item, path, methodId);
		}
		return ExecuteMethod(requestContext, item, path, methodId);
	}

	protected abstract ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId);

	protected abstract ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId);

	protected static void ParamNullCheck(object param, string name)
	{
		if (param != null)
		{
			return;
		}
		throw new ArgumentNullException(name ?? string.Empty);
	}

	protected static ConfigurationItem ConvertToArchiveStorageConfigurationItem(ConfigurationItemPath parentPath, ArchiveStorageDataContract archiveStorage)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "ArchiveStorage";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = archiveStorage.Name;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, archiveStorage.Id).Text;
		configurationItem.Properties = new Property[11]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(archiveStorage.Id),
			GeneralPropertyDefinitions.GetNameProperty(archiveStorage.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(archiveStorage.Description),
			GeneralPropertyDefinitions.GetCreatedDateProperty(archiveStorage.CreatedDate),
			GeneralPropertyDefinitions.GetLastModifiedProperty(archiveStorage.LastModified),
			ArchiveStoragePropertyDefinition.DiskPathProperty(archiveStorage.Path),
			ArchiveStoragePropertyDefinition.RetainMinutesProperty(archiveStorage.RetainMinutes),
			ArchiveStoragePropertyDefinition.MaxSizeProperty(archiveStorage.MaxSize),
			ArchiveStoragePropertyDefinition.FramerateReductionEnabledProperty(archiveStorage.FramerateReductionEnabled),
			ArchiveStoragePropertyDefinition.TargetFramerateProperty(archiveStorage.TargetFramerate),
			ArchiveStoragePropertyDefinition.MountedProperty(archiveStorage.Mounted)
		};
		configurationItem.MethodIds = new string[5] { "ReadArchiveStorageInformation", "SetFramerateReductionArchiveStorage", "MoveArchiveStorage", "MountArchiveStorage", "UnmountArchiveStorage" };
		string text = ConfigurationItemPath.CreateItemPath("ArchiveSchedule", archiveStorage.Id).Text;
		configurationItem.Children = new ConfigurationItem[1] { ArchiveScheduleConfigurationApi.ConvertToScheduleConfigurationItem(text, configurationItem.Path, archiveStorage.ArchiveSchedule) };
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	protected abstract void ThrowIfPathInvalid(ConfigurationItemPath path);

	protected abstract void ThrowIfParentPathInvalid(ConfigurationItemPath path);
}
