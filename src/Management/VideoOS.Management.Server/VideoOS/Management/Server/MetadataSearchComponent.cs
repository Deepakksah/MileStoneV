using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class MetadataSearchComponent : GeminiResourceComponent, IMetadataSearchComponent, IDisposable
{
	private static class MetadataCategoryColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder Visibility = new DatabaseColumnBinder("Visibility");

		public static MetadataCategory Bind(DatabaseDataReader reader)
		{
			return new MetadataCategory
			{
				Id = Id.GetString(reader),
				Visibility = Visibility.GetBoolean(reader)
			};
		}
	}

	private static class MetadataFilterColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder Visibility = new DatabaseColumnBinder("Visibility");

		public static MetadataFilter Bind(DatabaseDataReader reader)
		{
			return new MetadataFilter
			{
				Id = Id.GetString(reader),
				Visibility = Visibility.GetBoolean(reader)
			};
		}
	}

	private static class MetadataNamespaceColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder Prefix = new DatabaseColumnBinder("Prefix");

		private static DatabaseColumnBinder Uri = new DatabaseColumnBinder("Uri");

		public static MetadataSearchNamespace Bind(DatabaseDataReader reader)
		{
			return new MetadataSearchNamespace
			{
				Id = Id.GetString(reader),
				Prefix = Prefix.GetString(reader),
				Uri = new Uri(Uri.GetString(reader))
			};
		}
	}

	private static class MetadataObjectDefinitionsColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder ObjectRoot = new DatabaseColumnBinder("ObjectRoot");

		public static MetadataObjectDefinition Bind(DatabaseDataReader reader)
		{
			return new MetadataObjectDefinition
			{
				Id = Id.GetString(reader),
				ObjectRoot = ObjectRoot.GetString(reader)
			};
		}
	}

	private static class MetadataSearchPathColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder PropertyPath = new DatabaseColumnBinder("Path");

		private static DatabaseColumnBinder PropertyId = new DatabaseColumnBinder("PropertyId");

		public static MetadataSearchPath Bind(DatabaseDataReader reader)
		{
			return new MetadataSearchPath
			{
				Id = Id.GetString(reader, allowNulls: true),
				Path = PropertyPath.GetString(reader, allowNulls: true),
				PropertyId = PropertyId.GetString(reader, allowNulls: true)
			};
		}
	}

	private static class MetadataClassCandidateColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		public static MetadataClassCandidate Bind(DatabaseDataReader reader)
		{
			return new MetadataClassCandidate
			{
				Id = Id.GetString(reader),
				Name = Name.GetString(reader)
			};
		}
	}

	private static class MetadataPropertyColumns
	{
		private static DatabaseColumnBinder PropertyId = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder PropertyName = new DatabaseColumnBinder("PropertyName");

		public static MetadataProperty Bind(DatabaseDataReader reader)
		{
			return new MetadataProperty
			{
				Id = PropertyId.GetString(reader, allowNulls: true),
				Name = PropertyName.GetString(reader, allowNulls: true)
			};
		}
	}

	public MetadataCategory[] QueryCategoriesByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_categories_by_ids");
			BindIdTable("@Ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MetadataCategory> list = new List<MetadataCategory>();
			while (databaseDataReader.Read())
			{
				list.Add(MetadataCategoryColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MetadataCategory QueryCategoryById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_category_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MetadataCategory)null;
			}
			MetadataCategory result = MetadataCategoryColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string[] QueryCategoryIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_categories");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MetadataFilter QueryFilterById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_filter_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MetadataFilter)null;
			}
			MetadataFilter result = MetadataFilterColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string[] QueryFilterIdsByCategoryId(string categoryId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_filters_by_category_id");
			BindGuidType("@CategoryId", categoryId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MetadataFilter[] QueryFiltersByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_filters_by_ids");
			BindIdTable("@Ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MetadataFilter> list = new List<MetadataFilter>();
			while (databaseDataReader.Read())
			{
				list.Add(MetadataFilterColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryObjectDefinitionIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_object_definition_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MetadataObjectDefinition QueryObjectDefinitionById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_object_definition_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MetadataObjectDefinition)null;
			}
			MetadataObjectDefinition result = MetadataObjectDefinitionsColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MetadataObjectDefinition[] QueryObjectDefinitionsByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_object_definitions_by_ids");
			BindIdTable("@Ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MetadataObjectDefinition> list = new List<MetadataObjectDefinition>();
			while (databaseDataReader.Read())
			{
				list.Add(MetadataObjectDefinitionsColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QuerySearchPathIdsByObjectDefinitionId(string objectDefinitionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_path_ids_by_object_definition_id");
			BindGuidType("@ObjectDefinitionId", objectDefinitionId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MetadataSearchPath QuerySearchPathById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_path_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MetadataSearchPath)null;
			}
			MetadataSearchPath result = MetadataSearchPathColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MetadataSearchPath[] QuerySearchPathsByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_paths_by_ids");
			BindIdTable("@Ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MetadataSearchPath> list = new List<MetadataSearchPath>();
			while (databaseDataReader.Read())
			{
				list.Add(MetadataSearchPathColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryPropertyIdsByFilterId(string filterId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_property_ids");
			BindGuidType("@FilterId", filterId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MetadataProperty QueryPropertyById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_property_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MetadataProperty)null;
			}
			MetadataProperty result = MetadataPropertyColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MetadataProperty[] QueryPropertiesByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_properties_by_ids");
			BindIdTable("@Ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MetadataProperty> list = new List<MetadataProperty>();
			while (databaseDataReader.Read())
			{
				list.Add(MetadataPropertyColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MetadataClassCandidate QueryClassCandidateById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_class_candidate_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MetadataClassCandidate)null;
			}
			MetadataClassCandidate result = MetadataClassCandidateColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MetadataClassCandidate[] QueryClassCandidatesByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_class_candidates_by_ids");
			BindIdTable("@Ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MetadataClassCandidate> list = new List<MetadataClassCandidate>();
			while (databaseDataReader.Read())
			{
				list.Add(MetadataClassCandidateColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryClassCandidateIdsByCategoryId(string categoryId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadata_search_class_candidate_ids_by_category_id");
			BindGuidType("@CategoryId", categoryId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void UpdateCategoryVisibility(string id, bool visibility)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_metadata_search_category_visibility");
			BindGuidType("@Id", id);
			BindBoolean("@Visibility", visibility);
			ExecuteNonQuery();
		});
	}

	public void UpdateFilterVisibility(string id, bool visibility)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_metadata_search_filter_visibility");
			BindGuidType("@Id", id);
			BindBoolean("@Visibility", visibility);
			ExecuteNonQuery();
		});
	}
}
