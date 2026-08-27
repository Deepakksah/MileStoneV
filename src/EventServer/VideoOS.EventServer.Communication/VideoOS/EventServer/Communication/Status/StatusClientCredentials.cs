using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.EventServer.Communication.Status.Client;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Status;

public class StatusClientCredentials : StatusClient
{
	private readonly ClientPool<StatusCommandCredentialsClient> _clientPool;

	public StatusClientCredentials(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/StatusServiceCredentials"))
	{
		_clientPool = new ClientPool<StatusCommandCredentialsClient>(CreateClient, 2);
	}

	private StatusCommandCredentialsClient CreateClient()
	{
		StatusCommandCredentialsClient statusCommandCredentialsClient = new StatusCommandCredentialsClient(BindingFactory.CreateWSHttpBindingCredentials(_uri.IsHttps()), new EndpointAddress(identity: AbstractClient.CreateSpnIdentity(SpnFactory.GetSpn(_uri)), uri: _uri, addressHeaders: Array.Empty<AddressHeader>()));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		statusCommandCredentialsClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(statusCommandCredentialsClient);
		statusCommandCredentialsClient.EnsureLargeObjectGraphsAllowed<StatusCommandCredentialsClient, IStatusCommandCredentials>();
		return statusCommandCredentialsClient;
	}

	public override MapObjectState[] GetStatus(string id)
	{
		return _clientPool.CallWithRetry((StatusCommandCredentialsClient client) => client.GetStatus(id));
	}

	public override MapObjectState[] GetStatusTranslated(string id, string language)
	{
		return _clientPool.CallWithRetry((StatusCommandCredentialsClient client) => client.GetStatusTranslated(id, language));
	}

	public override MapObjectState[] GetStatusByUserIds(string token, string id, MapUserId[] userIds)
	{
		throw new NotImplementedException();
	}

	public override MapObjectStateTreeNode[] GetStatusForAllObjectsOfType(string id, Guid type, Guid relationType, int maxLevels)
	{
		return _clientPool.CallWithRetry((StatusCommandCredentialsClient client) => client.GetStatusForAllObjectsOfType(id, type, relationType, maxLevels));
	}

	public override string GetStatusDetails(string id, Guid type, string language)
	{
		return _clientPool.CallWithRetry((StatusCommandCredentialsClient client) => client.GetStatusDetails(id, type, language));
	}

	public override DateTime SetVisualization(VisualizationState visualizationState)
	{
		return _clientPool.CallWithRetry((StatusCommandCredentialsClient client) => client.SetVisualization(visualizationState));
	}

	public override VisualizationState GetVisualization()
	{
		return _clientPool.CallWithRetry((StatusCommandCredentialsClient client) => client.GetVisualization());
	}

	public override VisualizationState GetVisualizationChanges(DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((StatusCommandCredentialsClient client) => client.GetVisualizationChanges(timestamp, timeout));
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
