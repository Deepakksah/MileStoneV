using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlDoor
{
	public Guid Id { get; set; }

	public Guid AccessControlSystemId { get; set; }

	public string Name { get; set; }

	public string AccessControlSystemName { get; set; }

	public string Type { get; set; }

	public List<string> Categories { get; set; }

	public List<Tuple<Guid, string>> RelatedCameras { get; set; }

	public List<AccessControlCommandBase> Commands { get; set; }
}
