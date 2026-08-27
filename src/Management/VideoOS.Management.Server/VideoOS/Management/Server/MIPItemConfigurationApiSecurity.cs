using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Platform;

namespace VideoOS.Management.Server;

public static class MIPItemConfigurationApiSecurity
{
	public static void RegisterKindNamespace(IGeminiRequestContext requestContext, Guid kind, string name, List<string> actions, List<string> actionNames)
	{
		if (MIPKind.RegisterNamespace(requestContext, kind, name, actions, actionNames, visible: true))
		{
			try
			{
				MIPItem.RegisterSecurityObject(requestContext, kind, kind.ToString(), "/");
			}
			catch (Exception)
			{
			}
		}
	}

	public static void UnRegisterKindNamespace(IGeminiRequestContext requestContext, Guid kind)
	{
		MIPItem.UnRegisterSecurityObject(requestContext, kind, kind.ToString());
		MIPKind.UnRegisterNamespace(requestContext, kind);
	}

	public static void RegisterItem(IGeminiRequestContext requestContext, Guid parentId, Guid objectId, Guid kind)
	{
		try
		{
			Guid classId = GetClassId(parentId, kind);
			string text = ((!(parentId == Guid.Empty)) ? classId.ToString() : kind.ToString());
			try
			{
				EnvironmentManager.Instance.Log(error: false, "RegisterItem", string.Format(CultureInfo.InvariantCulture, "Registering new Item, id {0}", objectId));
				MIPItem.RegisterSecurityObject(requestContext, kind, objectId.ToString().ToUpperInvariant(), text.ToUpperInvariant());
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "RegisterItem", string.Format(CultureInfo.InvariantCulture, "Duplicate registration ignored: {0}", ex.Message));
			}
			CheckToRegisterChildItemNodes(requestContext, objectId, kind);
		}
		catch (Exception ex2)
		{
			EnvironmentManager.Instance.Log(error: true, "RegisterItem via Guids", ex2.Message);
		}
	}

	public static void UnregisterItem(IGeminiRequestContext requestContext, Guid objectId, Guid kind)
	{
		try
		{
			CheckToUnregisterChildItemNodes(requestContext, objectId, kind);
			EnvironmentManager.Instance.Log(error: false, "RegisterItem", string.Format(CultureInfo.InvariantCulture, "Un-Registering Item, id={0}", objectId));
			MIPItem.UnRegisterSecurityObject(requestContext, kind, objectId.ToString().ToUpperInvariant());
		}
		catch (Exception ex)
		{
			EnvironmentManager.Instance.Log(error: true, "UnregisterItem", ex.Message);
		}
	}

	private static void CheckToRegisterChildItemNodes(IGeminiRequestContext requestContext, Guid objectId, Guid kind)
	{
		UpdateSecurityNodeOnChilds(requestContext, objectId, kind, make: true);
	}

	private static void CheckToUnregisterChildItemNodes(IGeminiRequestContext requestContext, Guid objectId, Guid kind)
	{
		UpdateSecurityNodeOnChilds(requestContext, objectId, kind, make: false);
	}

	private static void UpdateSecurityNodeOnChilds(IGeminiRequestContext requestContext, Guid parentObjectId, Guid parentKind, bool make)
	{
		foreach (MIPKind item in MIPKind.GetKindsByParentKind(requestContext, parentKind))
		{
			try
			{
				if (make)
				{
					MIPItem.RegisterSecurityObject(requestContext, item.Id, parentObjectId.ToString().ToUpperInvariant(), "/");
				}
				else
				{
					MIPItem.UnRegisterSecurityObject(requestContext, item.Id, parentObjectId.ToString().ToUpperInvariant());
				}
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "UpdateSecurityNodeOnChilds", ex.Message);
			}
		}
	}

	private static Guid GetClassId(Guid parentObjectId, Guid childKind)
	{
		if (parentObjectId == Guid.Empty)
		{
			return childKind;
		}
		return parentObjectId;
	}
}
