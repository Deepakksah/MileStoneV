using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ApplicationSettings : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public string UICulture { get; set; }
}
