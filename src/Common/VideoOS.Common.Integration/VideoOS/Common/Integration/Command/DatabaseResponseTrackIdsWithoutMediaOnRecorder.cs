using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class DatabaseResponseTrackIdsWithoutMediaOnRecorder : DatabaseResponse
{
	public Guid[] TrackIds { get; private set; }

	public DatabaseResponseTrackIdsWithoutMediaOnRecorder(Guid[] trackIds)
		: base(DatabaseOperation.FilterTrackIdsToThoseWithoutMediaOnRecorder)
	{
		TrackIds = trackIds;
	}

	public DatabaseResponseTrackIdsWithoutMediaOnRecorder(DatabaseResponseStatus status, string message)
		: base(DatabaseOperation.FilterTrackIdsToThoseWithoutMediaOnRecorder, status, message)
	{
	}

	public DatabaseResponseTrackIdsWithoutMediaOnRecorder(Guid[] trackIds, DatabaseResponseStatus status, string message)
		: base(DatabaseOperation.FilterTrackIdsToThoseWithoutMediaOnRecorder, status, message)
	{
		TrackIds = trackIds;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
