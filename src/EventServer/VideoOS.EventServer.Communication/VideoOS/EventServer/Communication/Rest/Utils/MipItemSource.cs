using System;

namespace VideoOS.EventServer.Communication.Rest.Utils;

public class MipItemSource
{
	public ParentType ParentType { get; set; }

	public Guid ParentId { get; set; }

	public Guid MipItemId { get; set; }
}
