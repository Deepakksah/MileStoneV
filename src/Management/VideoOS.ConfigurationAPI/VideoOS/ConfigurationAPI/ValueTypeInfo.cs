using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
public class ValueTypeInfo
{
	[DataMember]
	public string TranslationId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Value { get; set; }
}
