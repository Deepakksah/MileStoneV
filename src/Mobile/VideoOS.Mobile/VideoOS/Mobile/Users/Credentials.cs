using System.Xml.Serialization;

namespace VideoOS.Mobile.Users;

public class Credentials : ICredentials
{
	[XmlAttribute("User")]
	public string User { get; set; }

	[XmlAttribute("Password")]
	public string Password { get; set; }

	public Credentials()
	{
	}

	public Credentials(string user, string password)
		: this()
	{
		User = user;
		Password = password;
	}
}
