using System;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Internal.Util;

namespace VideoOS.Platform.AccessControl.Internal.Data;

[DataContract(Name = "AccessControlUserDefinedCategory", Namespace = "urn:milestone-systems")]
public class AccessControlUserDefinedCategory : IExtensibleDataObject
{
	[DataMember(Order = 1)]
	public Guid Id { get; private set; }

	[DataMember(Order = 2)]
	public string Name { get; private set; }

	[DataMember(Order = 3)]
	public string AccessControlTypeString { get; private set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public AccessControlUserDefinedCategory(UserDefinedCategory userDefinedCategory)
	{
		Id = userDefinedCategory.Id;
		Name = userDefinedCategory.Name;
		AccessControlTypeString = AccessControlTypeToStringConverter.TypeToString(userDefinedCategory.AccessControlType);
	}

	public Type GetAccessControlType()
	{
		return AccessControlTypeToStringConverter.TypeFromString(AccessControlTypeString);
	}

	public override string ToString()
	{
		return Name;
	}

	public override bool Equals(object obj)
	{
		if (obj is AccessControlUserDefinedCategory { Id: var id })
		{
			return id.Equals(Id);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
}
