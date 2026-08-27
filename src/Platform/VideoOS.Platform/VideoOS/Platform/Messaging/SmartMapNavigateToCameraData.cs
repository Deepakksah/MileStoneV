using System;

namespace VideoOS.Platform.Messaging;

public class SmartMapNavigateToCameraData
{
	public Guid UniqueSmartMapIdentifier { get; set; }

	public FQID Source { get; set; }
}
