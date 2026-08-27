using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class AddGenericEventDataSourceServerTask : ServerTask
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

	public int DataSourcePort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DataSourcePort");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourcePort", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string DataSourceSeparator
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataSourceSeparator");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceSeparator", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int DataSourceEncoding
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DataSourceEncoding");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceEncoding", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public bool DataSourceLog
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("DataSourceLog");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceLog", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> DataSourceEchoValues => base.Wrapper.GetEnumValues("DataSourceEcho");

	public string DataSourceEcho
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataSourceEcho");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceEcho", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string DataSourceAllowed
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataSourceAllowed");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceAllowed", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string DataSourceAllowed6
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataSourceAllowed6");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceAllowed6", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> DataSourceProtocolValues => base.Wrapper.GetEnumValues("DataSourceProtocol");

	public string DataSourceProtocol
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataSourceProtocol");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceProtocol", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public Dictionary<string, string> DataSourceAddressFamilyValues => base.Wrapper.GetEnumValues("DataSourceAddressFamily");

	public string DataSourceAddressFamily
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DataSourceAddressFamily");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DataSourceAddressFamily", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public AddGenericEventDataSourceServerTask(ConfigurationApiItemWrapper paramWrapper)
		: base(paramWrapper)
	{
	}

	public ServerTask Execute()
	{
		return new ServerTask(base.Wrapper.ExecuteMethod("AddGenericEventDataSource"));
	}
}
