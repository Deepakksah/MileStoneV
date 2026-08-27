using System;

namespace VideoOS.Server.DataComponents;

public sealed class SystemBuilderLicenseRequest
{
	public string Name { get; set; }

	public string Sku { get; set; }

	public Guid PartnerGuid { get; set; }
}
