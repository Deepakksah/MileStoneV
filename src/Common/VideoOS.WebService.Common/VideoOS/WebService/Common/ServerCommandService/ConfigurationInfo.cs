using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class ConfigurationInfo
{
	[DataMember]
	public Guid ServerId { get; set; }

	[DataMember]
	public string ServerName { get; set; }

	[DataMember]
	public string ServerDescription { get; set; }

	[DataMember]
	public ServerOption[] ServerOptions { get; set; }

	[DataMember]
	public RecorderInfo[] Recorders { get; set; }

	[DataMember]
	public ApplicationSecurityInfo ApplicationAccess { get; set; }

	[DataMember]
	public int FailoverCheckInterval { get; set; }

	[DataMember]
	public LicenseInfo[] Licenses { get; set; }

	[DataMember]
	public DeviceMappingInfo[] DeviceMappings { get; set; }

	[DataMember]
	public CameraGroupInfo[] CameraGroups { get; set; }

	[DataMember]
	public InputGroupInfo[] InputGroups { get; set; }

	[DataMember]
	public OutputGroupInfo[] OutputGroups { get; set; }

	[DataMember]
	public MicrophoneGroupInfo[] MicrophoneGroups { get; set; }

	[DataMember]
	public SpeakerGroupInfo[] SpeakerGroups { get; set; }

	[DataMember]
	public MetadataDeviceGroupInfo[] MetadataDeviceGroups { get; set; }

	[DataMember]
	public EventTypeGroupInfo[] EventTypeGroups { get; set; }

	[DataMember]
	public EventTypeInfo[] EventTypes { get; set; }

	[DataMember]
	public SystemEventTypeInfo[] SystemEventTypes { get; set; }

	[DataMember]
	public AlertTypeGroupInfo[] AlertTypeGroups { get; set; }

	[DataMember]
	public AlertTypeInfo[] AlertTypes { get; set; }

	[DataMember]
	public MatrixMonitorInfo[] MatrixMonitors { get; set; }

	[DataMember]
	public BookmarkSettingInfo BookmarkSettings { get; set; }

	[DataMember]
	public RetentionSettingInfo RetentionSettings { get; set; }

	[DataMember]
	public AudioMessageInfo[] AudioMessages { get; set; }
}
