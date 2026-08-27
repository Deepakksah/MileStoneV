using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Unsupported;

[ClassDataProperty("UnsupportedData")]
public class UnsupportedData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	public string Name { get; set; }

	[DataProperty]
	public string DataType { get; set; }

	public DataProperty[] UnknownData { get; set; }

	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownUnsupportedData();
}
