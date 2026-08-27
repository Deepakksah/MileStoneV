using System;
using System.Net;
using System.ServiceModel;
using Microsoft.Extensions.Logging;
using VideoOS.Event.Server.WebService.Action.Client;
using VideoOS.Management.Common;

namespace VideoOS.Server.MIP;

public sealed class ActionClientCredentials : AbstractEventServerClient, IDisposable
{
	private ActionCommandClient _client;

	private bool _disposed;

	public override CommunicationState State
	{
		get
		{
			if (_client == null)
			{
				return CommunicationState.Closed;
			}
			return _client.State;
		}
	}

	public ActionClientCredentials()
	{
		CreateClient();
	}

	public void ExecuteAction(ActionExecutionParams actionParameters)
	{
		Retry(delegate
		{
			if (_client != null)
			{
				_client.ExecuteAction(actionParameters);
			}
		});
	}

	public void RuleConfigurationChanged()
	{
		Retry(delegate
		{
			if (_client != null)
			{
				_client.RuleConfigurationChanged();
			}
		});
	}

	private ActionCommandClient CreateClient()
	{
		Uri eventServerAddress = base.EventServerAddress;
		if (eventServerAddress != null)
		{
			_client = CreateClient(eventServerAddress);
			Server.Instance.Logger.LogInformation("ActionCommandClient created: {Uri}", eventServerAddress.AbsoluteUri);
			return _client;
		}
		return null;
	}

	protected override Uri ClientAddress(Uri eventServerAddress)
	{
		UriBuilder uriBuilder = new UriBuilder(eventServerAddress);
		uriBuilder.Path = "Central/ActionService";
		return uriBuilder.Uri;
	}

	public static ActionCommandClient CreateClient(Uri eventServerAddress)
	{
		WSHttpBinding binding = BindingFactory.CreateWSHttpBindingCredentials(eventServerAddress.Scheme == Uri.UriSchemeHttps);
		string spn = SpnFactory.GetSpn(eventServerAddress);
		ActionCommandClient actionCommandClient = new ActionCommandClient(binding, new EndpointAddress(eventServerAddress, EndpointIdentity.CreateSpnIdentity(spn)));
		actionCommandClient.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
		return actionCommandClient;
	}

	protected override void RecreateClient()
	{
		CreateClient();
	}

	protected override void CloseClient()
	{
		AbstractEventServerClient.ShutdownClient(_client);
		_client = null;
	}

	private void Dispose(bool disposing)
	{
		if (!_disposed && disposing)
		{
			try
			{
				if (State != CommunicationState.Faulted)
				{
					_client.Close();
				}
			}
			finally
			{
				if (State != CommunicationState.Closed)
				{
					_client.Abort();
				}
			}
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
