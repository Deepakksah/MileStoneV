using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetUserDefinedcategoriesParams", Namespace = "urn:milestone-systems")]
public class GetUserDefinedCategoriesParams : IExtensibleDataObject
{
	public ExtensionDataObject ExtensionData { get; set; }
}
