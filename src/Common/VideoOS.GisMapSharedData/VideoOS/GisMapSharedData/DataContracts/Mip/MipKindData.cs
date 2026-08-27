using System;
using VideoOS.GisMapSharedData.DataContracts.Attributes;
using VideoOS.GisMapSharedData.DataContracts.Interfaces;

namespace VideoOS.GisMapSharedData.DataContracts.Mip;

[ClassDataProperty("MipKindData")]
public class MipKindData : IUnknownDataProperties, IUnknownInstance
{
	[DataProperty]
	public Guid Id { get; set; }

	[DataProperty]
	public Guid ParentKind { get; set; }

	[DataProperty]
	public string Name { get; set; }

	[DataProperty]
	public byte[] Icon { get; set; }

	public DataProperty[] UnknownData { get; set; }

	IUnknownClassData IUnknownInstance.UnknownInstance => new UnknownMipKindData();
}
