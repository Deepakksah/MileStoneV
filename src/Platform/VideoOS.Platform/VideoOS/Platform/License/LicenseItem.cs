using System;

namespace VideoOS.Platform.License;

public class LicenseItem
{
	public string Id { get; set; }

	public string Name { get; set; }

	public bool ItemTrial { get; set; }

	public DateTime ItemTrialEnd { get; set; }
}
