using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class CredentialSelectionCollection : Collection<CredentialSelection>
{
	public void Add(bool selected, string userName, string password)
	{
		Add(new CredentialSelection(selected, new Credential(userName, password)));
	}
}
