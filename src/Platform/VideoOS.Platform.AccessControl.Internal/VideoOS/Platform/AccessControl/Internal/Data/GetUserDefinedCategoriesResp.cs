using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "GetUserDefinedCategoriesResp", Namespace = "urn:milestone-systems")]
public class GetUserDefinedCategoriesResp : IExtensibleDataObject
{
	[DataMember]
	public AccessControlUserDefinedCategory[] AccessControlUserDefinedCategories { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
