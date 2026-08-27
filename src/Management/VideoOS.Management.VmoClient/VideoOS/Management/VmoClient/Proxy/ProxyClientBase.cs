using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Security;
using VideoOS.IdentityServer.Common.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient.Proxy;

public abstract class ProxyClientBase : IDisposable
{
	private class AdditionalHeaderBehaviour : IEndpointBehavior
	{
		private IDictionary<string, MessageHeader> _extraHeaders;

		public AdditionalHeaderBehaviour(IDictionary<string, MessageHeader> extraHeaders)
		{
			_extraHeaders = extraHeaders;
		}

		public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			clientRuntime.ClientMessageInspectors.Add(new AdditionalHeaderInspector(_extraHeaders));
		}

		public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
		}

		public void Validate(ServiceEndpoint endpoint)
		{
		}
	}

	private class AdditionalHeaderInspector : IClientMessageInspector
	{
		private IDictionary<string, MessageHeader> _extraHeaders;

		public AdditionalHeaderInspector(IDictionary<string, MessageHeader> extraHeaders)
		{
			_extraHeaders = extraHeaders;
		}

		public object BeforeSendRequest(ref Message request, IClientChannel channel)
		{
			foreach (MessageHeader value in _extraHeaders.Values)
			{
				int num = request.Headers.FindHeader(value.Name, value.Namespace);
				if (num > -1)
				{
					request.Headers.RemoveAt(num);
				}
				request.Headers.Add(value);
			}
			return null;
		}

		public void AfterReceiveReply(ref Message reply, object correlationState)
		{
		}
	}

	private bool _disposed;

	private ServerVersion _serverVersion;

	private readonly Dictionary<string, MessageHeader> _extraHeaders = new Dictionary<string, MessageHeader>(StringComparer.OrdinalIgnoreCase);

	protected ConnectionSettings ConnectionSettings { get; private set; }

	public bool IsInitialized { get; set; }

	protected ServerVersion ServerVersion => _serverVersion;

	public abstract ICommunicationObject CommunicationObject { get; }

	public abstract string ServiceName { get; }

	public abstract Uri ServiceAddress { get; }

	public ProxyClientBase(ConnectionSettings connectionSettings)
	{
		ConnectionSettings = connectionSettings;
	}

	public abstract void Initialize();

	public void Initialize(ServerVersion serverVersion)
	{
		_serverVersion = serverVersion;
		Initialize();
	}

	protected Binding GetBinding()
	{
		return ConnectionSettings.GetBinding();
	}

	protected EndpointAddress GetEndpointAddress()
	{
		Uri uri = null;
		uri = ((!ConnectionSettings.IsWindowsLogin) ? GetServerAddress() : ServiceAddress);
		EndpointIdentity identity = EndpointIdentity.CreateSpnIdentity(SpnFactory.GetSpn(ConnectionSettings.ServerUri));
		return new EndpointAddress(uri, identity);
	}

	protected Uri GetServerAddress()
	{
		return new UriBuilder(ConnectionSettings.ServerUri)
		{
			Path = "/ManagementServer/" + ServiceName
		}.Uri;
	}

	protected NetworkCredential GetClientCredentials()
	{
		NetworkCredential networkCredential;
		if (ConnectionSettings.ConnectAsUser)
		{
			networkCredential = new NetworkCredential();
			string[] array = ConnectionSettings.ConnectAsUserName.Split('\\');
			if (array.Length == 2)
			{
				networkCredential.Domain = array[0];
				networkCredential.UserName = array[1];
			}
			else
			{
				networkCredential.UserName = ConnectionSettings.ConnectAsUserName;
			}
			networkCredential.Password = ConnectionSettings.ConnectAsUserPassword;
		}
		else
		{
			networkCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
		}
		return networkCredential;
	}

	protected string ResolveServiceName(string serviceName)
	{
		if (ConnectionSettings.TokenCache != null)
		{
			return serviceName + "OAuth.svc";
		}
		return serviceName + ".svc";
	}

	public void ConfigureCredentials(ClientCredentials clientCredentials)
	{
		if (ConnectionSettings.TokenCache == null)
		{
			if (ConnectionSettings.IsWindowsLogin)
			{
				clientCredentials.Windows.ClientCredential = GetClientCredentials();
				return;
			}
			clientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
			{
				CertificateValidationMode = X509CertificateValidationMode.None
			};
			clientCredentials.UserName.UserName = ConnectionSettings.ConnectAsUserName;
			clientCredentials.UserName.Password = ConnectionSettings.ConnectAsUserPassword;
		}
	}

	public void ConfigureEndpoint(ServiceEndpoint serviceEndpoint)
	{
		if (ConnectionSettings.IsWindowsLogin)
		{
			serviceEndpoint.Behaviors.Add(new ClientViaBehavior(GetServerAddress()));
		}
		if (ConnectionSettings.TokenCache != null)
		{
			serviceEndpoint.AddBearerTokenBehavior(ConnectionSettings.TokenCache);
		}
		serviceEndpoint.EndpointBehaviors.Add(new AdditionalHeaderBehaviour(_extraHeaders));
	}

	protected void ThrowConvertedExceptionIfNeeded(Exception ex)
	{
		Exception connectionConvertedException = ExceptionConverter.GetConnectionConvertedException(ex, ConnectionSettings.ServerName, ConnectionSettings.ServerPort, ConnectionSettings.TokenCache == null);
		if (connectionConvertedException != null)
		{
			throw connectionConvertedException;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			try
			{
				if (CommunicationObject != null)
				{
					CommunicationObject.Close();
				}
			}
			catch (Exception)
			{
				CommunicationObject.Abort();
			}
			IsInitialized = false;
		}
		_disposed = true;
	}

	protected void AddMessageHeader<T>(string ns, string name, T content)
	{
		if (_extraHeaders.ContainsKey(name))
		{
			_extraHeaders[name] = BuildMessageHeader(ns, name, content);
		}
		else
		{
			_extraHeaders.Add(name, BuildMessageHeader(ns, name, content));
		}
	}

	private static MessageHeader BuildMessageHeader<T>(string ns, string name, T content)
	{
		return new MessageHeader<T>(content).GetUntypedHeader(name, ns);
	}
}
