using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Mobile.Configuration.Section;

namespace VideoOS.Mobile.Configuration.Group;

[DataContract]
public class ManagementServerConfigGroup : IConfigGroup
{
	[DataMember]
	public AddressConfig Address { get; private set; }

	[DataMember]
	public CredentialsConfig Credentials { get; private set; }

	public TimeoutConfig ConnectionTimeout { get; private set; }

	public TimeoutConfig SuccessLoginTimeout { get; private set; }

	public TimeoutConfig FailLoginTimeout { get; private set; }

	public LoginSettingsConfig LoginSettings { get; private set; }

	public List<IConfigSection> Items { get; private set; }

	public string GroupName => "ManagementServer";

	public bool IsSecured => Address.Scheme.Equals(Uri.UriSchemeHttps, StringComparison.CurrentCultureIgnoreCase);

	public ManagementServerConfigGroup()
	{
		Address = new AddressConfig();
		Credentials = new CredentialsConfig();
		ConnectionTimeout = new TimeoutConfig(new TimeSpan(0, 0, 2), "ConnectionTimeout");
		SuccessLoginTimeout = new TimeoutConfig(new TimeSpan(0, 0, 2), "SuccessLoginTimeout");
		FailLoginTimeout = new TimeoutConfig(new TimeSpan(0, 0, 2), "FailLoginTimeout");
		LoginSettings = new LoginSettingsConfig();
		Items = new List<IConfigSection> { Address, Credentials, ConnectionTimeout, SuccessLoginTimeout, FailLoginTimeout, LoginSettings };
	}
}
