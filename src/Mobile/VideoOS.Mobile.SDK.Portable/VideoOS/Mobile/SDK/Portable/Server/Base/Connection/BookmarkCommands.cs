using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class BookmarkCommands : IErrorResult, IBookmarkAsync, IBookmarkCreateAsync, IBookmarkSync, IBookmarkCreateSync
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public BookmarkCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult CreateBookmark(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.CreateBookmark);
		bookmarkParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult UpdateBookmark(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.UpdateBookmark);
		bookmarkParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult DeleteBookmark(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.DeleteBookmark);
		bookmarkParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult GetBookmarks(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetBookmarks);
		bookmarkParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetBookmarkResponse(response));
	}

	public IAsyncResult RequestBookmarkCreation(BookmarkParams bookmarkParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.RequestBookmarkCreation);
		bookmarkParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public BaseCommandResponse CreateBookmark(BookmarkParams bookmarkParams, TimeSpan timeout)
	{
		IAsyncResult iResult = CreateBookmark(bookmarkParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse UpdateBookmark(BookmarkParams bookmarkParams, TimeSpan timeout)
	{
		IAsyncResult iResult = UpdateBookmark(bookmarkParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse DeleteBookmark(BookmarkParams bookmarkParams, TimeSpan timeout)
	{
		IAsyncResult iResult = DeleteBookmark(bookmarkParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse GetBookmarks(BookmarkParams bookmarkParams, TimeSpan timeout)
	{
		IAsyncResult bookmarks = GetBookmarks(bookmarkParams);
		return _connection.ProcessResult(bookmarks, timeout);
	}

	public BaseCommandResponse RequestBookmarkCreation(BookmarkParams bookmarkParams, TimeSpan timeout)
	{
		IAsyncResult iResult = RequestBookmarkCreation(bookmarkParams);
		return _connection.ProcessResult(iResult, timeout);
	}
}
