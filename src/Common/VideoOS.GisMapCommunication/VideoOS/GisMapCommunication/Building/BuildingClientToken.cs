using System;
using System.ServiceModel;
using VideoOS.GisMapCommunication.Building.Client;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication.Building;

public class BuildingClientToken : BuildingClientBase
{
	protected override SecurityMode CurrentSecurityMode
	{
		get
		{
			if (!(base.Uri.Scheme == Uri.UriSchemeHttps))
			{
				return SecurityMode.None;
			}
			return SecurityMode.Transport;
		}
	}

	protected override WSMessageEncoding CurrentMessageEncoding => WSMessageEncoding.Text;

	public BuildingClientToken(ServerId serverId, TimeSpan requestTimeout, Uri uri)
		: base(serverId, requestTimeout, uri)
	{
	}

	protected override GisBuildingCommandClient CreateClient()
	{
		GisBuildingCommandClient gisBuildingCommandClient = new GisBuildingCommandClient(CreateWSHttpBindingCredentials(), new EndpointAddress(base.FinalUri, CreateSpnIdentity(Name)));
		AddClientHeaderBehavior(gisBuildingCommandClient);
		AbstractClient<GisBuildingCommandClient, IGisBuildingCommand>.EnsureLargeObjectGraphsAllowed(gisBuildingCommandClient);
		return gisBuildingCommandClient;
	}
}
