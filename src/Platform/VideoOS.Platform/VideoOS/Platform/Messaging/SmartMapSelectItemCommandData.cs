using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SmartMapSelectItemCommandData
{
	public FQID Item { get; set; }

	public FQID Window { get; set; }

	public int Index { get; set; }
}
