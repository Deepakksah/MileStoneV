using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

public abstract class CommonResponseVisitorBaseAsync : ICommonResponseVisitorAsync
{
	private Func<ResponseBase, Task> _onResponseNotSupported = async delegate
	{
		await Task.CompletedTask;
	};

	protected CommonResponseVisitorBaseAsync()
	{
	}

	protected CommonResponseVisitorBaseAsync(Func<ResponseBase, Task> onResponseNotSupported)
		: this()
	{
		if (onResponseNotSupported == null)
		{
			throw new ArgumentNullException("onResponseNotSupported");
		}
		_onResponseNotSupported = onResponseNotSupported;
	}

	public virtual async Task Visit(RecorderNotFoundResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(RecorderUnavailableResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(RecorderNotSupportedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(RecorderNotLicensedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(SuccessResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(FailResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(BoolResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ErrorResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(WarningResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ExceptionResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ExceptionResponseDataContract response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(TimeoutResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(HeartBeatResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ProgressResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(SynchronizeDevicePresetsResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(HardwareSetupInformationResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(HardwareAlreadyAddedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(HardwareUnsupportedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(HardwareSetupErrorResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(BackupConfigStatusResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(BackupConfigCompletedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(BackupConfigAbortedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(BackupConfigFailedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(EvidenceLockFailedRemoveResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(RecorderStatusResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(FailoverStatusResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ReplaceHardwareResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ServerFileSystemResponseQueryDrives response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ServerFileSystemResponseQueryDirectories response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ServerFileSystemResponseCreateDirectory response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(DatabaseResponseDiskUsages response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(DatabaseResponseArchiveInformation response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(DatabaseResponseTrackIdsWithoutMediaOnRecorder response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(HardwareScanProgressResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(GetAbsolutePtzPositionResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(PtzReserveSessionResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(ReleasePtzSessionResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(InitializeDiscoveryComponentFailedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(RegisterSessionFailedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(StartSessionFailedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(DiscoveryComponentFailedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(DatabaseResponseDiskUsage response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(PrepareMoveHardwareResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(GetHardwareNetworkSettingsResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(GetHardwareUsersResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(QueryDeviceEvidenceLockSizesResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(DiscoveryComponentSecureScanUnsupportedResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(FileUploadResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(FirmwareUpgradeSuccessResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public virtual async Task Visit(MsFailoverStatusResponse response)
	{
		await OnResponseNotSupported(response);
	}

	public async Task NotSupported(ResponseBase response)
	{
		await OnResponseNotSupported(response);
	}

	protected virtual async Task OnResponseNotSupported(ResponseBase response)
	{
		await _onResponseNotSupported(response);
	}
}
