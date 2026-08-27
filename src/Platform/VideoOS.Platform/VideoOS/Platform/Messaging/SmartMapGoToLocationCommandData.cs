using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SmartMapGoToLocationCommandData
{
	public FQID Location { get; set; }

	public FQID Window { get; set; }

	public int Index { get; set; }
}
