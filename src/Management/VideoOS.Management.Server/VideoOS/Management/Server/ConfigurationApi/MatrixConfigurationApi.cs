using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIMatrix")]
[EntityName("ConfigAPIMatrixFolder")]
public class MatrixConfigurationApi : SecurityConfigurationApi, IConfigurationApiSupport
{
	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>> { 
	{
		"Matrix",
		new List<string> { "GENERIC_WRITE", "CREATE_MATRIX", "DELETE", "ADMIN_SECURITY" }
	} };

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string name;
		string description;
		string address;
		int? port;
		string password;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path.Id, out name, out description, out address, out port, out password);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			MatrixViewData matrixViewData = MatrixViewData.QueryMatrix(requestContext, path.Id);
			if (matrixViewData == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			if (name != null)
			{
				matrixViewData.Name = name;
			}
			if (description != null)
			{
				matrixViewData.Description = description;
			}
			if (address != null)
			{
				matrixViewData.Address = address;
			}
			if (port.HasValue)
			{
				matrixViewData.Port = port.Value;
			}
			if (password != null)
			{
				matrixViewData.Password = password;
			}
			MatrixViewData.UpdateMatrix(requestContext, matrixViewData);
			return validateResult;
		}
		catch (UnauthorizedAccessException)
		{
			throw new UnauthorizedAccessException(path.Text);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string name;
		string description;
		string address;
		int? port;
		string password;
		return ValidateItemInternal(requestContext, item, path.Id, out name, out description, out address, out port, out password);
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.IsFolder)
		{
			IEnumerable<MatrixViewData> enumerable = MatrixViewData.QueryMatrixViews(requestContext);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (MatrixViewData item in enumerable)
			{
				ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("Matrix", item.Id.ToString());
				list.Add(ConvertToConfigurationItem(item, path2));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("MatrixFolder", ConfigurationItemPath.CreateSystemPath());
		}
		ConfigurationApiBase.ThrowIfPathGuidInvalid(path);
		try
		{
			return ConvertToConfigurationItem(MatrixViewData.QueryMatrix(requestContext, path.Id) ?? throw new PathNotFoundException(path.Text), path);
		}
		catch (UnauthorizedAccessException)
		{
			throw new UnauthorizedAccessException(path.Text);
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
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
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (methodId)
		{
		case "AddMatrix":
			if (!flag)
			{
				return CreateInvokeInfoAddMatrix(item, methodId);
			}
			return AddMatrix(requestContext, item, path);
		case "RemoveMatrix":
			if (!flag)
			{
				return CreateInvokeInfoRemoveMatrix(item, GetChildItems(requestContext, path), methodId);
			}
			return RemoveMatrix(requestContext, item, path);
		case "ChangeSecurityPermissions":
			return InvokeSecurityMethod(requestContext, item, path, methodId);
		case "MatrixCommand":
			if (!flag)
			{
				return CreateInvokeInfoMatrixCommand(item, methodId);
			}
			return MatrixCommand(requestContext, item, path);
		default:
			throw new NotSupportedException();
		}
	}

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}

	private static ConfigurationItem CreateInvokeInfoAddMatrix(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddMatrix;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			MatrixPropertyDefinition.GetAddressProperty(""),
			MatrixPropertyDefinition.GetPortProperty(12345),
			MatrixPropertyDefinition.GetPasswordProperty("")
		};
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem AddMatrix(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath folderPath)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, folderPath);
		if (!string.Equals(folderPath.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + folderPath.Text);
		}
		string text = Guid.NewGuid().ToString();
		string name;
		string description;
		string address;
		int? port;
		string password;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, text, out name, out description, out address, out port, out password);
		if (!validateResult.ValidatedOk)
		{
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				throw new PropertyInvalidException(validateResult.ErrorResults[0].ErrorText, validateResult.ErrorResults[0].ErrorProperty);
			}
			throw new PropertyInvalidException();
		}
		MatrixViewData matrix = MatrixViewData.CreateMatrix(requestContext, text, name, description, address, port.Value, password);
		ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("Matrix", text.ToString());
		ConfigurationItem configurationItem = ConvertToConfigurationItem(matrix, path);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem CreateInvokeInfoRemoveMatrix(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveMatrix, "PropertyRemoveMatrix", ConfigurationApiResources.PropertyRemoveMatrix, item, members);
	}

	private static ConfigurationItem RemoveMatrix(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		MatrixViewData matrixViewData = MatrixViewData.QueryMatrix(requestContext, invokeMemberItemSelectionPath.Id);
		if (matrixViewData == null)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		MatrixViewData.DeleteMatrix(requestContext, invokeMemberItemSelectionPath.Id, matrixViewData.Name);
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem CreateInvokeInfoMatrixCommand(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardMatrixCommand;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		List<Property> list = new List<Property>
		{
			MatrixPropertyDefinition.GetCommandTypeProperty(1),
			MatrixPropertyDefinition.GetCameraListProperty()
		};
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem MatrixCommand(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		MatrixViewData matrixViewData = MatrixViewData.QueryMatrix(requestContext, path.Id);
		if (matrixViewData == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MatrixCommandType", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MatrixCameraList", out string[] values);
		if (value == "1")
		{
			string[] array = values;
			for (int i = 0; i < array.Length; i++)
			{
				string id = ConfigurationItemPath.CreatePath(array[i]).Id;
				MatrixViewData.SetMatrixCameras(requestContext, matrixViewData, id);
			}
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		if (value == "2")
		{
			MatrixViewData.SetMatrixCameras(requestContext, matrixViewData, Guid.Empty.ToString());
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		throw new PropertyInvalidException("Only 0 or 1 are valid values", "MatrixCommandType");
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, string id, out string name, out string description, out string address, out int? port, out string password)
	{
		name = null;
		description = null;
		address = null;
		port = null;
		password = null;
		try
		{
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Address", out address);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Password", out password);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Port", out string value);
				if (!int.TryParse(value, out var result))
				{
					throw new PropertyInvalidException("Integer expected", "Port");
				}
				port = result;
				ThrowIfDuplicateName(requestContext, name, id);
			}
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
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
	}

	protected static ConfigurationItem ConvertToConfigurationItem(MatrixViewData matrix, ConfigurationItemPath path)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateSystemFolderPath("MatrixFolder");
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			UIHint = "DirectionHorizontal",
			ItemType = path.ItemType,
			ItemCategory = ItemCategories.Item,
			DisplayName = matrix.Name,
			ParentPath = configurationItemPath.Text,
			Path = path.Text,
			ChildrenFilled = true
		};
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(matrix.Id));
		list.Add(GeneralPropertyDefinitions.GetNameProperty(matrix.Name));
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(matrix.Description));
		list.Add(MatrixPropertyDefinition.GetAddressProperty(matrix.Address));
		list.Add(MatrixPropertyDefinition.GetPortProperty(matrix.Port));
		list.Add(MatrixPropertyDefinition.GetPasswordProperty(matrix.Password));
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(matrix.LastModified));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[2] { "ChangeSecurityPermissions", "MatrixCommand" };
		return configurationItem;
	}

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, string objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		foreach (MatrixViewData item in MatrixViewData.QueryMatrixViews(requestContext))
		{
			if (!string.Equals(item.Id, objectId, StringComparison.OrdinalIgnoreCase) && item.Name == name)
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}
}
