using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "CustomPropertyInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class CustomProperty
{
	[DataMember]
	public string ObjectId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Value { get; set; }

	public static CustomProperty[] GetCustomProperties(IGeminiRequestContext requestContext, string objectId)
	{
		return GetCustomPropertiesNoSecurity(requestContext, objectId);
	}

	public static CustomProperty[] GetCustomPropertiesByServerGroupHierarchyNoSecurity(IGeminiRequestContext requestContext, string serverGroupId)
	{
		using ICustomPropertyComponent customPropertyComponent = requestContext.CreateCustomPropertyComponent();
		return customPropertyComponent.GetCustomPropertiesByServerGroupHierarchy(serverGroupId);
	}

	public static CustomProperty[] GetCustomPropertiesNoSecurity(IGeminiRequestContext requestContext, string objectId)
	{
		using ICustomPropertyComponent customPropertyComponent = requestContext.CreateCustomPropertyComponent();
		return customPropertyComponent.GetCustomProperties(objectId);
	}

	public static CustomProperty[] GetCustomPropertiesByNameNoSecurity(IGeminiRequestContext requestContext, string name)
	{
		using ICustomPropertyComponent customPropertyComponent = requestContext.CreateCustomPropertyComponent();
		return customPropertyComponent.GetCustomPropertiesByName(name);
	}

	public static CustomProperty GetCustomProperty(IGeminiRequestContext requestContext, string objectId, string name)
	{
		using ICustomPropertyComponent customPropertyComponent = requestContext.CreateCustomPropertyComponent();
		return customPropertyComponent.GetCustomProperty(objectId, name);
	}

	public static void UpdateCustomProperties(IGeminiRequestContext requestContext, string objectId, CustomProperty[] customPropertyArray)
	{
		using ICustomPropertyComponent customPropertyComponent = requestContext.CreateCustomPropertyComponent();
		customPropertyComponent.UpdateCustomProperties(objectId, customPropertyArray);
	}

	public static void DeleteCustomProperty(IGeminiRequestContext requestContext, string objectId, string name)
	{
		using ICustomPropertyComponent customPropertyComponent = requestContext.CreateCustomPropertyComponent();
		customPropertyComponent.DeleteCustomProperties(objectId, name);
	}
}
