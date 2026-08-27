using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public sealed class StreamInfo
{
	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. Hwid: {1}, Name: {2}, LiveFeed: {3}, LiveDefault: {4}, LiveRunAlways: {5}, IsPrimaryTrack: {6}, HasTrack: {7}", "StreamInfo", Hwid?.Text ?? "", Name, LiveFeed, LiveDefault, LiveRunAlways, IsPrimaryTrack, TrackDefinition.HasValue));

	public Guid StreamReferenceId { get; set; }

	public StreamHwid Hwid { get; set; }

	public string Name { get; set; }

	public bool LiveFeed { get; set; }

	public bool LiveDefault { get; set; }

	public bool LiveRunAlways { get; set; }

	public Guid? TrackDefinition { get; set; }

	public bool IsPrimaryTrack
	{
		get
		{
			if (TrackDefinition.HasValue)
			{
				return TrackDefinition.Value == WellKnownDeviceTrackDefinition.Primary;
			}
			return false;
		}
	}

	public StreamInfo(Guid streamReferenceId, StreamHwid streamHwid, string name, bool liveFeed, bool liveDefault, bool liveRunAlways, Guid? trackDefinition)
	{
		StreamReferenceId = streamReferenceId;
		Hwid = streamHwid;
		Name = name;
		LiveFeed = liveFeed;
		LiveDefault = liveDefault;
		LiveRunAlways = liveRunAlways;
		TrackDefinition = trackDefinition;
	}

	public StreamInfo()
	{
		StreamReferenceId = Guid.Empty;
		Hwid = new StreamHwid();
		Name = string.Empty;
		LiveFeed = false;
		LiveDefault = false;
		LiveRunAlways = false;
		TrackDefinition = null;
	}

	public StreamInfo(StreamInfo stream)
	{
		if (stream == null)
		{
			throw new ArgumentNullException("stream");
		}
		StreamReferenceId = stream.StreamReferenceId;
		Hwid = stream.Hwid;
		Name = stream.Name;
		LiveFeed = stream.LiveFeed;
		LiveDefault = stream.LiveDefault;
		LiveRunAlways = stream.LiveRunAlways;
		TrackDefinition = stream.TrackDefinition;
	}
}
