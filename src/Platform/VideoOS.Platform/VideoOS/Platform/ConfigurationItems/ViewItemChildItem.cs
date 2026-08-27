using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ViewItemChildItem : IConfigurationChildItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public int ViewItemPosition
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ViewItemPosition");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string ViewItemDefinitionXml
	{
		get
		{
			return base.Wrapper.GetPropertyValue("ViewItemDefinitionXml");
		}
		set
		{
			base.Wrapper.SetPropertyValue("ViewItemDefinitionXml", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ViewItemChildItem(ServerId serverId, ConfigurationItem viewItemChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, viewItemChildItem, parentWrapper)
	{
	}

	public ViewItemChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ViewItemChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
