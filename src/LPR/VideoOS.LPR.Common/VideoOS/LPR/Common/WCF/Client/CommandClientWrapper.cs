using System;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform.Login;
using VideoOS.Utilities;
using VideoOS.Utilities.Validation;

namespace VideoOS.LPR.Common.WCF.Client;

public abstract class CommandClientWrapper<TClient, TChannel> where TClient : ClientBase<TChannel> where TChannel : class
{
	private const int MessageBufferSize = 500000000;

	private const int MaxItems = 1000000;

	private readonly ErrorCode _defaultErrorCode;

	private TClient _client;

	private Binding _binding;

	private EndpointAddress _endpointAddress;

	private NetworkCredential _credential;

	private readonly object _clientLock = new object();

	public TClient Client{
		get
		{
			lock (_clientLock)
			{
				if (_client == null || _client.State == CommunicationState.Closed || _client.State == CommunicationState.Closing || _client.State == CommunicationState.Faulted)
				{
					_client = CreateClient(_binding, _endpointAddress);
					if (_credential != null && _client.ClientCredentials != null)
					{
						_client.ClientCredentials.Windows.ClientCredential = _credential;
					}
				}
				return _client;
			}
		}
	}

	protected abstract string GetToken();

	private static TClient CreateClient(Binding binding, EndpointAddress endpointAddress)
	{
		TClient val = typeof(TClient).GetConstructor(new Type[2]
		{
			typeof(Binding),
			typeof(EndpointAddress)
		}).Invoke(new object[2] { binding, endpointAddress }) as TClient;
		foreach (OperationDescription operation in val.Endpoint.Contract.Operations)
		{
			DataContractSerializerOperationBehavior dataContractSerializerOperationBehavior = operation.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (dataContractSerializerOperationBehavior != null)
			{
				dataContractSerializerOperationBehavior.MaxItemsInObjectGraph = 1000000;
			}
		}
		if (val.ClientCredentials != null)
		{
			val.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
			{
				CertificateValidationMode = X509CertificateValidationMode.None
			};
		}
		return val;
	}

	private static bool IsOAuthIdentity(LoginSettings ls)
	{
		PropertyInfo propertyInfo = ls.GetType().GetProperties().FirstOrDefault((PropertyInfo p) => p.Name == "IsOAuthIdentity");
		if (!(propertyInfo != null))
		{
			return false;
		}
		return (bool)propertyInfo.GetValue(ls);
	}

	protected CommandClientWrapper(LoginSettings logOnSettings, Uri address, string serviceName, ErrorCode defaultErrorCode)
	{
		_defaultErrorCode = defaultErrorCode;
		if (logOnSettings != null && (logOnSettings.IsBasicUser || IsOAuthIdentity(logOnSettings)))
		{
			CreateBasicUserBinding(address, serviceName);
		}
		else
		{
			CreateWindowsUserBinding(logOnSettings, address, serviceName);
		}
	}

	public void Close()
	{
		lock (_clientLock)
		{
			if (_client != null && _client.State != CommunicationState.Closed && _client.State != CommunicationState.Closing)
			{
				try
				{
					_client.Close();
					return;
				}
				catch (Exception)
				{
					_client.Abort();
					return;
				}
			}
		}
	}

