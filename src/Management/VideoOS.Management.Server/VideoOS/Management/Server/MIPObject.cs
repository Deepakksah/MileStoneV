using System;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;

namespace VideoOS.Management.Server;

public sealed class MIPObject
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Kind { get; set; }

	public string SubKind { get; set; }

	public string PluginId { get; set; }

	public string CustomData { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string RelationIds { get; set; }

	public string ParentGroupId { get; set; }

	public static MIPObject[] QueryMIPObjects(IGeminiRequestContext requestContext, string serverGroupId, bool checkApplicationPermission)
	{
		Validation.CheckObjectId(ref serverGroupId, "serverGroupId", allowNullOrEmpty: true);
		if (checkApplicationPermission)
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		}
		using IMIPObjectComponent iMIPObjectComponent = requestContext.CreateMIPObjectComponent();
		return iMIPObjectComponent.QueryMIPObjects(serverGroupId);
	}

	public static MIPObject QueryMIPObject(IGeminiRequestContext requestContext, string mipObjectId)
	{
		Validation.CheckObjectId(ref mipObjectId, "mipObjectId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IMIPObjectComponent iMIPObjectComponent = requestContext.CreateMIPObjectComponent();
		return iMIPObjectComponent.QueryMIPObject(mipObjectId);
	}

	public static MIPObject[] QueryMIPObjectsByIds(IGeminiRequestContext requestContext, string[] mipObjectIds, bool needAdminSecurity)
	{
		if (needAdminSecurity)
		{
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		}
		using IMIPObjectComponent iMIPObjectComponent = requestContext.CreateMIPObjectComponent();
		return iMIPObjectComponent.QueryMIPObjectsByIds(mipObjectIds);
	}

	public static MIPObject[] QueryMIPObjectsByKind(IGeminiRequestContext requestContext, string kind, string subKind)
	{
		Validation.CheckObjectId(ref kind, "kind", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref subKind, "subKind", allowNullOrEmpty: true);
		using IMIPObjectComponent iMIPObjectComponent = requestContext.CreateMIPObjectComponent();
		return iMIPObjectComponent.QueryMIPObjectsByKind(kind, subKind);
	}

	public static MIPObject CreateMIPObject(IGeminiRequestContext requestContext, string id, string parentId, string name, string kind, string subKind, string pluginId, string customData)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckName(name, "name");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckObjectId(kind, "kind", allowNullOrEmpty: false);
		Validation.CheckObjectId(subKind, "subKind", allowNullOrEmpty: true);
		Validation.CheckObjectId(pluginId, "pluginId", allowNullOrEmpty: false);
		id = GeminiResourceComponent.GetGuid(id);
		using IMIPObjectComponent iMIPObjectComponent = requestContext.CreateMIPObjectComponent();
		iMIPObjectComponent.CreateMIPObject(id, parentId, name, kind, subKind, pluginId, customData);
		if (string.CompareOrdinal(kind, "6d3bdb33-d913-470f-8784-3dd4ee94db8c".ToString()) == 0)
		{
			GroupMemberUtility.AddMember(requestContext, "9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF", id);
		}
		Integration.TryNotifyServerAsync();
		return FindMIPObject(requestContext, iMIPObjectComponent, id);
	}

	public static MIPObject UpdateMIPObject(IGeminiRequestContext requestContext, string id, string name, string customData)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckName(name, "name");
		using IMIPObjectComponent iMIPObjectComponent = requestContext.CreateMIPObjectComponent();
		iMIPObjectComponent.UpdateMIPObject(id, name, customData);
		Integration.TryNotifyServerAsync();
		return FindMIPObject(requestContext, iMIPObjectComponent, id);
	}

	public static void DeleteMIPObject(IGeminiRequestContext requestContext, string mipObjectId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(ref mipObjectId, "mipObject", allowNullOrEmpty: false);
		using (IMIPObjectComponent iMIPObjectComponent = requestContext.CreateMIPObjectComponent())
		{
			MIPObject mIPObject = iMIPObjectComponent.QueryMIPObject(mipObjectId);
			if (mIPObject != null)
			{
				if (string.CompareOrdinal(mIPObject.Kind, "6d3bdb33-d913-470f-8784-3dd4ee94db8c".ToString()) == 0)
				{
					GroupMemberUtility.RemoveMember(requestContext, null, null, mipObjectId);
				}
				iMIPObjectComponent.DeleteMIPObject(mipObjectId);
			}
		}
		Integration.TryNotifyServerAsync();
	}

	private static MIPObject FindMIPObject(IGeminiRequestContext requestContext, IMIPObjectComponent mipComponent, string mipObjectId)
	{
		bool flag = false;
		try
		{
			if (mipComponent == null)
			{
				mipComponent = requestContext.CreateMIPObjectComponent();
				flag = true;
			}
			return mipComponent.QueryMIPObject(mipObjectId);
		}
		finally
		{
			if (flag)
			{
				mipComponent.Dispose();
			}
		}
	}
}
