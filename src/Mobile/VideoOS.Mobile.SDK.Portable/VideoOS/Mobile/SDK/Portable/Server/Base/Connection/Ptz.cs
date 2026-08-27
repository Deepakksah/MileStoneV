using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class Ptz : IPtzSync, IPtzAsync, IPtzAwait
{
	private readonly IConnection _connection;

	private readonly Guid _videoId;

	public Ptz(IConnection connection)
		: this(connection, Guid.Empty)
	{
	}

	public Ptz(IConnection connection, Guid videoId)
	{
		_connection = connection;
		_videoId = videoId;
	}

	public BaseCommandResponse ControlPtz(PtzParams tPtzParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ControlPtz(tPtzParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public GetPtzPresetsResponse GetPtzPresets(Guid cameraId, TimeSpan timeout)
	{
		IAsyncResult ptzPresets = GetPtzPresets(cameraId);
		return new GetPtzPresetsResponse(_connection.ProcessResult(ptzPresets, timeout));
	}

	public IAsyncResult ControlPtz(PtzParams tPtzParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.ControlPTZ);
		if (_videoId != Guid.Empty)
		{
			tCommand.InputParams.Add(new Parameter("VideoId", _videoId.ToString()));
			tCommand.SetCommandKind(Command.CommandKinds.ChangeStream);
		}
		tPtzParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult GetPtzPresets(Guid cameraId, OnResponse<GetPtzPresetsResponse> onSuccess = null, OnResponse<GetPtzPresetsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetPtzPresets);
		if (_videoId != Guid.Empty)
		{
			command.InputParams.Add(new Parameter("VideoId", _videoId.ToString()));
		}
		if (cameraId != Guid.Empty)
		{
			command.InputParams.Add(new Parameter("CameraId", cameraId.ToString()));
		}
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetPtzPresetsResponse(response));
	}

	public async Task<BaseCommandResponse> ControlPtzAsync(PtzParams tPtzParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = ControlPtz(tPtzParams);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}

	public async Task<GetPtzPresetsResponse> GetPtzPresetsAsync(Guid cameraId, TimeSpan timeout)
	{
		IAsyncResult ptzPresets = GetPtzPresets(cameraId);
		return new GetPtzPresetsResponse(await _connection.ProcessResultTask(ptzPresets, timeout));
	}
}
