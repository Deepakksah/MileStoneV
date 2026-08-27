using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[DataContract(Namespace = "")]
public class ExtensibleDataObject : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
