using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class AddBookmarkModel : CommandModelBase
{
	public IEnumerable<Guid> DeviceIds { get; set; }

	public string Header { get; set; }

	public string Description { get; set; }

	public DateTime TimeTrigged { get; set; }

	public TimeSpan TimeBefore { get; set; }

	public TimeSpan TimeAfter { get; set; }
}
