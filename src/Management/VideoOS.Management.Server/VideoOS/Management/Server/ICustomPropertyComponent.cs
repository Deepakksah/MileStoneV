using System;

namespace VideoOS.Management.Server;

public interface ICustomPropertyComponent : IDisposable
{
	CustomProperty[] GetCustomPropertiesByServerGroupHierarchy(string serverGroupId);

	CustomProperty[] GetCustomProperties(string objectId);

	CustomProperty[] GetCustomPropertiesByName(string name);

	CustomProperty GetCustomProperty(string objectId, string name);

	void UpdateCustomProperties(string objectId, CustomProperty[] customPropertyArray);

	void DeleteCustomProperties(string objectId, string name);
}
