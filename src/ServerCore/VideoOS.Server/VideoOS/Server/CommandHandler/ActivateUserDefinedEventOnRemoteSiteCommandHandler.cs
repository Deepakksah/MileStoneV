using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server.CommandHandler;

public sealed class ActivateUserDefinedEventOnRemoteSiteCommandHandler : ICommandHandler
{
	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is ActivateUserDefinedEventOnRemoteSiteCommand activateUserDefinedEventOnRemoteSiteCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		IServerConnection serverConnection = null;
		IVmoClient vmoClient = null;
		Guid siteId = activateUserDefinedEventOnRemoteSiteCommand.SiteId;
		string userDefinedEventNameOrId = activateUserDefinedEventOnRemoteSiteCommand.UserDefinedEventNameOrId;
		try
		{
			serverConnection = new ServerConnection(ServerConfig.Instance.VmoCommunicationConfiguration.ServerName);
			serverConnection.ServerPort = ServerConfig.Instance.VmoCommunicationConfiguration.Port;
			serverConnection.Connect();
			vmoClient = new VmoClient(serverConnection);
			if (vmoClient.ManagementServer.Id != siteId || siteId == Guid.Empty)
			{
				ISite site = vmoClient.Repositories.GetObjectById<ChildSite>(siteId, refreshCachedObjects: false);
				if (site == null && vmoClient.ManagementServer.MasterSite != null && siteId == vmoClient.ManagementServer.MasterSite.Id)
				{
					site = vmoClient.ManagementServer.MasterSite;
				}
				if (site == null)
				{
					Server.Instance.Logger.LogWarning("Unable to find remote site on execute ActivateUserDefinedEventOnRemoteSite command. Site: {SiteId}, Event: {Event}", siteId, userDefinedEventNameOrId);
					return;
				}
				using SiteTokenCacheStore siteTokenCacheStore = new SiteTokenCacheStore();
				ISiteConnectorFactory siteConnectorFactory = new SiteConnectorFactory(siteTokenCacheStore, new ClientTokenSettings("GrantValidatorClient"));
				ISiteConnector siteConnector = siteConnectorFactory.Create();
				if (vmoClient != null)
				{
					vmoClient.Dispose();
					vmoClient = null;
				}
				ISiteConnectionResult siteConnection = siteConnector.GetSiteConnection(new SiteConnectionInfoWindows(site, allowUnsecure: true, forceLegacy: true));
				if (siteConnection.IsSuccess)
				{
					vmoClient = siteConnection.VmoClient;
				}
			}
			Guid guid = Guid.Empty;
			try
			{
				guid = new Guid(userDefinedEventNameOrId);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			UserDefinedEvent userDefinedEvent = null;
			userDefinedEvent = ((!(guid != Guid.Empty)) ? vmoClient.ManagementServer.UserDefinedEvents.FirstOrDefault((UserDefinedEvent f) => f.Name == userDefinedEventNameOrId) : vmoClient.ManagementServer.UserDefinedEvents[guid]);
			if (userDefinedEvent != null)
			{
				userDefinedEvent.Trigger();
			}
			else
			{
				Server.Instance.Logger.LogWarning("Unable to find event on remote site on execute ActivateUserDefinedEventOnRemoteSite command. Site: {SiteId}, Event: {Event}", siteId.ToString(), userDefinedEventNameOrId);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			Server.Instance.Logger.LogWarning("Failed to execute ActivateUserDefinedEventOnRemoteSite command. Error: {ExceptionMessage}", ex3.Message);
		}
		finally
		{
			serverConnection?.Dispose();
			vmoClient?.Dispose();
		}
		responseSink.AddResponse(new SuccessResponse());
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
