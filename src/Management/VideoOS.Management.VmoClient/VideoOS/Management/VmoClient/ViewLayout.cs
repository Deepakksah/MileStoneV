using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ViewLayout")]
public class ViewLayout : VmoObjectGuidBased
{
	public string Name => GetPropertyValue<string>("Name");

	public string Description => GetPropertyValue<string>("Description");

	public string DefinitionXml => GetPropertyValue<string>("DefinitionXml");

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public ViewLayout(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void UpdateCustomProperties()
	{
		base.Repositories.ServerCommunication.SaveCustomProperties(base.InternalId, base.CustomProperties.DirtyCustomPropertyArray());
		CreateCustomPropertyCollection();
	}
}
