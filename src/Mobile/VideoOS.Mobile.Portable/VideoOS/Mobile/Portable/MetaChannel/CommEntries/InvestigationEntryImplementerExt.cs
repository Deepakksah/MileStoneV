using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class InvestigationEntryImplementerExt : InvestigationEntryImplementer, IInvestigationEntryExt, IInvestigationEntry
{
	public string FilePath { get; set; }

	public string Username { get; set; }

	public string UserSid { get; set; }

	public string FullUserName { get; set; }

	public Guid ServerId { get; set; }
}
