using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddRegisteredClaimServerTask : ServerTask
{
	public string Name
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Name");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Name", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string DisplayName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DisplayName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DisplayName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool CaseSensitive
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CaseSensitive");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("CaseSensitive", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public AddRegisteredClaimServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddRegisteredClaim"));
	}
}
