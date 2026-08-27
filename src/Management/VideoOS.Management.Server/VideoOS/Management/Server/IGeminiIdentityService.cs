using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IGeminiIdentityService
{
	void SynchronizeWindowsGroup(Identity identity);
}
