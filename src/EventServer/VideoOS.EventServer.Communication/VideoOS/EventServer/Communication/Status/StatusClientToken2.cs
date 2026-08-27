using System;
using System.ServiceModel;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.EventServer.Communication.Status.Client;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Status;

public class StatusClientToken2 : StatusClient
{
	private readonly ClientPool<StatusCommandToken2Client> _clientPool;

	public StatusClientToken2(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/StatusServiceToken2"))
	{
		_clientPool = new ClientPool<StatusCommandToken2Client>(CreateClient, 2);
	}

	private StatusCommandToken2Client CreateClient()
	{
		StatusCommandToken2Client statusCommandToken2Client = new StatusCommandToken2Client(BindingFactory.CreateBasicHttpBindingToken(_uri.IsHttps()), new EndpointAddress(_uri));
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(statusCommandToken2Client);
		statusCommandToken2Client.EnsureLargeObjectGraphsAllowed<StatusCommandToken2Client, IStatusCommandToken2>();
		return statusCommandToken2Client;
	}

	private string GetToken()
	{
		return (LoginSettingsCache.GetLoginSettings(base.ServerId) ?? throw new LoginSettingsNotFoundException(base.ServerId)).Token;
	}

	public override MapObjectState[] GetStatus(string id)
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.GetStatus(GetToken(), id));
	}

	public override MapObjectState[] GetStatusTranslated(string id, string language)
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.GetStatusTranslated(GetToken(), id, language));
	}

	public override MapObjectState[] GetStatusByUserIds(string token, string id, MapUserId[] userIds)
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.GetStatusByUserIds(GetToken(), id, userIds));
	}

	public override MapObjectStateTreeNode[] GetStatusForAllObjectsOfType(string id, Guid type, Guid relationType, int maxLevels)
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.GetStatusForAllObjectsOfType(GetToken(), id, type, relationType, maxLevels));
	}

	public override string GetStatusDetails(string id, Guid type, string language)
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.GetStatusDetails(GetToken(), id, type, language));
	}

	public override DateTime SetVisualization(VisualizationState visualizationState)
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.SetVisualization(GetToken(), visualizationState));
	}

	public override VisualizationState GetVisualization()
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.GetVisualization(GetToken()));
	}

	public override VisualizationState GetVisualizationChanges(DateTime timestamp, int timeout)
	{
		return _clientPool.CallWithRetry((StatusCommandToken2Client client) => client.GetVisualizationChanges(GetToken(), timestamp, timeout));
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
