using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingServerTaskResultDefinition
{
	private const string MacAddressExistsGlobal = "MacAddressExistsGlobal";

	private const string MacAddressExistsLocal = "MacAddressExistsLocal";

	private const string MacAddress = "MacAddress";

	private const string HardwareScanValidated = "HardwareScanValidated";

	public static Property[] CreateHardwareScanResultProperties(IGeminiRequestContext requestContext, Task task)
	{
		List<Property> list = new List<Property>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			HardwareScanTaskResult hardwareScanTaskResult = HardwareScanTaskResult.FromXml(task.AsyncStatus.LastException);
			string value = task.TaskProperties.GetValue("idRecorder");
			HardwareDriver hardwareDriver = HardwareDriver.QueryActiveHardwareDrivers(requestContext, value).FirstOrDefault((HardwareDriver f) => f.Number == hardwareScanTaskResult.HardwareDriverId);
			UriBuilder uriBuilder = ParseAddress(task.TaskProperties.GetValue("address"));
			uriBuilder.Port = hardwareScanTaskResult.Port;
			list.Add(MakePropertyNotSettable(RecordingServerConfigurationApi.HardwareAddressProperty(uriBuilder.Uri.AbsoluteUri)));
			list.Add(MakePropertyNotSettable(RecordingServerConfigurationApi.UserNameProperty(hardwareScanTaskResult.UserName)));
			list.Add(MakePropertyNotSettable(RecordingServerConfigurationApi.PasswordProperty(hardwareScanTaskResult.Password)));
			list.Add(MacAddressExistsGlobalProperty(hardwareScanTaskResult.MacExistsGlobal));
			list.Add(MacAddressExistsLocalProperty(hardwareScanTaskResult.MacExistsLocal));
			list.Add(HardwareScanValidatedProperty(hardwareScanTaskResult.Validated));
			list.Add(MacAddressProperty(hardwareScanTaskResult.MacAddress));
			if (hardwareDriver != null)
			{
				ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("HardwareDriver", hardwareDriver.Id);
				list.Add(TaskConfigurationApi.GetPath(path, "HardwareDriverPath"));
			}
		}
		return list.ToArray();
	}

	private static Property MakePropertyNotSettable(Property property)
	{
		property.IsSettable = false;
		property.ServerValidation = false;
		return property;
	}

	private static Property MacAddressExistsGlobalProperty(bool macAddressExistsGlobal)
	{
		return new Property("MacAddressExistsGlobal", PropertyTypesHelper.GetValue(macAddressExistsGlobal))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMacAddressExistsGlobal,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMacAddressExistsGlobal",
			ToolTipTranslationId = "PropertyRecordingServerMacAddressExistsGlobalToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property MacAddressExistsLocalProperty(bool macAddressExistsLocal)
	{
		return new Property("MacAddressExistsLocal", PropertyTypesHelper.GetValue(macAddressExistsLocal))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMacAddressExistsLocal,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMacAddressExistsLocal",
			ToolTipTranslationId = "PropertyRecordingServerMacAddressExistsLocalToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property HardwareScanValidatedProperty(bool validated)
	{
		return new Property("HardwareScanValidated", PropertyTypesHelper.GetValue(validated))
		{
			ValueType = "Tick",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerHardwareScanValidated,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerHardwareScanValidated",
			ToolTipTranslationId = "PropertyRecordingServerHardwareScanValidatedToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	private static Property MacAddressProperty(string macAddress)
	{
		return new Property("MacAddress", macAddress)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMacAddress,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMacAddress",
			ToolTipTranslationId = "PropertyRecordingServerMacAddressToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}

	public static ConfigurationItem[] CreateHardwareScanExpressResultChildItems(IGeminiRequestContext requestContext, Task task, string parentPath, ConfigurationItemPath path)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			HardwareScanResult[] hardwareScanResults = HardwareScanExpressTaskResult.FromXml(task.AsyncStatus.LastException).HardwareScanResults;
			foreach (HardwareScanResult hardwareScanResult in hardwareScanResults)
			{
				List<Property> list2 = new List<Property>();
				string value = task.TaskProperties.GetValue("idRecorder");
				HardwareDriver hardwareDriver = HardwareDriver.QueryActiveHardwareDrivers(requestContext, value).FirstOrDefault((HardwareDriver f) => f.Number == hardwareScanResult.HardwareDriverId);
				UriBuilder uriBuilder = ParseAddress(hardwareScanResult.Address);
				uriBuilder.Port = hardwareScanResult.Port;
				list2.Add(MakePropertyNotSettable(RecordingServerConfigurationApi.HardwareAddressProperty(uriBuilder.ToString())));
				list2.Add(MakePropertyNotSettable(RecordingServerConfigurationApi.UserNameProperty(hardwareScanResult.UserName)));
				list2.Add(MakePropertyNotSettable(RecordingServerConfigurationApi.PasswordProperty(hardwareScanResult.Password)));
				list2.Add(MacAddressExistsGlobalProperty(hardwareScanResult.MacExistsGlobal));
				list2.Add(MacAddressExistsLocalProperty(hardwareScanResult.MacExistsLocal));
				list2.Add(HardwareScanValidatedProperty(hardwareScanResult.Validated));
				list2.Add(MacAddressProperty(hardwareScanResult.MacAddress));
				if (hardwareDriver != null)
				{
					ConfigurationItemPath path2 = ConfigurationItemPath.CreateItemPath("HardwareDriver", hardwareDriver.Id);
					list2.Add(TaskConfigurationApi.GetPath(path2, "HardwareDriverPath"));
				}
				ConfigurationItem item = TaskConfigurationApi.ConstructTaskResult(list2, parentPath, path, new string[0]);
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	public static ConfigurationItem[] CreateDeleteRecorderResultChildItems(IGeminiRequestContext requestContext, Task task, string parentPath, ConfigurationItemPath path)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		if (task.AsyncStatus.ExceptionStatus == ExceptionStatus.Succeeded)
		{
			ConfigurationItem item = TaskConfigurationApi.ConstructTaskResult(CreateDeleteRecorderResultProperties(task).ToList(), parentPath, path, new string[0]);
			list.Add(item);
		}
		return list.ToArray();
	}

	public static Property[] CreateDeleteRecorderResultProperties(Task task)
	{
		return new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(task.TaskProperties.GetValue("recordingServerId")),
			GeneralPropertyDefinitions.GetNameProperty(task.TaskProperties.GetValue("recordingServerName"))
		}.ToArray();
	}

	private static UriBuilder ParseAddress(string address)
	{
		if (Uri.TryCreate(address, UriKind.Absolute, out var result))
		{
			return new UriBuilder(result);
		}
		return new UriBuilder(Uri.UriSchemeHttp, address);
	}
}
