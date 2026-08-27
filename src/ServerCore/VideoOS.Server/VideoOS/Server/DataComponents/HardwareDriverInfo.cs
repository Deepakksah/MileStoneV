using System;

namespace VideoOS.Server.DataComponents;

public sealed class HardwareDriverInfo
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public int Number { get; set; }

	public string GroupName { get; set; }

	public int UseCount { get; set; }

	public string DriverVersion { get; set; }
}
