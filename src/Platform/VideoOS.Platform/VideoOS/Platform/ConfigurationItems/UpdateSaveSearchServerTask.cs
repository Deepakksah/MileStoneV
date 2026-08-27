using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

public class UpdateSaveSearchServerTask : ServerTask
{
	public SaveSearches Item => new SaveSearches(base.Wrapper.ServerId, base.Wrapper.ConfigurationItem);

	public UpdateSaveSearchServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}
}
