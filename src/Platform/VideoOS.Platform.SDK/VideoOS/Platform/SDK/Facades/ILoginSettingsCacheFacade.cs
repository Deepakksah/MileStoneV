using System;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.SDK.Facades;

public interface ILoginSettingsCacheFacade
{
	void Clear();

	bool Contains(LoginSettings ls);

	LoginSettings GetLoginSettings(Guid serverGuid);

	bool Remove(LoginSettings ls);
}
