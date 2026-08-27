using System.Diagnostics.CodeAnalysis;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class QueryUsersWithAccessServerTask : ServerTask
{
	public string QueryUsersWithAccessPath
	{
		get
		{
			return base.Wrapper.GetPropertyValue("QueryUsersWithAccessPath");
		}
		set
		{
			base.Wrapper.SetPropertyValue("QueryUsersWithAccessPath", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool IncludeAdministrators
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IncludeAdministrators");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("IncludeAdministrators", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ExpandGroups
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ExpandGroups");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExpandGroups", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public bool IncludeGroups
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("IncludeGroups");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("IncludeGroups", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public QueryUsersWithAccessServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("QueryUsersWithAccess"));
	}
}
