using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class TrackStatusEntryModel
{
	public Guid TrackId { get; set; }

	public DateTime MediaTime { get; set; }
}
