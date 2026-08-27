using System.Runtime.Serialization;

namespace VideoOS.ConfigurationAPI;

[DataContract]
public class ValidateResult
{
	[DataMember]
	public bool ValidatedOk { get; set; }

	[DataMember]
	public ConfigurationItem ResultItem { get; set; }

	[DataMember]
	public ErrorResult[] ErrorResults { get; set; }

	public ValidateResult Ok => new ValidateResult
	{
		ValidatedOk = true
	};

	public ValidateResult()
	{
	}

	public ValidateResult(ConfigurationItem resultItem)
	{
		ResultItem = resultItem;
		ValidatedOk = true;
	}
}
