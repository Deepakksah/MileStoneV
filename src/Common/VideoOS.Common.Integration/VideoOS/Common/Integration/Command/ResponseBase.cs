using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[KnownType(typeof(SuccessResponse))]
[KnownType(typeof(FailResponse))]
[KnownType(typeof(BoolResponse))]
[KnownType(typeof(ErrorResponse))]
[KnownType(typeof(WarningResponse))]
[KnownType(typeof(ExceptionResponseDataContract))]
[KnownType(typeof(TimeoutResponse))]
[KnownType(typeof(HeartBeatResponse))]
[KnownType(typeof(ProgressResponse))]
[KnownType(typeof(BackupConfigStatusResponse))]
[KnownType(typeof(BackupConfigCompletedResponse))]
[KnownType(typeof(BackupConfigAbortedResponse))]
[KnownType(typeof(BackupConfigFailedResponse))]
[KnownType(typeof(DatabaseResponse))]
[KnownType(typeof(HardwareScanProgressResponse))]
[KnownType(typeof(GetAbsolutePtzPositionResponse))]
[KnownType(typeof(PtzReserveSessionResponse))]
[KnownType(typeof(ReleasePtzSessionResponse))]
[KnownType(typeof(InitializeDiscoveryComponentFailedResponse))]
[KnownType(typeof(RegisterSessionFailedResponse))]
[KnownType(typeof(StartSessionFailedResponse))]
[KnownType(typeof(DiscoveryComponentFailedResponse))]
[KnownType(typeof(HardwareSetupInformationResponse))]
[KnownType(typeof(HardwareAlreadyAddedResponse))]
[KnownType(typeof(HardwareUnsupportedResponse))]
[KnownType(typeof(HardwareSetupErrorResponse))]
[KnownType(typeof(RecorderStatusResponse))]
[KnownType(typeof(RecorderNotSupportedResponse))]
[KnownType(typeof(RecorderNotLicensedResponse))]
[KnownType(typeof(RecorderUnavailableResponse))]
[KnownType(typeof(RecorderNotFoundResponse))]
[KnownType(typeof(RecorderAttachCommand))]
[KnownType(typeof(FailoverStatusResponse))]
[KnownType(typeof(ReplaceHardwareResponse))]
[KnownType(typeof(ServerFileSystemResponse))]
[KnownType(typeof(ServerFileSystemResponseQueryDrives))]
[KnownType(typeof(ServerFileSystemResponseQueryDirectories))]
[KnownType(typeof(ServerFileSystemResponseCreateDirectory))]
[KnownType(typeof(SynchronizeDevicePresetsResponse))]
[KnownType(typeof(EvidenceLockFailedRemoveResponse))]
[KnownType(typeof(GetHardwareNetworkSettingsResponse))]
[KnownType(typeof(GetHardwareUsersResponse))]
[KnownType(typeof(DiscoveryComponentSecureScanUnsupportedResponse))]
[KnownType(typeof(FirmwareUpgradeSuccessResponse))]
[KnownType(typeof(FileUploadResponse))]
[KnownType(typeof(MsFailoverStatusResponse))]
[KnownType(typeof(CommonResponseBase))]
public abstract class ResponseBase
{
	[DataMember]
	public Guid Token { get; set; }

	public ResponseBase()
	{
		Token = Guid.Empty;
	}

	protected void Deserialize(SerializationInfo info)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		Token = new Guid(info.GetString("Token"));
	}

	protected void Serialize(SerializationInfo info)
	{
		if (info == null)
		{
			throw new ArgumentNullException("info");
		}
		info.AddValue("Token", Token.ToString());
	}
}
