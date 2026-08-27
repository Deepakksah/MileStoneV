using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
public class EnablePropertyInfo
{
	[DataMember]
	public string DisplayName { get; set; }

	[DataMember]
	public bool Enabled { get; set; }

	[DataMember]
	public bool UIToFollowEnabled { get; set; }

	[DataMember]
	public string TranslationId { get; set; }
}
