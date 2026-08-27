using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Common.Integration;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class TrackConfiguration
{
	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. {1}", "TrackConfiguration", (DefinitionId == WellKnownDeviceTrackDefinition.Primary) ? "Primary" : ((DefinitionId == WellKnownDeviceTrackDefinition.Edge) ? "Edge" : ((DefinitionId == WellKnownDeviceTrackDefinition.Secondary) ? "Secondary" : DefinitionId.ToString())))) + FormattableString.Invariant(FormattableStringFactory.Create("{0}{1}", DefaultPlayback ? ", DefaultPlayback" : "", MergeEdge ? ", MergeEdge" : ""));

	public Guid DefinitionId { get; set; }

	public bool DefaultPlayback { get; set; }

	public bool MergeEdge { get; set; }

	public TrackConfiguration(Guid definitionId, bool defaultPlayback, bool mergeEdge)
	{
		DefinitionId = definitionId;
		DefaultPlayback = defaultPlayback;
		MergeEdge = mergeEdge;
	}
}
