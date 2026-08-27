using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using VideoOS.Vms.Types;

namespace VideoOS.TrayController.Communication.Recorder;

public sealed class RecorderTrayControllerClient : IRecorderTrayControllerClient
{
	private RecorderTrayControllerService.RecorderTrayControllerServiceClient _client;

	private Channel _channel;

	public RecorderTrayControllerClient(int portNumber)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		string loopbackClientHostAndPort = Utilities.GetLoopbackClientHostAndPort(portNumber);
		_channel = new Channel(loopbackClientHostAndPort, ChannelCredentials.Insecure);
		_client = new RecorderTrayControllerService.RecorderTrayControllerServiceClient((ChannelBase)(object)_channel);
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

	public RecorderState GetRecorderState()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_006a: Expected O, but got Unknown
		try
		{
			RecorderStateResponse serviceState = _client.GetServiceState(new Empty(), new CallOptions((Metadata)null, (DateTime?)DateTime.UtcNow.AddSeconds(2.0), default(CancellationToken), (WriteOptions)null, (ContextPropagationToken)null, (CallCredentials)null));
			return new RecorderState
			{
				serviceState = TrayControllerCommunicationConverter.ToServiceState(serviceState.ServiceState),
				connectStatus = TrayControllerCommunicationConverter.ToConnectStatus(serviceState.ConnectStatus)
			};
		}
		catch (RpcException ex)
		{
			RpcException innerException = ex;
			throw new IOException("Failed to retrieve state", (Exception)(object)innerException);
		}
	}

	public bool IsRecorderRunningAndConnected()
	{
		if (GetRecorderState().serviceState == ServiceState.Running)
		{
			return GetRecorderState().connectStatus == ConnectStatus.Connected;
		}
		return false;
	}

	public void CloseChannel()
	{
		((ChannelBase)_channel).ShutdownAsync().Wait();
	}
}
