using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Security;
using System.Text;
using System.Threading;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.Data.MediaProviderInternal;

public class ServerService
{
	private readonly int _port;

	private readonly SecureString _securePassword;

	private TcpListener _listener;

	private Func<RequestHeader, MediaProviderChannel> _mediaRequestHandler;

	private Action<RequestHeader> _sessionDisconnected;

	private Func<MediaProviderChannel, RequestHeader, MediaData> _getMediaDataDelegate;

	private bool _closed;

	private Func<RequestHeader, MetadataProviderChannel> _metadataRequestHandler;

	private const string CRLF = "\r\n";

	public IDictionary<string, string> Parameters { get; set; }

	public ServerService(int port, SecureString password, IDictionary<string, string> parameters)
	{
		_port = port;
		Parameters = parameters;
		_securePassword = password;
	}

	public void Init(Func<RequestHeader, MediaProviderChannel> requestHandler, Action<RequestHeader> sessionDisconnected, Func<MediaProviderChannel, RequestHeader, MediaData> getMediaDataDelegate, Func<RequestHeader, MetadataProviderChannel> metadataRequestHandler)
	{
		_mediaRequestHandler = requestHandler;
		_sessionDisconnected = sessionDisconnected;
		_getMediaDataDelegate = getMediaDataDelegate;
		_metadataRequestHandler = metadataRequestHandler;
		StartListen();
	}

	public void Close()
	{
		_closed = true;
		if (_listener != null)
		{
			try
			{
				_listener.Stop();
			}
			catch (SocketException)
			{
			}
		}
		_listener = null;
	}

	private void StartListen()
	{
		_listener = new TcpListener(IPAddress.Any, _port);
		try
		{
			_listener.Start();
			_listener.BeginAcceptSocket(AcceptCallback, null);
		}
		catch (SocketException)
		{
		}
	}

