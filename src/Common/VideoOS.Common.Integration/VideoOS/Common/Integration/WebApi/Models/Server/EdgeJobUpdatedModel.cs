using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class EdgeJobUpdatedModel : CommandModelBase
{
	public Guid DeviceId { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }
}
