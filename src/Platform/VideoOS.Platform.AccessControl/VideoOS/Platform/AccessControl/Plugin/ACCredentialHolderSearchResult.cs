using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACCredentialHolderSearchResult
{
	public string Id { get; private set; }

	public string Name { get; private set; }

	public IEnumerable<string> CredentialHolderRoles { get; private set; }

	public ACCredentialHolderSearchResult(string id, string name, IEnumerable<string> credentialHolderRoles)
	{
		Id = id;
		Name = name;
		CredentialHolderRoles = new ReadOnlyCollection<string>((credentialHolderRoles != null) ? credentialHolderRoles.ToList() : new List<string>());
	}
}
