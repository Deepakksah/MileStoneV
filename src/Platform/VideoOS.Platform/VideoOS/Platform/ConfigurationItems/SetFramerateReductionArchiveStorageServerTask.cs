using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class SetFramerateReductionArchiveStorageServerTask : ServerTask
{
	public bool FramerateReductionEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("FramerateReductionEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("FramerateReductionEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public SetFramerateReductionArchiveStorageServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("SetFramerateReductionArchiveStorage"));
	}
}
