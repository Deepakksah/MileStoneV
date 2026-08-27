using System;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Unsupported;

[ClassDataProperty("UnsupportedProperty")]
public class UnsupportedProperty : UnsupportedData
{
	[DataProperty]
	public Guid Id { get; set; }
}
