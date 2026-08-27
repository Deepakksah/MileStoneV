using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Communication.Internal;
using VideoOS.Common.Integration.WebApi.Models.Recorder;
using VideoOS.Common.Integration.WebApi.Models.Server;

namespace VideoOS.Server.RecorderCommunication;

public sealed class CommandTranslator223WebApi : CommonCommandVisitorBaseAsync
{
	private ICommunicator _communicator;

	private CommunicationCallContextDto _callContextInfo;

	private bool _incompatible;

	public CommandTranslator223WebApi(ICommunicator communicator, CommunicationCallContextDto callContextInfo, bool incompatible, Func<IVisitableCommandAsync, Task> onCommandNotSupported)
		: base(onCommandNotSupported)
	{
		_callContextInfo = callContextInfo;
		_incompatible = incompatible;
		_communicator = communicator;
	}

	public static async Task<bool> Translate(ICommunicator communicator, CommandBase command, CommunicationCallContextDto callContextInfo, bool incompatible)
	{
		bool translationFailed = false;
		if (command is IVisitableCommandAsync visitableCommandAsync)
		{
			await visitableCommandAsync.AcceptAsync(new CommandTranslator223WebApi(communicator, callContextInfo, incompatible, delegate
			{
				translationFailed = true;
				return Task.CompletedTask;
			}));
		}
		else
		{
			translationFailed = true;
		}
		return !translationFailed;
	}

