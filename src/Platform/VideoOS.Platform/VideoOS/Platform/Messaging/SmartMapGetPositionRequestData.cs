using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SmartMapGetPositionRequestData
{
	public FQID Window { get; set; }

	public int Index { get; set; }
}
