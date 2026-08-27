using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Communication.Internal;
using VideoOS.Common.Integration.WebApi.Models.Recorder;

namespace VideoOS.Server.RecorderCommunication;

public sealed class CommandTranslator132WebApi : CommonCommandVisitorBaseAsync
{
	private ICommunicator _communicator;

	private CommunicationCallContextDto _callContextInfo;

	private bool _incompatible;

	public CommandTranslator132WebApi(ICommunicator communicator, CommunicationCallContextDto callContextInfo, bool incompatible, Func<IVisitableCommandAsync, Task> onCommandNotSupported)
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
			await visitableCommandAsync.AcceptAsync(new CommandTranslator132WebApi(communicator, callContextInfo, incompatible, delegate
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
		GlobalEventModel obj = new GlobalEventModel
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
		ExecuteRuleActionsModel obj = new ExecuteRuleActionsModel
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
			CallContext = _callContextInfo
		};
		await _communicator.SendAsync("api/commands/hardware/changehardwarepassword", obj);
	}
}
