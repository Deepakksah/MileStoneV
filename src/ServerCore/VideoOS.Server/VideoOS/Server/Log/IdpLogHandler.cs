using System;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.IdentityServer.Common.Model.Responses;
using VideoOS.Management.VmoClient;
using VideoOS.Server.CommandHandler;

namespace VideoOS.Server.Log;

public class IdpLogHandler : IIdpLogHandler
{
	private readonly IVmoClient _vmoClient;

	private readonly IServerEnvironment _serverEnvironment;

	public IdpLogHandler(IVmoClient vmoClient, IServerEnvironment serverEnvironment)
	{
		_vmoClient = vmoClient ?? throw new ArgumentNullException("vmoClient");
		_serverEnvironment = serverEnvironment ?? throw new ArgumentNullException("serverEnvironment");
	}

	public void HandleEvent()
	{
	}

	public void Publish(EventLogModel[] idpEvents)
	{
		UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector configurationChangedEventCollector = new UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector(_serverEnvironment.Server.HomeSiteId, _serverEnvironment.Server.EVSEventConsumer);
		foreach (EventLogModel eventLogModel in idpEvents)
		{
			string text = eventLogModel.Event;
			if (!(text == "UserCreated"))
			{
				if (text == "UserDeleted")
				{
					string resourceName = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", "[BASIC]", eventLogModel.ObjectName);
					configurationChangedEventCollector.AddEvent(eventLogModel.ObjectId, resourceName, ChangeAction.Deleted, "BasicUser");
					_vmoClient.ManagementServer.SecuritySection.RemoveIdentity(eventLogModel.ObjectId);
				}
			}
			else
			{
				string resourceName = string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", "[BASIC]", eventLogModel.ObjectName);
				configurationChangedEventCollector.AddEvent(eventLogModel.ObjectId, resourceName, ChangeAction.Added, "BasicUser");
			}
		}
		configurationChangedEventCollector.FireConfigChangedEvent();
	}
}
