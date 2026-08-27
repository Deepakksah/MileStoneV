using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RegisteredClaim : IConfigurationItem
{
	public int Id
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Id");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public bool CaseSensitive
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("CaseSensitive");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public RegisteredClaim(ServerId serverId, ConfigurationItem registeredClaim)
		: base(serverId, registeredClaim)
	{
	}

	public RegisteredClaim(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RegisteredClaim(FQID fqid)
		: base(fqid)
	{
	}
}
