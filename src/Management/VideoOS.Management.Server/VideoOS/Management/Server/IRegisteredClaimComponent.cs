using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public interface IRegisteredClaimComponent : IDisposable
{
	RegisteredClaimData Create(Guid loginProviderId, string name, string displayName, bool caseSensitive);

	int[] QueryIdsByLoginProvider(Guid loginProviderId);

	RegisteredClaimData QueryById(int id);

	RegisteredClaimData[] QueryByIds(IEnumerable<int> ids);

	RegisteredClaimData[] QueryAll();

	void Update(int id, string name, string displayName);

	void Delete(int id, Guid sessionId);
}
