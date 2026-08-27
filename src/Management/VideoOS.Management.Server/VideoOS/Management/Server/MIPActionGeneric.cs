using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MIPAction")]
public sealed class MIPActionGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(MIPAction.QueryMIPAction(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(MIPAction.QueryMIPActionsByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		MIPAction[] array = MIPAction.QueryMIPActions(requestContext);
		List<string> list = new List<string>();
		MIPAction[] array2 = array;
		foreach (MIPAction mIPAction in array2)
		{
			list.Add(mIPAction.Id);
		}
		return list.ToArray();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		MIPAction mIPAction = ConvertToMIPAction(properties);
		mIPAction = MIPAction.CreateMIPAction(requestContext, mIPAction.Id, mIPAction.Name, mIPAction.DefiningPluginId, mIPAction.DefiningPluginName, mIPAction.SelectionText, mIPAction.DescriptionText, mIPAction.PickerDefaultText, mIPAction.MIPKinds, mIPAction.PickerDefaultText2, mIPAction.MIPKinds2);
		return ConvertToGenericObject2(mIPAction);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		MIPAction mIPAction = ConvertToMIPAction(genericObject.Properties);
		mIPAction = MIPAction.UpdateMIPAction(requestContext, mIPAction.Id, mIPAction.Name, mIPAction.DefiningPluginId, mIPAction.DefiningPluginName, mIPAction.SelectionText, mIPAction.DescriptionText, mIPAction.PickerDefaultText, mIPAction.MIPKinds, mIPAction.PickerDefaultText2, mIPAction.MIPKinds2);
		return ConvertToGenericObject2(mIPAction);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		if (MIPAction.QueryMIPAction(requestContext, id) == null)
		{
			throw new MIPActionNotFoundException();
		}
		MIPAction.DeleteMIPAction(requestContext, id);
	}

	private static GenericObject2[] ConvertToGenericObject2s(MIPAction[] mipActions)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		for (int i = 0; i < mipActions.Length; i++)
		{
			GenericObject2 genericObject = ConvertToGenericObject2(mipActions[i]);
			if (genericObject != null)
			{
				list.Add(genericObject);
			}
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToGenericObject2(MIPAction mipAction)
	{
		if (mipAction == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", mipAction.Id));
		list.Add(new ObjectProperty2("Name", mipAction.Name));
		list.Add(new ObjectProperty2("DefiningPluginId", mipAction.DefiningPluginId));
		list.Add(new ObjectProperty2("DefiningPluginName", mipAction.DefiningPluginName));
		list.Add(new ObjectProperty2("SelectionText", mipAction.SelectionText));
		list.Add(new ObjectProperty2("DescriptionText", mipAction.DescriptionText));
		list.Add(new ObjectProperty2("PickerDefaultText", mipAction.PickerDefaultText));
		list.Add(new ObjectProperty2("MIPKinds", mipAction.MIPKinds));
		list.Add(new ObjectProperty2("PickerDefaultText2", mipAction.PickerDefaultText2));
		list.Add(new ObjectProperty2("MIPKinds2", mipAction.MIPKinds2));
		list.Add(new ObjectProperty2("CreatedDate", mipAction.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", mipAction.LastModified));
		return new GenericObject2(mipAction.Id, "MIPAction", list.ToArray(), null);
	}

	private static MIPAction ConvertToMIPAction(ObjectProperty2[] properties)
	{
		MIPAction mIPAction = new MIPAction();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			mIPAction.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		else
		{
			mIPAction.Id = string.Empty;
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			mIPAction.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "DefiningPluginId"))
		{
			mIPAction.DefiningPluginId = GenericObjectHelper2.GetProperty<string>(properties, "DefiningPluginId");
		}
		if (GenericObjectHelper2.Contains(properties, "DefiningPluginName"))
		{
			mIPAction.DefiningPluginName = GenericObjectHelper2.GetProperty<string>(properties, "DefiningPluginName");
		}
		if (GenericObjectHelper2.Contains(properties, "SelectionText"))
		{
			mIPAction.SelectionText = GenericObjectHelper2.GetProperty<string>(properties, "SelectionText");
		}
		if (GenericObjectHelper2.Contains(properties, "DescriptionText"))
		{
			mIPAction.DescriptionText = GenericObjectHelper2.GetProperty<string>(properties, "DescriptionText");
		}
		if (GenericObjectHelper2.Contains(properties, "PickerDefaultText"))
		{
			mIPAction.PickerDefaultText = GenericObjectHelper2.GetProperty<string>(properties, "PickerDefaultText");
		}
		if (GenericObjectHelper2.Contains(properties, "MIPKinds"))
		{
			mIPAction.MIPKinds = GenericObjectHelper2.GetProperty<string>(properties, "MIPKinds");
		}
		if (GenericObjectHelper2.Contains(properties, "PickerDefaultText2"))
		{
			mIPAction.PickerDefaultText2 = GenericObjectHelper2.GetProperty<string>(properties, "PickerDefaultText2");
		}
		if (GenericObjectHelper2.Contains(properties, "MIPKinds2"))
		{
			mIPAction.MIPKinds2 = GenericObjectHelper2.GetProperty<string>(properties, "MIPKinds2");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			mIPAction.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			mIPAction.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		return mIPAction;
	}
}
