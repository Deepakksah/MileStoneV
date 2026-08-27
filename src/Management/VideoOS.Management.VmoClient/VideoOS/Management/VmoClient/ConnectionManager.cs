using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.ServiceModel;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public abstract class ConnectionManager : ConnectionSettings, IDisposable
{
	private Dictionary<Type, ProxyClientBase> proxyList = new Dictionary<Type, ProxyClientBase>();

	private bool _isUserConnected;

	private bool _disposed;

	private string _logOnName;

	private ServerVersion _serverVersion;

	public ServerVersion ServerVersion
	{
		get
		{
			if (_serverVersion == null)
			{
				UpdateServerVersion();
			}
			return _serverVersion;
		}
		public set
		{
			_serverVersion = value;
		}
	}

	public string LogOnName => _logOnName;

	public bool IsOpen
	{
		get
		{
			if (proxyList.Count > 0)
			{
				foreach (Type key in proxyList.Keys)
				{
					ProxyClientBase proxyClientBase = proxyList[key];
					if (proxyClientBase.CommunicationObject != null)
					{
						return proxyClientBase.CommunicationObject.State == CommunicationState.Opened;
					}
				}
			}
			return false;
		}
	}

	protected ConnectionManager()
	{
		InitializeDefaults();
	}

	private void InitializeDefaults()
	{
		_isUserConnected = false;
		_serverVersion = null;
	}

	public void RegisterClientProxy<T>(T proxyClientBase) where T : ProxyClientBase
	{
		proxyList.Add(proxyClientBase.GetType(), proxyClientBase);
	}

	public T GetClientProxy<T>() where T : ProxyClientBase
	{
		T val = (T)proxyList[typeof(T)];
		if (!val.IsInitialized)
		{
			val.Initialize();
			base.BlockUpdates = true;
		}
		return val;
	}

	public void Connect()
	{
		UpdateServerVersion();
		if (_isUserConnected)
		{
			_isUserConnected = true;
		}
		else
		{
			InternalConnect();
			base.BlockUpdates = true;
		}
		_isUserConnected = true;
	}

	private void InternalConnect()
	{
		foreach (ProxyClientBase value in proxyList.Values)
		{
			if (!value.IsInitialized)
			{
				value.Initialize();
			}
			if (value.CommunicationObject.State == CommunicationState.Created)
			{
				value.CommunicationObject.Open();
			}
		}
		ServerProxy clientProxy = GetClientProxy<ServerProxy>();
		_logOnName = clientProxy.CheckAuthentication();
		_isUserConnected = true;
	}

	public void Disconnect()
	{
		if (IsOpen)
		{
			foreach (ProxyClientBase value in proxyList.Values)
			{
				value.Dispose();
			}
		}
		_isUserConnected = false;
		_logOnName = string.Empty;
	}

	public void RefreshConnectionData()
	{
		UpdateServerVersion();
	}

	private void UpdateServerVersion()
	{
		if (base.TokenCache != null)
		{
			UpdateServerVersionOAuth(base.TokenCache);
		}
		else
		{
			UpdateServerVersionLegacy();
		}
	}

	private void UpdateServerVersionOAuth(ITokenCache tokenCache)
	{
		ChannelFactory<IServerService> channelFactory = null;
		try
		{
			Binding binding = GetBinding();
			Uri serviceUri = GetServiceUri("ServerServiceOAuth.svc", "serverservice", "Url");
			Uri serviceUri2 = GetServiceUri("ServerServiceOAuth.svc", "serverservice", "Urn");
			Version version = null;
			Exception ex = null;
			try
			{
				EndpointAddress remoteAddress = new EndpointAddress(serviceUri);
				channelFactory = new ChannelFactory<IServerService>(binding, remoteAddress);
				channelFactory.Endpoint.AddBearerTokenBehavior(tokenCache);
				version = channelFactory.CreateChannel().GetServerVersion();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				ex = ex3;
			}
			if (ex is EndpointNotFoundException)
			{
				ex = null;
				try
				{
					EndpointAddress remoteAddress2 = new EndpointAddress(serviceUri2);
					channelFactory = new ChannelFactory<IServerService>(binding, remoteAddress2);
					channelFactory.Endpoint.Behaviors.Add(new ClientViaBehavior(serviceUri));
					channelFactory.Endpoint.AddBearerTokenBehavior(tokenCache);
					version = channelFactory.CreateChannel().GetServerVersion();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex5)
				{
					ex = ex5;
				}
			}
			if (ex == null)
			{
				_serverVersion = new ServerVersion(version.Major, version.Minor, version.Revision);
				return;
			}
			Exception connectionConvertedException = ExceptionConverter.GetConnectionConvertedException(ex, base.ServerName, base.ServerPort);
			if (connectionConvertedException != null)
			{
				throw connectionConvertedException;
			}
			throw new VmoException(ex.Message, ex);
		}
		finally
		{
			if (channelFactory != null)
			{
				try
				{
					channelFactory.Close();
				}
				catch (OutOfMemoryException)
				{
				}
				catch (Exception)
				{
					channelFactory.Abort();
				}
			}
		}
	}

	private void UpdateServerVersionLegacy()
	{
		ChannelFactory<IServerService> channelFactory = null;
		try
		{
			Binding binding = GetBinding();
			NetworkCredential networkCredential = GetNetworkCredential();
			Uri serviceUri = GetServiceUri("ServerService.svc", "serverservice", "Url");
			Uri serviceUri2 = GetServiceUri("ServerService.svc", "serverservice", "Urn");
			string spn = SpnFactory.GetSpn(serviceUri);
			Version version = null;
			Exception ex = null;
			try
			{
				EndpointIdentity identity = EndpointIdentity.CreateSpnIdentity(spn);
				EndpointAddress remoteAddress = new EndpointAddress(serviceUri, identity);
				channelFactory = new ChannelFactory<IServerService>(binding, remoteAddress);
				if (base.IsWindowsLogin)
				{
					channelFactory.Credentials.Windows.ClientCredential = networkCredential;
				}
				else
				{
					channelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
					{
						CertificateValidationMode = X509CertificateValidationMode.None
					};
					channelFactory.Credentials.UserName.UserName = networkCredential.UserName;
					channelFactory.Credentials.UserName.Password = networkCredential.Password;
				}
				channelFactory.Credentials.SupportInteractive = false;
				version = channelFactory.CreateChannel().GetServerVersion();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				ex = ex3;
			}
			if (ex is EndpointNotFoundException)
			{
				ex = null;
				try
				{
					EndpointIdentity identity2 = EndpointIdentity.CreateSpnIdentity(spn);
					EndpointAddress remoteAddress2 = new EndpointAddress(serviceUri2, identity2);
					channelFactory = new ChannelFactory<IServerService>(binding, remoteAddress2);
					channelFactory.Endpoint.Behaviors.Add(new ClientViaBehavior(serviceUri));
					if (base.IsWindowsLogin)
					{
						channelFactory.Credentials.Windows.ClientCredential = networkCredential;
					}
					else
					{
						channelFactory.Credentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication
						{
							CertificateValidationMode = X509CertificateValidationMode.None
						};
						channelFactory.Credentials.UserName.UserName = networkCredential.UserName;
						channelFactory.Credentials.UserName.Password = networkCredential.Password;
					}
					channelFactory.Credentials.SupportInteractive = false;
					version = channelFactory.CreateChannel().GetServerVersion();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex5)
				{
					ex = ex5;
				}
			}
			if (ex == null)
			{
				_serverVersion = new ServerVersion(version.Major, version.Minor, version.Revision);
				return;
			}
			Exception connectionConvertedException = ExceptionConverter.GetConnectionConvertedException(ex, base.ServerName, base.ServerPort, legacy: true);
			if (connectionConvertedException != null)
			{
				throw connectionConvertedException;
			}
			throw new VmoException(ex.Message, ex);
		}
		finally
		{
			if (channelFactory != null)
			{
				try
				{
					channelFactory.Close();
				}
				catch (OutOfMemoryException)
				{
				}
				catch (Exception)
				{
					channelFactory.Abort();
				}
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				Disconnect();
			}
			_disposed = true;
		}
	}
}
