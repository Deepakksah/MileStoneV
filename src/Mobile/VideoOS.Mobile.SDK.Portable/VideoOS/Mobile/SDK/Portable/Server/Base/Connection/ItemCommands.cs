using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ItemCommands : IItemsSync, IItemsAsync, IItemsAwait
{
	private readonly IConnection _connection;

	public ItemCommands(IConnection connection)
	{
		_connection = connection;
	}

	public GetAllItemsResponse GetItems(ItemParams itemParams, TimeSpan timeout)
	{
		IAsyncResult items = GetItems(itemParams);
		return new GetAllItemsResponse(_connection.ProcessResult(items, timeout));
	}

	public IAsyncResult GetItems(ItemParams itemParams, OnResponse<GetAllItemsResponse> onSuccess = null, OnResponse<GetAllItemsResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetItems);
		itemParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetAllItemsResponse(response));
	}

	public async Task<GetAllItemsResponse> GetItemsAsync(ItemParams itemParams, TimeSpan timeout)
	{
		IAsyncResult items = GetItems(itemParams);
		return new GetAllItemsResponse(await _connection.ProcessResultTask(items, timeout));
	}
}
