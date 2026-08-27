using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.TrayController.Communication;

public static class Utilities
{
	public static void AddLoopbackServerPorts(int portNumber, Server server)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		if (Socket.OSSupportsIPv4)
		{
			string text = IPAddress.Loopback.ToString();
			server.Ports.Add(new ServerPort(text, portNumber, ServerCredentials.Insecure));
		}
		else
		{
			string text2 = IPAddress.IPv6Loopback.ToString();
			server.Ports.Add(new ServerPort(text2, portNumber, ServerCredentials.Insecure));
		}
	}

	public static string GetLoopbackClientHostAndPort(int portNumber)
	{
		if (Socket.OSSupportsIPv4)
		{
			string arg = IPAddress.Loopback.ToString();
			return $"{arg}:{portNumber}";
		}
		string arg2 = IPAddress.IPv6Loopback.ToString();
		return $"{arg2}:{portNumber}";
	}

	public static Task<ArrayOfMessages> GetMessages(GetMessages_Request request, ServerCallContext context, IMessageProvider messageProvider)
	{
		ArrayOfMessages arrayOfMessages = new ArrayOfMessages();
		foreach (LogMessage message in messageProvider.GetMessages((int)request.HighwaterMark))
		{
			Message_Response message_Response = new Message_Response
			{
				Sequence = message.SequenceNumber,
				When = Timestamp.FromDateTime(message.Timestamp.ToUniversalTime()),
				What = message.Message
			};
			arrayOfMessages.Messages.Add(message_Response);
		}
		return Task.FromResult(arrayOfMessages);
	}

	public static List<LogMessage> GetMessages(ArrayOfMessages response)
	{
		List<LogMessage> list = new List<LogMessage>();
		foreach (Message_Response message in response.Messages)
		{
			list.Add(new LogMessage
			{
				SequenceNumber = (int)message.Sequence,
				Timestamp = message.When.ToDateTime(),
				Message = message.What
			});
		}
		return list;
	}
}
