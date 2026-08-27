using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class HardwarePasswordConfigData
{
	public Guid HardwareId { get; set; }

	public string MacAddress { get; set; }

	public string Username { get; set; }

	public string PasswordRequirements { get; set; }

	public int? MinPasswordLength { get; set; }

	public int? MaxPasswordLength { get; set; }
}
