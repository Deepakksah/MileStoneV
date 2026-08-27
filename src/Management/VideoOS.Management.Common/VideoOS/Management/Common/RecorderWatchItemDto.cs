using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[Serializable]
[DebuggerDisplay("RecorderWatchItemDto, Id: {RecorderId}; Position: {Position}; Errors: {Errors}")]
[DataContract]
public sealed class RecorderWatchItemDto : IEquatable<RecorderWatchItemDto>
{
	[DataMember]
	public Guid RecorderId { get; set; }

	[DataMember]
	public string RecorderEndPoint { get; set; }

	[DataMember]
	public int Position { get; set; }

	[DataMember]
	public CommunicationErrors Errors { get; set; }

	public bool Equals(RecorderWatchItemDto other)
	{
		if (other == null)
		{
			return false;
		}
		if (RecorderId == other.RecorderId && RecorderEndPoint == other.RecorderEndPoint && Position == other.Position)
		{
			return Errors == other.Errors;
		}
		return false;
	}
}
