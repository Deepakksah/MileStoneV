using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class Views : IViewsSync, IViewsAsync, IViewsAwait
{
	private readonly IConnection _connection;

	public Views(IConnection connection)
	{
		_connection = connection;
	}

	public GetSubItemsResponse GetViews(ViewParams viewParams, TimeSpan timeout)
	{
		IAsyncResult views = GetViews(viewParams);
		return new GetSubItemsResponse(_connection.ProcessResult(views, timeout));
	}

	public GetAllItemsResponse GetAllViewsAndCameras(ViewParams viewParams, TimeSpan timeout)
	{
		IAsyncResult allViewsAndCameras = GetAllViewsAndCameras(viewParams);
		return new GetAllItemsResponse(_connection.ProcessResult(allViewsAndCameras, timeout));
	}

	public IAsyncResult GetViews(ViewParams viewParams, OnResponse<GetSubItemsResponse> onSuccess = null, OnResponse<GetSubItemsResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetViews);
		viewParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetSubItemsResponse(response));
	}

	public IAsyncResult GetAllViewsAndCameras(ViewParams viewParams, OnResponse<GetAllItemsResponse> onSuccess = null, OnResponse<GetAllItemsResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetAllViewsAndCameras);
		viewParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetAllItemsResponse(response));
	}

	public async Task<GetSubItemsResponse> GetViewsAsync(ViewParams viewParams, TimeSpan timeout)
	{
		IAsyncResult views = GetViews(viewParams);
		return new GetSubItemsResponse(await _connection.ProcessResultTask(views, timeout));
	}

	public async Task<GetAllItemsResponse> GetAllViewsAndCamerasAsync(ViewParams viewParams, TimeSpan timeout)
	{
		IAsyncResult allViewsAndCameras = GetAllViewsAndCameras(viewParams);
		return new GetAllItemsResponse(await _connection.ProcessResultTask(allViewsAndCameras, timeout));
	}
}
