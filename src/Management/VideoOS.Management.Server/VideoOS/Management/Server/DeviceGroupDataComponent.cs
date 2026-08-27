using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceGroupDataComponent : GeminiResourceComponent, IDeviceGroupDataComponent, IDisposable
{
	private static class DeviceGroupColumns
	{
		private static DatabaseColumnBinder IDDeviceGroup = new DatabaseColumnBinder("IDDeviceGroup");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder IDParentDeviceGroup = new DatabaseColumnBinder("IDParentDeviceGroup");

		private static DatabaseColumnBinder DeviceType = new DatabaseColumnBinder("DeviceType");

		public static DeviceGroupData Bind(DatabaseDataReader reader)
		{
			string input = IDParentDeviceGroup.GetString(reader, allowNulls: true);
			string deviceType = DeviceType.GetString(reader);
			Guid parentGroupId = Guid.Empty;
			if (Guid.TryParse(input, out var result))
			{
				parentGroupId = result;
			}
			return Bind(reader, parentGroupId, deviceType);
		}

		public static DeviceGroupData Bind(DatabaseDataReader reader, Guid parentGroupId, string deviceType)
		{
			return new DeviceGroupData
			{
				Id = IDDeviceGroup.GetGuid2(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				ParentDeviceGroupId = parentGroupId,
				DeviceType = deviceType
			};
		}
	}

	private static class DeviceIdColumn
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder IDDeviceGroup = new DatabaseColumnBinder("IDDeviceGroup");

		public static KeyValuePair<Guid, Guid> Bind(DatabaseDataReader reader)
		{
			Guid guid = IDDevice.GetGuid2(reader);
			return new KeyValuePair<Guid, Guid>(IDDeviceGroup.GetGuid2(reader), guid);
		}
	}

	public IEnumerable<DeviceGroupData> QueryDeviceGroupsAll()
	{
		string sql = "SELECT CAST([IDDeviceGroup] AS UNIQUEIDENTIFIER) AS [IDDeviceGroup], [Name], [Description], [DeviceType], [IDParentDeviceGroup]\r\n                                   FROM   [DeviceGroups]\r\n                                   WHERE [BuiltIn] = '0'";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DeviceGroupData> list = new List<DeviceGroupData>();
			while (databaseDataReader.Read())
			{
				list.Add(DeviceGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public Dictionary<Guid, IEnumerable<Guid>> QueryDeviceGroupMembersAll()
	{
		string sql = "SELECT CAST([IDDeviceGroup] AS UNIQUEIDENTIFIER) AS [IDDeviceGroup],  CAST([IDDevice] AS UNIQUEIDENTIFIER) AS [IDDevice]\r\n                               FROM   [DeviceGroupMembers]";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<Guid, IEnumerable<Guid>> dictionary = new Dictionary<Guid, IEnumerable<Guid>>();
			while (databaseDataReader.Read())
			{
				KeyValuePair<Guid, Guid> keyValuePair = DeviceIdColumn.Bind(databaseDataReader);
				if (dictionary.TryGetValue(keyValuePair.Key, out var value))
				{
					((List<Guid>)value).Add(keyValuePair.Value);
				}
				else
				{
					List<Guid> value2 = new List<Guid> { keyValuePair.Value };
					dictionary.Add(keyValuePair.Key, value2);
				}
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return dictionary;
		});
	}

	public Dictionary<Guid, HashSet<Guid>> QueryDeviceGroupMembersAllForType(DeviceType deviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			DatabaseDeviceTypeStrings.GetDeviceTypeString(deviceType);
			PrepareStoredProcedure("query_device_group_members");
			BindString("@DeviceDBType", DatabaseDeviceTypeStrings.GetDeviceTypeString(deviceType), 100, allowNull: false, SqlDbType.VarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<Guid, HashSet<Guid>> dictionary = new Dictionary<Guid, HashSet<Guid>>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDeviceGroup");
			DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				string g = databaseColumnBinder.GetString(databaseDataReader, allowNulls: true);
				string g2 = databaseColumnBinder2.GetString(databaseDataReader);
				Guid key = new Guid(g);
				Guid item = new Guid(g2);
				if (dictionary.TryGetValue(key, out var value))
				{
					value.Add(item);
				}
				else
				{
					dictionary.Add(key, new HashSet<Guid> { item });
				}
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return dictionary;
		});
	}

	public Dictionary<Guid, HashSet<Guid>> QueryDeviceGroupMembersAllNonAdmin(DeviceType deviceType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_group_members_non_admin");
			BindString("@DeviceType", DatabaseDeviceTypeStrings.GetDeviceTypeString(deviceType), 100, allowNull: false, SqlDbType.VarChar);
			BindString("@MemberSid", base.RequestContext.AuthenticatedUserSid, 100, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<Guid, HashSet<Guid>> dictionary = new Dictionary<Guid, HashSet<Guid>>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDDeviceGroup");
			DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("IDDevice");
			while (databaseDataReader.Read())
			{
				string g = databaseColumnBinder.GetString(databaseDataReader, allowNulls: true);
				string g2 = databaseColumnBinder2.GetString(databaseDataReader);
				Guid key = new Guid(g);
				Guid item = new Guid(g2);
				if (dictionary.TryGetValue(key, out var value))
				{
					value.Add(item);
				}
				else
				{
					dictionary[key] = new HashSet<Guid> { item };
				}
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return dictionary;
		});
	}
}
