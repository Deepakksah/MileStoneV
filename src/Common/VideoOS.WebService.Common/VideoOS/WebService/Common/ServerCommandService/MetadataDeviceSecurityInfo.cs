using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class MetadataDeviceSecurityInfo
{
	[DataMember]
	public bool Live { get; set; }

	[DataMember]
	public bool Browse { get; set; }

	[DataMember]
	public bool GetSequences { get; set; }

	[DataMember]
	public bool ExportDatabase { get; set; }

	[DataMember]
	public bool RetrieveEdgeRecordings { get; set; }

	[DataMember]
	public bool StartRecording { get; set; }

	[DataMember]
	public bool StopRecording { get; set; }

	[DataMember]
	public bool RetentionView { get; set; }

	[DataMember]
	public bool RetentionCreate { get; set; }

	[DataMember]
	public bool RetentionRemove { get; set; }

	[DataMember]
	public bool RestrictedMediaView { get; set; }

	[DataMember]
	public bool RestrictedMediaCreate { get; set; }

	[DataMember]
	public bool RestrictedMediaRemove { get; set; }
}
