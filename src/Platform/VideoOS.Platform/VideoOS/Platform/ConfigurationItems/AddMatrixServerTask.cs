using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddMatrixServerTask : ServerTask
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

	public string Description
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Description");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Description", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Address
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Address");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Address", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Port
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Port");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Port", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public SecureString SecurePassword
	{
		get
		{
			return base.Wrapper.GetPropertySecureValue("Password");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Password", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddMatrixServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddMatrix"));
	}
}
