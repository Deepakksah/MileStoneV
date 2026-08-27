using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries.AccessControl;

public class AccessControlCredentialHolder
{
	public string Name { get; set; }

	public byte[] PictureData { get; set; }

	public List<string> CredentialHolderRoles { get; set; }

	public Dictionary<string, string> Properties { get; set; }
}
