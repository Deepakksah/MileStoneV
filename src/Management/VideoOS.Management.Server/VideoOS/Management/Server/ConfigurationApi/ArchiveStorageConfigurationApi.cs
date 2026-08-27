using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIArchiveStorage")]
public class ArchiveStorageConfigurationApi : ArchiveStorageConfigurationApiObjectBase
{
	public const string DiskPath = "DiskPath";

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (ArchiveStorage.QueryArchiveStorage(requestContext, path.Id) == null)
		{
			throw new PathNotFoundException();
		}
		return new ConfigurationItem[0];
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		ArchiveStorageDataContract archiveStorageDataContract = ArchiveStorage.QueryArchiveStorage(requestContext, path.Id);
		if (archiveStorageDataContract == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ArchiveStorageConfigurationApiObjectBase.ConvertToArchiveStorageConfigurationItem(ConfigurationItemPath.CreateFolderPath("Storage", archiveStorageDataContract.RecordingStorageId, "ArchiveStorageFolder"), archiveStorageDataContract);
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		string name;
		string description;
		int? retainMinutes;
		bool? framerateReductionEnabled;
		long? maxSize;
		double? targetFramerate;
		string scheduleXml;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out name, out description, out retainMinutes, out framerateReductionEnabled, out maxSize, out targetFramerate, out scheduleXml);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			ArchiveStorageDataContract archiveStorage = new ArchiveStorageDataContract
			{
				Id = path.Id,
				Name = name,
				Description = description,
				RetainMinutes = retainMinutes.Value,
				MaxSize = maxSize.Value,
				TargetFramerate = targetFramerate.Value,
				FramerateReductionEnabled = framerateReductionEnabled.Value,
				ArchiveSchedule = scheduleXml
			};
			archiveStorage = ArchiveStorage.UpdateArchiveStorage(requestContext, archiveStorage);
			ConfigurationItem resultItem = ArchiveStorageConfigurationApiObjectBase.ConvertToArchiveStorageConfigurationItem(ConfigurationItemPath.CreatePath(item.ParentPath), archiveStorage);
			return new ValidateResult
			{
				ValidatedOk = true,
				ResultItem = resultItem
			};
		}
		catch (ArchiveStorageUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (ArchiveStorageRenameException ex2)
		{
			throw new PropertyInvalidException(ex2.Message, "Name");
		}
		catch (StorageRetentionTimeException ex3)
		{
			throw new PropertyInvalidException(ex3.Message, "RetainMinutes");
		}
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (ArchiveStorage.QueryArchiveStorage(requestContext, path.Id) == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		string name;
		string description;
		int? retainMinutes;
		bool? framerateReductionEnabled;
		long? maxSize;
		double? targetFramerate;
		string scheduleXml;
		return ValidateItemInternal(requestContext, item, path, out name, out description, out retainMinutes, out framerateReductionEnabled, out maxSize, out targetFramerate, out scheduleXml);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out int? retainMinutes, out bool? framerateReductionEnabled, out long? maxSize, out double? targetFramerate, out string scheduleXml)
	{
		name = null;
		description = null;
		retainMinutes = null;
		framerateReductionEnabled = null;
		maxSize = null;
		targetFramerate = null;
		scheduleXml = null;
		try
		{
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RetainMinutes", out retainMinutes);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FramerateReductionEnabled", out framerateReductionEnabled);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MaxSize", out maxSize);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TargetFramerate", out targetFramerate);
			}
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
		if (retainMinutes.HasValue)
		{
			try
			{
				Validation.CheckRetainMinutes(retainMinutes.Value, "RetainMinutes");
			}
			catch (ArgumentOutOfRangeException)
			{
				return NewErrorOutOfRangeValidateResult("RetainMinutes", ConfigurationApiResources.PropertyArchiveStorageRetainMinutes);
			}
		}
		if (maxSize.HasValue)
		{
			try
			{
				Validation.CheckMaxSize(maxSize.Value, "MaxSize");
			}
			catch (ArgumentOutOfRangeException)
			{
				return NewErrorOutOfRangeValidateResult("MaxSize", ConfigurationApiResources.PropertyArchiveStorageMaxSize);
			}
		}
		if (targetFramerate.HasValue)
		{
			try
			{
				Validation.CheckTargetFramerate(targetFramerate.Value, "TargetFramerate");
			}
			catch (ArgumentOutOfRangeException)
			{
				return NewErrorOutOfRangeValidateResult("TargetFramerate", ConfigurationApiResources.PropertyArchiveStorageTargetFramerate);
			}
		}
		try
		{
			ArchiveStorageDataContract archiveStorageDataContract = ArchiveStorage.QueryArchiveStorage(requestContext, path.Id);
			if (archiveStorageDataContract == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			name = name ?? archiveStorageDataContract.Name;
			description = description ?? archiveStorageDataContract.Description;
			retainMinutes = retainMinutes ?? archiveStorageDataContract.RetainMinutes;
			maxSize = maxSize ?? archiveStorageDataContract.MaxSize;
			framerateReductionEnabled = framerateReductionEnabled ?? archiveStorageDataContract.FramerateReductionEnabled;
			targetFramerate = targetFramerate ?? archiveStorageDataContract.TargetFramerate;
			ArchiveStorage.UpdateArchiveStorageValidation(requestContext, path.Id, name, description, retainMinutes.Value, maxSize.Value, framerateReductionEnabled.Value, targetFramerate.Value, archiveStorageDataContract.ArchiveSchedule);
			ValidateResult validateResult2 = ArchiveScheduleConfigurationApi.ValidateItem(item, archiveStorageDataContract, out scheduleXml);
			if (!validateResult2.ValidatedOk)
			{
				return validateResult2;
			}
			return new ValidateResult
			{
				ValidatedOk = true,
				ResultItem = item
			};
		}
		catch (ArgumentNullException exception2)
		{
			ValidateResult validateResult3 = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult3 != null)
			{
				return validateResult3;
			}
			throw;
		}
		catch (ArgumentException exception3)
		{
			ValidateResult validateResult4 = ErrorHelper.CreateValidateResult(exception3, item.Properties);
			if (validateResult4 != null)
			{
				return validateResult4;
			}
			throw;
		}
	}

	private static ValidateResult NewErrorOutOfRangeValidateResult(string propertyName, string propertyDisplayName)
	{
		ValidateResult validateResult = new ValidateResult();
		validateResult.ErrorResults = new ErrorResult[1]
		{
			new ErrorResult
			{
				ErrorProperty = propertyName,
				ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorOutOfRange, propertyDisplayName),
				ErrorTextId = "ErrorOutOfRange"
			}
		};
		validateResult.ValidatedOk = false;
		validateResult.ResultItem = null;
		return validateResult;
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		return methodId switch
		{
			"ReadArchiveStorageInformation" => CreateInvokeInfoReadArchiveStorageInformation(), 
			"MoveArchiveStorage" => CreateInvokeInfoMoveArchiveStorage(), 
			"SetFramerateReductionArchiveStorage" => CreateInvokeInfoSetFramerateReductionArchiveStorage(), 
			"MountArchiveStorage" => CreateInvokeInfoMountArchiveStorage(), 
			"UnmountArchiveStorage" => CreateInvokeInfoUnmountArchiveStorage(), 
			_ => throw new ArgumentException("Unknown methodId:" + methodId, "methodId"), 
		};
		ConfigurationItem CreateInvokeInfoMountArchiveStorage()
		{
			bool? value = null;
			item.Properties?.TryGetPropertyValue("FramerateReductionEnabled", out value);
			return new ConfigurationItem
			{
				ItemType = "InvokeInfo",
				UIHint = "DirectionVertical",
				ItemCategory = ItemCategories.Item,
				DisplayName = ConfigurationApiResources.MethodMountArchiveStorage,
				ParentPath = item.ParentPath,
				Path = item.Path,
				MethodIds = new string[1] { "MountArchiveStorage" },
				Properties = Array.Empty<Property>()
			};
		}
		ConfigurationItem CreateInvokeInfoMoveArchiveStorage()
		{
			string value = null;
			item.Properties?.TryGetPropertyValue("Path", out value);
			return new ConfigurationItem
			{
				ItemType = "InvokeInfo",
				UIHint = "DirectionVertical",
				ItemCategory = ItemCategories.Item,
				DisplayName = ConfigurationApiResources.WizardMoveArchiveStorage,
				ParentPath = item.ParentPath,
				Path = item.Path,
				MethodIds = new string[1] { "MoveArchiveStorage" },
				Properties = new Property[1] { ArchiveStoragePropertyDefinition.DiskPathProperty(value, isSettable: true) }
			};
		}
		ConfigurationItem CreateInvokeInfoReadArchiveStorageInformation()
		{
			return new ConfigurationItem
			{
				ItemType = "InvokeInfo",
				UIHint = "DirectionVertical",
				ItemCategory = ItemCategories.Item,
				DisplayName = ConfigurationApiResources.MethodReadArchiveStorageInformation,
				ParentPath = item.ParentPath,
				Path = item.Path,
				MethodIds = new string[1] { "ReadArchiveStorageInformation" },
				Properties = Array.Empty<Property>()
			};
		}
		ConfigurationItem CreateInvokeInfoSetFramerateReductionArchiveStorage()
		{
			bool? value = null;
			item.Properties?.TryGetPropertyValue("FramerateReductionEnabled", out value);
			return new ConfigurationItem
			{
				ItemType = "InvokeInfo",
				UIHint = "DirectionVertical",
				ItemCategory = ItemCategories.Item,
				DisplayName = ConfigurationApiResources.PropertyArchiveStorageFramerateReductionEnabled,
				ParentPath = item.ParentPath,
				Path = item.Path,
				MethodIds = new string[1] { "SetFramerateReductionArchiveStorage" },
				Properties = new Property[1] { ArchiveStoragePropertyDefinition.FramerateReductionEnabledProperty(value.Value) }
			};
		}
		ConfigurationItem CreateInvokeInfoUnmountArchiveStorage()
		{
			bool? value = null;
			item.Properties?.TryGetPropertyValue("FramerateReductionEnabled", out value);
			return new ConfigurationItem
			{
				ItemType = "InvokeInfo",
				UIHint = "DirectionVertical",
				ItemCategory = ItemCategories.Item,
				DisplayName = ConfigurationApiResources.MethodUnmountArchiveStorage,
				ParentPath = item.ParentPath,
				Path = item.Path,
				MethodIds = new string[1] { "UnmountArchiveStorage" },
				Properties = Array.Empty<Property>()
			};
		}
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		return methodId switch
		{
			"ReadArchiveStorageInformation" => ReadArchiveStorageInformation(requestContext, item, path), 
			"MoveArchiveStorage" => MoveArchiveStorage(requestContext, item, path), 
			"SetFramerateReductionArchiveStorage" => SetFramerateReductionArchiveStorage(requestContext, item, path), 
			"MountArchiveStorage" => MountArchiveStorage(requestContext, item, path), 
			"UnmountArchiveStorage" => UnmountArchiveStorage(requestContext, item, path), 
			_ => throw new ArgumentException("Unknown methodId:" + methodId, "methodId"), 
		};
	}

	private static ConfigurationItem ReadArchiveStorageInformation(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		StorageInformationConfigurationApi storageInformationConfigurationApi = new StorageInformationConfigurationApi();
		ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("ArchiveStorageInformation", path.Id);
		ConfigurationItem item2 = storageInformationConfigurationApi.GetItem(requestContext, path2);
		return MethodTypesHelper.CreateInvokeResult(item2, item2.Path, "Result", "Success", true);
	}

	private static ConfigurationItem MoveArchiveStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DiskPath", out string value))
		{
			throw new PropertyInvalidException("Property not set", "DiskPath");
		}
		try
		{
			Validation.CheckPath(value, "DiskPath");
		}
		catch (ArgumentNullException)
		{
			throw new PropertyInvalidException("Property not valid", "DiskPath");
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.MoveArchiveStorage(requestContext, path.Id, value), item);
	}

	private static ConfigurationItem MountArchiveStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.MountArchiveStorage(requestContext, path.Id), item);
	}

	private static ConfigurationItem UnmountArchiveStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		return TaskHelperFunctions.CreateTaskInvokeResultConfigurationItem(Task.UnmountArchiveStorage(requestContext, path.Id), item);
	}

	private static ConfigurationItem SetFramerateReductionArchiveStorage(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (item.Properties == null || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("FramerateReductionEnabled", out bool? value))
		{
			throw new PropertyInvalidException("Property not set", "FramerateReductionEnabled");
		}
		try
		{
			ArchiveStorage.EnableFramerateReduction(requestContext, path.Id, value.Value);
		}
		catch (ArchiveStorageUpdateException)
		{
			throw new PathNotFoundException(item.Path);
		}
		catch (EnableFramerateReductionArchiveStorageNotFoundException)
		{
			throw new PathNotFoundException(item.Path);
		}
		return MethodTypesHelper.CreateInvokeResult(item, item.Path, "Result", "Success", true);
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		ArchiveStorageFolderConfigurationApi.ThrowIfFolderPathInvalid(path);
	}
}
