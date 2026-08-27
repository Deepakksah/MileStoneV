using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MatrixPropertyDefinition
{
	public static Property GetAddressProperty(string address)
	{
		return new Property("Address", address)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMatrixAddress,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMatrixAddress",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetPortProperty(int port)
	{
		return new Property("Port", port.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMatrixPort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMatrixPort",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GetPasswordProperty(string password)
	{
		return new Property("Password", password)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMatrixPassword,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMatrixPassword",
			UIImportance = UIImportance.Password,
			SortKey = 100
		};
	}

	public static Property GetCommandTypeProperty(int commandType)
	{
		Property property = new Property("MatrixCommandType", commandType.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMatrixCommandType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMatrixCommandType",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
		property.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "ShowCameras",
				Value = "1"
			},
			new ValueTypeInfo
			{
				Name = "Clear",
				Value = "2"
			}
		};
		return property;
	}

	public static Property GetCameraListProperty()
	{
		Property property = new Property("MatrixCameraList", "");
		property.ValueType = "Array";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.PropertyMatrixCameraList;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyMatrixCameraList";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueArray = new string[0];
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}
}
