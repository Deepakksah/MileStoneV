using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ApplyViewItemsXmlServerTask : ServerTask
{
	public string ViewItemsXml
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewItemsXml");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewItemsXml", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int BeginPosition
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BeginPosition");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("BeginPosition", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public ApplyViewItemsXmlServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("ApplyViewItemsXml"));
	}
}
