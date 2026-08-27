using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class HardwareComponent : GeminiResourceComponent, IHardwareComponent, IDisposable
{
	private static class HardwareBlockColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("hardware_id");

		private static DatabaseColumnBinder Expires = new DatabaseColumnBinder("expires");

		private static DatabaseColumnBinder Owner = new DatabaseColumnBinder("owner");

		private static DatabaseColumnBinder Data = new DatabaseColumnBinder("data");

		public static HardwareBlock Bind(DatabaseDataReader reader)
		{
			return new HardwareBlock
			{
				Id = Id.GetInt32(reader),
				HardwareId = HardwareId.GetGuid(reader),
				Expires = Expires.GetDateTime(reader),
				Owner = Owner.GetString(reader),
				Data = Data.GetString(reader)
			};
		}
	}

	private static class HardwareColumns
	{
		private static DatabaseColumnBinder IDHardware = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder IDHardwareDriver = new DatabaseColumnBinder("IDHardwareDriver");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder HostedHardwareId = new DatabaseColumnBinder("HostedHardware_id");

		private static DatabaseColumnBinder LoginID = new DatabaseColumnBinder("LoginID");

		private static DatabaseColumnBinder Address = new DatabaseColumnBinder("Address");

		private static DatabaseColumnBinder VendorProductID = new DatabaseColumnBinder("VendorProductID");

		private static DatabaseColumnBinder DriverNumber = new DatabaseColumnBinder("DriverNumber");

		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder DetectedModel = new DatabaseColumnBinder("DetectedModel");

		private static DatabaseColumnBinder CredentialsLastModified = new DatabaseColumnBinder("CredentialsLastModified");

		private static DatabaseColumnBinder HardwareDataLastUpdated = new DatabaseColumnBinder("HardwareDataLastUpdated");

		public static Hardware Bind(DatabaseDataReader reader)
		{
			int @int = HostedHardwareId.GetInt32(reader, allowNulls: true, -1);
			DateTime dateTime = CredentialsLastModified.GetDateTime(reader, allowNulls: true, DateTime.MinValue);
			DateTime dateTime2 = HardwareDataLastUpdated.GetDateTime(reader, allowNulls: true, DateTime.MinValue);
			return new Hardware
			{
				Id = IDHardware.GetGuid(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				Address = Address.GetString(reader),
				CreatedDate = DateTime.MinValue,
				LastModified = LastModified.GetDateTime(reader),
				Enabled = Enabled.GetBoolean(reader),
				HardwareDriverId = IDHardwareDriver.GetGuid(reader),
				LoginId = LoginID.GetString(reader),
				ProductId = VendorProductID.GetString(reader),
				DriverNumber = DriverNumber.GetInt32(reader),
				RecordingServerId = IDRecorder.GetGuid(reader),
				DetectedModel = DetectedModel.GetString(reader, allowNulls: true),
				HostedHardwareId = ((@int == -1) ? ((int?)null) : new int?(@int)),
				CredentialsLastModified = ((dateTime == DateTime.MinValue) ? ((DateTime?)null) : new DateTime?(dateTime)),
				HardwareDataLastUpdated = ((dateTime2 == DateTime.MinValue) ? ((DateTime?)null) : new DateTime?(dateTime2))
			};
		}
	}

	private static class HardwareEventsColumns
	{
		private static readonly DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static readonly DatabaseColumnBinder HardwareId = new DatabaseColumnBinder("IDHardware");

		private static readonly DatabaseColumnBinder HardwareDefinitionDigest = new DatabaseColumnBinder("HardwareDefinitionDigest");

		private static readonly DatabaseColumnBinder EventSettings = new DatabaseColumnBinder("EventSettings");

		private static readonly DatabaseColumnBinder HardwareDefinition = new DatabaseColumnBinder("HardwareDefinition");

		public static DeviceEvents Bind(DatabaseDataReader reader)
		{
			return new DeviceEvents
			{
				Id = HardwareId.GetGuid(reader),
				LastModified = LastModified.GetDateTime(reader),
				HardwareId = HardwareId.GetGuid(reader),
				HardwareDefinitionDigest = HardwareDefinitionDigest.GetString(reader),
				EventSettings = EventSettings.GetString(reader),
				HardwareDefinition = HardwareDefinition.GetString(reader)
			};
		}
	}

	public HardwareComponent()
	{
	}

	public HardwareComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public Hardware[] QueryHardwareByRecordingServer(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_by_recorder");
			BindGuidType("@IDRecorder", recordingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Hardware> list = new List<Hardware>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Hardware QueryHardwareById(string hardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware");
			BindGuidType("@IDHardware", hardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Hardware)null;
			}
			Hardware result = HardwareColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Hardware[] QueryHardwareByIds(IEnumerable<string> ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Hardware> list = new List<Hardware>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryHardwareIdsByRecorder(string recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_ids_by_recorder");
			BindGuidType("@IDRecorder", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDHardware");
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

	public void UpdateHardware(string id, string name, string description, string hardwareAddress, string userName, bool? enabled, string password, Guid sessionId, DateTime? credentialsLastModified, DateTime? hardwareDataLastUpdated)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_hardware");
			BindGuidType("@IDHardware", id);
			BindNameType("@name", name, allowNull: true);
			BindDescription("@description", description, allowNull: true);
			BindUriTypeShort("@URI", hardwareAddress, allowNull: true);
			BindDescription("@LoginID", userName, allowNull: true);
			BindBoolean("@enabled", enabled);
			if (string.IsNullOrEmpty(password))
			{
				BindPasswordType("@encryptedPassword", password, allowNull: true);
			}
			else
			{
				BindPasswordType("@encryptedPassword", RequestContext.GetEncryptionService().Encrypt(password), allowNull: true);
			}
			BindGuidType("@session_id", sessionId.ToString());
			BindDateTime("@credentialsLastModified", credentialsLastModified);
			BindDateTime("@hardwareDataLastUpdated", hardwareDataLastUpdated);
			ExecuteNonQuery();
		});
	}

	public string QueryPasswordById(string hardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware");
			BindGuidType("@IDHardware", hardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (string)null;
			}
			string text = new DatabaseColumnBinder("EncryptedPassword").GetString(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return (text == null) ? null : RequestContext.GetEncryptionService().Decrypt(text);
		});
	}

	public int CreateHardwareBlock(string hardwareId, DateTime expires, string owner, string data)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_hardware_block");
			BindGuidType("@hardware_id", hardwareId);
			BindDateTime("@expires", expires);
			BindString("@owner", owner, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@data", data, 4000, allowNull: false, SqlDbType.NVarChar);
			return (int)ExecuteScalar();
		});
	}

	public void DeleteHardwareBlock(int id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_hardware_block");
			BindInt("@pkid", id);
			ExecuteScalar();
		});
	}

	public HardwareBlock[] QueryHardwareBlocks()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_blocks");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HardwareBlock> list = new List<HardwareBlock>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareBlockColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public DeviceEvents QueryHardwareEvents(string hardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_events_and_definition_by_hardware_id");
			BindGuidType("@IDHardware", hardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return databaseDataReader.Read() ? HardwareEventsColumns.Bind(databaseDataReader) : null;
		});
	}
}
