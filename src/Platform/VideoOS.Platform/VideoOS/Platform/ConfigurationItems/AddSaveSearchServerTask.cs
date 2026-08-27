using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class AddSaveSearchServerTask : ServerTask
{
	public SaveSearches Item => new SaveSearches(base.Wrapper.ServerId, base.Wrapper.ConfigurationItem);

	public AddSaveSearchServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}
}
