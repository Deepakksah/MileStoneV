using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[KnownType(typeof(AuditLogCommand))]
[KnownType(typeof(BackupConfigCommand))]
[KnownType(typeof(DatabaseOperationCommand))]
[KnownType(typeof(HardwareScanCommand))]
[KnownType(typeof(HardwareSetupInformationCommand))]
[KnownType(typeof(GetAbsolutePtzPositionCommand))]
[KnownType(typeof(PtzReserveSessionCommand))]
[KnownType(typeof(PtzReleaseSessionCommand))]
[KnownType(typeof(PtzStopSessionCommand))]
[KnownType(typeof(RecorderStatusCommand))]
[KnownType(typeof(ReplaceHardwareCommand))]
[KnownType(typeof(ServerFileSystemOperationCommand))]
[KnownType(typeof(SynchronizeDevicePresetsCommand))]
[KnownType(typeof(UpdateConfigurationManagerCommand))]
[KnownType(typeof(UpdateSecurityManagerCommand))]
[KnownType(typeof(DeleteConfigurationItemCommand))]
[KnownType(typeof(ChangeHardwarePasswordCommand))]
[KnownType(typeof(PingDeviceCommand))]
[KnownType(typeof(GetHardwareNetworkSettingsCommand))]
[KnownType(typeof(SetHardwareNetworkSettingsCommand))]
[KnownType(typeof(GetHardwareUsersCommand))]
[KnownType(typeof(AddHardwareUserCommand))]
[KnownType(typeof(SetupInitialHardwareUserCommand))]
[KnownType(typeof(DeleteHardwareUserCommand))]
[KnownType(typeof(FirmwareUpgradeCommand))]
[KnownType(typeof(UploadFileChunkCommand))]
[KnownType(typeof(FileUploadToRecorderCommand))]
[KnownType(typeof(CheckExternalRecorderRegistrationCommand))]
[KnownType(typeof(CheckMsFailoverStatusCommand))]
public abstract class CommandBase
{
	private Guid _token = Guid.NewGuid();

	private object _invocationInfo;

	[DataMember]
	public Guid Token
	{
		[DebuggerStepThrough]
		get
		{
			return _token;
		}
		[DebuggerStepThrough]
		set
		{
			_token = value;
		}
	}

	public object InvocationInfo
	{
		[DebuggerStepThrough]
		get
		{
			return _invocationInfo;
		}
		[DebuggerStepThrough]
		set
		{
			_invocationInfo = value;
		}
	}

	public CommandBase()
	{
	}
}
