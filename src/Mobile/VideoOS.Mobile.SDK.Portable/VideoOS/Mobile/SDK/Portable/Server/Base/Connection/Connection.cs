using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.CommEntries;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.Audio;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults.Processors;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;
using VideoOS.Mobile.SDK.Portable.Server.Communication;
using VideoOS.Mobile.SDK.Portable.Server.HTTP;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class Connection : IDisposable, IDisposableQuiet, IControl, IConnection, IConnectionInternal, IConnectionEndPoint, IOnVideoStream, IConnectionAsync, IAsyncModeCallback, IConnectionAwait, IConnectionSync, IErrorResult
{
	public delegate void OnReceivedResponse(CommunicationEntry response);

	public delegate EncryptionProperties ConnectionEncryptionProperties();

	public delegate IAsyncResult OnSendCommand(Command tCommand);

	private class QueueEntry
	{
		public string Address;

		public CommunicationEntry Entry;

		public CommandAsyncResultBase Result;

		public OnResponse<BaseCommandResponse> OnSuccess;

		public OnResponse<BaseCommandResponse> OnFail;
	}

	public class ProcessAsyncTask
	{
		public IAsyncResult Result { get; set; }

		public AsyncCallback Callback { get; set; }

		public TimeSpan Timeout { get; set; }

		public IConnection Processor { get; set; }

		public void Run()
		{
			try
			{
				Processor.ProcessResult(Result, Timeout);
				Callback(Result);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "ProcessAsyncTask.Run()", ex.Message);
			}
		}
	}

	[Disposing(true)]
	private CycleTaskMember _thread;

	private readonly Queue<QueueEntry> _commandsQueue;

	private readonly Dictionary<string, QueueEntry> _responsesQueue;

	private readonly List<Command.CommandKinds> _commandEncryptionList;

	private uint _commandNumber;

	private IChannel _iChannel;

	private readonly string _ipAddress;

	private readonly uint _port;

	private DateTime _lastSendHeartBeatTimeUtc;

	private string _connectionId;

	private DHEncryption _encryption;

	private readonly bool _plainAuthentication;

	private bool _disposed;

	private readonly ChallengesManager _challengesManager;

	private readonly bool _challengesEnabled;

	private readonly ServerCapabilities _serverCapabilities = new ServerCapabilities();

	private ServerFeatures _serverFeatures = new ServerFeatures();

	private UserIdentityParams _userIdentity = new UserIdentityParams();

	private readonly ErrorHandlers _errorHandlers = new ErrorHandlers();

	private readonly StateChanger _stateChanger = new StateChanger();

	private readonly VideoFactory _videoFactory;

	private List<ServerEndpoint> _serverEndpoints;

	private readonly AudioFactory _audioFactory;

	private readonly List<IStreamReconnection> _videos = new List<IStreamReconnection>();

	private readonly object _videosLocker = new object();

	public ChannelTypes ChannelType { get; private set; }

	public string CommunicationAlias { get; private set; }

	public string VideoAlias { get; private set; }

	public string AudioAlias { get; private set; }

	public string ConnectionId => _connectionId;

	public bool PlainTextAuthentication => _plainAuthentication;

	public bool RequireCommandEncryption { get; set; }

	public bool RunHeartBeat { get; set; }

	public int ConnectionTimeoutS { get; private set; }

	public bool PeriodicLiveMessage { get; private set; }

	public int HeartBeatTimeoutS { get; private set; }

	public CommandsQueueing CommandsQueueing { get; set; }

	public bool ChapSecurityEnabled { get; set; }

	public bool AutoUpdateConfiguration { get; set; }

	public IServerCapabilities ServerCapabilities => _serverCapabilities;

	public ICapabilities Capabilities => _serverCapabilities;

	public IServerFeatures ServerFeatures => _serverFeatures;

	public UserIdentityParams UserIdentity => _userIdentity;

	public IErrorHandlers ErrorHandlers => _errorHandlers;

	public IStateChange StateChange => _stateChanger;

	public AlarmCommands Alarms { get; }

	public AccessControlCommands AccessControl { get; }

	public NotificationCommands Notifications { get; }

	public NotificationEventHandler NotificationEventHandler { get; set; }

	public AddressBookServiceCommands AddressBookService { get; }

	public InvitaionCommands Invitation { get; }

	public InvestigationCommands Investigations { get; }

	public InvestigationExportsCommands InvestigationExports { get; }

	public ActionsCommands Actions { get; }

	public RegisteredDevicesCommands RegisteredDevices { get; }

	public ExportsCommands Exports { get; }

	public SequencesCommands Sequences { get; }

	public UtilitiesCommands Utilities { get; }

	public ThumbnailCommands Thumbnail { get; }

	public Views Views { get; }

	public ItemCommands Items { get; }

	public Ptz Ptz { get; }

	public VideoCommands Video { get; }

	public AudioCommands Audio { get; }

	public MulticameraPlayback MulticameraPlayback { get; }

	public TwoStepVerification TwoStepVerification { get; }

	public IVideoFactory VideoFactory => _videoFactory;

	public IAudioFactory AudioFactory => _audioFactory;

	public BookmarkCommands Bookmarks { get; }

	public SmartMapCommands SmartMap { get; }

	public IEnumerable<ServerEndpoint> ServerEndpoints => _serverEndpoints;

	public string IpAddress => _ipAddress;

	public uint Port => _port;

	public MessageTypes MessageType { get; set; }

	public Command.CommandResultTypes Result { get; private set; }

	public string ErrorMessage { get; private set; }

	public ErrorCodes ErrorCode { get; private set; }

	public Connection(ChannelTypes channelType, string ipAddress, uint port, string commandAlias = null, bool plainAuthentication = false, bool requireCommandEncryption = false, List<Command.CommandKinds> commandEncryptionList = null, IReadOnlyDictionary<string, string> headers = null, string videoAlias = "XProtectMobile/Video", string audioAlias = "XProtectMobile/Audio")
	{
		_disposed = false;
		_commandNumber = 0u;
		_commandsQueue = new Queue<QueueEntry>();
		_responsesQueue = new Dictionary<string, QueueEntry>();
		_lastSendHeartBeatTimeUtc = DateTime.UtcNow;
		_challengesManager = new ChallengesManager();
		_challengesManager.GetChallenges = GetChallengesCallback;
		ChannelType = channelType;
		_ipAddress = ipAddress;
		_port = port;
		CommunicationAlias = (string.IsNullOrWhiteSpace(commandAlias) ? Aliases.HttpCommunicationAlias : commandAlias);
		VideoAlias = videoAlias;
		_videoFactory = new VideoFactory(this, this, videoAlias);
		AudioAlias = audioAlias;
		_audioFactory = new AudioFactory(this, this, audioAlias);
		RequireCommandEncryption = requireCommandEncryption;
		_commandEncryptionList = commandEncryptionList ?? new List<Command.CommandKinds>();
		_plainAuthentication = plainAuthentication;
		CommandsQueueing = CommandsQueueing.SingleThread;
		ChapSecurityEnabled = true;
		AutoUpdateConfiguration = true;
		_thread = new CycleTaskMember((Action)Run, (Action)null, (Action)null, 10);
		_thread.Name = "Connection.Run";
		HttpChannel httpChannel = null;
		switch (channelType)
		{
		case ChannelTypes.HTTP:
			httpChannel = new HttpChannel(ipAddress, port, CommunicationAlias, () => _encryption);
			_challengesEnabled = true;
			break;
		case ChannelTypes.HTTPSecure:
			httpChannel = new HttpSecureChannel(ipAddress, port, CommunicationAlias, () => _encryption);
			break;
		default:
			throw new NotImplementedException("This ChannelType is NOT supported !");
		}
		httpChannel.Headers = headers;
		httpChannel.GetMesageTypeFunc = GetMessageTypeInternal;
		_iChannel = httpChannel;
		_iChannel.OnResponse = OnEntryResponse;
		IChannel iChannel = _iChannel;
		iChannel.ConnectionEncryptionProperties = (ConnectionEncryptionProperties)Delegate.Combine(iChannel.ConnectionEncryptionProperties, new ConnectionEncryptionProperties(GetEncryptionProperties));
		ErrorHandlers.SubscribeToError(ErrorCodes.WrongConnectionID, OnConnectionError);
		ErrorHandlers.SubscribeToError(ErrorCodes.HttpRequestError, OnConnectionError);
		ErrorHandlers.SubscribeToError(ErrorCodes.HttpResponseError, OnConnectionError);
		ErrorHandlers.SubscribeToError(ErrorCodes.SecurityError, OnConnectionError);
		Start();
		Alarms = new AlarmCommands(this);
		AccessControl = new AccessControlCommands(this);
		Audio = new AudioCommands(this);
		Notifications = new NotificationCommands(this);
		AddressBookService = new AddressBookServiceCommands(this);
		Investigations = new InvestigationCommands(this);
		InvestigationExports = new InvestigationExportsCommands(this);
		Invitation = new InvitaionCommands(this);
		Actions = new ActionsCommands(this);
		RegisteredDevices = new RegisteredDevicesCommands(this);
		Exports = new ExportsCommands(this);
		Sequences = new SequencesCommands(this);
		Utilities = new UtilitiesCommands(this, OnServerFeatures);
		Thumbnail = new ThumbnailCommands(this);
		Views = new Views(this);
		Items = new ItemCommands(this);
		TwoStepVerification = new TwoStepVerification(this, OnSuccessVerifyTsvCode);
		Ptz = new Ptz(this);
		Video = new VideoCommands(this);
		MulticameraPlayback = new MulticameraPlayback(this);
		Bookmarks = new BookmarkCommands(this);
		SmartMap = new SmartMapCommands(this);
	}

	public Connection(ChannelTypes channelType, string ipAddress, uint port, string connectionId, IServerCapabilities serverCapabilities, string commandAlias = null, string videoAlias = "XProtectMobile/Video", string audioAlias = "XProtectMobile/Audio")
		: this(channelType, ipAddress, port, commandAlias, plainAuthentication: false, requireCommandEncryption: false, null, null, videoAlias, audioAlias)
	{
		_connectionId = connectionId;
		_serverCapabilities.InitFromAnother(serverCapabilities);
		_stateChanger.ChangeState(ConnectionStates.LoggedIn);
		SetConnectionTimeoutS(_serverCapabilities.ConnectionTimeoutS);
	}

	public EncryptionProperties GetEncryptionProperties()
	{
		return new EncryptionProperties
		{
			RequireCommandEncryption = RequireCommandEncryption,
			ServerSupportsCommandEncryption = _serverCapabilities.CommandEncryptionSupported,
			CommandEncryptionList = _commandEncryptionList
		};
	}

	public void AddCommandForEncryption(Command cmd)
	{
		Command.CommandKinds commandKind = cmd.GetCommandKind();
		if (!_commandEncryptionList.Contains(commandKind) && commandKind != Command.CommandKinds.Connect)
		{
			_commandEncryptionList.Add(commandKind);
			return;
		}
		throw new Exception("CONNECT command can NOT be encrypted !");
	}

	public void RemoveEncryptedCommand(Command cmd)
	{
		Command.CommandKinds commandKind = cmd.GetCommandKind();
		if (_commandEncryptionList.Contains(commandKind))
		{
			_commandEncryptionList.Remove(commandKind);
		}
	}

	public string GetEncryptedText(string text)
	{
		if (PlainTextAuthentication)
		{
			return text;
		}
		return _encryption.Encrypt(text);
	}

	private void GetChallengesCallback(uint numberOfChallenges, AddChallenges addChallenges)
	{
		RequestChallenges(numberOfChallenges, delegate
		{
			addChallenges?.Invoke(new string[0]);
		});
	}

	private void OnServerFeatures(ServerFeatures features)
	{
		_serverFeatures = features;
	}

	private void OnConnectionError(object sender, EventArgs e)
	{
		if (e is ErrorHandlersArgs errorHandlersArgs && ((errorHandlersArgs.Request != null && errorHandlersArgs.Request.ConnectionId != null && _connectionId != errorHandlersArgs.Request.ConnectionId) || (errorHandlersArgs.Response != null && errorHandlersArgs.Response.ConnectionId != null && _connectionId != errorHandlersArgs.Response.ConnectionId)))
		{
			Logger.Instance.Log(LogContent.Error, "Connection.OnConnectionError", "Response with different connection ID - discarding the error");
			return;
		}
		if (_stateChanger.State == ConnectionStates.LoggedIn)
		{
			_stateChanger.ChangeState(ConnectionStates.Disconnected);
		}
		RunHeartBeat = false;
	}

	public IAsyncResult SendCommand(CommunicationEntry entry, string connectionId, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendCommandInternal(entry, connectionId, onSuccess, onFail);
	}

	public IAsyncResult SendCommandInternal(CommunicationEntry entry, string connectionId, OnResponse<BaseCommandResponse> onSuccesss = null, OnResponse<BaseCommandResponse> onFail = null, CommandAsyncResultBase processor = null, string address = null)
	{
		if (_iChannel == null)
		{
			return new CommandResult
			{
				IsCompleted = true,
				Result = Command.CommandResultTypes.ResultError,
				ErrorCode = ErrorCodes.InternalError
			};
		}
		if (processor == null)
		{
			processor = new CommandAsyncResult(this);
		}
		QueueEntry queEntry = new QueueEntry();
		queEntry.Entry = entry;
		queEntry.Entry.ConnectionId = connectionId;
		queEntry.Result = processor;
		queEntry.OnSuccess = onSuccesss;
		queEntry.OnFail = onFail;
		queEntry.Address = address;
		AddSecurityChallenge(queEntry.Entry);
		if (!_iChannel.IsSynchronized || CommandsQueueing.SingleThread == CommandsQueueing)
		{
			AddCommandToQueue(queEntry);
		}
		else if (CommandsQueueing == CommandsQueueing.SendDirect)
		{
			CheckSendCommand(queEntry);
		}
		else
		{
			new Task(delegate
			{
				CheckSendCommand(queEntry);
			}).Start();
		}
		return queEntry.Result;
	}

	private void AddSecurityChallenge(CommunicationEntry entry)
	{
		Command command = entry.Command;
		ChallengePair nextChallenge = _challengesManager.GetNextChallenge();
		if (nextChallenge != null)
		{
			command.InputParams.Add(new Parameter("Challenge", nextChallenge.Challenge));
			command.InputParams.Add(new Parameter("ChalAnswer", nextChallenge.Hash));
		}
	}

	private void AddCommandToQueue(QueueEntry queueEntry)
	{
		lock (_commandsQueue)
		{
			_commandsQueue.Enqueue(queueEntry);
		}
	}

	public IAsyncResult SendCommandInternal(CommunicationEntry entry, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null, CommandAsyncResultBase processor = null, string address = null)
	{
		return SendCommandInternal(entry, _connectionId, onSuccess, onFail, processor, address);
	}

	public IAsyncResult SendCommand(CommunicationEntry entry, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		return SendCommand(entry, _connectionId, onSuccess, onFail);
	}

	private void OnEntryResponse(CommunicationEntry response)
	{
		try
		{
			Command command = response.Command;
			if (command.GetCommandType() == Command.CommandTypes.Response)
			{
				command.ConvertToDictionary(Command.ParamsType.Output);
				ExtractSecurityChallenges(command);
				CommunicationEntry communicationEntry = new CommunicationEntry
				{
					ConnectionId = response.ConnectionId,
					Command = command
				};
				QueueEntry queueEntry = null;
				lock (_responsesQueue)
				{
					queueEntry = _responsesQueue[communicationEntry.Command.CommandNumber];
					_responsesQueue.Remove(communicationEntry.Command.CommandNumber);
				}
				if (queueEntry.Entry.Command == null && !string.IsNullOrEmpty(queueEntry.Entry.EncryptedCommand))
				{
					Command command2 = CommandsSeparator.DeserializeCommand(_encryption.Decrypt(queueEntry.Entry.EncryptedCommand));
					queueEntry.Entry.Command = command2;
				}
				ErrorCodes errorCode = command.GetErrorCode();
				if (errorCode != ErrorCodes.Ok)
				{
					_errorHandlers.ProcessError(errorCode, queueEntry.Entry, communicationEntry);
				}
				queueEntry.Result.ProcessResult(communicationEntry, queueEntry.Entry, queueEntry.OnSuccess, queueEntry.OnFail);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "Connection.OnEntryResponse", ex.Message);
		}
	}

	private void ExtractSecurityChallenges(Command command)
	{
		if (command.GetDictionary(Command.ParamsType.Output).ContainsKey("Challenge"))
		{
			List<string> challenges = (from parameter in command.OutputParams
				where parameter.ParamName == "Challenge"
				select parameter.ParamValue).ToList();
			_challengesManager.AddChallenges(challenges);
		}
	}

	private void Run()
	{
		CheckSendCommand();
		CheckSendHeartBeat();
	}

	private void CheckSendCommand()
	{
		try
		{
			QueueEntry queEntry = null;
			lock (_commandsQueue)
			{
				if (_commandsQueue.Count > 0)
				{
					queEntry = _commandsQueue.Dequeue();
				}
			}
			CheckSendCommand(queEntry);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "Connection.CheckSendCommand", ex.Message);
		}
	}

	private void CheckSendCommand(QueueEntry queEntry)
	{
		if (queEntry != null)
		{
			lock (_responsesQueue)
			{
				queEntry.Entry.Command.CommandNumber = _commandNumber++.ToString();
				_responsesQueue[queEntry.Entry.Command.CommandNumber] = queEntry;
			}
			_iChannel.SendCommandEntry(queEntry.Entry, queEntry.Address);
			if (!PeriodicLiveMessage)
			{
				_lastSendHeartBeatTimeUtc = DateTime.UtcNow;
			}
		}
	}

	private void CheckSendHeartBeat()
	{
		try
		{
			if (RunHeartBeat && HeartBeatTimeoutS > 0 && (DateTime.UtcNow - _lastSendHeartBeatTimeUtc).TotalSeconds > (double)HeartBeatTimeoutS)
			{
				LiveMessage();
				_lastSendHeartBeatTimeUtc = DateTime.UtcNow;
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "Connection.CheckSendHeartBreate", ex.Message);
		}
	}

	public void Start()
	{
		_thread.StartTask();
	}

	public void Stop()
	{
		_thread.SignalTaskToStop();
		lock (_commandsQueue)
		{
			_commandsQueue.Clear();
		}
	}

	~Connection()
	{
		Dispose();
	}

	public void Dispose()
	{
		DisposeCommon(disposing: true, quiet: false);
	}

	public void DisposeQuiet()
	{
		DisposeCommon(disposing: true, quiet: true);
	}

	private void DisposeCommon(bool disposing, bool quiet)
	{
		Dispose(disposing, quiet);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing, bool quiet)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			RunHeartBeat = false;
			if (!quiet)
			{
				SendDisconnectCommand(TimeSpan.FromSeconds(1.0));
			}
			DisposeVideos(quiet);
			DisposeInternal();
		}
		_disposed = true;
	}

	private void DisposeInternal()
	{
		Disposing.DisposeMember(ref _thread);
		Disposing.DisposeMember(ref _iChannel);
	}

	IAsyncResult IConnectionInternal.SendCommandInternal(Command tCommand, OnResponse<BaseCommandResponse> onSuccess, OnResponse<BaseCommandResponse> onFail, CommandAsyncResultBase processor, string address)
	{
		return SendCommandInternal(tCommand, onSuccess, onFail, processor, address);
	}

	public IAsyncResult SendCommandInternal(Command tCommand, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null, CommandAsyncResultBase processor = null, string address = null)
	{
		tCommand.SetCommandType(Command.CommandTypes.Command);
		ParamsValidator paramsValidator = new ParamsValidator(tCommand.GetCommandKind());
		ErrorCodes errorCodes = paramsValidator.ValidateParamsLength(tCommand.GetDictionary(Command.ParamsType.Input), tCommand.GetDictionary(Command.ParamsType.Output));
		if (errorCodes == ErrorCodes.Ok)
		{
			errorCodes = paramsValidator.SanitizeCommandParams(tCommand.GetDictionary(Command.ParamsType.Input), tCommand.GetDictionary(Command.ParamsType.Output));
		}
		if (errorCodes != ErrorCodes.Ok)
		{
			return new CommandResult
			{
				IsCompleted = true,
				Result = Command.CommandResultTypes.ResultError,
				ErrorCode = errorCodes
			};
		}
		CommunicationEntry communicationEntry = new CommunicationEntry();
		communicationEntry.Command = tCommand;
		return SendCommandInternal(communicationEntry, onSuccess, onFail, processor, address);
	}

	public IAsyncResult SendCommand(Command command, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null, string address = null)
	{
		return SendCommandInternal(command, onSuccess, onFail, null, address);
	}

	public IAsyncResult SendCommand<T>(Command command, OnResponse<T> onSuccess, OnResponse<T> onFail, Func<BaseCommandResponse, T> factoryT, string address = null) where T : BaseCommandResponse
	{
		return SendCommand(command, delegate(BaseCommandResponse baseResponse)
		{
			onSuccess?.Invoke(factoryT(baseResponse));
		}, delegate(BaseCommandResponse baseResponse)
		{
			onFail?.Invoke(factoryT(baseResponse));
		}, address);
	}

	public BaseCommandResponse ProcessResult(IAsyncResult iResult, TimeSpan timeout)
	{
		iResult?.AsyncWaitHandle.WaitOne(timeout);
		if (iResult != null && iResult.IsCompleted)
		{
			IErrorResult errorResult = iResult as IErrorResult;
			Result = errorResult.Result;
			ErrorMessage = errorResult.ErrorMessage;
			ErrorCode = errorResult.ErrorCode;
		}
		else
		{
			Result = Command.CommandResultTypes.ResultError;
			ErrorMessage = "Command Timed Out !";
			ErrorCode = ErrorCodes.CommandTimedOut;
		}
		return BaseCommandResponse.Get(iResult, this);
	}

	public Task<BaseCommandResponse> ProcessResultTask(IAsyncResult asyncResult, TimeSpan timeout)
	{
		Task<BaseCommandResponse> task = new Task<BaseCommandResponse>(() => ProcessResult(asyncResult, timeout));
		task.Start();
		return task;
	}

	void IOnVideoStream.CreatedVideo(IStreamReconnection iVideo)
	{
		lock (_videosLocker)
		{
			_videos.Add(iVideo);
		}
	}

	void IOnVideoStream.DestroyedVideo(IStreamReconnection iVideo)
	{
		lock (_videosLocker)
		{
			_videos.Remove(iVideo);
		}
	}

	public void IterateVideos(Action<IStreamReconnection> handler)
	{
		List<IStreamReconnection> list;
		lock (_videosLocker)
		{
			list = new List<IStreamReconnection>(_videos);
		}
		foreach (IStreamReconnection item in list)
		{
			try
			{
				handler(item);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "Connection.IterateVideos", ex.Message);
			}
		}
	}

	public void DisposeVideos(bool quiet = false)
	{
		if (!quiet)
		{
			IterateVideos(delegate(IStreamReconnection video)
			{
				Disposing.DisposeMember(ref video);
			});
		}
		else
		{
			IterateVideos(delegate(IStreamReconnection video)
			{
				Disposing.DisposeMemberQuiet(ref video);
			});
		}
		_videos.Clear();
	}

	public void RecreateVideos(bool async)
	{
		if (_videos.Count == 0)
		{
			return;
		}
		if (async)
		{
			new Task(delegate
			{
				RecreateVideos(async: false);
			}).Start();
		}
		else
		{
			IterateVideos(delegate(IStreamReconnection video)
			{
				video.Reconnect();
			});
		}
	}

	private MessageTypes GetMessageTypeInternal()
	{
		if (MessageType == MessageTypes.Json || (MessageType == MessageTypes.Undefined && ServerCapabilities.JsonCommandsSupported))
		{
			return MessageTypes.Json;
		}
		return MessageTypes.Xml;
	}

	private IAsyncResult SendConnectCommand(ConnectParams connectParams, OnResponse<ConnectResponse> onSuccess = null, OnResponse<ConnectResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.Connect);
		connectParams?.ConvertToCommand(ref tCommand);
		return SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new ConnectResponse(response));
	}

	public IAsyncResult Connect(ConnectParams connectParams = null, OnResponse<ConnectResponse> onSuccess = null, OnResponse<ConnectResponse> onFail = null)
	{
		_connectionId = "";
		_stateChanger.ChangeState(ConnectionStates.Connecting);
		if (connectParams != null)
		{
			PeriodicLiveMessage = connectParams.PeriodicLiveMessage;
		}
		connectParams = connectParams ?? new ConnectParams();
		if (!connectParams.CheckForMask(ConnectParamsHelper.ParamMask.PrimeLength))
		{
			connectParams.PrimeLength = PrimeLength.Prime2048;
		}
		if (!connectParams.CheckForMask(ConnectParamsHelper.ParamMask.EncryptionPadding))
		{
			connectParams.EncodingPadding = EncodingPadding.Iso10126;
		}
		_encryption = new DHEncryption(connectParams.EncodingPadding, connectParams.PrimeLength);
		string publicKey = (PlainTextAuthentication ? null : _encryption.GetPublicKey());
		OnResponse<BaseCommandResponse> onSuccess2 = delegate(BaseCommandResponse response)
		{
			ConnectResponse connectResponse = new ConnectResponse(response);
			_serverCapabilities.InitFromConnectCommand(connectResponse.OutputParams);
			_connectionId = _serverCapabilities.ConnectionId.ToString();
			SetConnectionTimeoutS(_serverCapabilities.ConnectionTimeoutS);
			if (!_serverCapabilities.CommandEncryptionSupported)
			{
				RequireCommandEncryption = false;
			}
			if (!_plainAuthentication)
			{
				if (_serverCapabilities.PlainAuthentication)
				{
					ErrorMessage = "Server didn't respond with public key";
				}
				_encryption.GenerateSecretKey(_serverCapabilities.ServerPublicKey);
			}
			_stateChanger.ChangeState(ConnectionStates.Connected);
			if (connectResponse.Servers != null)
			{
				_serverEndpoints = connectResponse.Servers;
			}
			onSuccess?.Invoke(connectResponse);
		};
		OnResponse<BaseCommandResponse> onFail2 = delegate(BaseCommandResponse response)
		{
			_stateChanger.ChangeState(ConnectionStates.Disconnected);
			onFail?.Invoke(new ConnectResponse(response));
		};
		connectParams.PublicKey = publicKey;
		connectParams.RequireCommandEncryption = RequireCommandEncryption;
		return SendConnectCommand(connectParams, onSuccess2, onFail2);
	}

	private void SetConnectionTimeoutS(int connectionTimeoutS)
	{
		ConnectionTimeoutS = connectionTimeoutS;
		HeartBeatTimeoutS = ConnectionTimeoutS >> 1;
		HeartBeatTimeoutS = System.Math.Max(HeartBeatTimeoutS, 1);
		if (PeriodicLiveMessage)
		{
			HeartBeatTimeoutS = System.Math.Min(HeartBeatTimeoutS, 5);
		}
	}

	private IAsyncResult SendDisconnectCommand()
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.Disconnect);
		return SendCommand(command);
	}

	private IAsyncResult Login(Command tCommand, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		_stateChanger.ChangeState(ConnectionStates.LoggingIn);
		if (!_plainAuthentication && ChapSecurityEnabled && _challengesEnabled && ServerCapabilities.ChapSupported)
		{
			_challengesManager.Initialize(_encryption.GetSecretKeyFull());
			_challengesManager.Activate();
		}
		if (_challengesManager.IsActive)
		{
			tCommand.InputParams.Add(new Parameter("NumChallenges", 100u.ToString()));
		}
		OnResponse<BaseCommandResponse> onSuccess2 = delegate(BaseCommandResponse response)
		{
			_serverFeatures.InitFromCommand(response.OutputParams);
			_userIdentity.InitFromCommand(response.OutputParams);
			_stateChanger.ChangeState(ConnectionStates.LoggedIn);
			RecreateVideos(async: true);
			onSuccess?.Invoke(response);
		};
		OnResponse<BaseCommandResponse> onFail2 = delegate(BaseCommandResponse baseCommandResponse)
		{
			if (baseCommandResponse != null && baseCommandResponse.ErrorCode == ErrorCodes.SecondStepVerificationRequired && baseCommandResponse.OutputParams.ContainsKey("SecondStepAuthenticationProvider"))
			{
				ErrorMessage = baseCommandResponse.OutputParams["SecondStepAuthenticationProvider"];
				_stateChanger.ChangeState(ConnectionStates.SecondStepVerificationRequired);
			}
			onFail?.Invoke(baseCommandResponse);
		};
		return SendCommand(tCommand, onSuccess2, onFail2);
	}

	[Obsolete]
	public IAsyncResult LogIn(string username, string password, string clientType, UserType loginType = UserType.Unknown, LoginParams loginParams = null, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		LoginSettingsParams loginSettings = new LoginSettingsParams
		{
			UserName = username,
			Password = password,
			LoginType = loginType,
			ClientType = clientType
		};
		return LogIn(loginSettings, loginParams, onSuccess, onFail);
	}

	public IAsyncResult LogIn(LoginSettingsParams loginSettings, LoginParams loginParams = null, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.LogIn);
		bool isEncrypted = !_plainAuthentication && ((loginSettings.CheckParams(1u) == ErrorCodes.Ok && loginSettings.LoginType != UserType.External) || loginSettings.CheckParams(1u) != ErrorCodes.Ok);
		loginSettings.ConvertToCommand(ref tCommand, isEncrypted, _encryption);
		loginParams?.ConvertToCommand(ref tCommand);
		return Login(tCommand, onSuccess, onFail);
	}

	private void OnSuccessVerifyTsvCode(BaseCommandResponse response)
	{
		_serverFeatures.InitFromCommand(response.OutputParams);
		_stateChanger.ChangeState(ConnectionStates.LoggedIn);
		RecreateVideos(async: true);
	}

	public IAsyncResult LiveMessage(OnResponse<LiveMessageResponse> onSuccess = null, OnResponse<LiveMessageResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.LiveMessage);
		OnResponse<BaseCommandResponse> onSuccess2 = delegate(BaseCommandResponse baseResponse)
		{
			LiveMessageResponse liveMessageResponse = new LiveMessageResponse(baseResponse);
			if (liveMessageResponse.ServerConfigurationId != ServerFeatures.ConfigurationId && AutoUpdateConfiguration)
			{
				CapabilitiesParams tCapabilitiesParams = new CapabilitiesParams
				{
					ItemKind = "Server"
				};
				Utilities.GetCapabilities(tCapabilitiesParams);
			}
			if (liveMessageResponse.Notifications != null && liveMessageResponse.Notifications.Any() && NotificationEventHandler != null)
			{
				try
				{
					NotificationEventHandler(this, new NotificationsEventArgs(liveMessageResponse.Notifications));
				}
				catch (Exception ex)
				{
					Logger.Instance.Log(LogContent.Error, "ConnectionAsync.LiveMessage", ex.Message);
				}
			}
			onSuccess?.Invoke(liveMessageResponse);
		};
		OnResponse<BaseCommandResponse> onFail2 = delegate(BaseCommandResponse baseResponse)
		{
			onFail?.Invoke(new LiveMessageResponse(baseResponse));
		};
		return SendCommand(command, onSuccess2, onFail2);
	}

	private IAsyncResult RequestChallenges(uint count, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.RequestChallenges);
		command.InputParams.Add(new Parameter("NumChallenges", count.ToString()));
		return SendCommand(command, onSuccess, onFail);
	}

	public void ProcessIAsyncResult(IAsyncResult result, AsyncCallback callback, TimeSpan timeout)
	{
		ProcessAsyncTask processAsyncTask = new ProcessAsyncTask
		{
			Result = result,
			Callback = callback,
			Timeout = timeout,
			Processor = this
		};
		Task.Factory.StartNew(processAsyncTask.Run);
	}

	public async Task<ConnectResponse> ConnectAsync(ConnectParams connectParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = Connect(connectParams);
		return new ConnectResponse(await ProcessResultTask(asyncResult, timeout));
	}

	public async Task<BaseCommandResponse> LogInAsync(string username, string password, string clientType, LoginParams loginParams, TimeSpan timeout, UserType loginType = UserType.Unknown)
	{
		IAsyncResult asyncResult = LogIn(username, password, clientType, loginType, loginParams);
		return await ProcessResultTask(asyncResult, timeout);
	}

	public async Task<BaseCommandResponse> LogInAsync(LoginSettingsParams loginSettings, LoginParams loginParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = LogIn(loginSettings, loginParams);
		return await ProcessResultTask(asyncResult, timeout);
	}

	public async Task<LiveMessageResponse> LiveMessageAsync(TimeSpan timeout)
	{
		IAsyncResult asyncResult = LiveMessage();
		return new LiveMessageResponse(await ProcessResultTask(asyncResult, timeout));
	}

	private BaseCommandResponse SendConnectCommand(ConnectParams connectParams, TimeSpan timeout)
	{
		IAsyncResult iResult = SendConnectCommand(connectParams);
		return ProcessResult(iResult, timeout);
	}

	public ConnectResponse Connect(ConnectParams connectParams, TimeSpan timeout)
	{
		IAsyncResult iResult = Connect(connectParams);
		return new ConnectResponse(ProcessResult(iResult, timeout));
	}

	private BaseCommandResponse SendDisconnectCommand(TimeSpan timeout)
	{
		IAsyncResult iResult = SendDisconnectCommand();
		return ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse LogIn(string username, string password, string clientType, TimeSpan timeout, UserType loginType = UserType.Unknown, LoginParams loginParams = null)
	{
		IAsyncResult iResult = LogIn(username, password, clientType, loginType, loginParams);
		return ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse LogIn(LoginSettingsParams loginSettings, LoginParams loginParams, TimeSpan timeout)
	{
		IAsyncResult iResult = LogIn(loginSettings, loginParams);
		return ProcessResult(iResult, timeout);
	}

	public LiveMessageResponse LiveMessage(TimeSpan timeout)
	{
		IAsyncResult iResult = LiveMessage();
		return new LiveMessageResponse(ProcessResult(iResult, timeout));
	}
}
