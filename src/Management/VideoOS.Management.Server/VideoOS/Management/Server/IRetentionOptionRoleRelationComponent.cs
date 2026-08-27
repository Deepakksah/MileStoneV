using System;

namespace VideoOS.Management.Server;

public interface IRetentionOptionRoleRelationComponent : IDisposable
{
	void CreateRetentionRelation(string idRole, string idRetention);

	string[] QueryRetentionOptionRoleRelationIds();

	RetentionOptionRoleRelation QueryRetentionOptionByRoleId(string id);

	RetentionOptionRoleRelation[] QueryRetentionOptionRelations();

	RetentionOptionRoleRelation[] QueryRetentionOptionRoleRelationsByIds(string[] ids);

	void UpdateRetentionRelation(string idRole, string idRetention, string retentionName, Guid sessionId);

	void DeleteRetentionRelation(string idRole);
}
