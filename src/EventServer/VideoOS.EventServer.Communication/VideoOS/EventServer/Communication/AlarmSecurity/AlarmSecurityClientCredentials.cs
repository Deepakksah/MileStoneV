using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.EventServer.Communication.AlarmSecurity.Client;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class AlarmSecurityClientCredentials : AlarmSecurityClient
{
	private readonly ClientPool<VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient> _clientPool;

	public AlarmSecurityClientCredentials(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/AlarmSecurityService"))
	{
		_clientPool = new ClientPool<VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient>(CreateClient, 2);
	}

	private VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient CreateClient()
	{
		VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient alarmSecurityClient = new VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient(BindingFactory.CreateWSHttpBindingCredentials(_uri.IsHttps()), new EndpointAddress(identity: AbstractClient.CreateSpnIdentity(SpnFactory.GetSpn(_uri)), uri: _uri, addressHeaders: Array.Empty<AddressHeader>()));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		alarmSecurityClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(alarmSecurityClient);
		alarmSecurityClient.EnsureLargeObjectGraphsAllowed<VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient, IAlarmSecurity>();
		return alarmSecurityClient;
	}

	public override Identity[] GetUsersPermittedByObject(string objectId)
	{
		return _clientPool.CallWithRetry((VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient client) => client.GetUsersPermittedByObject(objectId));
	}

	public override Identity[] GetAllUsers()
	{
		return _clientPool.CallWithRetry((VideoOS.EventServer.Communication.AlarmSecurity.Client.AlarmSecurityClient client) => client.GetAllUsers());
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
