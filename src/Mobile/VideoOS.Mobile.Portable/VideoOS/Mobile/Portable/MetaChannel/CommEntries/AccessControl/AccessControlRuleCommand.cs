using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlRuleCommand : AccessControlCommandBase
{
	public Guid AccessControlOperationalInstanceId { get; set; }

	public Guid AccessControlCommandTypeId { get; set; }

	public int Order { get; set; }

	public AccessControlRuleCommand()
	{
		base.CommandType = AccessControlCommandType.RuleCommand;
	}
}
