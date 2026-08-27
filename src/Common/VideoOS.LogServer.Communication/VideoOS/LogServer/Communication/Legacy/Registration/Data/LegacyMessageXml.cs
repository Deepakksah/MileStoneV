using System.Collections.Generic;
using VideoOS.LogServer.Communication.Legacy.Registration.Data.Serialization;

namespace VideoOS.LogServer.Communication.Legacy.Registration.Data;

public class LegacyMessageXml
{
	public IEnumerable<Entry> Entries { get; set; }

	public string Version { get; set; }

	public string Culture { get; set; }

	public IEnumerable<LegacyLogMessageEnumEntry> Categories { get; set; }

	public IEnumerable<LegacyLogMessageEnumEntry> SourceTypes { get; set; }
}
