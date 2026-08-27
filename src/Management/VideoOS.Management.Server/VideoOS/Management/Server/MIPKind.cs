#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VideoOS.Management.Server;

public sealed class MIPKind
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public Guid PluginId { get; set; }

	public string PluginName { get; set; }

	public int DataVersion { get; set; }

	public string KindType { get; set; }

	public string SecurityAction { get; set; }

	public string SecurityActionNames { get; set; }

	public bool DisplayOnGisMap { get; set; } = true;

	public Guid ParentKindId { get; set; }

	public DateTime LastModified { get; set; }

	public string ModifiedUser { get; set; }

	public string SecurityActionAndNames
	{
		get
		{
			if (string.IsNullOrEmpty(SecurityAction) && string.IsNullOrEmpty(SecurityActionNames))
			{
				return string.Empty;
			}
			return SecurityAction + ";" + SecurityActionNames;
		}
		set
		{
			if (string.IsNullOrEmpty(value))
			{
				SecurityAction = string.Empty;
				SecurityActionNames = string.Empty;
				return;
			}
			string[] array = value.Split(';');
			SecurityAction = array[0];
			if (array.Length > 1)
			{
				SecurityActionNames = array[1];
			}
			else
			{
				SecurityActionNames = SecurityAction;
			}
		}
	}

	public static Dictionary<Guid, string> GetPlugins(IGeminiRequestContext requestContext)
	{
		using IMIPKindComponent iMIPKindComponent = requestContext.CreateMIPKindComponent();
		return iMIPKindComponent.QueryPlugins();
	}

	public static MIPKind GetKind(IGeminiRequestContext requestContext, Guid kind)
	{
		using IMIPKindComponent iMIPKindComponent = requestContext.CreateMIPKindComponent();
		return iMIPKindComponent.QueryKind(kind);
	}

	public static List<MIPKind> GetKinds(IGeminiRequestContext requestContext)
	{
		using IMIPKindComponent iMIPKindComponent = requestContext.CreateMIPKindComponent();
		return iMIPKindComponent.QueryKinds().ToList();
	}

	public static List<MIPKind> GetKinds(IGeminiRequestContext requestContext, Guid pluginId)
	{
		using IMIPKindComponent iMIPKindComponent = requestContext.CreateMIPKindComponent();
		return iMIPKindComponent.QueryKindsByPluginId(pluginId).ToList();
	}

	public static List<MIPKind> GetKindsByParentKind(IGeminiRequestContext requestContext, Guid parentKind)
	{
		using IMIPKindComponent iMIPKindComponent = requestContext.CreateMIPKindComponent();
		return iMIPKindComponent.QueryKindsByParentKindId(parentKind).ToList();
	}

	public static void SaveKind(IGeminiRequestContext requestContext, MIPKind kind)
	{
		using (IMIPKindComponent iMIPKindComponent = requestContext.CreateMIPKindComponent())
		{
			iMIPKindComponent.SaveKind(kind);
		}
		requestContext.GetNameLookupService().InitializeMipNamespaces(requestContext);
	}

	public static void DeleteKind(IGeminiRequestContext requestContext, Guid kindId)
	{
		if (!requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			throw new UnauthorizedAccessException();
		}
		using (IMIPKindComponent iMIPKindComponent = requestContext.CreateMIPKindComponent())
		{
			iMIPKindComponent.DeleteKind(kindId);
		}
		requestContext.GetNameLookupService().InitializeMipNamespaces(requestContext);
	}

	public static bool RegisterNamespace(IGeminiRequestContext requestContext, Guid namespaceid, string kindName, List<string> actions, List<string> actionNames, bool visible)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		if (actions.Count != actionNames.Count)
		{
			throw new ArgumentException($"MIPKind: SecurityAction id and name count incorrect, {actions.Count}, {actionNames.Count}");
		}
		List<ActionDefinition> list = new List<ActionDefinition>();
		ulong num = 1uL;
		IGeminiSecurityNamespace securityNamespace = securityManagerService.GetSecurityNamespace(requestContext, namespaceid);
		if (securityNamespace != null)
		{
			list = securityNamespace.NamespaceInfo.Actions;
			foreach (ActionDefinition item in list)
			{
				if (item.PermissionBit > num)
				{
					num = item.PermissionBit;
				}
			}
		}
		bool flag = false;
		for (int i = 0; i < actions.Count; i++)
		{
			string text = actions[i];
			string displayName = actionNames[i];
			bool flag2 = false;
			foreach (ActionDefinition item2 in list)
			{
				if (item2.Name == text)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				flag = true;
				num *= 2;
				list.Add(new ActionDefinition
				{
					DisplayName = displayName,
					Name = text,
					NamespaceId = kindName.ToString().ToUpperInvariant(),
					Visible = visible,
					PermissionBit = num
				});
			}
		}
		if (flag)
		{
			SecurityNamespaceInfo description = new SecurityNamespaceInfo
			{
				NamespaceId = namespaceid.ToString().ToUpperInvariant(),
				DisplayName = kindName,
				Name = kindName,
				UserDefined = true,
				Structure = 1,
				Separator = '/',
				Actions = list
			};
			if (securityNamespace == null)
			{
				securityManagerService.CreateSecurityNamespace(requestContext, description, Guid.Empty.ToString());
			}
			else
			{
				securityManagerService.UpdateSecurityNamespace(requestContext, description);
			}
			requestContext.GetNameLookupService().InitializeMipNamespaces(requestContext);
		}
		return securityNamespace == null || flag;
	}

	public static void UnRegisterNamespace(IGeminiRequestContext requestContext, Guid kindId)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		try
		{
			securityManagerService.DeleteSecurityNamespace(requestContext, kindId);
		}
		catch (Exception)
		{
			Trace.WriteLine("Ignoring stored procedure issue for security_delete_namespace.sql");
		}
		requestContext.GetNameLookupService().InitializeMipNamespaces(requestContext);
	}
}
