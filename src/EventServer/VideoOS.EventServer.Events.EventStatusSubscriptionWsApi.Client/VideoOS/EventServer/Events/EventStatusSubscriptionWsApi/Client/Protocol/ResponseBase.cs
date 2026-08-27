using System;
using Newtonsoft.Json.Linq;
using VideoOS.Platform;

namespace VideoOS.EventServer.Events.EventStatusSubscriptionWsApi.Client.Protocol;

public class ResponseBase
{
	public ulong? CommandId { get; }

	public int Status { get; }

	public ResponseBase(ulong? commandId, int status)
	{
		CommandId = commandId;
		Status = status;
	}

	public static T FromJson<T>(JObject jsonData)
	{
		try
		{
			return ((JToken)jsonData).ToObject<T>();
		}
		catch (Exception innerException)
		{
			throw new CommunicationContentMIPException("Unexpected response. Response was not of type " + typeof(T).Name, innerException);
		}
	}
}
