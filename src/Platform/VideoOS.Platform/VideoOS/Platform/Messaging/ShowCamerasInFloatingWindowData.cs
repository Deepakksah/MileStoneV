using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Messaging;

public class ShowCamerasInFloatingWindowData
{
	public IEnumerable<Item> Cameras { get; set; }

	public Mode Mode { get; set; }

	public DateTime BrowseTime { get; set; }
}
