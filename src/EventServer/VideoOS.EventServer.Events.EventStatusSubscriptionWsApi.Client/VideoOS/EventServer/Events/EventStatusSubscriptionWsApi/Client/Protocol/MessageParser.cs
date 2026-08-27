using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideoOS.Platform;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class MessageParser : IMessageParser
{
	private Dictionary<string, Type> _knownResponses = new Dictionary<string, Type>
	{
		{
			"StartSessionResponse".ToCamelcase(),
			typeof(StartSessionResponse)
		},
		{
			"SubscriptionUpdateResponse".ToCamelcase(),
			typeof(SubscriptionUpdateResponse)
		},
		{
			"RemoveSubscriptionResponse".ToCamelcase(),
			typeof(RemoveSubscriptionResponse)
		},
		{
			"GetStateResponse".ToCamelcase(),
			typeof(GetStateResponse)
		}
	};

	public MessageType GetMessageType(JObject messageData)
	{
		JToken val = default(JToken);
		if (messageData.TryGetValue("CommandId".ToCamelcase(), ref val))
		{
			return MessageType.Response;
		}
		if (messageData.TryGetValue("Events".ToCamelcase(), ref val))
		{
			return MessageType.Event;
		}
		return MessageType.Unknown;
	}

	public JObject ParseMessage(string receivedMessage)
	{
		//IL_000a: Expected O, but got Unknown
		try
		{
			return JObject.Parse(receivedMessage);
		}
		catch (JsonReaderException ex)
		{
			JsonReaderException ex2 = ex;
			throw new CommunicationContentMIPException(FormattableString.Invariant($"Badly formed message: {((Exception)(object)ex2).Message}"));
		}
		catch (Exception ex3)
		{
			throw new CommunicationContentMIPException(FormattableString.Invariant($"Badly formed message: {ex3.Message}"));
		}
	}

	public ulong GetCommandId(JObject responseMessage)
	{
		//IL_0029: Expected O, but got Unknown
		try
		{
			JToken val = default(JToken);
			if (!responseMessage.TryGetValue("CommandId".ToCamelcase(), ref val))
			{
				throw new CommunicationContentMIPException("Missing 'commandId'");
			}
			return Extensions.Value<ulong>((IEnumerable<JToken>)val);
		}
		catch (JsonReaderException ex)
		{
			JsonReaderException ex2 = ex;
			throw new CommunicationContentMIPException(FormattableString.Invariant($"Badly formed message: {((Exception)(object)ex2).Message}"));
		}
		catch (InvalidCastException ex3)
		{
			throw new CommunicationContentMIPException(FormattableString.Invariant($"Invalid arguments: 'commandId' is unreadable {ex3.Message}"));
		}
		catch (Exception ex4)
		{
			throw new CommunicationContentMIPException(FormattableString.Invariant($"Badly formed message: {ex4.Message}"));
		}
	}

	public bool IsSuccessStatusCode(JObject responseMessage)
	{
		JToken val = default(JToken);
		if (responseMessage.TryGetValue("Status", StringComparison.OrdinalIgnoreCase, ref val))
		{
			try
			{
				int num = Extensions.Value<int>((IEnumerable<JToken>)val);
				if (num >= 200 && num < 300)
				{
					return true;
				}
			}
			catch (InvalidCastException)
			{
			}
		}
		return false;
	}
}
