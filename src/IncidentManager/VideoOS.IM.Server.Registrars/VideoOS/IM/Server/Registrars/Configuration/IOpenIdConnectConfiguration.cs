using System;

namespace VideoOS.IM.Server.Registrars.Configuration;

public interface IOpenIdConnectConfiguration
{
	Uri AuthorizationServerAddress { get; set; }
}
