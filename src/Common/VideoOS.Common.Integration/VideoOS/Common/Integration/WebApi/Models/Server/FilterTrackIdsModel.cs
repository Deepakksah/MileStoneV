using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class FilterTrackIdsModel
{
	[RequiredNonemptyGuid]
	public Guid TargetRecorderId { get; set; }

	public IEnumerable<Guid> TrackIds { get; set; }
}
