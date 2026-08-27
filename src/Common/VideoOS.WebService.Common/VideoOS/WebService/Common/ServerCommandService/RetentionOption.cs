using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class RetentionOption
{
	[DataMember]
	public int RetentionUnits { get; set; }

	[DataMember]
	public RetentionOptionType RetentionOptionType { get; set; }
}
