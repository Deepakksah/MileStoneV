using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class AuthorizationServerAddress : ServerAddressBase
{
	[DataMember(Order = 0)]
	public string Path { get; set; }
}
