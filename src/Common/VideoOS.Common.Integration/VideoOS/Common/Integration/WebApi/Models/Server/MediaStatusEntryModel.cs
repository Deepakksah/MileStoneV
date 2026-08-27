using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class MediaStatusEntryModel
{
	public Guid DeviceId { get; set; }

	public DateTime MediaRetentionTime { get; set; }
}
