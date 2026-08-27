using System.Collections.Specialized;
using System.Runtime.Serialization;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Configuration.Section;

[DataContract]
public class CredentialsConfig : IConfigSection
{
	public const string NameEncrypted = "Encrypted";

	public const string NameUserName = "Username";

	public const string NamePassWord = "Password";

	public const string NameAuthType = "Authtype";

	public const string NamePlainTextAuthentication = "PlainTextAuthenticationEnabled";

	[DataMember]
	public bool Encrypted { get; set; } = true;

	[DataMember]
	public string UserName { get; set; } = "";

	[DataMember]
	public string Password { get; set; } = "";

	[DataMember]
	public string AuthType { get; private set; } = "Negotiate";

	[DataMember]
	public bool PlainTextAuthenticationEnabled { get; private set; }

	public string SectionName => "Credentials";

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			Encrypted = section["Encrypted"].Sanitized().IsTrue();
		}
		catch
		{
			result = false;
		}
		try
		{
			UserName = (Encrypted ? section["Username"].Sanitized("'+/=") : section["Username"]);
		}
		catch
		{
		}
		try
		{
			Password = (Encrypted ? section["Password"].Sanitized("'+/=") : section["Password"]);
		}
		catch
		{
		}
		try
		{
			AuthType = section["Authtype"].Sanitized();
		}
		catch
		{
		}
		try
		{
			PlainTextAuthenticationEnabled = section["PlainTextAuthenticationEnabled"].Sanitized().IsTrue();
		}
		catch
		{
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		return true;
	}
}
