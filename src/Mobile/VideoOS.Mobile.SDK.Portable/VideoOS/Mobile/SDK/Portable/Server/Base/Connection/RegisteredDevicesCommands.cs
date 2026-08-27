using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class RegisteredDevicesCommands : IRegisteredDevicesSync, IRegisteredDevicesAsync, IRegisteredDevicesAwait
{
	private readonly IConnection _connection;

	public RegisteredDevicesCommands(IConnection connection)
	{
		_connection = connection;
	}

	public GetRegisteredDevicesResponse GetRegisteredDevices(TimeSpan timeout)
	{
		IAsyncResult registeredDevices = GetRegisteredDevices();
		return new GetRegisteredDevicesResponse(_connection.ProcessResult(registeredDevices, timeout));
	}

	public BaseCommandResponse SetRegisteredDevicesState(IEnumerable<RegisteredDevice> devices, TimeSpan timeout)
	{
		IAsyncResult iResult = SetRegisteredDevicesState(devices);
		return _connection.ProcessResult(iResult, timeout);
	}

	public IAsyncResult GetRegisteredDevices(OnResponse<GetRegisteredDevicesResponse> onSuccess = null, OnResponse<GetRegisteredDevicesResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetRegisteredDevices);
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetRegisteredDevicesResponse(response));
	}

	public IAsyncResult SetRegisteredDevicesState(IEnumerable<RegisteredDevice> devices, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.SetRegisteredDevicesState);
		command.RegisteredDevices = new RegisteredDevices(devices.ToList());
		return _connection.SendCommand(command, onSuccess, onFail);
	}

	public async Task<GetRegisteredDevicesResponse> GetRegisteredDevicesAsync(TimeSpan timeout)
	{
		IAsyncResult registeredDevices = GetRegisteredDevices();
		return new GetRegisteredDevicesResponse(await _connection.ProcessResultTask(registeredDevices, timeout));
	}

	public async Task<BaseCommandResponse> SetRegisteredDevicesStateAsync(IEnumerable<RegisteredDevice> devices, TimeSpan timeout)
	{
		IAsyncResult asyncResult = SetRegisteredDevicesState(devices);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}
}
