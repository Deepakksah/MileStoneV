using System;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class RecorderServiceVersion
{
	public Guid ServiceId { get; set; }

	public Version Version { get; set; }
}
