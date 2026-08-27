using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Mobile.Portable.Encryption;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel.Serializers;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.MetaChannel;

public class CommandsSeparator
{
	public delegate void OnMessage(CommunicationEntry tCommEntry);

	private readonly MessageSeparatorRNRN _messageSeparator;

	private readonly IMessageSerializer<CommunicationEntry> _serializer;

	private readonly bool _separateCommands;

	public OnMessage MessageEvent { get; set; }

	public Func<string, Tuple<DHEncryption, bool>> ChannelEncryption { get; set; }

	public IMessageSerializer<CommunicationEntry> MessageSerializer => _serializer;

	private static XmlReaderSettings XmlReaderSettings => SecurityXmlReaderSettings.Settings;

	public int MessagesCount => _messageSeparator.MessagesCount;

	public CommandsSeparator(IMessageSerializer<CommunicationEntry> serializer, bool separateCommands = true)
	{
		_separateCommands = separateCommands;
		_serializer = serializer;
		_messageSeparator = new MessageSeparatorRNRN();
		MessageSeparatorRNRN messageSeparator = _messageSeparator;
		messageSeparator.MessageEvent = (MessageSeparator.OnMessage)Delegate.Combine(messageSeparator.MessageEvent, new MessageSeparator.OnMessage(OnNewMessage));
	}

	private void ProcessCommands(CommunicationEntry tCommEntry)
	{
		tCommEntry.Command.ConvertToDictionary(Command.ParamsType.Input);
		tCommEntry.Command.ConvertToDictionary(Command.ParamsType.Output);
	}

	private ErrorCodes OnNewMessage(byte[] data, int offset, int length)
	{
		ErrorCodes result = ErrorCodes.Ok;
		try
		{
			CommunicationEntry communicationEntry;
			using (MemoryStream stream = new MemoryStream(data, offset, length))
			{
				communicationEntry = _serializer.Deserialize(stream);
			}
			DecryptCommandIfRequired(communicationEntry);
			ProcessCommands(communicationEntry);
			if (_separateCommands)
			{
				CommunicationEntry communicationEntry2 = new CommunicationEntry();
				communicationEntry2.ConnectionId = communicationEntry.ConnectionId;
				communicationEntry2.Command = communicationEntry.Command;
				communicationEntry2.EncryptedCommand = communicationEntry.EncryptedCommand;
				MessageEvent(communicationEntry2);
			}
			else
			{
				MessageEvent(communicationEntry);
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CommandsSeparator.OnNewMessage", ex.Message);
			result = ErrorCodes.ParserError;
		}
		return result;
	}

	private void DecryptCommandIfRequired(CommunicationEntry tCommEntry)
	{
		if (ChannelEncryption == null || string.IsNullOrEmpty(tCommEntry.ConnectionId))
		{
			return;
		}
		Tuple<DHEncryption, bool> tuple = ChannelEncryption(tCommEntry.ConnectionId);
		if (tuple != null)
		{
			if (!string.IsNullOrEmpty(tCommEntry.EncryptedCommand))
			{
				Command command = DeserializeCommand(tuple.Item1.Decrypt(tCommEntry.EncryptedCommand));
				tCommEntry.Command = command;
			}
			else if (tuple.Item2)
			{
				tCommEntry.Command = null;
			}
		}
	}

	public static Command DeserializeCommand(string commandXml)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(Command));
		using TextReader input = new StringReader(commandXml);
		using XmlReader xmlReader = XmlReader.Create(input, XmlReaderSettings);
		try
		{
			Command obj = (Command)xmlSerializer.Deserialize(xmlReader);
			obj.ConvertToDictionary(Command.ParamsType.Input);
			obj.ConvertToDictionary(Command.ParamsType.Output);
			return obj;
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CommandsSeparator.DeserializeCommand", ex.Message);
			return null;
		}
	}

	public ErrorCodes ProcessBuffer(ByteBuffer tBuffer, bool isEndOfStream = false)
	{
		ErrorCodes result = _messageSeparator.ProcessBuffer(tBuffer, isEndOfStream);
		_messageSeparator.CompactBuffer(tBuffer);
		return result;
	}
}
