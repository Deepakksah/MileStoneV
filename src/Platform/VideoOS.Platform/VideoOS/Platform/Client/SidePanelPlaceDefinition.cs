using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Client;

public class SidePanelPlaceDefinition
{
	public Guid WorkSpaceId { get; set; }

	public List<WorkSpaceState> WorkSpaceStates { get; set; }
}
