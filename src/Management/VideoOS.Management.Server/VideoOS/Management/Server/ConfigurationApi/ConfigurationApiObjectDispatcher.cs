using System;
using System.Reflection;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ConfigurationApiObjectDispatcher
{
	public static IConfigurationApiSupport GetObject(string itemType)
	{
		Type entityClassTypeNoAssert = EntityNameAttributeHandlerWrapper.GetEntityClassTypeNoAssert("ConfigAPI" + itemType);
		if (entityClassTypeNoAssert == null)
		{
			throw new NotSupportedException("No generic business object registered on server with entity name: " + itemType);
		}
		object[] args = new object[0];
		return (IConfigurationApiSupport)Activator.CreateInstance(entityClassTypeNoAssert, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, args, null);
	}
}
