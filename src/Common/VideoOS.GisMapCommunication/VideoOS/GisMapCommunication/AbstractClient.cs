using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.GisMapSharedData;
using VideoOS.GisMapSharedData.CustomHeader;
using VideoOS.GisMapSharedData.DataContracts;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.GisMapCommunication;

public abstract class AbstractClient<T, TChannel> : IDisposable where T : ClientBase<TChannel> where TChannel : class
{
	private const int DEFAULT_RETRY_COUNT = 1;

	private bool _closed;

	private HashSet<T> _clients = new HashSet<T>();

	protected ServerId ServerId { get; private set; }

	protected TimeSpan RequestTimeout { get; private set; }

	protected Uri Uri { get; private set; }

	protected abstract string Name { get; }

	protected abstract SecurityMode CurrentSecurityMode { get; }

	protected abstract WSMessageEncoding CurrentMessageEncoding { get; }

	protected Uri FinalUri => new UriBuilder(Uri)
	{
		Path = string.Format(CultureInfo.InvariantCulture, "MapService/{0}/{1}", Name, CurrentSecurityMode.ToString())
	}.Uri;

	protected AbstractClient(ServerId serverId, TimeSpan requestTimeout, Uri uri)
	{
		ServerId = serverId;
		RequestTimeout = requestTimeout;
		Uri = uri;
	}

	protected abstract T CreateClient();

	private T TakeClient()
	{
		T val = null;
		lock (_clients)
		{
			using HashSet<T>.Enumerator enumerator = _clients.GetEnumerator();
			if (enumerator.MoveNext())
			{
				val = enumerator.Current;
				_clients.Remove(val);
			}
		}
		if (val == null)
		{
			val = CreateClient();
		}
		return val;
	}

	private void PutClient(T client)
	{
		if (_closed || client.State == CommunicationState.Closing || client.State == CommunicationState.Closed || client.State == CommunicationState.Faulted)
		{
			ShutdownClient(client);
			return;
		}
		lock (_clients)
		{
			_clients.Add(client);
		}
	}

	private static void ShutdownClient(T client)
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
		catch (Exception)
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

	private static string GetLoginToken(ServerId serverId)
	{
		return (LoginSettingsCache.GetLoginSettings(serverId) ?? throw new LoginSettingsNotFoundException(serverId)).Token;
	}

	protected async Task<TRet> CallWithRetryAsync<TRet>(Func<T, Func<Task<TRet>>> methodFactory)
	{
		return await CallWithRetryAsync(methodFactory, 1).ConfigureAwait(continueOnCapturedContext: false);
	}

	protected TRet CallWithRetry<TRet>(Func<T, Func<TRet>> methodFactory)
	{
		return CallWithRetry(methodFactory, 1);
	}

	protected async Task<TRet> CallWithRetryAsync<TRet>(Func<T, Func<Task<TRet>>> methodFactory, int retryCount)
	{
		if (methodFactory == null)
		{
			throw new ArgumentNullException("methodFactory");
		}
		if (retryCount < 0)
		{
			throw new ArgumentException("retryCount should be greater than or equal to 0", "retryCount");
		}
		while (true)
		{
			T client = TakeClient();
			try
			{
				return await methodFactory(client)().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (FaultException<GisMapServiceFault> error)
			{
				FaultExceptionHandler.HandleFaultException(error);
			}
			catch (CommunicationException)
			{
				if (retryCount == 0)
				{
					throw;
				}
				retryCount--;
			}
			finally
			{
				PutClient(client);
			}
		}
	}

	protected TRet CallWithRetry<TRet>(Func<T, Func<TRet>> methodFactory, int retryCount)
	{
		if (methodFactory == null)
		{
			throw new ArgumentNullException("methodFactory");
		}
		if (retryCount < 0)
		{
			throw new ArgumentException("retryCount should be greater than or equal to 0", "retryCount");
		}
		while (true)
		{
			T val = TakeClient();
			try
			{
				return methodFactory(val)();
			}
			catch (FaultException<GisMapServiceFault> error)
			{
				FaultExceptionHandler.HandleFaultException(error);
			}
			catch (CommunicationException)
			{
				if (retryCount == 0)
				{
					throw;
				}
				retryCount--;
			}
			finally
			{
				PutClient(val);
			}
		}
	}

	protected WSHttpBinding CreateWSHttpBindingCredentials()
	{
		WSHttpBinding wSHttpBinding = new WSHttpBinding(CurrentSecurityMode);
		if (CurrentSecurityMode == SecurityMode.Message || CurrentSecurityMode == SecurityMode.TransportWithMessageCredential)
		{
			wSHttpBinding.Security.Message.ClientCredentialType = MessageCredentialType.Windows;
			wSHttpBinding.Security.Message.NegotiateServiceCredential = true;
			wSHttpBinding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
			wSHttpBinding.Security.Message.EstablishSecurityContext = false;
		}
		wSHttpBinding.MessageEncoding = CurrentMessageEncoding;
		wSHttpBinding.MaxReceivedMessageSize = WebServiceConstants.MessageSize;
		wSHttpBinding.MaxBufferPoolSize = WebServiceConstants.MessageSize;
		wSHttpBinding.ReaderQuotas.MaxArrayLength = WebServiceConstants.MessageSize;
		wSHttpBinding.ReaderQuotas.MaxBytesPerRead = WebServiceConstants.MessageSize;
		wSHttpBinding.ReaderQuotas.MaxNameTableCharCount = WebServiceConstants.MessageSize;
		wSHttpBinding.ReaderQuotas.MaxDepth = WebServiceConstants.MessageSize;
		wSHttpBinding.ReaderQuotas.MaxStringContentLength = WebServiceConstants.MessageSize;
		wSHttpBinding.SendTimeout = RequestTimeout;
		return wSHttpBinding;
	}

	protected static void EnsureLargeObjectGraphsAllowed(T client)
	{
		foreach (OperationDescription operation in client.Endpoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = WebServiceConstants.MaxItemsInObjectGraph;
			}
		}
	}

	protected void AddClientHeaderBehavior(T client)
	{
		if (client == null)
		{
			throw new ArgumentException("client was null");
		}
		CustomHeaderEndpointBehavior item = new CustomHeaderEndpointBehavior(() => new MessageHeader<string>(GetLoginToken(ServerId)).GetUntypedHeader("Token", "http://videoos.net/2/GisMapCommand"));
		client.ChannelFactory.Endpoint.EndpointBehaviors.Add(item);
	}

	protected EndpointIdentity CreateSpnIdentity(string spnName)
	{
		return EndpointIdentity.CreateSpnIdentity(spnName);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposing)
		{
			return;
		}
		_closed = true;
		lock (_clients)
		{
			foreach (T client in _clients)
			{
				ShutdownClient(client);
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
