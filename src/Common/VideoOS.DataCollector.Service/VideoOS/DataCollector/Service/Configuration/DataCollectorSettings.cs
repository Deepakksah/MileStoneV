using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.DataCollector.Service.Configuration;

public class DataCollectorSettings
{
	[Required]
	public string ManagementServerAddress { get; set; }

	[Required]
	public Uri AuthorizationServerUri { get; set; }

	[Required]
	public Guid? ClientRegistrationId { get; set; }

	public string Urls { get; set; }
}
