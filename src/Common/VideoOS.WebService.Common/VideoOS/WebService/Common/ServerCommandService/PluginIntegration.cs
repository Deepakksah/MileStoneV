using System;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public sealed class PluginIntegration
{
	[DataMember]
	public Guid IntegrationId { get; set; }

	[DataMember]
	public string IntegrationVersion { get; set; }

	[DataMember]
	public string IntegrationName { get; set; }

	[DataMember]
	public string ManufacturerName { get; set; }

	[DataMember]
	public bool AllowIntegrationRegistration { get; set; }
}
