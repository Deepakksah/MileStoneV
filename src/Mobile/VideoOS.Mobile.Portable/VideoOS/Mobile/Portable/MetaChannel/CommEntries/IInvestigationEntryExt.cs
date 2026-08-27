using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public interface IInvestigationEntryExt : IInvestigationEntry
{
	string FilePath { get; }

	string Username { get; }

	string UserSid { get; }

	string FullUserName { get; }

	Guid ServerId { get; }
}
