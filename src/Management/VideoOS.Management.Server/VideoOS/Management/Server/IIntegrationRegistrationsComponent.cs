using System;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public interface IIntegrationRegistrationsComponent : IDisposable
{
	void CreateIntegrationRegistration(string instanceId, Guid integrationId, IntegrationType integrationType, string integrationVersion, string integrationName, string manufacturerName, bool allowIntegrationRegistration);
}
