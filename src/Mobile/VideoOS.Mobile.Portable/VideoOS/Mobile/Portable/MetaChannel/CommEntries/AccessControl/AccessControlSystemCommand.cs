using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlSystemCommand : AccessControlCommandBase
{
	public Guid EventId { get; set; }

	public Guid TriggerSourceObjectId { get; set; }

	public Guid KindId { get; set; }

	public AccessControlSystemCommand()
	{
		base.CommandType = AccessControlCommandType.SystemCommand;
	}
}
