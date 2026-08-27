using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class GetSaveSearchServerTask : ServerTask
{
	public SaveSearches Item => new SaveSearches(base.Wrapper.ServerId, base.Wrapper.ConfigurationItem);

	public GetSaveSearchServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}
}
