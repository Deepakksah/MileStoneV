using System;
using System.ServiceModel;
using VideoOS.EventServer.Communication.AlarmSecurity.Client;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class AlarmSecurityClientToken : AlarmSecurityClient
{
	private readonly ClientPool<AlarmSecurityTokenClient> _clientPool;

	public AlarmSecurityClientToken(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/AlarmSecurityServiceToken"))
	{
		_clientPool = new ClientPool<AlarmSecurityTokenClient>(CreateClient, 2);
	}

	private AlarmSecurityTokenClient CreateClient()
	{
		AlarmSecurityTokenClient alarmSecurityTokenClient = new AlarmSecurityTokenClient(BindingFactory.CreateBasicHttpBindingToken(_uri.IsHttps()), new EndpointAddress(_uri));
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(alarmSecurityTokenClient);
		alarmSecurityTokenClient.EnsureLargeObjectGraphsAllowed<AlarmSecurityTokenClient, IAlarmSecurityToken>();
		return alarmSecurityTokenClient;
	}

	private string GetToken()
	{
		return (LoginSettingsCache.GetLoginSettings(base.ServerId) ?? throw new LoginSettingsNotFoundException(base.ServerId)).Token;
	}

	public override Identity[] GetUsersPermittedByObject(string objectId)
	{
		return _clientPool.CallWithRetry((AlarmSecurityTokenClient client) => client.GetUsersPermittedByObject(GetToken(), objectId));
	}

	public override Identity[] GetAllUsers()
	{
		return _clientPool.CallWithRetry((AlarmSecurityTokenClient client) => client.GetAllUsers(GetToken()));
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
