using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class UpdateHardwareReadOnlySettingsModel : CommandModelBase
{
	public Guid HardwareId { get; set; }

	public string UpdatedSettingsXml { get; set; }
}
