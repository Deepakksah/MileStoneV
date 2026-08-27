using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public static class SpecialCases
{
	private static List<string> _internalMethods = null;

	private static readonly List<string> RestApiPrivateProperties = new List<string> { "recordingServerPath" };

	private static readonly List<string> QueryTypes = new List<string> { "Hardware", "Camera", "Microphone", "Speaker", "Metadata", "InputEvent", "Output" };

	public static INameConvert NameConvert { get; set; } = new NameConvert();

	public static bool IsMethodInternal(string id)
	{
		if (_internalMethods == null)
		{
			_internalMethods = new List<string> { "LoadTasks", "AddDeviceGroup", "RemoveDeviceGroup", "AddBasicOwnerInfo", "RemoveBasicOwnerInfo" };
			foreach (string understoodItemType in NameConvert.UnderstoodItemTypes)
			{
				_internalMethods.Add("Add" + understoodItemType);
				_internalMethods.Add("Remove" + understoodItemType);
				_internalMethods.Add("Delete" + understoodItemType);
				_internalMethods.Add("MethodIdAdd" + understoodItemType);
				_internalMethods.Add("MethodIdRemove" + understoodItemType);
				_internalMethods.Add("MethodIdDelete" + understoodItemType);
			}
			_internalMethods.Remove("AddStream");
			_internalMethods.Remove("RemoveStream");
			_internalMethods.Remove("AddHardware");
			_internalMethods.Remove("RemoveRecordingServer");
		}
		return _internalMethods.Contains(id);
	}

	public static bool IsPropertyRestApiPrivate(string key)
	{
		return RestApiPrivateProperties.Contains(key);
	}

	public static bool UseQueryForMethodSupportedForArray(string itemType)
	{
		return QueryTypes.Contains(itemType);
	}

	public static ConfigurationItemPath GetConfigurationItemPath(string itemType)
	{
		if (itemType == "Hardware")
		{
			string folderType = "HardwareFolder";
			return ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), folderType);
		}
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.First((DeviceTypeRelation r) => r.ItemType == itemType);
		return ConfigurationItemPath.CreateFolderPath(deviceTypeRelation.GroupType, deviceTypeRelation.AllFolderId, deviceTypeRelation.FolderType);
	}
}
