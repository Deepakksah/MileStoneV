using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetUserDefinedCategoriesResp", Namespace = "urn:milestone-systems")]
public class SetUserDefinedCategoriesResp : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
