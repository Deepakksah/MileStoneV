using System;
using System.Collections.Generic;
using System.Net;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.IdentityServer.Common.Handlers.IdentityServerToken;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server.CertificateRegistration;

public class VmoSystemSettingClient : IVmoSystemSettingClient, IDisposable
{
	private const int IdpDiscoveryTimeout = 3;

	private VmoClient _client;

	private SimpleTokenCache _tokenCache;

	private bool disposedValue;

	public IEnumerable<Address> ManagementServerAddresses => _client.ManagementServer.Addresses;

	public VmoSystemSettingClient(Uri serverAddress, Uri identityProvider, NetworkCredential networkCredential)
	{
		if (serverAddress == null)
		{
			throw new ArgumentNullException("serverAddress");
		}
		IdpDiscoveryType idpDiscoveryType = (identityProvider.Scheme.Equals(Uri.UriSchemeHttps) ? IdpDiscoveryType.Secure : IdpDiscoveryType.Unsecure);
		ServerConnection serverConnection;
		if (new IdpServerValidation(new HttpGet(), 3, idpDiscoveryType, null).ValidateIdpServerAsync(identityProvider).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
			.GetResult()
			.IsOAuthSupported)
		{
			IdentityServerWindowsTokenHandler tokenHandler = new IdentityServerWindowsTokenHandler(networkCredential, new IdentityServerClientSettings(identityProvider, "GrantValidatorClient"));
			_tokenCache = new SimpleTokenCache(tokenHandler);
			serverConnection = new ServerConnection(serverAddress.Host, serverAddress.Scheme == Uri.UriSchemeHttps, _tokenCache);
		}
		else
		{
			serverConnection = (string.IsNullOrEmpty(networkCredential?.UserName) ? new ServerConnection(serverAddress.Host) : new ServerConnection(serverAddress.Host, networkCredential.UserName, networkCredential.Password));
			serverConnection.NetworkProtocol = (serverAddress.Scheme.Equals(Uri.UriSchemeHttps) ? NetworkProtocol.Https : NetworkProtocol.Http);
		}
		serverConnection.ServerPort = serverAddress.Port;
		_client = new VmoClient(serverConnection);
	}

	public void Connect()
	{
		_client.ServerConnection.Connect();
	}

	public void Disconnect()
	{
		_client.ServerConnection?.Disconnect();
	}

	public string GetSystemSetting(string name)
	{
		return _client.ManagementServer.GetSystemSetting(name);
	}

	public void SetSystemSetting(string name, string value)
	{
		_client.ManagementServer.SetSystemSetting(name, value);
	}

	public void RemoveSiteAddresses(IEnumerable<Address> addresses)
	{
		if (addresses == null)
		{
			throw new ArgumentNullException("addresses");
		}
		bool flag = false;
		foreach (Address address in addresses)
		{
			flag = _client.ManagementServer.Addresses.Remove(address) || flag;
		}
		if (flag)
		{
			_client.ManagementServer.Update();
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposedValue)
		{
			return;
		}
		if (disposing)
		{
			if (_client != null)
			{
				_client.Dispose();
			}
			if (_tokenCache != null)
			{
				_tokenCache.Dispose();
			}
		}
		disposedValue = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
