namespace VideoOS.Common.Integration.Command;

public interface ICommonResponseVisitor
{
	void NotSupported(ResponseBase response);

	void Visit(RecorderNotFoundResponse response);

	void Visit(RecorderUnavailableResponse response);

	void Visit(RecorderNotSupportedResponse response);

	void Visit(RecorderNotLicensedResponse response);

	void Visit(SuccessResponse response);

	void Visit(FailResponse response);

	void Visit(BoolResponse response);

	void Visit(ErrorResponse response);

	void Visit(WarningResponse response);

	void Visit(ExceptionResponse response);

	void Visit(ExceptionResponseDataContract response);

	void Visit(TimeoutResponse response);

	void Visit(HeartBeatResponse response);

	void Visit(ProgressResponse response);

	void Visit(SynchronizeDevicePresetsResponse response);

	void Visit(HardwareSetupInformationResponse response);

	void Visit(HardwareAlreadyAddedResponse response);

	void Visit(BackupConfigStatusResponse response);

	void Visit(BackupConfigCompletedResponse response);

	void Visit(BackupConfigAbortedResponse response);

	void Visit(BackupConfigFailedResponse response);

	void Visit(EvidenceLockFailedRemoveResponse response);

	void Visit(RecorderStatusResponse response);

	void Visit(FailoverStatusResponse response);

	void Visit(ReplaceHardwareResponse response);

	void Visit(ServerFileSystemResponseQueryDrives response);

	void Visit(ServerFileSystemResponseQueryDirectories response);

	void Visit(ServerFileSystemResponseCreateDirectory response);

	void Visit(DatabaseResponseDiskUsages response);

	void Visit(DatabaseResponseArchiveInformation response);

	void Visit(DatabaseResponseTrackIdsWithoutMediaOnRecorder response);

	void Visit(HardwareScanProgressResponse response);

	void Visit(GetAbsolutePtzPositionResponse response);

	void Visit(PtzReserveSessionResponse response);

	void Visit(ReleasePtzSessionResponse response);

	void Visit(InitializeDiscoveryComponentFailedResponse response);

	void Visit(RegisterSessionFailedResponse response);

	void Visit(StartSessionFailedResponse response);

	void Visit(DiscoveryComponentFailedResponse response);

	void Visit(DatabaseResponseDiskUsage response);

	void Visit(HardwareUnsupportedResponse response);

	void Visit(HardwareSetupErrorResponse response);

	void Visit(PrepareMoveHardwareResponse response);

	void Visit(GetHardwareNetworkSettingsResponse response);

	void Visit(GetHardwareUsersResponse response);

	void Visit(DiscoveryComponentSecureScanUnsupportedResponse response);

	void Visit(QueryDeviceEvidenceLockSizesResponse response);

	void Visit(FileUploadResponse response);

	void Visit(FirmwareUpgradeSuccessResponse response);

	void Visit(MsFailoverStatusResponse response);
}
