using System;

namespace VideoOS.IM.Server.Service.Licenses.DTOs;

public sealed class LicenseInstalledProduct
{
	public string DisplayName { get; set; } = string.Empty;

	public string ExpirationDate { get; set; } = DateTime.MinValue.ToShortDateString();
}
