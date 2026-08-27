using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents;

public sealed class IntegrationRegistrationsComponent : VmoResourceComponent, IIntegrationRegistrationsComponent, IDisposable
{
	private static class IntegrationRegistrationColumns
	{
		private static readonly DatabaseColumnBinder IntegrationId = new DatabaseColumnBinder("IntegrationId");

		private static readonly DatabaseColumnBinder IntegrationName = new DatabaseColumnBinder("IntegrationName");

		private static readonly DatabaseColumnBinder ManufacturerName = new DatabaseColumnBinder("ManufacturerName");

		private static readonly DatabaseColumnBinder Type = new DatabaseColumnBinder("Type");

		private static readonly DatabaseColumnBinder IntegrationVersion = new DatabaseColumnBinder("IntegrationVersion");

		private static readonly DatabaseColumnBinder VersionCount = new DatabaseColumnBinder("VersionCount");

		private static readonly DatabaseColumnBinder RegistrationTime = new DatabaseColumnBinder("RegistrationTime");

		private static readonly DatabaseColumnBinder AllowIntegrationRegistration = new DatabaseColumnBinder("AllowIntegrationRegistration");

		public static IntegrationRegistrationRow Bind(DatabaseDataReader reader)
		{
			return new IntegrationRegistrationRow
			{
				IntegrationId = IntegrationId.GetGuid2(reader),
				IntegrationName = IntegrationName.GetString(reader, allowNulls: false),
				ManufacturerName = ManufacturerName.GetString(reader, allowNulls: false),
				IntegrationType = Type.GetString(reader, allowNulls: false),
				IntegrationVersion = IntegrationVersion.GetString(reader, allowNulls: false),
				VersionCount = VersionCount.GetInt32(reader),
				RegistrationTime = RegistrationTime.GetDateTime(reader),
				AllowIntegrationRegistration = AllowIntegrationRegistration.GetBoolean(reader)
			};
		}
	}

	public List<IntegrationRegistrationRow> QueryIntegrationRegistrations()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryIntegrationRegistrations]");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<IntegrationRegistrationRow> list = new List<IntegrationRegistrationRow>();
			while (databaseDataReader.Read())
			{
				IntegrationRegistrationRow item = IntegrationRegistrationColumns.Bind(databaseDataReader);
				list.Add(item);
			}
			return list;
		});
	}

	public void DeleteIntegrationRegistrationsOlderThan(int days)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[DeleteIntegrationRegistrationsOlderThan]");
			BindInt("@Days", days);
			ExecuteNonQuery();
		});
	}

	public IntegrationRegistrationsComponent()
	{
	}
}
