using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public interface ICommonResponseVisitorAsync
{
	Task NotSupported(ResponseBase response);

	Task Visit(RecorderNotFoundResponse response);

	Task Visit(RecorderUnavailableResponse response);

	Task Visit(RecorderNotSupportedResponse response);

	Task Visit(RecorderNotLicensedResponse response);

	Task Visit(SuccessResponse response);

	Task Visit(FailResponse response);

	Task Visit(BoolResponse response);

	Task Visit(ErrorResponse response);

	Task Visit(WarningResponse response);

	Task Visit(ExceptionResponse response);

	Task Visit(ExceptionResponseDataContract response);

	Task Visit(TimeoutResponse response);

	Task Visit(HeartBeatResponse response);

	Task Visit(ProgressResponse response);

	Task Visit(SynchronizeDevicePresetsResponse response);

	Task Visit(HardwareSetupInformationResponse response);

	Task Visit(HardwareAlreadyAddedResponse response);

	Task Visit(BackupConfigStatusResponse response);

	Task Visit(BackupConfigCompletedResponse response);

	Task Visit(BackupConfigAbortedResponse response);

	Task Visit(BackupConfigFailedResponse response);

	Task Visit(EvidenceLockFailedRemoveResponse response);

	Task Visit(RecorderStatusResponse response);

	Task Visit(FailoverStatusResponse response);

	Task Visit(ReplaceHardwareResponse response);

	Task Visit(ServerFileSystemResponseQueryDrives response);

	Task Visit(ServerFileSystemResponseQueryDirectories response);

	Task Visit(ServerFileSystemResponseCreateDirectory response);

	Task Visit(DatabaseResponseDiskUsages response);

	Task Visit(DatabaseResponseArchiveInformation response);

	Task Visit(DatabaseResponseTrackIdsWithoutMediaOnRecorder response);

	Task Visit(HardwareScanProgressResponse response);

	Task Visit(GetAbsolutePtzPositionResponse response);

	Task Visit(PtzReserveSessionResponse response);

	Task Visit(ReleasePtzSessionResponse response);

	Task Visit(InitializeDiscoveryComponentFailedResponse response);

	Task Visit(RegisterSessionFailedResponse response);

	Task Visit(StartSessionFailedResponse response);

	Task Visit(DiscoveryComponentFailedResponse response);

	Task Visit(DatabaseResponseDiskUsage response);

	Task Visit(HardwareUnsupportedResponse response);

	Task Visit(HardwareSetupErrorResponse response);

	Task Visit(PrepareMoveHardwareResponse response);

	Task Visit(GetHardwareNetworkSettingsResponse response);

	Task Visit(GetHardwareUsersResponse response);

	Task Visit(DiscoveryComponentSecureScanUnsupportedResponse response);

	Task Visit(QueryDeviceEvidenceLockSizesResponse response);

	Task Visit(FileUploadResponse response);

	Task Visit(FirmwareUpgradeSuccessResponse response);

	Task Visit(MsFailoverStatusResponse response);
}
