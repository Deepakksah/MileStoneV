using System;

namespace VideoOS.Server.Licensing;

public sealed class RecorderHardwareRelation
{
	public string Mac { get; private set; }

	public Guid RecorderId { get; private set; }

	public RecorderHardwareRelation(string mac, Guid recorderId)
	{
		Mac = mac;
		RecorderId = recorderId;
	}
}
