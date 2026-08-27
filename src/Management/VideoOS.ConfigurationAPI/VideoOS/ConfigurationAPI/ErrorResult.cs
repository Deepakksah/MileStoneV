using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

public class ErrorResult
{
	[DataMember]
	public string ErrorProperty { get; set; }

	[DataMember]
	public string ErrorTextId { get; set; }

	[DataMember]
	public string ErrorText { get; set; }
}
