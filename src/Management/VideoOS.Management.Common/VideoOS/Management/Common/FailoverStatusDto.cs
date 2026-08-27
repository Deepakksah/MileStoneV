using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[Serializable]
[DebuggerDisplay("FailoverStatusDto, Status: {Status}, ConnectStatus: {ConnectStatus}, WatchingState: {WatchingState}, FailingRecorderId: {FailingRecorderId}")]
[DataContract]
public sealed class FailoverStatusDto : IEquatable<FailoverStatusDto>
{
	[DataMember]
	public string Status { get; set; }

	[DataMember]
	public string ConnectStatus { get; set; }

	[DataMember]
	public string WatchingState { get; set; }

	[DataMember]
	public RecorderWatchItemDto[] RecorderWatchItems { get; set; }

	[DataMember]
	public CommunicationErrors LocalRecorderCommunicationErrors { get; set; }

	[DataMember]
	public bool LocalRecorderStartupError { get; set; }

	[DataMember]
	public string FailingRecorderHostName { get; set; }

	[DataMember]
	public int FailingRecorderHostPort { get; set; }

	[DataMember]
	public Guid FailingRecorderId { get; set; }

	public bool Equals(FailoverStatusDto other)
	{
		if (other == null)
		{
			return false;
		}
		if (Status != other.Status || ConnectStatus != other.ConnectStatus || WatchingState != other.WatchingState || FailingRecorderHostName != other.FailingRecorderHostName || FailingRecorderHostPort != other.FailingRecorderHostPort || FailingRecorderId != other.FailingRecorderId || LocalRecorderCommunicationErrors != other.LocalRecorderCommunicationErrors || LocalRecorderStartupError != other.LocalRecorderStartupError)
		{
			return false;
		}
		if (RecorderWatchItems == null && other.RecorderWatchItems == null)
		{
			return true;
		}
		if (RecorderWatchItems == null || other.RecorderWatchItems == null)
		{
			return false;
		}
		if (RecorderWatchItems.Length != other.RecorderWatchItems.Length)
		{
			return false;
		}
		for (int i = 0; i < RecorderWatchItems.Length; i++)
		{
			if (RecorderWatchItems[i] != null || other.RecorderWatchItems[i] != null)
			{
				if (RecorderWatchItems[i] == null || other.RecorderWatchItems[i] == null)
				{
					return false;
				}
				if (!RecorderWatchItems[i].Equals(other.RecorderWatchItems[i]))
				{
					return false;
				}
			}
		}
		return true;
	}

	public override string ToString()
	{
		return FormattableString.Invariant($"Status: {Status}, ConnectStatus: {ConnectStatus}, WatchingState: {WatchingState}, FailingRecorderId: {FailingRecorderId}, LocalRecorderCommunicationErrors: {LocalRecorderCommunicationErrors}");
	}
}
