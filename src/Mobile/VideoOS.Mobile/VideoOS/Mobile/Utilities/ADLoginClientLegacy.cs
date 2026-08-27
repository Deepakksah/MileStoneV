using System;
using System.Net;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Mobile.Utilities;

public class ADLoginClientLegacy : ILoginClient
{
	public Uri Url { get; set; }

	public NetworkCredential Credentials { get; set; }

	public void Login(Guid instanceId, string currentToken)
	{
		if (string.IsNullOrEmpty(Credentials.UserName))
		{
			using (IServerCommandServiceProxy serverCommandServiceProxy = ServerCommandServiceProxy.CreateNew(Url.Host, Url.Port))
			{
				serverCommandServiceProxy.Login(instanceId, currentToken);
				return;
			}
		}
		using IServerCommandServiceProxy serverCommandServiceProxy2 = ServerCommandServiceProxy.CreateNew(Url.Host, Url.Port, Credentials.UserName, Credentials.Password, Credentials.Domain);
		serverCommandServiceProxy2.Login(instanceId, currentToken);
	}
}
