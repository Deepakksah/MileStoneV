using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Client;

public class ViewItemToolbarPlaceDefinition
{
	public List<Guid> ViewItemIds { get; set; }

	public List<Guid> WorkSpaceIds { get; set; }

	public List<WorkSpaceState> WorkSpaceStates { get; set; }
}
