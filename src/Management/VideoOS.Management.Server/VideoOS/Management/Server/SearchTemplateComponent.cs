using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VideoOS.Management.Server.BusinessObjects.ManagementServer.SearchTemplate;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class SearchTemplateComponent : GeminiResourceComponent, ISearchTemplateComponent, IDisposable
{
	private static class SearchTemplateColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Availability = new DatabaseColumnBinder("Availability");

		private static DatabaseColumnBinder IncludesScopeItems = new DatabaseColumnBinder("IncludesScopeItems");

		private static DatabaseColumnBinder SearchQuery = new DatabaseColumnBinder("SearchQuery");

		public static ISearchTemplateItem Bind(DatabaseDataReader reader)
		{
			if (!Enum.TryParse<SearchTemplateAvailability>(Availability.GetString(reader), ignoreCase: true, out var result))
			{
				result = SearchTemplateAvailability.Public;
			}
			return new SearchTemplateItem
			{
				Id = Id.GetGuid2(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				Availability = result,
				IncludesScopeItems = IncludesScopeItems.GetBoolean(reader),
				SearchQuery = Encoding.UTF8.GetString(SearchQuery.GetBinary(reader, allowNulls: false))
			};
		}
	}

	private static class SearchTemplateFindColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder Count = new DatabaseColumnBinder("Count");

		private static DatabaseColumnBinder StartIndex = new DatabaseColumnBinder("StartIndex");

		private static DatabaseColumnBinder KeywordFilter = new DatabaseColumnBinder("KeywordFilter");

		private static DatabaseColumnBinder PrivateOnly = new DatabaseColumnBinder("PrivateOnly");

		public static ISearchTemplateFind Bind(DatabaseDataReader reader)
		{
			return new SearchTemplateFind
			{
				Id = Id.GetGuid2(reader),
				Count = Count.GetInt32(reader),
				StartIndex = StartIndex.GetInt32(reader),
				KeywordFilter = KeywordFilter.GetString(reader),
				PrivateOnly = PrivateOnly.GetBoolean(reader)
			};
		}
	}

	private const int KEYWORD_FILTER_MAX_SIZE = 256;

	private const int AVAILABILITY_MAX_SIZE = 40;

	public ISearchTemplateItem CreateSearchTemplate(string authenticatedUserSid, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[create_search_template]");
			BindGuidType("@id", id.ToString());
			BindNameType("@name", name);
			BindDescription("@description", description);
			BindString("@availability", availability.ToString(), 40, allowNull: true, SqlDbType.NVarChar);
			BindBoolean("@includesScopeItems", includesScopeItems);
			BindVarBinaryMax("@searchQuery", Encoding.UTF8.GetBytes(searchQuery));
			BindSIDType("@owner", authenticatedUserSid);
			BindGuid("@sessionId", sessionId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SearchTemplateNotExistException();
			}
			ISearchTemplateItem result = SearchTemplateColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ISearchTemplateFind CreateSearchTemplateFind(string authenticatedUserSid, int count, int startIndex, string keywordFilter, bool privateOnly)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[create_search_template_find]");
			BindInt("@count", count);
			BindInt("@startIndex", startIndex);
			BindString("@keywordFilter", keywordFilter, 256, allowNull: false, SqlDbType.NVarChar);
			BindBoolean("@privateOnly", privateOnly);
			BindSIDType("@owner", authenticatedUserSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SearchTemplateNotExistException();
			}
			ISearchTemplateFind result = SearchTemplateFindColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void DeleteSearchTemplate(string authenticatedUserSid, Guid id, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[delete_search_template]");
			BindGuidType("@id", id.ToString());
			BindSIDType("@owner", authenticatedUserSid);
			BindGuid("@sessionId", sessionId);
			ExecuteNonQuery();
		});
	}

	public ISearchTemplateItem[] FindSearchTemplates(string authenticatedUserSid, Guid findId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[query_search_templates_by_findid]");
			BindGuid("@id", findId);
			BindSIDType("@owner", authenticatedUserSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ISearchTemplateItem> list = new List<ISearchTemplateItem>();
			while (databaseDataReader.Read())
			{
				list.Add(SearchTemplateColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ISearchTemplateItem[] FindSearchTemplates(string authenticatedUserSid, int count, int startIndex, string keywordFilter, bool privateOnly)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[query_search_templates_by_searchquery]");
			BindSIDType("@owner", authenticatedUserSid);
			BindInt("@count", count);
			BindInt("@startIndex", startIndex);
			BindString("@keywordFilter", keywordFilter, 256, allowNull: false, SqlDbType.NVarChar);
			BindBoolean("@privateOnly", privateOnly);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ISearchTemplateItem> list = new List<ISearchTemplateItem>();
			while (databaseDataReader.Read())
			{
				list.Add(SearchTemplateColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ISearchTemplateItem QuerySearchTemplate(string authenticatedUserSid, Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[query_search_template_by_id]");
			BindGuid("@id", id);
			BindSIDType("@owner", authenticatedUserSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SearchTemplateNotExistException();
			}
			ISearchTemplateItem result = SearchTemplateColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ISearchTemplateFind QuerySearchTemplateFind(string authenticatedUserSid, Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[query_search_template_find_by_id]");
			BindGuid("@id", id);
			BindSIDType("@owner", authenticatedUserSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SearchTemplateNotExistException();
			}
			ISearchTemplateFind result = SearchTemplateFindColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ISearchTemplateFind[] QuerySearchTemplateFinds(string authenticatedUserSid)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[query_search_template_finds]");
			BindSIDType("@owner", authenticatedUserSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ISearchTemplateFind> list = new List<ISearchTemplateFind>();
			while (databaseDataReader.Read())
			{
				list.Add(SearchTemplateFindColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ISearchTemplateItem UpdateSearchTemplate(string authenticatedUserSid, Guid id, string name, string description, string searchQuery, SearchTemplateAvailability availability, bool includesScopeItems, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[update_search_template]");
			BindGuidType("@id", id.ToString());
			BindNameType("@name", name);
			BindDescription("@description", description);
			BindString("@availability", availability.ToString(), 40, allowNull: true, SqlDbType.NVarChar);
			BindBoolean("@includesScopeItems", includesScopeItems);
			BindVarBinaryMax("@searchQuery", Encoding.UTF8.GetBytes(searchQuery));
			BindSIDType("@owner", authenticatedUserSid);
			BindGuid("@sessionId", sessionId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SearchTemplateNotExistException();
			}
			ISearchTemplateItem result = SearchTemplateColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ISearchTemplateFind UpdateSearchTemplateFind(string authenticatedUserSid, Guid id, int count, int startIndex, string keywordFilter, bool privateOnly)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Search].[update_search_template_find]");
			BindUniqueIdentifier("@id", id);
			BindInt("@count", count);
			BindInt("@startIndex", startIndex);
			BindString("@keywordFilter", keywordFilter, 256, allowNull: false, SqlDbType.NVarChar);
			BindBoolean("@privateOnly", privateOnly);
			BindSIDType("@owner", authenticatedUserSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SearchTemplateNotExistException();
			}
			ISearchTemplateFind result = SearchTemplateFindColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
