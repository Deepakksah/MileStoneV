using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Text;
using System.Threading;

namespace VideoOS.Platform.Data.Speaker;

public class ImageServerClient
{
	public delegate void ClientStateEventHandler(object sender, ClientStateEventArgs e);

	public class ClientStateEventArgs : EventArgs
	{
		protected ClientState _clientState;

		protected string _errorMessage;

		protected ArrayList _failedDevices;

		protected int _errorCode;

		public virtual ClientState ClientState
		{
			get
			{
				return _clientState;
			}
			set
			{
				_clientState = value;
			}
		}

		public virtual string ErrorMessage
		{
			get
			{
				return _errorMessage;
			}
			set
			{
				_errorMessage = value;
			}
		}

		public virtual int ErrorCode
		{
			get
			{
				return _errorCode;
			}
			set
			{
				_errorCode = value;
			}
		}

		public virtual ArrayList FailedDevices
		{
			get
			{
				return _failedDevices;
			}
			set
			{
				_failedDevices = value;
			}
		}

		public ClientStateEventArgs(ClientState clientState, string errorMessage, ArrayList failedDevices, int errorCode)
		{
			_clientState = clientState;
			_errorMessage = errorMessage;
			_failedDevices = failedDevices;
			_errorCode = errorCode;
		}
	}

	private class Connection
	{
		private Socket _socket;

		private Stream _stream;

		private bool _disposed;

		public Connection(Socket socket, Stream stream)
		{
			if (socket == null)
			{
				throw new ArgumentNullException("socket", "The socket must not be null");
			}
			if (stream == null)
			{
				throw new ArgumentNullException("stream", "The stream must not be null");
			}
			_socket = socket;
			_stream = stream;
			_disposed = false;
		}

		public bool Write(byte[] buffer, int offset, int count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("Connection");
			}
			_stream.Write(buffer, offset, count);
			return true;
		}

		public int Read(byte[] buffer, int offset, int count)
		{
			if (_disposed)
			{
				throw new ObjectDisposedException("Connection");
			}
			if (!_socket.Poll(1, SelectMode.SelectRead))
			{
				return 0;
			}
			if (_socket.Available == 0)
			{
				return -1;
			}
			return _stream.Read(buffer, offset, count);
		}

