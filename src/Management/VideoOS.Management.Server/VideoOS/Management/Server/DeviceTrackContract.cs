using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VideoOS.Common.Integration.Configuration;

namespace VideoOS.Management.Server;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class DeviceTrackContract
{
	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. {1}", "DeviceTrackContract", (TrackDefinitionId == WellKnownDeviceTrackDefinition.Primary) ? "Primary" : ((TrackDefinitionId == WellKnownDeviceTrackDefinition.Edge) ? "Edge" : ((TrackDefinitionId == WellKnownDeviceTrackDefinition.Secondary) ? "Secondary" : TrackDefinitionId.ToString())))) + FormattableString.Invariant(FormattableStringFactory.Create("{0}{1}{2}", DefaultPlayback ? ", DefaultPlayback" : "", MergeEdge ? ", MergeEdge" : "", InUse ? ", InUse" : ""));

	public string DeviceId { get; set; }

	public Guid TrackId { get; set; }

	public Guid TrackDefinitionId { get; set; }

	public bool DefaultPlayback { get; set; }

	public bool MergeEdge { get; set; }

	public bool InUse { get; set; }

	public DateTime? OnlyUseMediaAfter { get; set; }

	public DeviceTrackContract(string deviceId, Guid trackId, Guid trackDefinitionId, bool defaultPlayback, bool mergeEdge, bool inUse, DateTime? onlyUseMediaAfter)
	{
		DeviceId = deviceId;
		TrackId = trackId;
		TrackDefinitionId = trackDefinitionId;
		DefaultPlayback = defaultPlayback;
		MergeEdge = mergeEdge;
		InUse = inUse;
		OnlyUseMediaAfter = onlyUseMediaAfter;
	}
}
