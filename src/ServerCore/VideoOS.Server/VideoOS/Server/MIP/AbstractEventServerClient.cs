using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Services;

namespace VideoOS.Server.MIP;

public abstract class AbstractEventServerClient
{
	private const int MAX_ITEMS = 1000000;

	private const int DEFAULT_RETRY_COUNT = 3;

	public abstract CommunicationState State { get; }

	protected Uri EventServerAddress
	{
		get
		{
			RegisteredServer eventServerRegistration = GetEventServerRegistration();
			if (eventServerRegistration != null && eventServerRegistration.Addresses.Length != 0)
			{
				return ClientAddress(new Uri(eventServerRegistration.Addresses[0].URL));
			}
			return null;
		}
	}

	protected Uri GatewayServerAddress
	{
		get
		{
			RegisteredServer gatewayServerRegistration = GetGatewayServerRegistration();
			if (gatewayServerRegistration != null && gatewayServerRegistration.Addresses.Length != 0)
			{
				return ClientAddress(new Uri(gatewayServerRegistration.Addresses[0].URL));
			}
			return null;
		}
	}

	protected abstract void RecreateClient();

	protected abstract void CloseClient();

	public void Open()
	{
		RecreateClient();
	}

	public void Close()
	{
		CloseClient();
	}

	protected static void ShutdownClient<T>(ClientBase<T> client) where T : class
	{
		if (client == null)
		{
			return;
		}
		try
		{
			if (client.State != CommunicationState.Faulted)
			{
				client.Close();
			}
		}
		catch (CommunicationObjectFaultedException)
		{
			client.Abort();
		}
		catch (TimeoutException)
		{
			client.Abort();
		}
		finally
		{
			if (client.State != CommunicationState.Closed)
			{
				client.Abort();
			}
		}
	}

	protected static void EnsureLargeObjectGraphsAllowed<T>(ClientBase<T> client) where T : class
	{
		if (client == null)
		{
			return;
		}
		foreach (OperationDescription operation in client.Endpoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 1000000;
			}
		}
	}

	protected T Retry<T>(Func<T> call)
	{
		return Retry(call, 3);
	}

	protected T Retry<T>(Func<T> call, int retryCount)
	{
		int num = retryCount;
		while (num-- > 0)
		{
			try
			{
				if (call != null)
				{
					return call();
				}
				throw new ArgumentException("Call function is null");
			}
			catch (FaultException)
			{
				throw;
			}
			catch (TimeoutException)
			{
				throw;
			}
			catch (CommunicationException)
			{
				if (num == 0 || State != CommunicationState.Faulted)
				{
					throw;
				}
				CloseClient();
				RecreateClient();
			}
		}
		return default(T);
	}

	protected void Retry(Action call)
	{
		Retry(call, 3);
	}

	protected void Retry(Action call, int retryCount)
	{
		int num = retryCount;
		while (num-- > 0)
		{
			try
			{
				if (call != null)
				{
					call();
					break;
				}
				throw new ArgumentException("Call function is null");
			}
			catch (FaultException)
			{
				throw;
			}
			catch (TimeoutException)
			{
				throw;
			}
			catch (CommunicationException)
			{
				if (num == 0 || State != CommunicationState.Faulted)
				{
					throw;
				}
				CloseClient();
				RecreateClient();
			}
		}
	}

	protected abstract Uri ClientAddress(Uri eventServerAddress);

	public static RegisteredServer GetServerRegistration(Guid serviceType)
	{
		using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(Server.Instance.Data)))
		{
			foreach (RegisteredServer value in configurationManager.RegisteredServers.Values)
			{
				if (value.Type.Equals(serviceType))
				{
					return value;
				}
			}
		}
		return null;
	}

	public static RegisteredServer GetGatewayServerRegistration()
	{
		return GetServerRegistration(ServiceTypes.ServiceType_GatewayService);
	}

	public static RegisteredServer GetEventServerRegistration()
	{
		return GetServerRegistration(ServiceTypes.ServiceType_EventServerService);
	}
}
