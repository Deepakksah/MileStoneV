#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class SecurityService : GeminiWebServiceBase, ISecurityService
{
	private SecuredSecurityManager _securedSecurityManager;

	public SecurityService()
	{
		_securedSecurityManager = base.RequestContext.GetService<SecuredSecurityManager>();
	}

	public RoleOld[] QueryRoles()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRoles");
			EnterWebMethod(methodInformation);
			return RoleOld.QueryRoles(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RoleOld QueryRoleById(string roleId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRoleById");
			methodInformation.AddParameter("roleId", roleId);
			EnterWebMethod(methodInformation);
			return RoleOld.QueryRoleById(base.RequestContext, roleId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RoleOld QueryRoleByName(string name)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRoleByName");
			methodInformation.AddParameter("name", name);
			EnterWebMethod(methodInformation);
			return RoleOld.QueryRoleByName(base.RequestContext, name);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Identity[] QueryRoleMembers(string roleId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRoleMembers");
			methodInformation.AddParameter("roleId", roleId);
			EnterWebMethod(methodInformation);
			return Role.QueryRoleMembersExtended(base.RequestContext, roleId, QueryMembership.None, includeMappedExternalUsers: true);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers = true)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRoleMembersExtended");
			methodInformation.AddParameter("roleId", roleId);
			methodInformation.AddParameter("queryMembership", queryMembership);
			methodInformation.AddParameter("includeMappedExternalUsers", includeMappedExternalUsers);
			EnterWebMethod(methodInformation);
			return Role.QueryRoleMembersExtended(base.RequestContext, roleId, queryMembership, includeMappedExternalUsers);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RoleOld CreateRole(string name, string description)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreateRole");
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("description", description);
			EnterWebMethod(methodInformation);
			return RoleOld.CreateRole(base.RequestContext, name, description);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RoleOld UpdateRole(string roleId, string name, string description)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateRole");
			methodInformation.AddParameter("roleId", roleId);
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("description", description);
			EnterWebMethod(methodInformation);
			return RoleOld.UpdateRole(base.RequestContext, roleId, name, description);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RoleOld CopyRole(string sourceRoleId, string name, string description)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("CopyRole");
			methodInformation.AddParameter("sourceRoleId", sourceRoleId);
			methodInformation.AddParameter("name", name);
			methodInformation.AddParameter("description", description);
			EnterWebMethod(methodInformation);
			return RoleOld.CopyRole(base.RequestContext, sourceRoleId, name, description);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteRole(string roleId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteRole");
			methodInformation.AddParameter("roleId", roleId);
			EnterWebMethod(methodInformation);
			RoleOld.DeleteRole(base.RequestContext, roleId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Identity AddMemberToRole(string roleId, string memberSid)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("AddMemberToRole");
			methodInformation.AddParameter("roleId", roleId);
			methodInformation.AddParameter("memberSid", memberSid);
			EnterWebMethod(methodInformation);
			return RoleOld.AddMemberToRole(base.RequestContext, roleId, memberSid);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RemoveMemberFromRole(string roleId, string memberSid)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RemoveMemberFromRole");
			methodInformation.AddParameter("roleId", roleId);
			methodInformation.AddParameter("memberSid", memberSid);
			EnterWebMethod(methodInformation);
			RoleOld.RemoveMemberFromRole(base.RequestContext, roleId, memberSid);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool IsMember(string identitySid, string groupSid)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("IsMember");
			methodInformation.AddParameter("identitySid", identitySid);
			methodInformation.AddParameter("groupSid", groupSid);
			EnterWebMethod(methodInformation);
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationReadPermission(base.RequestContext);
			SecurityValidation.CheckSid(ref identitySid, "identitySid");
			SecurityValidation.CheckSid(ref groupSid, "groupSid");
			return IdentityCache.IsMember(base.RequestContext, identitySid, groupSid);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Identity[] ReadIdentities(SearchFactor factor, string[] factorValueArray, QueryMembership queryMembership)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("ReadIdentities");
			methodInformation.AddParameter("factor", factor);
			methodInformation.AddArrayParameter("factorValueArray", factorValueArray);
			methodInformation.AddParameter("queryMembership", queryMembership);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckFactorAndValueArray(factor, ref factorValueArray, "factor", "factorValueArray");
			SecurityValidation.CheckQueryMembership(queryMembership, "queryMembership");
			for (int i = 0; i < factorValueArray.Length; i++)
			{
				factorValueArray[i] = IdentityAccessor.GetSidBySearchFactor(base.RequestContext, factor, factorValueArray[i]);
			}
			return IdentityCache.ReadIdentitiesFromCache(base.RequestContext, factorValueArray, queryMembership);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Identity ReadIdentityFromSource(SearchFactor factor, string factorValue)
	{
		Identity result = null;
		try
		{
			MethodInformation methodInformation = new MethodInformation("ReadIdentityFromSource");
			methodInformation.AddParameter("factor", factor);
			methodInformation.AddParameter("factorValue", factorValue);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckFactorAndValue(factor, ref factorValue, "factor", "factorValue");
			string sidBySearchFactor = IdentityAccessor.GetSidBySearchFactor(base.RequestContext, factor, factorValue);
			if (!string.IsNullOrEmpty(sidBySearchFactor))
			{
				result = IdentityAccessor.ReadIdentityFromSource(base.RequestContext, sidBySearchFactor);
			}
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
		return result;
	}

	public void RemoveIdentity(string userSid)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RemoveIdentity");
			methodInformation.AddParameter("userSid", userSid);
			EnterWebMethod(methodInformation);
			_securedSecurityManager.RemoveIdentity(base.RequestContext, userSid);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string[] QueryObjectClasses()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryObjectClasses");
			EnterWebMethod(methodInformation);
			return _securedSecurityManager.QueryObjectClasses(base.RequestContext);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string QueryObjectClass(string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryObjectClass");
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			return base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId).NamespaceInfo.Name;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SecurityAction[] QueryObjectClassActions(string objectClassId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryObjectClassActions");
			methodInformation.AddParameter("objectClassId", objectClassId);
			EnterWebMethod(methodInformation);
			Trace.WriteLine("objectClassId: " + objectClassId);
			SecurityValidation.CheckObjectClassId(ref objectClassId, "objectClassId");
			return _securedSecurityManager.QueryObjectClassActions(base.RequestContext, objectClassId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void CreateObjectClass(string objectClassId, string[] classActions)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreateObjectClass");
			methodInformation.AddParameter("objectClassId", objectClassId);
			methodInformation.AddArrayParameter("classActions", classActions);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectClassId(ref objectClassId, "objectClassId");
			SecurityValidation.CheckActionIdArray(classActions, "classActions");
			if (!classActions.Any((string e) => e == "ADMIN_SECURITY"))
			{
				classActions = new List<string>(classActions) { "ADMIN_SECURITY" }.ToArray();
			}
			_securedSecurityManager.CreateObjectClass(base.RequestContext, objectClassId, classActions);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteObjectClass(string objectClassId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteObjectClass");
			methodInformation.AddParameter("objectClassId", objectClassId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectClassId(ref objectClassId, "objectClassId");
			_securedSecurityManager.DeleteObjectClass(base.RequestContext, objectClassId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RegisterSecurityObject(string objectId, string objectClassId, string parentObjectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterSecurityObject");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("objectClassId", objectClassId);
			methodInformation.AddParameter("parentObjectId", parentObjectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			SecurityValidation.CheckObjectClassId(ref objectClassId, "objectClassId");
			SecurityValidation.CheckObjectId(ref parentObjectId, "parentObjectId", allowNull: true);
			_securedSecurityManager.RegisterObject(base.RequestContext, objectClassId, objectId, parentObjectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UnregisterSecurityObject(string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("UnregisterSecurityObject");
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			_securedSecurityManager.UnregisterObject(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public AccessControlEntry[] QueryAccessControlList(string objectId)
	{
		AccessControlEntry[] array = null;
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryAccessControlList");
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			return _securedSecurityManager.QueryPermissions(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public AccessControlEntry[] QueryAccessControlListByIdentity(string identity, string objectId)
	{
		AccessControlEntry[] array = null;
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryAccessControlListByIdentity");
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckSid(ref identity, "identity");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			return _securedSecurityManager.QueryPermissions(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId, identity);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void AddAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("AddAccessControlEntries");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddArrayParameter("entries", entries);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			SecurityValidation.CheckAccessControlEntryArray(entries, "entries");
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId);
			_securedSecurityManager.AddPermissions(base.RequestContext, securityNamespaceFromToken, objectId, objectName, entries);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RemoveAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RemoveAccessControlEntries");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddArrayParameter("entries", entries);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			SecurityValidation.CheckAccessControlEntryArray(entries, "entries");
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId);
			_securedSecurityManager.RemovePermissions(base.RequestContext, securityNamespaceFromToken, objectId, objectName, entries);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ReplaceAccessControlEntries(string objectId, AccessControlEntry[] entries)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("ReplaceAccessControlEntries");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddArrayParameter("entries", entries);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			SecurityValidation.CheckAccessControlEntryArray(entries, "entries");
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId);
			_securedSecurityManager.ReplacePermissions(base.RequestContext, securityNamespaceFromToken, objectId, objectName, entries);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public AccessControlTimeProfileEntry QueryAccessControlTimeProfileEntry(string objectId, string identity, string actionId)
	{
		AccessControlTimeProfileEntry accessControlTimeProfileEntry = null;
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryAccessControlTimeProfileEntry");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddParameter("actionId", actionId);
			EnterWebMethod(methodInformation);
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(base.RequestContext, "TimeControlledAccessRights");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			base.RequestContext.GetService<SecurityManagerService>().CheckPermission(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, "GENERIC_READ", objectId);
			return base.RequestContext.GetService<SecurityManagerService>().QueryAccessControlTimeProfile(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, objectId, identity, actionId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public AccessControlTimeProfileEntry AddUpdateAccessControlTimeProfileEntry(string objectId, string identity, string actionId, string timeProfileId, bool isDefaultTimeProfile)
	{
		AccessControlTimeProfileEntry accessControlTimeProfileEntry = null;
		try
		{
			MethodInformation methodInformation = new MethodInformation("AddUpdateAccessControlTimeProfileEntry");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddParameter("actionId", actionId);
			methodInformation.AddParameter("timeProfileId", timeProfileId);
			methodInformation.AddParameter("isDefaultTimeProfile", isDefaultTimeProfile);
			EnterWebMethod(methodInformation);
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(base.RequestContext, "TimeControlledAccessRights");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId);
			return _securedSecurityManager.AddUpdateAccessControlTimeProfileEntry(base.RequestContext, securityNamespaceFromToken, objectId, objectName, identity, actionId, timeProfileId, isDefaultTimeProfile);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteAccessControlTimeProfile(string objectId, string identity, string actionId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteAccessControlTimeProfile");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddParameter("actionId", actionId);
			EnterWebMethod(methodInformation);
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(base.RequestContext, "TimeControlledAccessRights");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			base.RequestContext.GetService<SecurityManagerService>().CheckPermission(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, "ADMIN_SECURITY", objectId);
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId);
			_securedSecurityManager.DeleteAccessControlTimeProfileEntry(base.RequestContext, securityNamespaceFromToken, objectId, objectName, identity, actionId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void SetInheritance(string objectId, string parentObjectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetInheritance");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("parentObjectId", parentObjectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			SecurityValidation.CheckObjectId(ref parentObjectId, "parentObjectId", allowNull: true);
			SecurityValidation.CheckParentObjectNotSelf(objectId, parentObjectId, "parentObjectId");
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			_securedSecurityManager.SetInheritance(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, objectId, parentObjectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void CheckPermission(string objectId, string actionId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("CheckPermission");
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("actionId", actionId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			SecurityValidation.CheckActionId(ref actionId, "actionId");
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			base.RequestContext.GetService<SecurityManagerService>().CheckObjectPermission(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, actionId, objectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool[] IsPermittedByIdentityList(string[] identityList, string actionId, string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("IsPermittedByIdentityList");
			methodInformation.AddArrayParameter("identityList", identityList);
			methodInformation.AddParameter("actionId", actionId);
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationReadPermission(base.RequestContext);
			SecurityValidation.CheckSidArray(identityList, "identityList");
			SecurityValidation.CheckActionId(ref actionId, "actionId");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			bool[] array = new bool[identityList.Length];
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			for (int i = 0; i < identityList.Length; i++)
			{
				array[i] = _securedSecurityManager.HasPermission(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, identityList[i], actionId, objectId);
			}
			return array;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool[] IsPermittedByActionList(string identity, string[] actionIdList, string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("IsPermittedByActionList");
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddArrayParameter("actionIdList", actionIdList);
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckSid(ref identity, "identity");
			SecurityValidation.CheckActionIdArray(actionIdList, "actionIdList");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			bool[] array = new bool[actionIdList.Length];
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			for (int i = 0; i < actionIdList.Length; i++)
			{
				array[i] = _securedSecurityManager.HasPermission(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, identity, actionIdList[i], objectId);
			}
			return array;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool[] IsPermittedByObjectList(string identity, string actionId, string[] objectIdList)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("IsPermittedByObjectList");
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddParameter("actionId", actionId);
			methodInformation.AddArrayParameter("objectIdList", objectIdList);
			EnterWebMethod(methodInformation);
			base.RequestContext.GetService<SecurityManagerService>().CheckApplicationReadPermission(base.RequestContext);
			SecurityValidation.CheckSid(ref identity, "identity");
			SecurityValidation.CheckActionId(ref actionId, "actionId");
			SecurityValidation.CheckObjectIdArray(objectIdList, "objectIdList");
			bool[] array = new bool[objectIdList.Length];
			for (int i = 0; i < objectIdList.Length; i++)
			{
				IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectIdList[i]);
				array[i] = _securedSecurityManager.HasPermission(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceId, identity, actionId, objectIdList[i]);
			}
			return array;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool IsPermittedTimeBasedAction(string actionId, string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("IsPermittedTimeBasedAction");
			methodInformation.AddParameter("actionId", actionId);
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckActionId(ref actionId, "actionId");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			IGeminiSecurityNamespace securityNamespaceFromToken = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespaceFromToken(base.RequestContext, objectId);
			return base.RequestContext.GetService<SecurityManagerService>().TimeBasedSecurityIsPermitted(base.RequestContext, securityNamespaceFromToken.NamespaceInfo.NamespaceGuid, objectId, actionId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SecurityNamespaceInfo[] QuerySecurityNamespaces(string namespaceId)
	{
		SecurityNamespaceInfo[] array = null;
		try
		{
			MethodInformation methodInformation = new MethodInformation("QuerySecurityNamespaces");
			methodInformation.AddParameter("namespaceId", namespaceId);
			EnterWebMethod(methodInformation);
			return _securedSecurityManager.QuerySecurityNamespaces(base.RequestContext, namespaceId).ToArray();
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void CreateSecurityNamespace(SecurityNamespaceInfo description, string orderAfterNamespaceId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreateSecurityNamespace");
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("orderAfterNamespaceId", orderAfterNamespaceId);
			EnterWebMethod(methodInformation);
			_securedSecurityManager.CreateSecurityNamespace(base.RequestContext, description, orderAfterNamespaceId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UpdateSecurityNamespace(SecurityNamespaceInfo description)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdateSecurityNamespace");
			methodInformation.AddParameter("description", description);
			EnterWebMethod(methodInformation);
			_securedSecurityManager.UpdateSecurityNamespace(base.RequestContext, description);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeleteSecurityNamespace(string namespaceId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeleteSecurityNamespace");
			methodInformation.AddParameter("namespaceId", namespaceId);
			EnterWebMethod(methodInformation);
			_securedSecurityManager.DeleteSecurityNamespace(base.RequestContext, namespaceId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void CreateSecurityGroup(string namespaceId, string path, string name)
	{
	}

	public void RenameSecurityGroup(string namespaceId, string path, string newName)
	{
	}

	public void DeleteSecurityGroup(string namespaceId, string path, string moveItemsToPath)
	{
	}

	public void MoveSecurityGroup(string namespaceId, string sourcePath, string targetPath)
	{
	}

	public void MoveItems(string namespaceId, string[] sourcePath, string[] targetPath)
	{
	}

	public void RegisterObject(string namespaceId, string objectId, string parentObjectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterObject");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("objectId", objectId);
			methodInformation.AddParameter("parentObjectId", parentObjectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			SecurityValidation.CheckObjectId(ref parentObjectId, "parentObjectId", allowNull: true);
			_securedSecurityManager.RegisterObject(base.RequestContext, namespaceId, objectId, parentObjectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UnregisterObject(string namespaceId, string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("UnregisterObject");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			_securedSecurityManager.UnregisterObject(base.RequestContext, namespaceId, objectId);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public AccessControlEntry[] QueryPermissions(string namespaceId, string securityToken, string identity)
	{
		AccessControlEntry[] array = null;
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryPermissions");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("securityToken", securityToken);
			methodInformation.AddParameter("identity", identity);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			return _securedSecurityManager.QueryPermissions(base.RequestContext, namespaceId, securityToken, identity);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void AddPermissions(string namespaceId, string securityToken, AccessControlEntry[] entries)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("AddPermissions");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("securityToken", securityToken);
			methodInformation.AddArrayParameter("entries", entries);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			SecurityValidation.CheckAccessControlEntryArray(entries, "entries");
			IGeminiSecurityNamespace securityNamespace = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespace(base.RequestContext, new Guid(namespaceId));
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, namespaceId, securityToken);
			_securedSecurityManager.AddPermissions(base.RequestContext, securityNamespace, securityToken, objectName, entries);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] entries)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("RemovePermissions");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("securityToken", securityToken);
			methodInformation.AddArrayParameter("entries", entries);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			SecurityValidation.CheckAccessControlEntryArray(entries, "entries");
			IGeminiSecurityNamespace securityNamespace = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespace(base.RequestContext, new Guid(namespaceId));
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, namespaceId, securityToken);
			_securedSecurityManager.RemovePermissions(base.RequestContext, securityNamespace, securityToken, objectName, entries);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void AddRemovePermissions(string namespaceId, string securityToken, AccessControlEntry[] addEntries, AccessControlEntry[] removeEntries)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("AddRemovePermissions");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("securityToken", securityToken);
			methodInformation.AddArrayParameter("addEntries", addEntries);
			methodInformation.AddArrayParameter("removeEntries", removeEntries);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			SecurityValidation.CheckAccessControlEntryArray(addEntries, "addEntries");
			SecurityValidation.CheckAccessControlEntryArray(removeEntries, "removeEntries");
			IGeminiSecurityNamespace securityNamespace = base.RequestContext.GetService<SecurityManagerService>().GetSecurityNamespace(base.RequestContext, new Guid(namespaceId));
			string objectName = base.RequestContext.GetService<NameLookupService>().GetObjectName(base.RequestContext, namespaceId, securityToken);
			_securedSecurityManager.AddRemovePermissions(base.RequestContext, securityNamespace, securityToken, objectName, addEntries, removeEntries);
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool[] HasPermissionByIdentityList(string namespaceId, string[] identityList, string actionId, string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("HasPermissionByIdentityList");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddArrayParameter("identityList", identityList);
			methodInformation.AddArrayParameter("actionId", actionId);
			methodInformation.AddArrayParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			SecurityValidation.CheckSidArray(identityList, "identityList");
			SecurityValidation.CheckActionId(ref actionId, "actionId");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			bool[] array = new bool[identityList.Length];
			for (int i = 0; i < identityList.Length; i++)
			{
				array[i] = _securedSecurityManager.HasPermission(base.RequestContext, namespaceId, identityList[i], actionId, objectId);
			}
			return array;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool[] HasPermissionByActionList(string namespaceId, string identity, string[] actionList, string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("HasPermissionByActionList");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddArrayParameter("actionList", actionList);
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			SecurityValidation.CheckSid(ref identity, "identity");
			SecurityValidation.CheckActionIdArray(actionList, "actionList");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			bool[] array = new bool[actionList.Length];
			for (int i = 0; i < actionList.Length; i++)
			{
				array[i] = _securedSecurityManager.HasPermission(base.RequestContext, namespaceId, identity, actionList[i], objectId);
			}
			return array;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool[] HasPermissionByActionListCurrentUser(string namespaceId, string[] actionList, string objectId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("HasPermissionByActionListCurrentUser");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddArrayParameter("actionList", actionList);
			methodInformation.AddParameter("objectId", objectId);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			SecurityValidation.CheckActionIdArray(actionList, "actionList");
			SecurityValidation.CheckObjectId(ref objectId, "objectId", allowNull: false);
			bool[] array = new bool[actionList.Length];
			for (int i = 0; i < actionList.Length; i++)
			{
				array[i] = _securedSecurityManager.HasPermission(base.RequestContext, namespaceId, actionList[i], objectId);
			}
			return array;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool[] HasPermissionByObjectList(string namespaceId, string identity, string actionId, string[] objectList)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("HasPermissionByObjectList");
			methodInformation.AddParameter("namespaceId", namespaceId);
			methodInformation.AddParameter("identity", identity);
			methodInformation.AddParameter("actionId", actionId);
			methodInformation.AddArrayParameter("objectList", objectList);
			EnterWebMethod(methodInformation);
			SecurityValidation.CheckObjectId(ref namespaceId, "namespaceId", allowNull: false);
			SecurityValidation.CheckSid(ref identity, "identity");
			SecurityValidation.CheckActionId(ref actionId, "actionId");
			SecurityValidation.CheckObjectIdArray(objectList, "objectList");
			bool[] array = new bool[objectList.Length];
			for (int i = 0; i < objectList.Length; i++)
			{
				array[i] = _securedSecurityManager.HasPermission(base.RequestContext, namespaceId, identity, actionId, objectList[i]);
			}
			return array;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
		finally
		{
			LeaveWebMethod();
		}
	}
}
