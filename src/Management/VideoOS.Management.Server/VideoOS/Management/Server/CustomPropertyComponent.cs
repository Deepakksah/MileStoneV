using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class CustomPropertyComponent : GeminiResourceComponent, ICustomPropertyComponent, IDisposable
{
	private static class CustomPropertyColumns
	{
		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Value = new DatabaseColumnBinder("value");

		public static CustomProperty Bind(DatabaseDataReader reader)
		{
			return new CustomProperty
			{
				Name = Name.GetString(reader),
				Value = Value.GetString(reader, allowNulls: true)
			};
		}
	}

	private static class CustomPropertyColumnsWithObjectId
	{
		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Value = new DatabaseColumnBinder("value");

		private static DatabaseColumnBinder ObjectId = new DatabaseColumnBinder("object_id");

		public static CustomProperty BindWithObjectId(DatabaseDataReader reader)
		{
			return new CustomProperty
			{
				Name = Name.GetString(reader),
				Value = Value.GetString(reader, allowNulls: true),
				ObjectId = ObjectId.GetString(reader)
			};
		}
	}

	public CustomProperty[] GetCustomPropertiesByServerGroupHierarchy(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_custom_properties_by_server_group_hierarchy");
			BindGuidType("@serverGroupId", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<CustomProperty> list = new List<CustomProperty>();
			while (databaseDataReader.Read())
			{
				list.Add(CustomPropertyColumnsWithObjectId.BindWithObjectId(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public CustomProperty[] GetCustomProperties(string objectId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_custom_properties");
			BindGuidType("@object_id", objectId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<CustomProperty> list = new List<CustomProperty>();
			while (databaseDataReader.Read())
			{
				list.Add(CustomPropertyColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public CustomProperty[] GetCustomPropertiesByName(string name)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_custom_properties_by_name");
			BindGuidType("@name", name);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<CustomProperty> list = new List<CustomProperty>();
			while (databaseDataReader.Read())
			{
				list.Add(CustomPropertyColumnsWithObjectId.BindWithObjectId(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public CustomProperty GetCustomProperty(string objectId, string name)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_custom_property");
			BindString("@object_id", objectId, 128, allowNull: false, SqlDbType.VarChar);
			BindNameType("@name", name);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new CustomPropertyNotFoundException();
			}
			CustomProperty result = CustomPropertyColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void UpdateCustomProperties(string objectId, CustomProperty[] customPropertyArray)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_custom_properties");
			BindString("@object_id", objectId, 128, allowNull: false, SqlDbType.VarChar);
			BindCustomProperties("@custom_properties", customPropertyArray);
			ExecuteNonQuery();
		});
	}

	public void DeleteCustomProperties(string objectId, string name)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_custom_property");
			BindString("@object_id", objectId, 128, allowNull: false, SqlDbType.VarChar);
			BindNameType("@name", name);
			ExecuteNonQuery();
		});
	}
}
