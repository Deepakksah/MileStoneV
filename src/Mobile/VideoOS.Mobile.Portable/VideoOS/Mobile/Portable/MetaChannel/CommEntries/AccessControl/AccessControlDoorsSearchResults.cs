using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlDoorsSearchResults
{
	public IEnumerable<AccessControlDoor> AccessControlDoors { get; set; }

	public int AccessControlSystemsCount { get; set; }
}