	public override async Task Visit(TriggerCustomEventBulkCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		List<TriggerEventCommandPackageDto> list = new List<TriggerEventCommandPackageDto>();
		foreach (CommandPackage command2 in command.Commands)
		{
			list.Add(new TriggerEventCommandPackageDto(command2.CommandBase, new CommunicationCallContextDto(command2.CallContextInfo.UserSid, command2.CallContextInfo.UserDomainAccountName, command2.CallContextInfo.LocationIPAddress)));
		}
		BulkEventModel obj = new BulkEventModel
		{
			Token = command.Token,
			EventCommands = list,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/events/triggercustomeventbulk", obj);
	}

	public override async Task Visit(TriggerEventAsGlobalCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		VideoOS.Common.Integration.WebApi.Models.Recorder.GlobalEventModel obj = new VideoOS.Common.Integration.WebApi.Models.Recorder.GlobalEventModel
		{
			Token = command.Token,
			SourceId = command.SourceId,
			EventCommand = TriggerEventDto.Create(command.TriggerEvent),
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/events/triggerglobalevent", obj);
	}

	public override async Task Visit(ExecuteRuleActionsCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		RuleActionExecutionRequestDto[] array = new RuleActionExecutionRequestDto[command.RuleActionExecutionRequests.Length];
		for (int i = 0; i < command.RuleActionExecutionRequests.Length; i++)
		{
			array[i] = RuleActionExecutionRequestDto.Create(command.RuleActionExecutionRequests[i]);
		}
		VideoOS.Common.Integration.WebApi.Models.Recorder.ExecuteRuleActionsModel obj = new VideoOS.Common.Integration.WebApi.Models.Recorder.ExecuteRuleActionsModel
		{
			Token = command.Token,
			SourceId = command.SourceId,
			RuleActionExecutionRequest = array,
			SenderUtcTime = command.SenderUtcTime,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/recorder/executeruleactions", obj);
	}

	public override async Task Visit(ServerFileSystemOperationCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		ServerFileSystemServerTypeDto serverType = DtoConvert.ToServerFileSystemServerTypeDto(command.ServerType);
		ServerFileSystemOperationDto operation = DtoConvert.ToServerFileSystemOperationDto(command.Operation);
		PerformFileSystemOperationModel obj = new PerformFileSystemOperationModel
		{
			ServerId = command.ServerId,
			Operation = operation,
			ServerType = serverType,
			Path = command.Path,
			CallContext = _callContextInfo,
			Token = command.Token
		};
		await _communicator.SendAsync("api/commands/recorder/performfilesystemoperation", obj);
	}

	public override async Task Visit(ActivateArchivingCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		ActivateArchiveModel obj = new ActivateArchiveModel
		{
			ArchiveStorageId = command.ArchiveStorageId,
			Token = command.Token,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/database/activatearchiving", obj);
	}

	public override async Task Visit(RecorderAttachCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		RecorderAttachModel obj = new RecorderAttachModel
		{
			CallContext = _callContextInfo,
			Token = command.Token
		};
		await _communicator.SendAsync("api/commands/recorder/attach", obj);
	}

	public override async Task Visit(HardwareScanCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		HardwareScanModel obj = new HardwareScanModel
		{
			AddressDriverPairsXml = command.AddressDriverPairsXml,
			AddressIntervalsXml = command.AddressIntervalsXml,
			CredentialsXml = command.CredentialsXml,
			HardwareScanOperationDto = DtoConvert.ToHardwareScanOperationDto(command.Operation),
			HostedScanParametersXml = command.HostedScanParametersXml,
			PrioritizedDriversXml = command.PrioritizedDriversXml,
			RecorderId = command.RecorderId,
			SelectedDriversXml = command.SelectedDriversXml,
			SessionId = command.SessionId,
			Token = command.Token,
			UseDefaultCredentials = command.UseDefaultCredentials,
			UseSecurePort = command.UseSecurePort,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/hardware/scan", obj);
	}

	public override async Task Visit(ReplaceHardwareUpdateSystemConfigurationCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		SystemConfigurationModel obj = new SystemConfigurationModel(command.SystemConfigurationDto.DataSet)
		{
			Token = command.Token,
			LastUpdate = command.SystemConfigurationDto.LastUpdate,
			NextUpdate = command.SystemConfigurationDto.NextUpdate,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/configuration/replacehardware", obj);
	}

	public override async Task Visit(UpdateSystemConfigurationCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		SystemConfigurationModel obj = new SystemConfigurationModel(command.SystemConfigurationDto.DataSet)
		{
			Token = command.Token,
			LastUpdate = command.SystemConfigurationDto.LastUpdate,
			NextUpdate = command.SystemConfigurationDto.NextUpdate,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/configuration/update", obj);
	}

	public override async Task Visit(UpdateSecurityConfigurationCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		SecurityConfigurationModel obj = new SecurityConfigurationModel(command.SecurityConfigurationDto.DataSet)
		{
			Token = command.Token,
			LastUpdate = command.SecurityConfigurationDto.LastUpdate,
			NextUpdate = command.SecurityConfigurationDto.NextUpdate,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/configuration/securityupdate", obj);
	}

	public override async Task Visit(UpdateUserSecurityTokensCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		SecurityUserTokensModel obj = new SecurityUserTokensModel
		{
			Token = command.Token,
			SecurityTokensDto = command.SecurityTokensDto,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/configuration/securityupdatetokens", obj);
	}

	public override async Task Visit(HardwareSetupInformationCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		HardwareSetupInformationModel obj = new HardwareSetupInformationModel
		{
			Token = command.Token,
			RecorderId = command.RecorderId,
			Address = command.Uri,
			UserName = command.UserName,
			Password = command.Password,
			CustomDriverData = command.CustomDriverData,
			DriverNumber = command.DriverNumber,
			AcceptMacAddress = command.AcceptMacAddress,
			AacSupported = command.AacSupported,
			HttpsPort = command.HttpsPort,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/hardware/getsetupinformation", obj);
	}

	public override async Task Visit(GetAbsolutePtzPositionCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		GetAbsolutePtzPositionModel obj = new GetAbsolutePtzPositionModel
		{
			DeviceId = command.DeviceId,
			Token = command.Token,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/ptz/getabsoluteptzposition", obj);
	}

	public override async Task Visit(PtzReserveSessionCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		PtzReserveSessionModel obj = new PtzReserveSessionModel
		{
			DeviceId = command.DeviceId,
			Token = command.Token,
			UserToken = command.UserToken,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/ptz/reservesession", obj);
	}

	public override async Task Visit(PtzReleaseSessionCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		PtzReleaseSessionModel obj = new PtzReleaseSessionModel
		{
			DeviceId = command.DeviceId,
			Token = command.Token,
			UserToken = command.UserToken,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/ptz/releasesession", obj);
	}

	public override async Task Visit(PtzStopSessionCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		PtzStopSessionModel obj = new PtzStopSessionModel
		{
			DeviceId = command.DeviceId,
			Token = command.Token,
			UserToken = command.UserToken,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/ptz/stopsession", obj);
	}

	public override async Task Visit(SynchronizeDevicePresetsCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		SynchronizeDevicePresetsModel obj = new SynchronizeDevicePresetsModel
		{
			DeviceId = command.DeviceId,
			Token = command.Token,
			RecorderId = command.RecorderId,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/ptz/synchronizedevicepresets", obj);
	}

	public override async Task Visit(DatabaseOperationCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		DatabaseOperationDto databaseOperation = DtoConvert.ToDatabaseOperationDto(command.DatabaseOperationType);
		Guid[] deviceIds = command.DeviceIds.ToArray();
		Guid[] trackIds = command.TrackIds.ToArray();
		DatabaseOperationModel obj = new DatabaseOperationModel
		{
			ArchiveDefinition = command.ArchiveDefinition,
			ArchiveId = command.ArchiveId,
			DatabaseOperation = databaseOperation,
			DeviceId = command.DeviceId,
			DeviceIds = deviceIds,
			NewPath = command.NewPath,
			Path = command.Path,
			RecorderId = command.RecorderId,
			StorageId = command.StorageId,
			Token = command.Token,
			TrackIds = trackIds,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/database/databaseoperation", obj);
	}

	public override async Task Visit(PrepareMoveHardwareCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		PrepareMoveHardwareModel obj = new PrepareMoveHardwareModel
		{
			Token = command.Token,
			HardwareId = command.HardwareId,
			TimeoutSeconds = command.TimeoutSeconds,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/hardware/preparemove", obj);
	}

	public override async Task Visit(ChangeHardwarePasswordCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		ChangeHardwarePasswordModel obj = new ChangeHardwarePasswordModel
		{
			Token = command.Token,
			HardwareId = command.HardwareId,
			NewPassword = command.NewPassword,
			TargetUserName = command.TargetUserName,
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/hardware/changehardwarepassword", obj);
	}

	public override async Task Visit(GetHardwareNetworkSettingsCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		GetHardwareNetworkSettingsModel obj = new GetHardwareNetworkSettingsModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			HardwareId = command.HardwareId
		};
		await _communicator.SendAsync("api/commands/hardware/gethardwarenetworksettings", obj);
	}

	public override async Task Visit(SetHardwareNetworkSettingsCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		SetHardwareNetworkSettingsModel obj = new SetHardwareNetworkSettingsModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			HardwareId = command.HardwareId,
			HardwareAddress = command.HardwareAddress,
			HardwarePort = command.HardwarePort,
			DefaultGateway = command.DefaultGateway,
			SubnetMask = command.SubnetMask
		};
		await _communicator.SendAsync("api/commands/hardware/sethardwarenetworksettings", obj);
	}

	public override async Task Visit(GetHardwareUsersCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		GetHardwareUsersModel obj = new GetHardwareUsersModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			HardwareId = command.HardwareId
		};
		await _communicator.SendAsync("api/commands/hardware/gethardwareusers", obj);
	}

	public override async Task Visit(DeleteHardwareUserCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		DeleteHardwareUserModel obj = new DeleteHardwareUserModel
		{
			Token = command.Token,
			CallContext = _callContextInfo,
			HardwareId = command.HardwareId,
			UserName = command.UserName
		};
		await _communicator.SendAsync("api/commands/hardware/deletehardwareuser", obj);
	}

	public override async Task Visit(PingDeviceCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		PingDeviceModel obj = new PingDeviceModel
		{
			Token = command.Token,
			CallContext = _callContextInfo,
			DeviceAddress = command.DeviceAddress,
			RecorderId = command.RecorderId,
			Timeout = command.Timeout
		};
		await _communicator.SendAsync("api/commands/hardware/pingdevice", obj);
	}

	public override async Task Visit(AddHardwareUserCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		AddHardwareUserModel obj = new AddHardwareUserModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			HardwareId = command.HardwareId,
			HardwareUser = HardwareUserModel.FromHardwareUser(command.HardwareUser)
		};
		await _communicator.SendAsync("api/commands/hardware/addhardwareuser", obj);
	}

	public override async Task Visit(QueryDeviceEvidenceLockSizesCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		QueryDeviceEvidenceLockSizesModel queryDeviceEvidenceLockSizesModel = QueryDeviceEvidenceLockSizesModel.FromCommand(command);
		queryDeviceEvidenceLockSizesModel.CallContext = _callContextInfo;
		queryDeviceEvidenceLockSizesModel.Token = command.Token;
		await _communicator.SendAsync("api/commands/database/querydeviceevidencelocksizes", queryDeviceEvidenceLockSizesModel);
	}

	public override async Task Visit(EvidenceLockOnOldRecorderCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		EvidenceLockOnOldRecorderModel evidenceLockOnOldRecorderModel = EvidenceLockOnOldRecorderModel.FromCommand(command);
		evidenceLockOnOldRecorderModel.CallContext = _callContextInfo;
		evidenceLockOnOldRecorderModel.Token = command.Token;
		await _communicator.SendAsync("api/commands/database/evidencelockonoldrecorder", evidenceLockOnOldRecorderModel);
	}

	public override async Task Visit(SetupInitialHardwareUserCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		HardwareInitialSetupModel obj = new HardwareInitialSetupModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			RecorderId = command.RecorderId,
			HardwareUri = command.HardwareUri,
			DriverNumber = command.DriverNumber,
			HardwareUser = HardwareUserModel.FromHardwareUser(command.HardwareUser),
			SecurePort = command.SecurePort
		};
		await _communicator.SendAsync("api/commands/hardware/setupinitialhardwareuser", obj);
	}

	public override async Task Visit(FirmwareUpgradeCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		FirmwareUpgradeModel obj = new FirmwareUpgradeModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			HardwareId = command.HardwareId,
			FileId = command.FileId
		};
		await _communicator.SendAsync("api/commands/hardware/firmwareupgrade", obj);
	}

	public override async Task Visit(FileUploadCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		FileUploadModel value = new FileUploadModel
		{
			CallContext = _callContextInfo,
			Token = command.Token
		};
		_ = ServerConfig.FileStorageFolderPath;
		using Stream fileStream = command.Storage.CreateStream();
		MultipartFormDataContent val = new MultipartFormDataContent();
		ObjectContent<FileUploadModel> objectContent = new ObjectContent<FileUploadModel>(value, new JsonMediaTypeFormatter());
		HttpContent val2 = (HttpContent)(object)((command.UploadProgress == null) ? ((StreamContentWithProgress)new StreamContent(fileStream)) : new StreamContentWithProgress(fileStream, command.UploadProgress));
		((MultipartContent)val).Add((HttpContent)(object)objectContent);
		val.Add(val2, "payload", "firmware");
		await _communicator.SendRawAsync("api/commands/hardware/fileupload", (HttpContent)(object)val);
	}

	public override async Task Visit(CheckFilePresentOnRecorderCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		CheckFilePresentOnRecorderModel obj = new CheckFilePresentOnRecorderModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			FileId = command.FileId
		};
		await _communicator.SendAsync("api/commands/hardware/checkfileonrecorder", obj);
	}

	public override async Task Visit(CheckExternalRecorderRegistrationCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		if (_incompatible)
		{
			await OnCommandNotSupported(command);
			return;
		}
		CheckExternalRecorderRegistrationModel obj = new CheckExternalRecorderRegistrationModel
		{
			CallContext = _callContextInfo,
			Token = command.Token,
			HardwareId = command.HardwareId,
			Address = command.Address
		};
		await _communicator.SendAsync("api/commands/hardware/checkexternalrecorderregistration", obj);
	}
}
