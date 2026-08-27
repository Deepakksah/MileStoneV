using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceGroupComponent : GeminiResourceComponent, IDeviceGroupComponent, IDisposable
{
	private static class DeviceGroupColumns
	{
		private static DatabaseColumnBinder IDDeviceGroup = new DatabaseColumnBinder("IDDeviceGroup");

		private static DatabaseColumnBinder IDParentDeviceGroup = new DatabaseColumnBinder("IDParentDeviceGroup");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder BuiltIn = new DatabaseColumnBinder("BuiltIn");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("DeviceType");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder RelationIds = new DatabaseColumnBinder("RelationIds");

		public static DeviceGroup Bind(DatabaseDataReader reader)
		{
			return new DeviceGroup
			{
				Id = IDDeviceGroup.GetGuid(reader),
				ParentGroupId = IDParentDeviceGroup.GetGuid(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				BuiltIn = BuiltIn.GetBoolean(reader),
				DeviceType = DatabaseDeviceTypeStrings.GetDeviceTypeEnum(DeviceType.GetString(reader)),
				LastModified = LastModified.GetDateTime(reader),
				RelationIds = RelationIds.GetString(reader, allowNulls: true, string.Empty)
			};
		}
	}

	public string[] QueryDeviceGroupIds(string parentGroupId, DeviceType type)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_group_ids");
			BindGuidType("@IDParentDeviceGroup", parentGroupId, allowNullOrEmpty: true);
			BindEnumType("@DeviceType", typeof(DeviceType), type);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDeviceGroup");
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

	public DeviceGroup QueryDeviceGroupNonAdmin(string deviceGroupid, DeviceType deviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_group_ids_non_admin");
			BindString("@MemberSid", base.RequestContext.AuthenticatedUserSid, 100, allowNull: false, SqlDbType.NVarChar);
			BindInternalIdType("@DeviceType", DatabaseDeviceTypeStrings.GetDeviceTypeString(deviceType));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			bool flag = false;
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDParentDeviceGroup");
			DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("IDDeviceGroup");
			while (databaseDataReader.Read())
			{
				databaseColumnBinder.GetString(databaseDataReader, allowNulls: true);
				string value = databaseColumnBinder2.GetString(databaseDataReader);
				if (deviceGroupid.Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
				}
			}
			if (flag)
			{
				return QueryDeviceGroup(deviceGroupid);
			}
			throw new DeviceGroupNotFoundException();
		});
	}

	public List<Tuple<Guid, Guid>> QueryDeviceGroupParentAndGroupIdsNonAdmin(string parentGroupId, DeviceType deviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_group_ids_non_admin");
			BindString("@MemberSid", base.RequestContext.AuthenticatedUserSid, 100, allowNull: false, SqlDbType.NVarChar);
			BindInternalIdType("@DeviceType", DatabaseDeviceTypeStrings.GetDeviceTypeString(deviceType));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Tuple<Guid, Guid>> list = new List<Tuple<Guid, Guid>>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDParentDeviceGroup");
			DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("IDDeviceGroup");
			while (databaseDataReader.Read())
			{
				string text = databaseColumnBinder.GetString(databaseDataReader, allowNulls: true);
				string g = databaseColumnBinder2.GetString(databaseDataReader);
				list.Add(new Tuple<Guid, Guid>((text != null) ? new Guid(text) : Guid.Empty, new Guid(g)));
			}
			return list;
		});
	}

	public DeviceGroup[] QueryDeviceGroups(DeviceType deviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_groups_all");
			BindInternalIdType("@DeviceDBType", DatabaseDeviceTypeStrings.GetDeviceTypeString(deviceType));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceGroup> list = new List<DeviceGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public DeviceGroup[] QueryDeviceGroups(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_groups_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceGroup> list = new List<DeviceGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryDeviceGroupMembers(string[] deviceGroupIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_group_members_by_group_ids");
			BindIdTable("@DeviceGroupIds", deviceGroupIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDevice");
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

	public DeviceGroup QueryDeviceGroup(string deviceGroupid)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_group_by_id");
			BindGuidType("@IDDeviceGroup", deviceGroupid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new DeviceGroupNotFoundException();
			}
			DeviceGroup result = DeviceGroupColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void CreateDeviceGroup(string id, string parentId, DeviceType deviceType, string name, string description)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_device_group");
			BindGuidType("@IDDeviceGroup", id);
			BindGuidType("@IDParentDeviceGroup", parentId, allowNullOrEmpty: true);
			BindInternalIdType("@DeviceType", DatabaseDeviceTypeStrings.GetDeviceTypeString(deviceType));
			BindNameType("@Name", name, allowNull: true);
			BindDescription("@Description", description, allowNull: true);
			ExecuteNonQuery();
		});
	}

	public void DeleteDeviceGroup(string id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_device_group");
			BindGuidType("@IDDeviceGroup", id);
			ExecuteNonQuery();
		});
	}

	public void UpdateDeviceGroup(string id, string name, string description)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_device_group");
			BindGuidType("@IDDeviceGroup", id);
			BindNameType("@name", name, allowNull: true);
			BindDescription("@description", description, allowNull: true);
			ExecuteNonQuery();
		});
	}

	public void AddDeviceGroupMembers(string deviceGroupId, string[] deviceIds)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("add_device_group_members");
			BindGuidType("@IDDeviceGroup", deviceGroupId);
			BindIdTable("@DeviceIds", deviceIds);
			ExecuteNonQuery();
		});
	}

	public void RemoveDeviceGroupMembers(string deviceGroupId, string[] deviceIds)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("remove_device_group_members");
			BindGuidType("@IDDeviceGroup", deviceGroupId);
			BindIdTable("@DeviceIds", deviceIds);
			ExecuteNonQuery();
		});
	}
}
