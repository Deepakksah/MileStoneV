using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class FindSaveSearchesServerTask : ServerTask
{
	public FindSaveSearches Item => new FindSaveSearches(base.Wrapper.ServerId, base.Wrapper.ConfigurationItem);

	public FindSaveSearchesServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}
}
