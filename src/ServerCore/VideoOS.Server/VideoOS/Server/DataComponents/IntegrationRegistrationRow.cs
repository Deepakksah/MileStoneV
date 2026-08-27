using System;

namespace VideoOS.Server.DataComponents;

public sealed class IntegrationRegistrationRow
{
	public Guid IntegrationId { get; set; }

	public string IntegrationName { get; set; }

	public string ManufacturerName { get; set; }

	public string IntegrationType { get; set; }

	public string IntegrationVersion { get; set; }

	public int VersionCount { get; set; }

	public DateTime RegistrationTime { get; set; }

	public bool AllowIntegrationRegistration { get; set; }
}
