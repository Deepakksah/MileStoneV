using System;

namespace VideoOS.Management.Common;

public static class StreamPropertyName
{
	public const string StreamReferenceId = "StreamReferenceId";

	public const string LiveMode = "LiveMode";

	public const string LiveDefault = "LiveDefault";

	public const string RecordTo = "RecordTo";

	public const string UseEdge = "UseEdge";

	public const string DefaultPlayback = "DefaultPlayback";

	public const string UpdateUseMediaAfter = "UpdateUseMediaAfter";

	public const string EdgeTrackId = "EdgeTrackId";

	[Obsolete("Used last in version 23.1", false)]
	public const string Record = "Record";
}
