using System.Collections.ObjectModel;

namespace VideoOS.Common.Integration.DeviceDiscovery;

public class CredentialCollection : Collection<Credential>
{
	public void Add(string password)
	{
		Add(new Credential(password));
	}

	public void Add(string userName, string password)
	{
		Add(new Credential(userName, password));
	}
}
