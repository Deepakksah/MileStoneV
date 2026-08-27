using System;

namespace VideoOS.Server.CommandHandler;

public interface IVmoCache
{
	string GetTimeProfileName(Guid profileId);
}