	public void CallWithRetry(Action<TClient, string> operation, [CallerMemberName] string methodName = "")
	{
		operation.NullCheck("", "CallWithRetry", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\WCF\\Client\\CommandClientWrapper.cs", 128);
		bool flag = true;
		while (true)
		{
			try
			{
				operation(Client, GetToken());
				break;
			}
			catch (TimeoutException innerException)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException);
			}
			catch (FaultException<ServiceFault> ex)
			{
				throw new ServiceFaultException(ex.Detail.ErrorCode, ex.Detail.ErrorMessage, methodName, ex);
			}
			catch (FaultException innerException2)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException2);
			}
			catch (ProtocolException innerException3)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException3);
			}
			catch (CommunicationException innerException4)
			{
				if (!flag)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException4);
				}
			}
			catch (Exception innerException5)
			{
				if (!flag)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException5);
				}
			}
			AbortIfFailed();
			flag = false;
		}
	}

	public TResult CallWithRetry<TResult>(Func<TClient, string, TResult> operation, [CallerMemberName] string methodName = "")
	{
		operation.NullCheck("", "CallWithRetry", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\WCF\\Client\\CommandClientWrapper.cs", 179);
		bool flag = true;
		while (true)
		{
			try
			{
				return operation(Client, GetToken());
			}
			catch (TimeoutException innerException)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException);
			}
			catch (FaultException<ServiceFault> ex)
			{
				throw new ServiceFaultException(ex.Detail.ErrorCode, ex.Detail.ErrorMessage, methodName, ex);
			}
			catch (FaultException innerException2)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException2);
			}
			catch (ProtocolException innerException3)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException3);
			}
			catch (CommunicationException innerException4)
			{
				if (!flag)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException4);
				}
			}
			catch (Exception innerException5)
			{
				if (!flag)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException5);
				}
			}
			AbortIfFailed();
			flag = false;
		}
	}

	public async Task CallWithRetryAsync(Func<TClient, string, Task> operation, [CallerMemberName] string methodName = "")
	{
		operation.NullCheck("", "CallWithRetryAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\WCF\\Client\\CommandClientWrapper.cs", 229);
		bool retry = true;
		while (true)
		{
			try
			{
				await operation(Client, GetToken()).ConfigureAwait(continueOnCapturedContext: false);
				break;
			}
			catch (TimeoutException innerException)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException);
			}
			catch (FaultException<ServiceFault> ex)
			{
				throw new ServiceFaultException(ex.Detail.ErrorCode, ex.Detail.ErrorMessage, methodName, ex);
			}
			catch (FaultException innerException2)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException2);
			}
			catch (ProtocolException innerException3)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException3);
			}
			catch (CommunicationException innerException4)
			{
				if (!retry)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException4);
				}
			}
			catch (Exception innerException5)
			{
				if (!retry)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException5);
				}
			}
			AbortIfFailed();
			retry = false;
		}
	}

	public async Task<TResult> CallWithRetryAsync<TResult>(Func<TClient, string, Task<TResult>> operation, [CallerMemberName] string methodName = "")
	{
		operation.NullCheck("", "CallWithRetryAsync", "C:\\actions-runner\\_work\\XPCO\\XPCO\\LPR\\Components\\Analytics\\VideoOS.LPR.Common\\WCF\\Client\\CommandClientWrapper.cs", 280);
		bool retry = true;
		while (true)
		{
			try
			{
				return await operation(Client, GetToken()).ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (TimeoutException innerException)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException);
			}
			catch (FaultException<ServiceFault> ex)
			{
				throw new ServiceFaultException(ex.Detail.ErrorCode, ex.Detail.ErrorMessage, methodName, ex);
			}
			catch (FaultException innerException2)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException2);
			}
			catch (ProtocolException innerException3)
			{
				throw new ServiceFaultException(_defaultErrorCode, methodName, innerException3);
			}
			catch (CommunicationException innerException4)
			{
				if (!retry)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException4);
				}
			}
			catch (Exception innerException5)
			{
				if (!retry)
				{
					throw new ServiceFaultException(_defaultErrorCode, methodName, innerException5);
				}
			}
			AbortIfFailed();
			retry = false;
		}
	}

	private void CreateBasicUserBinding(Uri address, string serviceName)
	{
		BasicHttpSecurityMode mode = ((address.Scheme == Uri.UriSchemeHttps) ? BasicHttpSecurityMode.Transport : BasicHttpSecurityMode.None);
		_binding = new BasicHttpBinding
		{
			MaxReceivedMessageSize = 500000000L,
			MaxBufferPoolSize = 500000000L,
			ReaderQuotas = 
			{
				MaxArrayLength = 500000000,
				MaxBytesPerRead = 500000000,
				MaxNameTableCharCount = 500000000,
				MaxDepth = 500000000,
				MaxStringContentLength = 500000000
			},
			Security = 
			{
				Mode = mode
			}
		};
		Uri relativeUri = new Uri(serviceName + "2", UriKind.Relative);
		Uri serverUri = new Uri(address, relativeUri);
		_endpointAddress = new EndpointAddress(new Uri(address, relativeUri), EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(serverUri)));
		_credential = null;
	}

	private void CreateWindowsUserBinding(LoginSettings logOnSettings, Uri address, string serviceName)
	{
		SecurityMode mode = ((address.Scheme == Uri.UriSchemeHttps) ? SecurityMode.TransportWithMessageCredential : SecurityMode.Message);
		_binding = new WSHttpBinding
		{
			MaxReceivedMessageSize = 500000000L,
			MaxBufferPoolSize = 500000000L,
			ReaderQuotas = 
			{
				MaxArrayLength = 500000000,
				MaxBytesPerRead = 500000000,
				MaxNameTableCharCount = 500000000,
				MaxDepth = 500000000,
				MaxStringContentLength = 500000000
			},
			Security = 
			{
				Mode = mode,
				Transport = 
				{
					ClientCredentialType = HttpClientCredentialType.Windows,
					ProxyCredentialType = HttpProxyCredentialType.None
				},
				Message = 
				{
					ClientCredentialType = MessageCredentialType.Windows,
					EstablishSecurityContext = false,
					NegotiateServiceCredential = true
				}
			},
			CloseTimeout = TimeSpan.FromSeconds(3.0)
		};
		Uri relativeUri = new Uri(serviceName, UriKind.Relative);
		Uri serverUri = new Uri(address, relativeUri);
		_endpointAddress = new EndpointAddress(new Uri(address, relativeUri), EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(serverUri)));
		_credential = logOnSettings?.NetworkCredential;
	}

	private void AbortIfFailed()
	{
		lock (_clientLock)
		{
			if (_client != null && _client.State == CommunicationState.Faulted)
			{
				_client.Abort();
				_client = null;
			}
		}
	}
}
