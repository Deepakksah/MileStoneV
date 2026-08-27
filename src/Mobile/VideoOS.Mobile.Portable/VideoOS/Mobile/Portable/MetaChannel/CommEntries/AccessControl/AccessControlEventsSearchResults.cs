using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlEventsSearchResults
{
	public IEnumerable<AccessControlEventDetails> AccessControlEvents { get; set; }

	public int AccessControlEventsCount { get; set; }

	public int AccessControlSystemsCount { get; set; }
}
