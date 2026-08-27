using System;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public class RetentionOptionRoleRelation
{
	public string IdRole { get; set; }

	public string IdRetention { get; set; }

	public string RetentionName { get; set; }

	public static RetentionOptionRoleRelation[] QueryRetentionOptionRoleRelation(IGeminiRequestContext requestContext)
	{
		using IRetentionOptionRoleRelationComponent retentionOptionRoleRelationComponent = requestContext.CreateRetentionOptionRoleRelationComponent();
		return retentionOptionRoleRelationComponent.QueryRetentionOptionRelations();
	}

	public static RetentionOptionRoleRelation QueryRetentionOptionRoleRelationByRoleId(IGeminiRequestContext requestContext, string id)
	{
		using IRetentionOptionRoleRelationComponent retentionOptionRoleRelationComponent = requestContext.CreateRetentionOptionRoleRelationComponent();
		return retentionOptionRoleRelationComponent.QueryRetentionOptionByRoleId(id);
	}

	public static RetentionOptionRoleRelation CreateRetentionOptionRoleRelation(IGeminiRequestContext requestContext, string idRole, string idRetention)
	{
		using IRetentionOptionRoleRelationComponent retentionOptionRoleRelationComponent = requestContext.CreateRetentionOptionRoleRelationComponent();
		retentionOptionRoleRelationComponent.CreateRetentionRelation(idRole, idRetention);
		return retentionOptionRoleRelationComponent.QueryRetentionOptionByRoleId(idRole);
	}

	public static RetentionOptionRoleRelation UpdateRetentionOptionRoleRelation(IGeminiRequestContext requestContext, string idRole, string idRetention, string retentionName)
	{
		Guid sessionId = Guid.NewGuid();
		RetentionOptionRoleRelation result;
		using (IRetentionOptionRoleRelationComponent retentionOptionRoleRelationComponent = requestContext.CreateRetentionOptionRoleRelationComponent())
		{
			retentionOptionRoleRelationComponent.UpdateRetentionRelation(idRole, idRetention, retentionName, sessionId);
			result = retentionOptionRoleRelationComponent.QueryRetentionOptionByRoleId(idRole);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static void DeleteRetentionOptionRoleRelation(IGeminiRequestContext requestContext, string idRole)
	{
		using IRetentionOptionRoleRelationComponent retentionOptionRoleRelationComponent = requestContext.CreateRetentionOptionRoleRelationComponent();
		retentionOptionRoleRelationComponent.DeleteRetentionRelation(idRole);
	}
}
