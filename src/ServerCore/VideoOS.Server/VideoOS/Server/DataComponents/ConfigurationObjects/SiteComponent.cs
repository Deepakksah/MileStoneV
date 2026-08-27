using System;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class SiteComponent : VmoResourceComponent, ISiteConfigDataComponent, IDisposable
{
	private static class SiteConfigDataColumns
	{
		private static DatabaseColumnBinder IDSite = new DatabaseColumnBinder("IDSite");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		public static Site Bind(DatabaseDataReader reader)
		{
			Site site = new Site();
			site.Id = IDSite.GetStringAsGuid(reader);
			site.Name = Name.GetString(reader);
			return site;
		}
	}

	public SiteComponent()
	{
	}

	public Site QueryHomeSite()
	{
		string sql = "SELECT [IDSite], [Name] FROM [Sites] WHERE [IDSiteType] = 1";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new DatabaseConfigMissingRowException();
			}
			Site result = SiteConfigDataColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
