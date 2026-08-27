using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MoveFailoverGroupServerTask : ServerTask
{
	public string FailoverServer
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FailoverServer");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FailoverServer", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string FailoverGroup
	{
		get
		{
			return base.Wrapper.GetPropertyValue("FailoverGroup");
		}
		set
		{
			base.Wrapper.SetPropertyValue("FailoverGroup", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Position
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Position");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Position", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public MoveFailoverGroupServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("MoveFailoverGroup"));
	}
}
