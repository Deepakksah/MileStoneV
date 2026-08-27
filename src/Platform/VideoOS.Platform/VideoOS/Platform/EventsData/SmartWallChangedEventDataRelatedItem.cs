using System;
using System.Collections.Generic;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.EventsData;

public class SmartWallChangedEventDataRelatedItem
{
	public Guid Id { get; set; }

	public Guid ParentId { get; set; }

	public FolderType FolderType { get; set; }

	public Guid Kind { get; set; }

	public IEnumerable<ViewItemInformation> ViewItemInformation { get; set; }
}
