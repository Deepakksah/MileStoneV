using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.DataCollectorService;

[DataContract(Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Management.Common")]
public class CounterStateRulesConfigurationUpdate
{
	[DataMember]
	public bool Deleted { get; set; }

	[DataMember]
	public CounterStateRuleConfiguration RuleConfig { get; set; }
}
