using System;
using VideoOS.Platform.Metadata;

namespace VideoOS.Platform.Data;

public class MetadataPlaybackData
{
	public DateTime DateTime;

	public DateTime? NextDateTime;

	public DateTime? PreviousDateTime;

	public MetadataContent Content { get; set; }

	public virtual long SequenceNumber { get; set; }
}
