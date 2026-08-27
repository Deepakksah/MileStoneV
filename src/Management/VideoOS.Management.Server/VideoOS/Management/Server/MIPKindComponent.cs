using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MIPKindComponent : GeminiResourceComponent, IMIPKindComponent, IDisposable
{
	private static class MIPKindColumns
	{
		private static DatabaseColumnBinder KindId = new DatabaseColumnBinder("KindId");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder PluginId = new DatabaseColumnBinder("PluginId");

		private static DatabaseColumnBinder PluginName = new DatabaseColumnBinder("PluginName");

		private static DatabaseColumnBinder DataVersion = new DatabaseColumnBinder("DataVersion");

		private static DatabaseColumnBinder KindType = new DatabaseColumnBinder("KindType");

		private static DatabaseColumnBinder SecurityAction = new DatabaseColumnBinder("SecurityAction");

		private static DatabaseColumnBinder DisplayOnGisMap = new DatabaseColumnBinder("DisplayOnGisMap");

		private static DatabaseColumnBinder ParentKindId = new DatabaseColumnBinder("ParentKindId");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder ModifiedUser = new DatabaseColumnBinder("ModifiedUser");

		public static MIPKind Bind(DatabaseDataReader reader)
		{
			SecurityAction.GetString(reader).Split(';');
			return new MIPKind
			{
				Id = KindId.GetGuid2(reader),
				Name = Name.GetString(reader),
				PluginId = PluginId.GetGuid2(reader),
				PluginName = PluginName.GetString(reader),
				DataVersion = DataVersion.GetInt32(reader),
				KindType = KindType.GetString(reader),
				SecurityActionAndNames = SecurityAction.GetString(reader),
				DisplayOnGisMap = DisplayOnGisMap.GetBoolean(reader),
				ParentKindId = ParentKindId.GetGuid2(reader),
				LastModified = LastModified.GetDateTime(reader),
				ModifiedUser = ModifiedUser.GetString(reader)
			};
		}
	}

	public void CreateKind(MIPKind kind)
	{
		if (kind == null)
		{
			throw new ArgumentNullException("kind");
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[CreateKind]");
			BindUniqueIdentifier("@KindId", kind.Id);
			BindText("@Name", kind.Name ?? "", allowNull: false);
			BindUniqueIdentifier("@PluginId", kind.PluginId);
			BindText("@PluginName", kind.PluginName ?? "", allowNull: false);
			BindText("@KindType", kind.KindType, allowNull: false);
			BindText("@SecurityAction", kind.SecurityActionAndNames, allowNull: false);
			BindBoolean("@DisplayOnGisMap", kind.DisplayOnGisMap);
			BindUniqueIdentifier("@ParentKindId", kind.ParentKindId);
			BindText("@ModifiedUser", kind.ModifiedUser ?? "", allowNull: false);
			ExecuteNonQuery();
		});
	}

	public void DeleteKind(Guid id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[DeleteKind]");
			BindUniqueIdentifier("@KindId", id);
			ExecuteNonQuery();
		});
	}

	public MIPKind QueryKind(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryKind]");
			BindUniqueIdentifier("@KindId", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MIPKind)null;
			}
			MIPKind result = MIPKindColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MIPKind[] QueryKindsByParentKindId(Guid parentKindId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryKindsByParentKindId]");
			BindUniqueIdentifier("@ParentKindId", parentKindId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPKind> list = new List<MIPKind>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPKindColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public void UpdateKind(MIPKind kind)
	{
		if (kind == null)
		{
			throw new ArgumentNullException("kind");
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[UpdateKind]");
			BindUniqueIdentifier("@KindId", kind.Id);
			BindText("@Name", kind.Name ?? "", allowNull: false);
			BindText("@PluginName", kind.PluginName ?? "", allowNull: false);
			BindInt("@DataVersion", kind.DataVersion);
			BindText("@KindType", kind.KindType, allowNull: false);
			BindText("@SecurityAction", kind.SecurityActionAndNames, allowNull: false);
			BindBoolean("@DisplayOnGisMap", kind.DisplayOnGisMap);
			BindUniqueIdentifier("@ParentKindId", kind.ParentKindId);
			BindDateTime("@LastModifiedBefore", kind.LastModified);
			BindText("@ModifiedUser", kind.ModifiedUser ?? "", allowNull: false);
			ExecuteNonQuery();
		});
	}

	public void SaveKind(MIPKind kind)
	{
		if (kind == null)
		{
			throw new ArgumentNullException("kind");
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[SaveKind]");
			BindUniqueIdentifier("@KindId", kind.Id);
			BindText("@Name", kind.Name ?? "", allowNull: false);
			BindUniqueIdentifier("@PluginId", kind.PluginId);
			BindText("@PluginName", kind.PluginName ?? "", allowNull: false);
			BindInt("@DataVersion", kind.DataVersion);
			BindText("@KindType", kind.KindType, allowNull: false);
			BindText("@SecurityAction", kind.SecurityActionAndNames, allowNull: false);
			BindBoolean("@DisplayOnGisMap", kind.DisplayOnGisMap);
			BindUniqueIdentifier("@ParentKindId", kind.ParentKindId);
			BindDateTime("@LastModifiedBefore", kind.LastModified);
			BindText("@ModifiedUser", kind.ModifiedUser ?? "", allowNull: false);
			ExecuteNonQuery();
		});
	}

	public MIPKind[] QueryKinds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryKinds]");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPKind> list = new List<MIPKind>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPKindColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public MIPKind[] QueryKindsByPluginId(Guid pluginId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryKindsByPluginId]");
			BindUniqueIdentifier("@PluginId", pluginId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPKind> list = new List<MIPKind>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPKindColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public Dictionary<Guid, string> QueryPlugins()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("[Mip].[QueryPlugins]");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("PluginId");
			DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("PluginName");
			Dictionary<Guid, string> dictionary = new Dictionary<Guid, string>();
			while (databaseDataReader.Read())
			{
				dictionary[databaseColumnBinder.GetGuid2(databaseDataReader)] = databaseColumnBinder2.GetString(databaseDataReader);
			}
			return dictionary;
		});
	}
}
