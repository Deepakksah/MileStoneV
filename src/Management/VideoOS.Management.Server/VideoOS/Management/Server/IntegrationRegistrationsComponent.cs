using System;
using System.Data;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public sealed class IntegrationRegistrationsComponent : GeminiResourceComponent, IIntegrationRegistrationsComponent, IDisposable
{
	public void CreateIntegrationRegistration(string instanceId, Guid integrationId, IntegrationType integrationType, string integrationVersion, string integrationName, string manufacturerName, bool allowIntegrationRegistration)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[CreateOrUpdateIntegrationRegistration]");
			BindString("@InstanceId", instanceId, 256, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@IntegrationId", integrationId);
			BindString("@Type", integrationType.ToString(), 32, allowNull: false, SqlDbType.NVarChar);
			BindString("@IntegrationVersion", integrationVersion, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@IntegrationName", integrationName, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@ManufacturerName", manufacturerName, 256, allowNull: false, SqlDbType.NVarChar);
			BindBoolean("@AllowIntegrationRegistration", allowIntegrationRegistration);
			ExecuteNonQuery();
		});
	}
}
