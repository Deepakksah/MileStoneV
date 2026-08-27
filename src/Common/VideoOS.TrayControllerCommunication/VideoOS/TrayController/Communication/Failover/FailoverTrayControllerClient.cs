using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.TrayController.Communication.Failover;

public sealed class FailoverTrayControllerClient : IFailoverTrayControllerClient
{
	private FailoverTrayControllerService.FailoverTrayControllerServiceClient _client;

	private Channel _channel;

	public FailoverTrayControllerClient(int portNumber)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		string loopbackClientHostAndPort = Utilities.GetLoopbackClientHostAndPort(portNumber);
		_channel = new Channel(loopbackClientHostAndPort, ChannelCredentials.Insecure);
		_client = new FailoverTrayControllerService.FailoverTrayControllerServiceClient((ChannelBase)(object)_channel);
	}

	public FailoverState GetFailoverState()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0076: Expected O, but got Unknown
		try
		{
			FailoverStateResponse serviceState = _client.GetServiceState(new Empty(), new CallOptions((Metadata)null, (DateTime?)DateTime.UtcNow.AddSeconds(2.0), default(CancellationToken), (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null));
			return new FailoverState
			{
				serviceState = TrayControllerCommunicationConverter.ToServiceState(serviceState.ServiceState),
				connectStatus = TrayControllerCommunicationConverter.ToConnectStatus(serviceState.ConnectStatus),
				failoverRecorderState = serviceState.FailoverRecorderState
			};
		}
		catch (RpcException ex)
		{
			RpcException innerException = ex;
			throw new IOException("Failed to retrieve state", (Exception)(object)innerException);
		}
	}

	public List<LogMessage> GetMessages(int sequenceNumber)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			GetMessages_Request request = new GetMessages_Request
			{
				HighwaterMark = sequenceNumber
			};
			return Utilities.GetMessages(_client.GetMessagesAsync(request, new CallOptions((Metadata)null, (DateTime?)DateTime.UtcNow.AddSeconds(2.0), default(CancellationToken), (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null)).GetAwaiter().GetResult());
		}
		catch (RpcException ex)
		{
			RpcException innerException = ex;
			throw new IOException("Failed to retrieve messages", (Exception)(object)innerException);
		}
	}

	public void CloseChannel()
	{
		((ChannelBase)_channel).ShutdownAsync();
	}
}
