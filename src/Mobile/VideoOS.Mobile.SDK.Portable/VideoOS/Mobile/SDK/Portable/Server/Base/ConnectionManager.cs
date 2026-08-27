using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class ConnectionManager : IErrorResult
{
	private readonly List<ServerEndpoint> _endpoints;

	private readonly object _lockObject = new object();

	private readonly List<ResponseHandler<ConnectResponse>> _responseHandlers = new List<ResponseHandler<ConnectResponse>>();

	private readonly ManualResetEvent _waitEvent = new ManualResetEvent(initialState: false);

	private string _alias;

	private VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection _connection;

	private ChannelTypes _httpChannel;

	private bool _isRunning;

	private bool _isSecured;

	private Task _taskProcessResult;

	private string _uriScheme;

	public bool IsSecured
	{
		get
		{
			return _isSecured;
		}
		set
		{
			_isSecured = value;
			_httpChannel = (_isSecured ? ChannelTypes.HTTPSecure : ChannelTypes.HTTP);
			_uriScheme = (_isSecured ? "https" : "http");
		}
	}

	public string Alias
	{
		get
		{
			return _alias;
		}
		set
		{
			_alias = value;
		}
	}

	public VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection Connection => _connection;

	public Command.CommandResultTypes Result { get; private set; }

	public string ErrorMessage { get; private set; }

	public ErrorCodes ErrorCode { get; private set; }

	public ConnectionManager(List<ServerEndpoint> endpoints, bool isSecured)
	{
		_endpoints = endpoints ?? new List<ServerEndpoint>();
		IsSecured = isSecured;
	}

	public VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection Connect(TimeSpan timeout, ConnectParams connectParams = null)
	{
		IAsyncResult iResult = Connect(connectParams);
		ProcessResult(iResult, timeout);
		return Connection;
	}

	public void ProcessResult(IAsyncResult iResult, TimeSpan timeout)
	{
		iResult.AsyncWaitHandle.WaitOne(timeout);
		if (iResult.IsCompleted)
		{
			Result = Command.CommandResultTypes.ResultOk;
			ErrorMessage = "";
		}
		else
		{
			Result = Command.CommandResultTypes.ResultError;
			ErrorMessage = "Command Timed Out !";
			ErrorCode = ErrorCodes.CommandTimedOut;
		}
		ClearConnections();
	}

	private void ClearConnections()
	{
		List<ResponseHandler<ConnectResponse>> list;
		lock (_lockObject)
		{
			list = new List<ResponseHandler<ConnectResponse>>(_responseHandlers);
			_responseHandlers.Clear();
		}
		foreach (ResponseHandler<ConnectResponse> item in list)
		{
			item.Connection.Dispose();
		}
	}

	private void OnResponse(object sender, ResponseHandlerEventArgs<ConnectResponse> eventArgs)
	{
		ResponseHandler<ConnectResponse> responseHandler = sender as ResponseHandler<ConnectResponse>;
		VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection connection = null;
		bool flag = true;
		if (responseHandler != null)
		{
			lock (_lockObject)
			{
				if (eventArgs.Response.ErrorCode == ErrorCodes.Ok && _connection == null)
				{
					_connection = responseHandler.Connection;
					flag = false;
				}
				if (_responseHandlers.Contains(responseHandler))
				{
					_responseHandlers.Remove(responseHandler);
					if (flag)
					{
						connection = responseHandler.Connection;
					}
				}
			}
		}
		connection?.Dispose();
	}

	private void ProcessConnectResult()
	{
		while (Connection == null && _responseHandlers.Count > 0)
		{
			_waitEvent.WaitOne(10);
		}
		ClearConnections();
		_isRunning = false;
	}

	public IAsyncResult Connect(ConnectParams connectParams = null)
	{
		if (!_isRunning)
		{
			_isRunning = true;
			ClearConnection();
			List<Uri> list = new List<Uri>();
			foreach (ServerEndpoint endpoint in _endpoints)
			{
				try
				{
					Uri item = new Uri(endpoint.ConnectionString);
					list.Add(item);
				}
				catch (Exception ex)
				{
					Logger.Instance.Log(LogContent.Error, "ConnectionManager.Connect", ex.Message);
				}
			}
			IEnumerable<Uri> enumerable = list.Where((Uri uri) => uri.Scheme.Equals(_uriScheme, StringComparison.CurrentCultureIgnoreCase));
			lock (_lockObject)
			{
				foreach (Uri item2 in enumerable)
				{
					VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection obj = (string.IsNullOrEmpty(_alias) ? new VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection(_httpChannel, item2.Host, (uint)item2.Port) : new VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection(_httpChannel, item2.Host, (uint)item2.Port, $"{_alias}/Communication", plainAuthentication: false, requireCommandEncryption: false, null, null, $"{_alias}/Video"));
					obj.CommandsQueueing = CommandsQueueing.PerCommandTask;
					ResponseHandler<ConnectResponse> responseHandler = new ResponseHandler<ConnectResponse>(obj);
					responseHandler.OnResponse += OnResponse;
					_responseHandlers.Add(responseHandler);
				}
			}
			foreach (ResponseHandler<ConnectResponse> item3 in new List<ResponseHandler<ConnectResponse>>(_responseHandlers))
			{
				IAsyncResult asyncResult = item3.Connection.Connect(connectParams, item3.OnResponseHandler);
				item3.AsyncResult = asyncResult;
			}
			_taskProcessResult = Task.Factory.StartNew(ProcessConnectResult);
		}
		return _taskProcessResult;
	}

	private void ClearConnection()
	{
		if (_connection != null)
		{
			Disposing.DisposeMember(ref _connection);
		}
	}
}
