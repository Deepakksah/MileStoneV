using System.Linq;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

public class DeviceTypeRelation
{
	public static readonly DeviceTypeRelation[] Relations = new DeviceTypeRelation[6]
	{
		new DeviceTypeRelation("Camera", "CameraFolder", "CameraGroup", "CameraGroupFolder", DeviceType.Camera, "Camera", WellKnownDevicesGroupIds.AllCameras.ToString()),
		new DeviceTypeRelation("Microphone", "MicrophoneFolder", "MicrophoneGroup", "MicrophoneGroupFolder", DeviceType.Microphone, "AudioInput", WellKnownDevicesGroupIds.AllMicrophones.ToString()),
		new DeviceTypeRelation("Speaker", "SpeakerFolder", "SpeakerGroup", "SpeakerGroupFolder", DeviceType.Speaker, "AudioOutput", WellKnownDevicesGroupIds.AllSpeakers.ToString()),
		new DeviceTypeRelation("InputEvent", "InputEventFolder", "InputEventGroup", "InputEventGroupFolder", DeviceType.Input, "Input", WellKnownDevicesGroupIds.AllInputs.ToString()),
		new DeviceTypeRelation("Output", "OutputFolder", "OutputGroup", "OutputGroupFolder", DeviceType.Output, "Output", WellKnownDevicesGroupIds.AllOutputs.ToString()),
		new DeviceTypeRelation("Metadata", "MetadataFolder", "MetadataGroup", "MetadataGroupFolder", DeviceType.Metadata, "Metadata", WellKnownDevicesGroupIds.AllMetadata.ToString())
	};

	public string ItemType { get; private set; }

	public string FolderType { get; private set; }

	public string GroupType { get; private set; }

	public string GroupFolderType { get; private set; }

	public DeviceType DeviceType { get; private set; }

	public string BuiltInType { get; private set; }

	public string AllFolderId { get; private set; }

	private DeviceTypeRelation(string itemType, string folderType, string groupType, string groupFolderType, DeviceType deviceType, string builtInType, string allFolderId)
	{
		ItemType = itemType;
		FolderType = folderType;
		GroupType = groupType;
		GroupFolderType = groupFolderType;
		DeviceType = deviceType;
		BuiltInType = builtInType;
		AllFolderId = allFolderId;
	}

	public static string TryGetItemType(string builtInType)
	{
		DeviceTypeRelation deviceTypeRelation = Relations.FirstOrDefault((DeviceTypeRelation r) => r.BuiltInType == builtInType);
		if (deviceTypeRelation != null)
		{
			return deviceTypeRelation.ItemType;
		}
		return "Unknown";
	}
}
