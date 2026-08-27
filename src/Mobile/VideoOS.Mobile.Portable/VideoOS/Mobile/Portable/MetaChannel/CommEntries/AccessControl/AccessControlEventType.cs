using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlEventType
{
	public Guid Id { get; set; }

	public Guid AccessControlSystemId { get; set; }

	public string Name { get; set; }
}
