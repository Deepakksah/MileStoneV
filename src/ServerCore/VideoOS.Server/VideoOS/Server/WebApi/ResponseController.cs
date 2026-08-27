using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.WebApi.Models.Server;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.WebApi;

[RoutePrefix("api/recorders/{serviceid:guid}/responses")]
public sealed class ResponseController : ApiController
{
	private IRecorderCommManager _communicationManager;

	public ResponseController(IRecorderCommManager communicationManager)
	{
		_communicationManager = communicationManager;
	}

	[HttpPost]
	[Route("success")]
	public IHttpActionResult SuccessResponse(Guid serviceId, [FromBody] SuccessResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		SuccessResponse response = new SuccessResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("bool")]
	public IHttpActionResult BoolResponse(Guid serviceId, [FromBody] BoolResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		BoolResponse response = new BoolResponse(model.Value);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("error")]
	public IHttpActionResult ErrorResponse(Guid serviceId, [FromBody] ErrorResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ErrorResponse response = new ErrorResponse(model.ErrorCode, model.SubErrorCode, model.Message);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("exception")]
	public IHttpActionResult ExceptionResponse(Guid serviceId, [FromBody] ExceptionResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ExceptionResponse response = new ExceptionResponse(model.Exception);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("fail")]
	public IHttpActionResult FailResponse(Guid serviceId, [FromBody] FailResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		FailResponse response = new FailResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("timeout")]
	public IHttpActionResult TimeoutResponse(Guid serviceId, [FromBody] TimeoutResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		TimeoutResponse response = new TimeoutResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("warning")]
	public IHttpActionResult WarningResponse(Guid serviceId, [FromBody] WarningResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		WarningResponse response = new WarningResponse(model.Message);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("heartbeat")]
	public IHttpActionResult HeartbeatResponse(Guid serviceId, [FromBody] HeartbeatResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		HeartBeatResponse response = new HeartBeatResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("progress")]
	public IHttpActionResult ProgressResponse(Guid serviceId, [FromBody] ProgressResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ProgressResponse response = new ProgressResponse(model.Progress);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("getabsoluteptzposition")]
	public IHttpActionResult GetAbsolutePtzPositionResponse(Guid serviceId, [FromBody] GetAbsolutePtzPositionResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		GetAbsolutePtzPositionResponse response = VideoOS.Common.Integration.Command.GetAbsolutePtzPositionResponse.Create(model.DeviceId, model.Pan, model.Tilt, model.Zoom);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("hardwarescanprogress")]
	public IHttpActionResult HardwareScanProgressResponse(Guid serviceId, [FromBody] HardwareScanProgressResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		HardwareScanProgressResponse response = VideoOS.Common.Integration.Command.HardwareScanProgressResponse.Create(model.ScanResultXml);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("initializediscoverycomponentfailed")]
	public IHttpActionResult InitializeDiscoveryComponentFailedResponse(Guid serviceId, [FromBody] InitializeDiscoveryComponentFailedResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		InitializeDiscoveryComponentFailedResponse response = new InitializeDiscoveryComponentFailedResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("ptzreleasesession")]
	public IHttpActionResult ReleasePtzSessionResponse(Guid serviceId, [FromBody] PtzReleaseSessionResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		PtzSessionInfo ptzSessionInfo = new PtzSessionInfo(model.DeviceId, model.PatrollingProfileId, model.ManualPatrollingProfileId, model.OwnerIdentity, model.IsLocked, model.IsAnonymous, model.IsPaused, model.IsActive, model.Priority, model.IsLicensed);
		ReleasePtzSessionResponse response = VideoOS.Common.Integration.Command.ReleasePtzSessionResponse.Create(ptzSessionInfo);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("ptzreservesession")]
	public IHttpActionResult PtzReserveSessionResponse(Guid serviceId, [FromBody] PtzReserveSessionResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		PtzSessionInfo ptzSessionInfo = new PtzSessionInfo(model.DeviceId, model.PatrollingProfileId, model.ManualPatrollingProfileId, model.OwnerIdentity, model.IsLocked, model.IsAnonymous, model.IsPaused, model.IsActive, model.Priority, model.IsLicensed);
		PtzReserveSessionResponse response = VideoOS.Common.Integration.Command.PtzReserveSessionResponse.Create(ptzSessionInfo);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("registersessionfailed")]
	public IHttpActionResult RegisterSessionFailedResponse(Guid serviceId, [FromBody] RegisterSessionFailedResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		RegisterSessionFailedResponse response = new RegisterSessionFailedResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("startsessionfailed")]
	public IHttpActionResult StartSessionFailedResponse(Guid serviceId, [FromBody] StartSessionFailedResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		StartSessionFailedResponse response = new StartSessionFailedResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("discoverycomponentfailed")]
	public IHttpActionResult DiscoveryComponentFailedResponse(Guid serviceId, [FromBody] DiscoveryComponentFailedResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		DiscoveryComponentFailedResponse response = new DiscoveryComponentFailedResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("hardwarealreadyadded")]
	public IHttpActionResult HardwareAlreadyAddedResponse(Guid serviceId, [FromBody] HardwareAlreadyAddedResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		HardwareAlreadyAddedResponse response = new HardwareAlreadyAddedResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("hardwareunsupported")]
	public IHttpActionResult HardwareUnsupportedResponse(Guid serviceId, [FromBody] HardwareUnsupportedResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		HardwareUnsupportedResponse response = new HardwareUnsupportedResponse(model.ProductName, model.ProductVersion, model.ErrorCode);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("hardwaresetuperror")]
	public IHttpActionResult HardwareSetupErrorResponse(Guid serviceId, [FromBody] HardwareSetupErrorResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		HardwareSetupErrorResponse response = new HardwareSetupErrorResponse(model.ErrorCode, model.ErrorMessageInternal);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("synchronizedevicepresets")]
	public IHttpActionResult SynchronizeDevicePresetsResponse(Guid serviceId, [FromBody] SynchronizeDevicePresetsResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		SynchronizeDevicePresetsResponse response = new SynchronizeDevicePresetsResponse(model.PresetsXml);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("databasetrackidswithoutmediaonrecorder")]
	public IHttpActionResult DatabaseResponseTrackIdsWithoutMediaOnRecorder(Guid serviceId, [FromBody] DatabaseResponseTrackIdsWithoutMediaOnRecorderModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		DatabaseResponseTrackIdsWithoutMediaOnRecorder response = DatabaseResponseTrackIdsWithoutMediaOnRecorderDto.CreateDatabaseResponseTrackIdsWithoutMediaOnRecorder(model.DatabaseResponseTrackIdsWithoutMediaOnRecorder);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("preparemovehardware")]
	public IHttpActionResult PrepareMoveHardwareResponse(Guid serviceId, [FromBody] PrepareMoveHardwareResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		PrepareMoveHardwareResponse response = new PrepareMoveHardwareResponse(model.DeviceTracksWithLiveTable.Select((DeviceTrackIdDto t) => new DeviceTrackId(t.DeviceId, t.TrackId)));
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("databasediskusage")]
	public IHttpActionResult DatabaseResponseDiskUsage(Guid serviceId, [FromBody] DatabaseResponseDiskUsageModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		DatabaseResponseDiskUsage response = DatabaseResponseDiskUsageDto.CreateDatabaseResponseDiskUsage(model.DatabaseResponseDiskUsage);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("hardwaresetupinformation")]
	public IHttpActionResult HardwareSetupInformationResponse(Guid serviceId, [FromBody] HardwareSetupInformationResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		HardwareSetupInformation hardwareSetupInformation = HardwareSetupInformationDto.CreateHardwareSetupInformation(model.HardwareSetupInformation);
		HardwareSetupInformationResponse response = new HardwareSetupInformationResponse(hardwareSetupInformation);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("databasediskusages")]
	public IHttpActionResult DatabaseResponseDiskUsages(Guid serviceId, [FromBody] DatabaseResponseDiskUsagesModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		DatabaseResponseDiskUsages response = DatabaseResponseDiskUsagesDto.CreateDatabaseResponseDiskUsages(model.DatabaseResponseDiskUsages);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("databasearchiveinformation")]
	public IHttpActionResult DatabaseResponseArchiveInformation(Guid serviceId, [FromBody] DatabaseResponseArchiveInformationModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		DatabaseResponseArchiveInformation response = DatabaseResponseArchiveInformationDto.CreateDatabaseResponseArchiveInformation(model.DatabaseResponseArchiveInformation);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("serverfilesystemquerydrives")]
	public IHttpActionResult ServerFileSystemResponseQueryDrives(Guid serviceId, [FromBody] ServerFileSystemResponseQueryDrivesModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ServerFileSystemResponseQueryDrives response = ServerFileSystemResponseDtoConvert.CreateServerFileSystemResponseQueryDrives(model.Status, model.Message, model.Drives);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("serverfilesystemquerydirectories")]
	public IHttpActionResult ServerFileSystemResponseQueryDirectories(Guid serviceId, [FromBody] ServerFileSystemResponseQueryDirectoriesModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ServerFileSystemResponseQueryDirectories response = ServerFileSystemResponseDtoConvert.CreateServerFileSystemResponseQueryDirectories(model.Status, model.Message, model.Directories);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("serverfilesystemcreatedirectory")]
	public IHttpActionResult ServerFileSystemResponseCreateDirectory(Guid serviceId, [FromBody] ServerFileSystemResponseCreateDirectoryModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		ServerFileSystemResponseCreateDirectory response = ServerFileSystemResponseDtoConvert.CreateServerFileSystemResponseCreateDirectory(model.Status, model.Message);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("gethardwarenetworksettings")]
	public IHttpActionResult GetHardwareNetworkSettings(Guid serviceId, [FromBody] GetHardwareNetworkSettingsResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		GetHardwareNetworkSettingsResponse response = new GetHardwareNetworkSettingsResponse(model.HardwareId, model.HardwareAddress, model.HardwarePort, model.DefaultGateway, model.SubnetMask);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("gethardwareusers")]
	public IHttpActionResult GetHardwareUsers(Guid serviceId, [FromBody] GetHardwareUsersResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		GetHardwareUsersResponse response = new GetHardwareUsersResponse(model.HardwareId, model.HardwareUsers.Select(HardwareUserModel.ToHardwareUser).ToList());
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("querydeviceevidencelocksizes")]
	public IHttpActionResult QueryDeviceEvidenceLockSizes(Guid serviceId, [FromBody] QueryDeviceEvidenceLockSizesResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		QueryDeviceEvidenceLockSizesResponse response = model.ToResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("discoverycomponentsecurescanunsupported")]
	public IHttpActionResult DiscoveryComponentFailedDiscoveryComponentSecureScanUnsupportedResponse(Guid serviceId, [FromBody] DiscoveryComponentSecureScanUnsupportedResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		DiscoveryComponentSecureScanUnsupportedResponse response = new DiscoveryComponentSecureScanUnsupportedResponse();
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("fileupload")]
	public IHttpActionResult FileUploadResponse(Guid serviceId, [FromBody] FileUploadResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		FileUploadResponse response = new FileUploadResponse(model.StorageId);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	[HttpPost]
	[Route("firmwareupgradesuccess")]
	public IHttpActionResult FirmwareUpgradeSuccessResponse(Guid serviceId, [FromBody] FirmwareUpgradeSuccessResponseModel model)
	{
		if (model == null)
		{
			return BadRequest();
		}
		FirmwareUpgradeSuccessResponse response = new FirmwareUpgradeSuccessResponse(model.NewFirmwareVersion);
		ProcessResponse(response, model, serviceId);
		return StatusCode(HttpStatusCode.NoContent);
	}

	private void ProcessResponse(ResponseBase response, ResponseModel model, Guid serviceId)
	{
		response.Token = model.Token;
		_communicationManager.ProcessReceivedResponse(serviceId, response);
	}
}
