using System.Runtime.Serialization;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Unsupported;

namespace VideoOS.GisMapSharedData.DataContracts;

[DataContract(Name = "UnsupportedRespBase", Namespace = "http://videoos.net/2/GisMapCommand")]
public class UnsupportedRespBase : ParamsRespBase
{
	[DataProperty]
	public UnsupportedData[] UnsupportedData { get; set; }
}
