using System.Runtime.Serialization;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "SetUserDefinedcategoriesParams", Namespace = "urn:milestone-systems")]
public class SetUserDefinedCategoriesParams : IExtensibleDataObject
{
	[DataMember]
	public AccessControlUserDefinedCategory[] AccessControlUserDefinedCategories { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
