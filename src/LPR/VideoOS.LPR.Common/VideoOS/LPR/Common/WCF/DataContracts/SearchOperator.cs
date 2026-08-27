using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public enum SearchOperator
{
	[EnumMember]
	Equals,
	[EnumMember]
	NotEquals,
	[EnumMember]
	LessThan,
	[EnumMember]
	GreaterThan,
	[EnumMember]
	Contains,
	[EnumMember]
	BeginsWith
}
