using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class HardwareDriver : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string GroupName => base.Wrapper.GetPropertyValue("GroupName");

	public int Number
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Number");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public int UseCount
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UseCount");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string DriverVersion => base.Wrapper.GetPropertyValue("DriverVersion");

	public string DriverRevision => base.Wrapper.GetPropertyValue("DriverRevision");

	public Dictionary<string, string> DriverTypeValues => base.Wrapper.GetEnumValues("DriverType");

	public string DriverType => base.Wrapper.GetPropertyValue("DriverType");

	public HardwareDriver(ServerId serverId, ConfigurationItem hardwareDriver)
		: base(serverId, hardwareDriver)
	{
	}

	public HardwareDriver(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public HardwareDriver(FQID fqid)
		: base(fqid)
	{
	}
}
