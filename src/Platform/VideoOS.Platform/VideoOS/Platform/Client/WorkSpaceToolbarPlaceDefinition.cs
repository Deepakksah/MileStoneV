using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Client;

public class WorkSpaceToolbarPlaceDefinition
{
	public List<Guid> WorkSpaceIds { get; set; }

	public List<WorkSpaceState> WorkSpaceStates { get; set; }
}
