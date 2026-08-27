using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddGenericEventServerTask : ServerTask
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

	public Dictionary<string, string> DataSourceValues => base.Wrapper.GetEnumValues("DataSource");

	public string DataSource
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataSource");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSource", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Expression
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Expression");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Expression", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> ExpressionTypeValues => base.Wrapper.GetEnumValues("ExpressionType");

	public string ExpressionType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ExpressionType");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ExpressionType", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int Priority
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Priority");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("Priority", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public AddGenericEventServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddGenericEvent"));
	}
}
