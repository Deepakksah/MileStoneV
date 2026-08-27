using System;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class CertificateGroupData : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public Guid Id { get; set; }

	[DataMember(Order = 1)]
	public bool UseEncryption { get; set; }

	[XmlElement(IsNullable = false)]
	[DataMember(Order = 2, EmitDefaultValue = false)]
	public SelectedCertificate SelectedCertificate { get; set; }

	[DataMember(Order = 3)]
	public string PresentationImplementationType { get; set; }

	[DataMember(Order = 4, EmitDefaultValue = false)]
	public string PresentationImplementationAssemblyFile { get; set; }

	[DataMember(Order = 5)]
	public string TitleKey { get; set; }

	[DataMember(Order = 6)]
	public string SubtitleKey { get; set; }

	[XmlArray]
	[DataMember(Order = 7)]
	public FilterStringCollection FilterStrings { get; set; } = new FilterStringCollection();
}
