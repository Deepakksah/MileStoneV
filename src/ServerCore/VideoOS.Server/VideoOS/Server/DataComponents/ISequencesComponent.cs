using System;

namespace VideoOS.Server.DataComponents;

public interface ISequencesComponent : IDisposable
{
	long LatestIdpEventLogId();

	void UpdateIdpEventLogId(long newId);
}
