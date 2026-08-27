using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class StreamControl<T> : IStreamControlAsync<T>, IStreamControlSync<T>, IStreamControlAwait<T> where T : StreamParams, new()
{
	protected readonly IConnection _connection;

	protected readonly Guid _videoId;

	protected readonly OnResponse<BaseCommandResponse> _onSuccessChangeStream;

	public T StreamParams { get; } = new T();

	public StreamControl(IConnection connection)
		: this(connection, Guid.Empty, (OnResponse<BaseCommandResponse>)null)
	{
	}

	public StreamControl(IConnection connection, Guid videoId, OnResponse<BaseCommandResponse> onSuccessChangeStream)
	{
		_connection = connection;
		_videoId = videoId;
		_onSuccessChangeStream = onSuccessChangeStream;
	}

	public IAsyncResult ChangeStream(T streamParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.ChangeStream);
		if (_videoId != Guid.Empty)
		{
			streamParams.VideoId = _videoId;
		}
		streamParams.ConvertToCommand(ref tCommand);
		OnResponse<BaseCommandResponse> a = delegate
		{
			StreamParams.InitFromAnother(streamParams);
		};
		return _connection.SendCommand(tCommand, (OnResponse<BaseCommandResponse>)Delegate.Combine((OnResponse<BaseCommandResponse>)Delegate.Combine(a, _onSuccessChangeStream), onSuccess), onFail);
	}

	public BaseCommandResponse ChangeStream(T streamParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ChangeStream(streamParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public async Task<BaseCommandResponse> ChangeStreamAsync(T tVideoParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = ChangeStream(tVideoParams);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}
}