		public void Dispose()
		{
			_socket?.Dispose();
			_socket = null;
			_stream?.Dispose();
			_stream = null;
			_disposed = true;
		}
	}

	private Encoding _encoding = Encoding.UTF8;

	private volatile bool _stopped;

	private Thread _processingThread;

	private string _recorderName = string.Empty;

	private Uri _serverUri;

	private List<Guid> _deviceIds;

	private List<Guid> _failedDeviceIds;

	private string _token;

	private Guid _serverId = Guid.Empty;

	private ClientState _clientState;

	private byte[] _inputBuffer = new byte[5000];

	private int _inputPos;

	private Queue _outputMessageQueue = new Queue();

	private object _lockObject = new object();

	private int _requestId = 1;

	private int _seqNumber = 1;

	private int _outgoingWaveFormatChannels;

	private int _outgoingWaveFormatBitsPerSample;

	private int _outgoingAudioWaveFormatSamplesPerSecond;

	public string RecorderName
	{
		get
		{
			lock (_lockObject)
			{
				return _recorderName;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_recorderName = value;
			}
		}
	}

	public Uri ServerUri
	{
		get
		{
			lock (_lockObject)
			{
				return _serverUri;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_serverUri = value;
			}
		}
	}

	public List<Guid> DeviceIds
	{
		get
		{
			lock (_lockObject)
			{
				return _deviceIds;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_deviceIds = value;
			}
		}
	}

	public List<Guid> FailedDeviceIds
	{
		get
		{
			lock (_lockObject)
			{
				return _failedDeviceIds;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_failedDeviceIds = value;
			}
		}
	}

	public string Token
	{
		get
		{
			lock (_lockObject)
			{
				return _token;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_token = value;
			}
		}
	}

	public ClientState ClientState
	{
		get
		{
			lock (_lockObject)
			{
				return _clientState;
			}
		}
	}

	public Guid ServerId
	{
		get
		{
			lock (_lockObject)
			{
				return _serverId;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_serverId = value;
			}
		}
	}

	public event ClientStateEventHandler ClientStateEvent;

	public ImageServerClient()
	{
		_failedDeviceIds = new List<Guid>();
	}

	public ImageServerClient(int outgoingWaveFormatChannels, int outgoingWaveFormatBitsPerSample, int outgoingAudioWaveFormatSamplesPerSecond)
	{
		_failedDeviceIds = new List<Guid>();
		_outgoingWaveFormatChannels = outgoingWaveFormatChannels;
		_outgoingWaveFormatBitsPerSample = outgoingWaveFormatBitsPerSample;
		_outgoingAudioWaveFormatSamplesPerSecond = outgoingAudioWaveFormatSamplesPerSecond;
	}

	protected virtual void FireClientStateEvent(ClientState clientState, string ErrorMessage, ArrayList failedDevices, int errorCode = 0)
	{
		if (this.ClientStateEvent != null && !_stopped)
		{
			this.ClientStateEvent(this, new ClientStateEventArgs(clientState, ErrorMessage, failedDevices, errorCode));
		}
	}

	public bool Connect()
	{
		if (!StartThread())
		{
			return false;
		}
		if (!ConnectDevices())
		{
			return false;
		}
		return true;
	}

	public bool Disconnect()
	{
		UninitializeDevices();
		if (!StopThread())
		{
			return false;
		}
		return true;
	}

	public bool IsConnected()
	{
		if (_clientState != ClientState.Initialized && _clientState != ClientState.BeginningSpeak && _clientState != ClientState.Speaking)
		{
			return _clientState == ClientState.EndingSpeak;
		}
		return true;
	}

	public bool BeginSpeak()
	{
		if (_clientState != ClientState.Initialized)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "BeginSpeak - Not in a state for BeginSpeak.");
			return false;
		}
		_seqNumber = 1;
		ArrayList arrayList = new ArrayList();
		foreach (Guid deviceId in _deviceIds)
		{
			if (!_failedDeviceIds.Contains(deviceId))
			{
				arrayList.Add(deviceId);
			}
		}
		byte[] message = OutgoingAudioServerMessages.BuildBeginSpeakRequest(++_requestId, "speak", arrayList, exclusive: true);
		_clientState = ClientState.BeginningSpeak;
		QueueOutputMessage(message);
		return true;
	}

	public bool EndSpeak()
	{
		if (!IsSpeaking())
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "EndSpeak - Not in a state for EndSpeak.");
			return false;
		}
		byte[] message = OutgoingAudioServerMessages.BuildEndSpeakRequest(++_requestId);
		_clientState = ClientState.EndingSpeak;
		QueueOutputMessage(message);
		return true;
	}

	public bool IsSpeaking()
	{
		if (_clientState != ClientState.BeginningSpeak)
		{
			return _clientState == ClientState.Speaking;
		}
		return true;
	}

	public bool Speak(byte[] captureData, bool includeGenericByteDataHeader)
	{
		if (_clientState == ClientState.Speaking)
		{
			byte[] message = (includeGenericByteDataHeader ? OutgoingAudioServerMessages.BuildSpeakRequest(++_requestId, _seqNumber++, captureData, _outgoingWaveFormatChannels, _outgoingWaveFormatBitsPerSample, _outgoingAudioWaveFormatSamplesPerSecond) : OutgoingAudioServerMessages.BuildDirectSpeakRequest(++_requestId, captureData));
			QueueOutputMessage(message);
			return true;
		}
		return false;
	}

	private bool StartThread()
	{
		if (DeviceIds == null)
		{
			return false;
		}
		if (Token.Length == 0)
		{
			return false;
		}
		if (ServerUri == null)
		{
			return false;
		}
		if (_processingThread != null)
		{
			throw new Exception("Processing thread is already started.");
		}
		_processingThread = new Thread(ThreadFunction);
		_processingThread.IsBackground = true;
		_processingThread.Name = "OutgoingAudioSocket-WorkThread";
		_processingThread.Start();
		return true;
	}

	private bool StopThread()
	{
		_stopped = true;
		_processingThread?.Join(500);
		_processingThread = null;
		return true;
	}

	private void ThreadFunction()
	{
		Connection connection = null;
		Stream stream = null;
		try
		{
			Socket socket = ConnectSocket();
			if (_stopped)
			{
				socket?.Dispose();
				return;
			}
			if (socket == null)
			{
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Not able to connect to socket.", null, 1);
				return;
			}
			stream = new NetworkStream(socket, ownsSocket: true);
			if (string.Equals(ServerUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
			{
				SslStream sslStream = new SslStream(stream, leaveInnerStreamOpen: false);
				stream = sslStream;
				try
				{
					sslStream.AuthenticateAsClient(ServerUri.Host);
				}
				catch (AuthenticationException ex)
				{
					EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "OutgoingAudio thread, SSL exception: " + ex.Message);
					_clientState = ClientState.ErrorDetected;
					FireClientStateEvent(_clientState, "OutgoingAudio, authentication to recording server failed", null, 3);
					return;
				}
			}
			connection = new Connection(socket, stream);
			stream = null;
			while (!_stopped || !EmptyQueue())
			{
				ProcessOutput(connection);
				ProcessInput(connection);
				Thread.Sleep(5);
			}
		}
		catch (Exception ex2)
		{
			_clientState = ClientState.ErrorDetected;
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "OutgoingAudio, exception during data push:" + ex2.Message);
			FireClientStateEvent(_clientState, "OutgoingAudio, exception during data push:" + ex2.Message, null, 4);
		}
		finally
		{
			stream?.Dispose();
			connection?.Dispose();
		}
	}

	private bool ProcessOutput(Connection connection)
	{
		bool result = false;
		byte[] array = null;
		lock (_outputMessageQueue)
		{
			if (_outputMessageQueue.Count > 0)
			{
				array = _outputMessageQueue.Dequeue() as byte[];
			}
		}
		if (array != null)
		{
			result = Send(connection, array);
		}
		return result;
	}

	private void ProcessInput(Connection connection)
	{
		if (Receive(connection) == -1 && _clientState != ClientState.ErrorDetected)
		{
			_clientState = ClientState.ErrorDetected;
			FireClientStateEvent(_clientState, "Network connection has been closed, reset, or terminated", null, 1);
		}
		if (_inputPos > 0)
		{
			HandleResponse(connection);
		}
	}

	private bool EmptyQueue()
	{
		bool result = false;
		lock (_outputMessageQueue)
		{
			if (_outputMessageQueue.Count == 0)
			{
				result = true;
			}
		}
		return result;
	}

	private int ReadResponse(Connection connection, out int requestId, out string methodCall, out ArrayList guids)
	{
		requestId = -1;
		methodCall = string.Empty;
		guids = null;
		int num = -1;
		string text = ReadResponseLine(connection);
		string[] array = text.Split(new char[1] { ' ' }, 3);
		if (array.Length != 3)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Response - ReturnCode: " + num.ToString(CultureInfo.InvariantCulture));
			return num;
		}
		string value = array[1];
		try
		{
			num = Convert.ToInt32(value, CultureInfo.InvariantCulture);
		}
		catch
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Response - ReturnCode: " + num.ToString(CultureInfo.InvariantCulture));
			return num;
		}
		if (num != 200 && num != 401 && num != 404 && num != 409 && num != 500)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Response - ReturnCode: " + num.ToString(CultureInfo.InvariantCulture));
			do
			{
				text = ReadResponseLine(connection);
			}
			while (text != null && text.Length > 0);
			return num;
		}
		StringDictionary stringDictionary = new StringDictionary();
		bool flag = false;
		int num2 = 1000;
		while (!flag && num2 > 0)
		{
			text = ReadResponseLine(connection);
			if (text.Length == 0)
			{
				flag = true;
			}
			else
			{
				int num3 = text.IndexOf(":");
				if (num3 != -1)
				{
					string key = text.Substring(0, num3).ToLowerInvariant();
					string value2 = text.Substring(num3 + 1).Trim();
					stringDictionary[key] = value2;
				}
			}
			num2--;
		}
		int num4 = Convert.ToInt32(stringDictionary["Content-Length"], CultureInfo.InvariantCulture);
		if (num4 > 0)
		{
			byte[] bytes = ReadResponseContent(connection, num4);
			OutgoingAudioServerMessages.ReadResponseXml(new string(_encoding.GetChars(bytes, 0, num4)), out guids);
		}
		requestId = Convert.ToInt32(stringDictionary["X-RequestId"], CultureInfo.InvariantCulture);
		methodCall = stringDictionary["X-MethodName"];
		return num;
	}

	private string ReadResponseLine(Connection connection)
	{
		int i = 0;
		int num = 1000;
		bool flag = false;
		while (!flag && num > 0)
		{
			for (i = 0; i + 1 < _inputPos; i++)
			{
				if (_inputBuffer[i] == 13 && _inputBuffer[i + 1] == 10)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				Receive(connection);
				num--;
			}
		}
		if (_inputPos - i < 2)
		{
			return string.Empty;
		}
		string result = new string(_encoding.GetChars(_inputBuffer, 0, i));
		Buffer.BlockCopy(_inputBuffer, i + 2, _inputBuffer, 0, _inputPos - i - 2);
		_inputPos -= i + 2;
		return result;
	}

	private byte[] ReadResponseContent(Connection connection, int contentLength)
	{
		byte[] array = new byte[contentLength];
		int num = 10000;
		while (_inputPos < contentLength && num > 0)
		{
			Receive(connection);
			num--;
		}
		Buffer.BlockCopy(_inputBuffer, 0, array, 0, contentLength);
		Buffer.BlockCopy(_inputBuffer, contentLength, _inputBuffer, 0, _inputPos - contentLength);
		_inputPos -= contentLength;
		return array;
	}

	private void HandleResponse(Connection connection)
	{
		int requestId = -1;
		string methodCall = "";
		ArrayList guids = null;
		bool flag = false;
		int num = ReadResponse(connection, out requestId, out methodCall, out guids);
		if (num != 200)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Error code received:" + num.ToString(CultureInfo.InvariantCulture));
			if (num != 409)
			{
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Error code:" + num.ToString(CultureInfo.InvariantCulture), guids, num);
				return;
			}
			foreach (Guid item in guids)
			{
				if (!_failedDeviceIds.Contains(item))
				{
					_failedDeviceIds.Add(item);
					flag = true;
					EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "failed device: " + item.ToString());
				}
			}
			if (_failedDeviceIds.Count == _deviceIds.Count)
			{
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Error code:" + num.ToString(CultureInfo.InvariantCulture), guids, num);
				return;
			}
		}
		switch (methodCall)
		{
		case "connect":
			if (_clientState == ClientState.Connecting)
			{
				_clientState = ClientState.Connected;
				FireClientStateEvent(_clientState, "", null);
				InitializeDevices();
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Unexpected 'connect' response received.");
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Unexpected 'connect' response received.", null, 2);
			}
			break;
		case "initialize":
			if (_clientState == ClientState.Initializing)
			{
				_clientState = ClientState.Initialized;
				FireClientStateEvent(_clientState, "", null);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Unexpected 'initialize' response received.");
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Unexpected 'initialize' response received.", null, 2);
			}
			break;
		case "beginspeak":
			if (_clientState == ClientState.BeginningSpeak)
			{
				_clientState = ClientState.Speaking;
				FireClientStateEvent(_clientState, "", null);
			}
			else if (_clientState == ClientState.EndingSpeak)
			{
				FireClientStateEvent(_clientState, "", null);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Unexpected 'beginspeak' response received.");
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Unexpected 'beginspeak' response received.", null, 2);
			}
			break;
		case "speak":
			if (flag)
			{
				FireClientStateEvent(_clientState, "", null);
			}
			break;
		case "endspeak":
			if (_clientState == ClientState.EndingSpeak)
			{
				_clientState = ClientState.Initialized;
				FireClientStateEvent(_clientState, "", null);
			}
			else if (_clientState == ClientState.BeginningSpeak)
			{
				FireClientStateEvent(_clientState, "", null);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Unexpected 'endspeak' response received." + _clientState);
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Unexpected 'endspeak' response received.", null, 2);
			}
			break;
		case "uninitialize":
			if (_clientState == ClientState.UnInitializing)
			{
				_clientState = ClientState.Disconnected;
				FireClientStateEvent(_clientState, "", null);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Unexpected 'uninitialize' response received." + _clientState);
				_clientState = ClientState.ErrorDetected;
				FireClientStateEvent(_clientState, "Unexpected 'uninitialize' response received.", null, 2);
			}
			break;
		default:
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Unknown response received:" + methodCall);
			break;
		}
	}

	private Socket ConnectSocket()
	{
		IPAddress iPAddress = null;
		Socket socket = null;
		try
		{
			IPAddress[] hostAddresses = Dns.GetHostAddresses(ServerUri.Host);
			bool flag = false;
			IPAddress[] array = hostAddresses;
			foreach (IPAddress iPAddress2 in array)
			{
				if (iPAddress2.AddressFamily == AddressFamily.InterNetwork)
				{
					iPAddress = iPAddress2;
					flag = true;
					break;
				}
			}
			if (!flag && hostAddresses.Length != 0)
			{
				iPAddress = hostAddresses[0];
			}
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "GetHostEntry.Exception: " + ex.Message);
		}
		if (iPAddress == null)
		{
			return null;
		}
		IPEndPoint iPEndPoint = new IPEndPoint(iPAddress, ServerUri.Port);
		socket = new Socket(iPEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		socket.NoDelay = true;
		try
		{
			socket.Connect(iPEndPoint);
		}
		catch (Exception ex2)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "ConnectSocket.Exception: " + ex2.Message);
		}
		if (!socket.Connected)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "Connect.Could not connect to server.");
			socket.Dispose();
			socket = null;
		}
		return socket;
	}

	private bool ConnectDevices()
	{
		if (_clientState != ClientState.Disconnected)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "ConnectDevices - Not in a state for Connect.");
			return false;
		}
		byte[] message = OutgoingAudioServerMessages.BuildConnectRequest(++_requestId, Token);
		_clientState = ClientState.Connecting;
		QueueOutputMessage(message);
		return true;
	}

	private bool InitializeDevices()
	{
		if (_clientState != ClientState.Connected)
		{
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "initializeDevices - Not in a state for Initialize.");
			return false;
		}
		byte[] message = OutgoingAudioServerMessages.BuildInitializeRequest(++_requestId, "speak", DeviceIds);
		_clientState = ClientState.Initializing;
		QueueOutputMessage(message);
		return true;
	}

	private bool UninitializeDevices()
	{
		if (_clientState == ClientState.Disconnected)
		{
			return true;
		}
		byte[] message = OutgoingAudioServerMessages.BuildUninitializeRequest(++_requestId);
		_clientState = ClientState.UnInitializing;
		QueueOutputMessage(message);
		return true;
	}

	private void QueueOutputMessage(byte[] message)
	{
		if (_stopped)
		{
			return;
		}
		lock (_outputMessageQueue)
		{
			_outputMessageQueue.Enqueue(message);
		}
	}

	private bool Send(Connection connection, byte[] message)
	{
		bool result = false;
		try
		{
			connection.Write(message, 0, message.Length);
			result = true;
		}
		catch (Exception ex)
		{
			if (!(ex is IOException) && !(ex is ObjectDisposedException))
			{
				throw;
			}
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "OutgoingAudio, exception writing to stream:" + ex.Message);
			_clientState = ClientState.ErrorDetected;
			FireClientStateEvent(_clientState, "OutgoingAudio, exception writing to stream", null, 1);
		}
		return result;
	}

	private int Receive(Connection connection)
	{
		int num = 0;
		try
		{
			num = connection.Read(_inputBuffer, _inputPos, _inputBuffer.Length - _inputPos);
			if (num == -1)
			{
				EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "OutgoingAudio - Receive. Socket not available");
				return num;
			}
			_inputPos += num;
		}
		catch (Exception ex)
		{
			if (!(ex is IOException) && !(ex is ObjectDisposedException))
			{
				throw;
			}
			EnvironmentManager.Instance.Log(error: false, "ImageServerClient", "OutgoingAudio, exception reading from to stream:" + ex.Message);
			num = -1;
		}
		return num;
	}
}
