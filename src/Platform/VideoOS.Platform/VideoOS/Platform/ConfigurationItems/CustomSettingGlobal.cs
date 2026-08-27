using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class CustomSettingGlobal : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

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

	public string DataXml
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataXml");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataXml", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int DataVersion
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DataVersion");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string LastModifiedUser => base.Wrapper.GetPropertyValue("LastModifiedUser");

	public CustomSettingGlobal(ServerId serverId, ConfigurationItem customSettingGlobal)
		: base(serverId, customSettingGlobal)
	{
	}

	public CustomSettingGlobal(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public CustomSettingGlobal(FQID fqid)
		: base(fqid)
	{
	}
}
