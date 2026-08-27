using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MIPKind : IConfigurationItem
{
	public MIPItemFolder MIPItemFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MIPItemFolder")
				{
					return new MIPItemFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public new Guid Guid => Guid.Parse(Id);

	public string PluginId => base.Wrapper.GetPropertyValue("PluginId");

	public string PluginName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PluginName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PluginName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public long DataVersion
	{
		get
		{
			if (long.TryParse(base.Wrapper.GetPropertyValue("DataVersion"), out var result))
			{
				return result;
			}
			return 0L;
		}
	}

	public string KindType
	{
		get
		{
			return base.Wrapper.GetPropertyValue("KindType");
		}
		set
		{
			base.Wrapper.SetProperty("KindType", value);
		}
	}

	public bool DisplayOnGisMap
	{
		get
		{
			return bool.Parse(base.Wrapper.GetPropertyValue("DisplayOnGisMap"));
		}
		set
		{
			base.Wrapper.SetProperty("DisplayOnGisMap", value.ToString());
		}
	}

	public Guid ParentKindId
	{
		get
		{
			return Guid.Parse(base.Wrapper.GetPropertyValue("ParentKind"));
		}
		set
		{
			base.Wrapper.SetProperty("ParentKind", value.ToString());
		}
	}

	public string SecurityAction
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SecurityAction");
		}
		set
		{
			base.Wrapper.SetProperty("SecurityAction", value);
		}
	}

	public string SecurityActionNames
	{
		get
		{
			return base.Wrapper.GetPropertyValue("SecurityActionNames");
		}
		set
		{
			base.Wrapper.SetProperty("SecurityActionNames", value);
		}
	}

	public MIPKind(ServerId serverId, ConfigurationItem mipKind)
		: base(serverId, mipKind)
	{
	}

	public MIPKind(ServerId serverId, Guid kind)
		: base(serverId, string.Format(CultureInfo.InvariantCulture, "MIPKind[{0}]", kind))
	{
	}
}
