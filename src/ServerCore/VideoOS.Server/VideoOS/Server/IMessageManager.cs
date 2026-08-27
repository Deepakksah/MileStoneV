using System;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server;

public interface IMessageManager
{
	IMessageManagerResponseCollector SendCommand(Guid serviceId, UpdateSystemConfigurationCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, PrepareMoveHardwareCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, DatabaseOperationCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, ChangeHardwarePasswordCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, GetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, SetHardwareNetworkSettingsCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, GetHardwareUsersCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, PingDeviceCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, AddHardwareUserCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, DeleteHardwareUserCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, QueryDeviceEvidenceLockSizesCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, EvidenceLockOnOldRecorderCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, EvidenceLockOnOldRecorderCommand2 command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, SetupInitialHardwareUserCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, FirmwareUpgradeCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, FileUploadCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, CheckFilePresentOnRecorderCommand command, ICallContextInfo callContextInfo);

	IMessageManagerResponseCollector SendCommand(Guid serviceId, CheckExternalRecorderRegistrationCommand command, ICallContextInfo callContextInfo);

	void SendCommand(Guid serviceId, GetHardwareUsersCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo);

	void SendCommand(Guid serviceId, GetHardwareNetworkSettingsCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo);

	void SendCommand(Guid serviceId, SetHardwareNetworkSettingsCommand command, IMessageManagerCallback messageManagerCallback, TimeSpan timeout, ICallContextInfo callContextInfo);
}
