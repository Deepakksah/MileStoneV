using System;

namespace VideoOS.Common.Integration.Command;

public abstract class CommonResponseVisitorBase : ICommonResponseVisitor
{
	private Action<ResponseBase> _onResponseNotSupported = delegate
	{
	};

	protected CommonResponseVisitorBase()
	{
	}

	protected CommonResponseVisitorBase(Action<ResponseBase> onResponseNotSupported)
		: this()
	{
		if (onResponseNotSupported == null)
		{
			throw new ArgumentNullException("onResponseNotSupported");
		}
		_onResponseNotSupported = onResponseNotSupported;
	}

	public virtual void Visit(RecorderNotFoundResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(RecorderUnavailableResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(RecorderNotSupportedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(RecorderNotLicensedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(SuccessResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(FailResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(BoolResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ErrorResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(WarningResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ExceptionResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ExceptionResponseDataContract response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(TimeoutResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(HeartBeatResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ProgressResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(SynchronizeDevicePresetsResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(HardwareSetupInformationResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(HardwareAlreadyAddedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(HardwareUnsupportedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(HardwareSetupErrorResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(BackupConfigStatusResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(BackupConfigCompletedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(BackupConfigAbortedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(BackupConfigFailedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(EvidenceLockFailedRemoveResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(RecorderStatusResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(FailoverStatusResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ReplaceHardwareResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ServerFileSystemResponseQueryDrives response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ServerFileSystemResponseQueryDirectories response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ServerFileSystemResponseCreateDirectory response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(DatabaseResponseDiskUsages response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(DatabaseResponseArchiveInformation response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(DatabaseResponseTrackIdsWithoutMediaOnRecorder response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(HardwareScanProgressResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(GetAbsolutePtzPositionResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(PtzReserveSessionResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(ReleasePtzSessionResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(InitializeDiscoveryComponentFailedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(RegisterSessionFailedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(StartSessionFailedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(DiscoveryComponentFailedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(DatabaseResponseDiskUsage response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(PrepareMoveHardwareResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(GetHardwareNetworkSettingsResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(GetHardwareUsersResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(QueryDeviceEvidenceLockSizesResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(DiscoveryComponentSecureScanUnsupportedResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(FileUploadResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(FirmwareUpgradeSuccessResponse response)
	{
		OnResponseNotSupported(response);
	}

	public virtual void Visit(MsFailoverStatusResponse response)
	{
		OnResponseNotSupported(response);
	}

	public void NotSupported(ResponseBase response)
	{
		OnResponseNotSupported(response);
	}

	protected virtual void OnResponseNotSupported(ResponseBase response)
	{
		_onResponseNotSupported(response);
	}
}
