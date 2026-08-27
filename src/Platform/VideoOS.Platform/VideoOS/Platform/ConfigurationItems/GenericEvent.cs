using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class GenericEvent : IConfigurationItem
{
	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

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

	public string ShadowId => base.Wrapper.GetPropertyValue("ShadowId");

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

	public GenericEvent(ServerId serverId, ConfigurationItem genericEvent)
		: base(serverId, genericEvent)
	{
	}

	public GenericEvent(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public GenericEvent(FQID fqid)
		: base(fqid)
	{
	}
}
