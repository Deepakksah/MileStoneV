using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class HardwareConfigData
{
	public Guid RecorderId { get; set; }

	public Guid HardwareId { get; set; }

	public string Name { get; set; }

	public string MacAddress { get; set; }

	public string Username { get; set; }
}
