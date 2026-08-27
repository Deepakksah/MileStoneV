using System;
using VideoOS.IM.Server.Repository.EntityFramework.Properties;

namespace VideoOS.IM.Server.Service.ConfigSettings;

public interface IConfigSettingsService
{
	[NotNull]
	Guid ClientRegistrationId { get; }

	[NotNull]
	Uri AuthorizationServerUri { get; }
}
