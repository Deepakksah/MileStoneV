using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MIPActionComponent : GeminiResourceComponent, IMIPActionComponent, IDisposable
{
	private static class MIPActionColumns
	{
		private static DatabaseColumnBinder IdMIPAction = new DatabaseColumnBinder("IDMIPAction");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder DefiningPluginId = new DatabaseColumnBinder("DefiningPluginId");

		private static DatabaseColumnBinder DefiningPluginName = new DatabaseColumnBinder("DefiningPluginName");

		private static DatabaseColumnBinder SelectionText = new DatabaseColumnBinder("SelectionText");

		private static DatabaseColumnBinder DescriptionText = new DatabaseColumnBinder("DescriptionText");

		private static DatabaseColumnBinder PickerText = new DatabaseColumnBinder("PickerText");

		private static DatabaseColumnBinder MIPKinds = new DatabaseColumnBinder("MIPKinds");

		private static DatabaseColumnBinder PickerText2 = new DatabaseColumnBinder("PickerText2");

		private static DatabaseColumnBinder MIPKinds2 = new DatabaseColumnBinder("MIPKinds2");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("CreatedDate");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static MIPAction Bind(DatabaseDataReader reader)
		{
			return new MIPAction
			{
				Id = IdMIPAction.GetGuid2(reader).ToString(),
				Name = Name.GetString(reader),
				DefiningPluginId = DefiningPluginId.GetGuid2(reader).ToString(),
				DefiningPluginName = DefiningPluginName.GetString(reader),
				SelectionText = SelectionText.GetString(reader),
				DescriptionText = DescriptionText.GetString(reader),
				PickerDefaultText = PickerText.GetString(reader),
				MIPKinds = MIPKinds.GetString(reader),
				PickerDefaultText2 = PickerText2.GetString(reader),
				MIPKinds2 = MIPKinds2.GetString(reader),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	public MIPAction[] QueryMIPActions()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_mip_actions");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPAction> list = new List<MIPAction>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPActionColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public MIPAction QueryMIPAction(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_mip_action_by_id");
			BindUniqueIdentifier("@IDMIPAction", new Guid(id));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new MIPActionNotFoundException();
			}
			MIPAction result = MIPActionColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MIPAction[] QueryMIPActionsByIds(string[] mipActionIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_mip_action_by_ids");
			BindIdTable("@MIPActionIds", mipActionIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPAction> list = new List<MIPAction>();
			int num = 0;
			while (databaseDataReader.Read())
			{
				list.Add(MIPActionColumns.Bind(databaseDataReader));
				num++;
			}
			if (num != mipActionIds.Length)
			{
				throw new MIPActionNotFoundException();
			}
			return list.ToArray();
		});
	}

	public string CreateMIPAction(string id, string name, string definingPluginId, string definingPluginName, string selectionText, string descriptionText, string pickerText, string mipKinds, string pickerText2, string mipKinds2)
	{
		if (string.IsNullOrEmpty(id))
		{
			id = Guid.NewGuid().ToString();
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_create_mip_action");
			BindUniqueIdentifier("@IDMIPAction", id);
			BindNameType("@Name", name);
			BindUniqueIdentifier("@DefiningPluginId", definingPluginId);
			BindNameType("@DefiningPluginName", definingPluginName);
			BindNameType("@SelectionText", selectionText);
			BindNameType("@DescriptionText", descriptionText);
			BindNameType("@PickerText", pickerText);
			BindXml("@MIPKinds", mipKinds);
			BindNameType("@PickerText2", pickerText2);
			BindXml("@MIPKinds2", mipKinds2);
			ExecuteNonQuery();
			return id;
		});
	}

	public void UpdateMIPAction(string id, string name, string definingPluginId, string definingPluginName, string selectionText, string descriptionText, string pickerText, string mipKinds, string pickerText2, string mipKinds2)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_mip_action");
			BindUniqueIdentifier("@IDMIPAction", id);
			BindNameType("@Name", name);
			BindUniqueIdentifier("@DefiningPluginId", definingPluginId);
			BindNameType("@DefiningPluginName", definingPluginName);
			BindNameType("@SelectionText", selectionText);
			BindNameType("@DescriptionText", descriptionText);
			BindNameType("@PickerText", pickerText);
			BindXml("@MIPKinds", mipKinds);
			BindNameType("@PickerText2", pickerText2);
			BindXml("@MIPKinds2", mipKinds2);
			ExecuteNonQuery();
		});
	}

	public void DeleteMIPAction(string mipActionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_delete_mip_action");
			BindUniqueIdentifier("@IDMIPAction", mipActionId);
			ExecuteNonQuery();
		});
	}
}
