using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts;

public class UnknownParamRespProperty : ParamsRespBase, IUnknownClassData
{
	public string UnknownTypeName { get; set; }
}
