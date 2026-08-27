using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;

namespace VideoOS.Platform.AccessControl.Plugin;

public sealed class ACCredentialHolder
{
	public string Id { get; private set; }

	public string Name { get; private set; }

	public IEnumerable<string> CredentialHolderRoles { get; private set; }

	public Image Picture { get; private set; }

	public IEnumerable<ACProperty> Properties { get; private set; }

	public ACCredentialHolder(string id, string name, IEnumerable<string> credentialHolderRoles, Image picture, IEnumerable<ACProperty> properties)
	{
		Id = id;
		Name = name;
		CredentialHolderRoles = new ReadOnlyCollection<string>((credentialHolderRoles != null) ? credentialHolderRoles.ToList() : new List<string>());
		Picture = picture;
		Properties = new ReadOnlyCollection<ACProperty>((properties != null) ? properties.ToList() : new List<ACProperty>());
	}
}
