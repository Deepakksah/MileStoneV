using System;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public class BasicOwnerInfo
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Property { get; set; }

	public string Order { get; set; }

	public static BasicOwnerInfo[] QueryBasicOwnerInfo(IGeminiRequestContext requestContext)
	{
		using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
		return basicOwnerInfoComponent.QueryBasicOwnerInfo();
	}

	public static BasicOwnerInfo QueryBasicOwnerInfoById(IGeminiRequestContext requestContext, string id)
	{
		using IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent();
		return basicOwnerInfoComponent.QueryBasicOwnerInfoById(id);
	}

	public static BasicOwnerInfo CreateBasicOwnerInfo(IGeminiRequestContext requestContext, string id, string name, string property, string order)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		BasicOwnerInfo basicOwnerInfo;
		using (IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent())
		{
			basicOwnerInfoComponent.CreateBasicOwnerInfo(id, name, property, order);
			basicOwnerInfo = basicOwnerInfoComponent.QueryBasicOwnerInfoById(id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditBasicOwnerInfoCreated(basicOwnerInfo.Id, basicOwnerInfo.Name, basicOwnerInfo.Property, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return basicOwnerInfo;
	}

	public static BasicOwnerInfo UpdateBasicOwnerInfo(IGeminiRequestContext requestContext, string id, string name, string property, string order)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		BasicOwnerInfo result;
		using (IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent())
		{
			basicOwnerInfoComponent.UpdateBasicOwnerInfo(id, name, property, order, sessionId);
			result = basicOwnerInfoComponent.QueryBasicOwnerInfoById(id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static void DeleteBasicOwnerInfo(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		using (IBasicOwnerInfoComponent basicOwnerInfoComponent = requestContext.CreateBasicOwnerInfoComponent())
		{
			basicOwnerInfoComponent.DeleteBasicOwnerInfo(id, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}
}
