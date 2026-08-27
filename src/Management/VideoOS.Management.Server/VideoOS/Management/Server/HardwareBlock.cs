using System;

namespace VideoOS.Management.Server;

public sealed class HardwareBlock
{
	public int Id { get; set; }

	public string HardwareId { get; set; }

	public DateTime Expires { get; set; }

	public string Owner { get; set; }

	public string Data { get; set; }
}
