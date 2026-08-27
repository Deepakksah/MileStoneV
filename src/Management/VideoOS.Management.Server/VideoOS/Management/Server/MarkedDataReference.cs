using System;

namespace VideoOS.Management.Server;

public sealed class MarkedDataReference
{
	public string Reference { get; set; }

	public DateTime Time { get; set; }

	public string[] Devices { get; set; }
}
