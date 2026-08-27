using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.MetaChannel.Serializers;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

namespace VideoOS.Mobile.SDK.Portable.Server.HTTP;

public class HttpEntry
{
	private readonly IHttpChannel _httpChannel;

	private readonly CommunicationEntry _entry;

	private readonly CommandsSeparator _commSeparator;

	private readonly ManualResetEvent _requestCompleted;

	private readonly IReadOnlyDictionary<string, string> _headers;

	private readonly MessageTypes _messageType;

	public const int MinimalBufferSize = 2048;

	public const int MaximalBufferSize = 1048576;

	public HttpEntry(IHttpChannel httpChannel, CommunicationEntry entry, IReadOnlyDictionary<string, string> headers, MessageTypes messageType)
	{
		_httpChannel = httpChannel;
		_entry = entry;
		_headers = headers ?? new Dictionary<string, string>();
		_messageType = messageType;
		_commSeparator = CommandSeparatorFactory.CreateSeparator(HttpChannelHelper.ContentTypeFromMessageType(_messageType));
		CommandsSeparator commSeparator = _commSeparator;
		commSeparator.MessageEvent = (CommandsSeparator.OnMessage)Delegate.Combine(commSeparator.MessageEvent, new CommandsSeparator.OnMessage(OnNewMessage));
		CommandsSeparator commSeparator2 = _commSeparator;
		commSeparator2.ChannelEncryption = (Func<string, Tuple<DHEncryption, bool>>)Delegate.Combine(commSeparator2.ChannelEncryption, new Func<string, Tuple<DHEncryption, bool>>(GetChanelEncryption));
		_requestCompleted = new ManualResetEvent(initialState: false);
	}

	private Tuple<DHEncryption, bool> GetChanelEncryption(string chanelId)
	{
		return new Tuple<DHEncryption, bool>(_httpChannel.EncryptionGetter?.Invoke(), _httpChannel.ConnectionEncryptionProperties().RequireCommandEncryption);
	}

	private void OnNewMessage(CommunicationEntry response)
	{
		_httpChannel.OnResponse?.Invoke(response);
	}

	public void Run(string extension = null)
	{
		string text = "";
		try
		{
			text = _httpChannel.WebAddress + (extension ?? string.Empty);
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(text);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = HttpChannelHelper.ContentTypeFromMessageType(_messageType);
			httpWebRequest.Accept = "application/xml";
			foreach (KeyValuePair<string, string> header in _headers)
			{
				httpWebRequest.Headers[header.Key] = header.Value;
			}
			httpWebRequest.BeginGetRequestStream(GetRequestStreamCallback, httpWebRequest);
			_requestCompleted.WaitOne();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpEntry.Run() at " + text, ex.Message);
			ProcessHttpException(ex, ErrorCodes.HttpRequestError);
		}
	}

	private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
	{
		string text = "";
		try
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)asynchronousResult.AsyncState;
			text = httpWebRequest.RequestUri.ToString();
			Stream stream = httpWebRequest.EndGetRequestStream(asynchronousResult);
			Command command = _entry.Command;
			if (_httpChannel.ConnectionEncryptionProperties != null)
			{
				EncryptionProperties encryptionProperties = _httpChannel.ConnectionEncryptionProperties();
				if (!string.IsNullOrEmpty(_entry.ConnectionId) && encryptionProperties.ServerSupportsCommandEncryption && (encryptionProperties.RequireCommandEncryption || (!encryptionProperties.RequireCommandEncryption && encryptionProperties.CommandEncryptionList.Contains(command.GetCommandKind()))))
				{
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(Command));
					EncodedStringWriter encodedStringWriter = new EncodedStringWriter(Encoding.UTF8);
					XmlWriter xmlWriter = XmlWriter.Create(encodedStringWriter);
					xmlSerializer.Serialize(xmlWriter, command);
					_entry.Command = null;
					_entry.EncryptedCommand = _httpChannel.EncryptionGetter?.Invoke().Encrypt(encodedStringWriter.GetStringBuilder().ToString());
				}
			}
			MessageSerializerFactory.CreateSerializer<CommunicationEntry>(HttpChannelHelper.ContentTypeFromMessageType(_messageType)).Serialize(_entry, stream);
			stream.Write(HttpChannel.BytesEoc, 0, HttpChannel.BytesEoc.Length);
			stream.Dispose();
			httpWebRequest.BeginGetResponse(GetResponseCallback, httpWebRequest);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "HttpEntry.GetRequestStreamCallback() at " + text, ex.Message);
			ProcessHttpException(ex, ErrorCodes.HttpRequestError);
		}
		_requestCompleted.Set();
	}

	private void GetResponseCallback(IAsyncResult asynchronousResult)
	{
		string text = "";
		try
		{
			HttpWebRequest obj = (HttpWebRequest)asynchronousResult.AsyncState;
			text = obj.RequestUri.ToString();
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)obj.EndGetResponse(asynchronousResult))
			{
				using MemoryStream memoryStream = new MemoryStream();
				using (Stream stream = httpWebResponse.GetResponseStream())
				{
					stream.CopyTo(memoryStream);
				}
				ByteBuffer byteBuffer = new ByteBuffer((int)memoryStream.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				int num = 0;
				while ((num = memoryStream.Read(byteBuffer.data, byteBuffer.end, byteBuffer.rest)) != 0)
				{
					byteBuffer.lenWritten += num;
					_commSeparator.ProcessBuffer(byteBuffer);
				}
				_commSeparator.ProcessBuffer(byteBuffer, isEndOfStream: true);
			}
			if (_commSeparator.MessagesCount == 0)
			{
				ProcessHttpException(new Exception("Incorrect server response"), ErrorCodes.IncorrectServerResponse);
			}
		}
		catch (WebException ex) when (ex.InnerException is AuthenticationException)
		{
			Logger.Instance.Log(LogContent.Error, "HttpEntry.GetResponseCallback() at " + text, ex.Message);
			ProcessHttpException(ex, ErrorCodes.InvalidCertificate);
		}
		catch (Exception ex2)
		{
			Logger.Instance.Log(LogContent.Error, "HttpEntry.GetResponseCallback() at " + text, ex2.Message);
			ProcessHttpException(ex2, ErrorCodes.HttpResponseError);
		}
	}

	private void CheckBufferSize(ByteBuffer buffer)
	{
		if (buffer.rest == 0)
		{
			int size = buffer.size;
			if (size >= 1048576)
			{
				throw new Exception("Extreamly large server response - not supported by the SDK");
			}
			size *= 5;
			buffer.Resize(size);
		}
	}

	private void ProcessHttpException(Exception exception, ErrorCodes errorCode)
	{
		CommunicationEntry communicationEntry = new CommunicationEntry();
		Command command = _entry.Command;
		Command command2 = new Command
		{
			CommandNumber = command.CommandNumber
		};
		command2.SetCommandKind(command.GetCommandKind());
		command2.SetCommandType(Command.CommandTypes.Response);
		command2.SetCommandResult(Command.CommandResultTypes.ResultError);
		command2.SetErrorCode(errorCode);
		command2.ErrorString = exception.Message;
		communicationEntry.Command = command2;
		OnNewMessage(communicationEntry);
	}
}
