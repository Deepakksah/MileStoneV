using System;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Map;

[ClassDataProperty("GisMapBrowseData")]
public class GisMapBrowseData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	public Guid Id { get; set; }

	[DataProperty]
	public string Name { get; set; }

	[DataProperty]
	public string Note { get; set; }

	public DataProperty[] UnknownData { get; set; }

	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownGisMapBrowseData();
}
