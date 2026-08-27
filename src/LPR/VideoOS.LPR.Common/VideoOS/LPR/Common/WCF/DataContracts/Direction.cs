using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public enum Direction
{
	[EnumMember]
	Ascending,
	[EnumMember]
	Descending
}
