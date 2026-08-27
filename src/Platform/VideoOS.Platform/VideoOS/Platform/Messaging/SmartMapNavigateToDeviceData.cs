using System;

namespace VideoOS.Platform.Messaging;

public class SmartMapNavigateToDeviceData
{
	public Guid UniqueSmartMapIdentifier { get; set; }

	public FQID Source { get; set; }
}