	private void AcceptCallback(IAsyncResult ar)
	{
		Socket socket = null;
		RequestHeader requestHeader = null;
		try
		{
			if (_closed)
			{
				return;
			}
			socket = _listener.EndAcceptSocket(ar);
			_listener.BeginAcceptSocket(AcceptCallback, null);
			byte[] array = new byte[1024];
			int count = socket.Receive(array, 1024, SocketFlags.None);
			string request = Encoding.UTF8.GetString(array, 0, count);
			requestHeader = Protocol.ProcessRequest(socket, request, _securePassword, Parameters);
			if (!requestHeader.Error && requestHeader.Command == "stream")
			{
				if (requestHeader.MediaProviderSession.MediaContent == MediaContent.MetaData)
				{
					HandleMetadataStream(requestHeader, socket);
				}
				else
				{
					HandleMediaStream(requestHeader, socket);
				}
			}
			else if (!requestHeader.Error && requestHeader.Command == "param" && requestHeader.Action == "Set")
			{
				FillParameters(requestHeader);
			}
			else
			{
				if (requestHeader.Error || !(requestHeader.Command == "ptz"))
				{
					return;
				}
				MediaProviderChannel mediaProviderChannel = _mediaRequestHandler(requestHeader);
				HttpStatusCode code = HttpStatusCode.OK;
				string moreInfo = "";
				if (mediaProviderChannel != null)
				{
					try
					{
						if (mediaProviderChannel.FireHandlePTZCommandEvent(requestHeader.CommandMessageId, requestHeader.CommandData) is PTZGetAbsoluteRequestData pTZGetAbsoluteRequestData)
						{
							moreInfo = "Pan=" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.0}", pTZGetAbsoluteRequestData.Pan) + "\r\nTilt=" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.0}", pTZGetAbsoluteRequestData.Tilt) + "\r\nZoom=" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.0}", pTZGetAbsoluteRequestData.Zoom);
						}
					}
					catch (Exception ex)
					{
						code = HttpStatusCode.NotAcceptable;
						moreInfo = ex.Message;
					}
				}
				else
				{
					code = HttpStatusCode.NotAcceptable;
				}
				Protocol.SendStreamResponse(socket, code, moreInfo);
			}
		}
		catch (Exception ex2)
		{
			if (socket != null)
			{
				try
				{
					Protocol.SendStreamResponse(socket, HttpStatusCode.InternalServerError, ex2.Message);
				}
				catch (Exception)
				{
				}
			}
			if (requestHeader != null)
			{
				try
				{
					_sessionDisconnected(requestHeader);
					return;
				}
				catch
				{
					return;
				}
			}
		}
		finally
		{
			if (socket != null)
			{
				try
				{
					socket.Shutdown(SocketShutdown.Send);
				}
				catch
				{
				}
				try
				{
					socket.Close();
				}
				catch
				{
				}
			}
		}
	}

	private void FillParameters(RequestHeader requestHeader)
	{
		MediaProviderChannel mediaProviderChannel = _mediaRequestHandler(requestHeader);
		if (mediaProviderChannel != null)
		{
			foreach (string key in requestHeader.SetParameters.Keys)
			{
				mediaProviderChannel.SetParameterDictionary[key] = requestHeader.SetParameters[key];
			}
			return;
		}
		_metadataRequestHandler(requestHeader);
	}

	private void HandleMetadataStream(RequestHeader requestHeader, Socket acceptSocket)
	{
		MetadataProviderChannel metadataProviderChannel = _metadataRequestHandler(requestHeader);
		if (metadataProviderChannel != null)
		{
			bool flag = false;
			Protocol.SendStreamResponseStartMultiPart(acceptSocket);
			while (!metadataProviderChannel.ToBeDisconnnected && Connected(acceptSocket) && !flag)
			{
				if (acceptSocket.Available > 0)
				{
					byte[] buffer = new byte[acceptSocket.Available];
					acceptSocket.Receive(buffer);
				}
				if (metadataProviderChannel.GetMetadataQueue(requestHeader).TryTake(out var item, 20) && !Protocol.SendMediaData(acceptSocket, item))
				{
					flag = true;
				}
			}
			_sessionDisconnected(requestHeader);
		}
		else
		{
			Thread.Sleep(2000);
			Protocol.SendStreamResponse(acceptSocket, HttpStatusCode.Forbidden);
		}
	}

	private void HandleMediaStream(RequestHeader requestHeader, Socket acceptSocket)
	{
		MediaProviderChannel mediaProviderChannel = _mediaRequestHandler(requestHeader);
		if (mediaProviderChannel != null)
		{
			bool flag = false;
			Protocol.SendStreamResponseStartMultiPart(acceptSocket);
			DateTime dateTime = DateTime.MinValue;
			while (!mediaProviderChannel.ToBeDisconnnected && Connected(acceptSocket) && !flag)
			{
				bool flag2 = true;
				if (mediaProviderChannel.InIdleMode)
				{
					if (DateTime.Now > dateTime)
					{
						dateTime = DateTime.Now + TimeSpan.FromSeconds(2.0);
					}
					else
					{
						flag2 = false;
					}
				}
				if (flag2)
				{
					if (acceptSocket.Available > 0)
					{
						byte[] buffer = new byte[acceptSocket.Available];
						acceptSocket.Receive(buffer);
					}
					MediaData mediaData = _getMediaDataDelegate(mediaProviderChannel, requestHeader);
					if (mediaData != null && !Protocol.SendMediaData(acceptSocket, mediaData))
					{
						flag = true;
					}
				}
				int num = ((requestHeader.MediaProviderSession.FPS != 0) ? requestHeader.MediaProviderSession.FPS : 10);
				Thread.Sleep(1000 / num);
			}
			_sessionDisconnected(requestHeader);
		}
		else
		{
			Thread.Sleep(2000);
			Protocol.SendStreamResponse(acceptSocket, HttpStatusCode.Forbidden);
		}
	}

	private static bool Connected(Socket socket)
	{
		if (socket.Poll(1, SelectMode.SelectRead))
		{
			return socket.Available != 0;
		}
		return true;
	}
}
