using System;
using VideoOS.GisMapSharedData.DataContracts.Attributes;

namespace VideoOS.GisMapSharedData.DataContracts.Unsupported;

[ClassDataProperty("UnsupportedType")]
public class UnsupportedType : UnsupportedData
{
	[DataProperty]
	public Guid Id { get; set; }
}
