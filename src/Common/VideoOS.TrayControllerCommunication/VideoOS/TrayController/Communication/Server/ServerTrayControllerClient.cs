using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.TrayController.Communication.Server;

public sealed class ServerTrayControllerClient : IServerTrayControllerClient
{
	private ServerTrayControllerService.ServerTrayControllerServiceClient _client;

	private Channel _channel;

	public ServerTrayControllerClient(int portNumber)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		string loopbackClientHostAndPort = Utilities.GetLoopbackClientHostAndPort(portNumber);
		_channel = new Channel(loopbackClientHostAndPort, ChannelCredentials.Insecure);
		_client = new ServerTrayControllerService.ServerTrayControllerServiceClient((ChannelBase)(object)_channel);
	}

	public List<LogMessage> GetMessages(int sequenceNumber)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			GetMessages_Request request = new GetMessages_Request
			{
				HighwaterMark = sequenceNumber
			};
			return Utilities.GetMessages(_client.GetMessages(request, new CallOptions((Metadata)null, (DateTime?)DateTime.UtcNow.AddSeconds(2.0), default(CancellationToken), (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null)));
		}
		catch (RpcException ex)
		{
			RpcException innerException = ex;
			throw new IOException("Failed to retrieve messages", (Exception)(object)innerException);
		}
	}

	public ServerState GetServerState()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0067: Expected O, but got Unknown
		try
		{
			ServerStateResponse result = _client.GetServiceStateAsync(new Empty(), new CallOptions((Metadata)null, (DateTime?)DateTime.UtcNow.AddSeconds(2.0), default(CancellationToken), (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null)).GetAwaiter().GetResult();
			return new ServerState
			{
				serviceState = TrayControllerCommunicationConverter.ToServiceState(result.ServiceState)
			};
		}
		catch (RpcException ex)
		{
			RpcException innerException = ex;
			throw new IOException("Failed to retrieve state", (Exception)(object)innerException);
		}
	}

	public void CloseChannel()
	{
		((ChannelBase)_channel).ShutdownAsync();
	}
}
