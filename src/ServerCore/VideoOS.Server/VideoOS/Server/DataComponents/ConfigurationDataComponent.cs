using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Plugins;
using VideoOS.Licensing;
using VideoOS.Management.Common;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents;

public sealed class ConfigurationDataComponent : VmoResourceComponent, IConfigurationDataComponent, IDisposable
{
	private static class SingleStringColumn
	{
		private static DatabaseColumnBinder StringResult = new DatabaseColumnBinder("StringResult");

		public static string Bind(DatabaseDataReader reader)
		{
			return StringResult.GetString(reader);
		}
	}

	private static class SingleIDColumn
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("ID");

		public static Guid Bind(DatabaseDataReader reader)
		{
			return ID.GetStringAsGuid(reader);
		}
	}

	private static class IdNameDescriptionDataColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("ID");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		public static IdNameDescription Bind(DatabaseDataReader reader)
		{
			IdNameDescription idNameDescription = new IdNameDescription();
			idNameDescription.Id = ID.GetStringAsGuid(reader);
			idNameDescription.Name = Name.GetString(reader);
			idNameDescription.Description = Description.GetString(reader);
			return idNameDescription;
		}
	}

	public ConfigurationDataComponent()
	{
	}

	public ConfigurationDataComponent(ISqlConnectionManager connectionManager, ISymmetricEncryption encryption)
		: base(connectionManager, encryption)
	{
	}

	public string FindRecorderNameByRecorderId(Guid recorderId)
	{
		string sql = "SELECT r.[Name] AS [StringResult] FROM [Recorders] r WHERE r.[IDRecorder] = @IDRecorder";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRecorder", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindRecorderNameByDeviceId(Guid deviceId)
	{
		string sql = "SELECT r.[Name] AS [StringResult] FROM [Recorders] r\r\n                                INNER JOIN [Hardware] h ON r.[IDRecorder] = h.[IDRecorder]\r\n                                INNER JOIN [Devices] d ON h.[IDHardware] = d.[IDHardware]\r\n                            WHERE d.[IDDevice] = @IDDevice";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindRecorderWebServerUriByDevice(Guid deviceId)
	{
		string sql = "SELECT iif([certificate_configured] = 1, 'https://', 'http://') + [webserver_hostname] + ':' + cast([webserver_port] as varchar(256)) + '/' AS [StringResult]\r\n                            FROM RecordingServerService rss\r\n                            INNER JOIN [RecordingServerServiceRecorder] rssr ON rss.pkid = rssr.recording_server_service_id\r\n                            INNER JOIN [Hardware] h ON rssr.[recorder_id] = h.[IDRecorder]\r\n                            INNER JOIN [Devices] d ON h.[IDHardware] = d.[IDHardware]\r\n                            WHERE d.[IDDevice] = @IDDevice\r\n                            AND rssr.active = 1";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public Guid FindRecorderIdFromServiceId(Guid serviceId)
	{
		string sql = "SELECT [recorder_id] FROM [RecordingServerServiceRecorder] WHERE [recording_server_service_id] = @serviceId";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@serviceId", serviceId);
			string text = (string)ExecuteScalar();
			return (text == null) ? Guid.Empty : new Guid(text);
		});
	}

	public Guid QueryServiceIdFromRecorderId(Guid recorderId, RecordingServerServiceType serviceType, bool activeServiceOnly)
	{
		string sql = "SELECT [recording_server_service_id] FROM [RecordingServerServiceRecorder] WHERE [recorder_id] = @recorder_id AND [type] = @type AND ([active] = 1 OR @active_only = 0)";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@recorder_id", recorderId);
			BindInt("@type", (int)serviceType);
			BindBoolean("@active_only", activeServiceOnly);
			string text = (string)ExecuteScalar();
			return (text == null) ? Guid.Empty : new Guid(text);
		});
	}

	public Guid FindDefaultStorageIdByRecorder(Guid recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_default_storage_id_by_recording_server");
			BindGuidType("@recording_server_id", recorderId);
			string text = (string)ExecuteScalar();
			return (text == null) ? Guid.Empty : new Guid(text);
		});
	}

	public Guid FindRecorderIdByRecordingStorageId(Guid recordingStorageId)
	{
		string sql = "SELECT [recordingserver_id] AS [ID] FROM [RecordingStorage] WHERE [pkid] = @pkid";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@pkid", recordingStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			Guid result = SingleIDColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public Guid FindRecordingServerServiceIdByArchiveStorageId(Guid archiveStorageId)
	{
		string sql = "SELECT rssr.[recording_server_service_id] AS [ID] FROM [RecordingServerServiceRecorder] rssr\r\n                        INNER JOIN [Recorders]r ON r.[IDRecorder] = rssr.[recorder_id]\r\n                        INNER JOIN [RecordingStorage] rs ON rs.[recordingserver_id] = r.[IDRecorder]\r\n                        INNER JOIN [ArchiveStorage] [as] ON [as].[recordingstorage_id] = rs.[pkid]\r\n                        WHERE rssr.[type] = 0 AND [as].[pkid] = @pkid";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@pkid", archiveStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			Guid result = SingleIDColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public Guid FindRecorderIdByHardwareId(Guid hardwareId)
	{
		string sql = "SELECT h.[IDRecorder] AS [ID] FROM [Hardware] h WHERE h.[IDHardware] = @IDHardware";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDHardware", hardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			Guid result = SingleIDColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public Guid QueryRecorderIdByDeviceId(Guid deviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch("SELECT h.[IDRecorder] AS [ID] FROM [Hardware] h INNER JOIN [Devices] d ON h.[IDHardware] = d.[IDHardware] WHERE d.[IDDevice] = @IDDevice".Length);
			AddStatement("SELECT h.[IDRecorder] AS [ID] FROM [Hardware] h INNER JOIN [Devices] d ON h.[IDHardware] = d.[IDHardware] WHERE d.[IDDevice] = @IDDevice");
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return (!databaseDataReader.Read()) ? Guid.Empty : SingleIDColumn.Bind(databaseDataReader);
		});
	}

	public Guid FindActiveRecorderServiceIdByDeviceId(Guid deviceId)
	{
		string sql = "SELECT rssr.[recording_server_service_id] AS [ID] FROM [RecordingServerServiceRecorder] rssr\r\n                                INNER JOIN [Hardware] h ON h.[IDRecorder] = rssr.[recorder_id]\r\n                                INNER JOIN [Devices] d ON h.[IDHardware] = d.[IDHardware]\r\n                                WHERE rssr.[active] = 1 AND d.[IDDevice] = @IDDevice";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			Guid result = SingleIDColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindHardwareNameByHardwareId(Guid hardwareId)
	{
		string sql = "SELECT [Name] AS [StringResult] FROM [Hardware] WHERE IDHardware = @IDHardware";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDHardware", hardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindHardwareNameByDeviceId(Guid deviceId)
	{
		string sql = "SELECT h.[Name] AS [StringResult] FROM [Hardware] h\r\n                                INNER JOIN [Devices] d ON h.[IDHardware] = d.[IDHardware]\r\n                            WHERE d.IDDevice = @IDDevice";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindHardwareMacAddressByDeviceId(Guid deviceId)
	{
		string sql = "SELECT\r\n                                UPPER(h.[MacAddress]) AS [MacAddress]\r\n                                FROM\r\n                                [Hardware] h\r\n                                INNER JOIN [Devices] d ON h.[IDHardware] = d.[IDHardware]\r\n                                WHERE d.IDDevice = @IDDevice";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			return (string)ExecuteScalar();
		});
	}

	public string[] QueryHardwareAddressesByRecorder(Guid recorderId)
	{
		string sql = "SELECT [URI]\r\n                        FROM   [Hardware]\r\n                        WHERE  [IDRecorder] = @IDRecorder";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRecorder", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(databaseDataReader.GetString(0));
			}
			if (databaseDataReader.NextResult())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return list.ToArray();
		});
	}

	public string FindDeviceNameByDeviceId(Guid deviceId)
	{
		string sql = "SELECT d.[Name] AS [StringResult] FROM [Devices] d WHERE d.[IDDevice] = @IDDevice";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindMatrixNameByMatrixId(Guid matrixId)
	{
		string sql = "SELECT m.[Name] AS [StringResult] FROM [Matrix] m WHERE m.[IDMatrix] = @IDMatrix";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDMatrix", matrixId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindDeviceTypeByDeviceId(Guid deviceId)
	{
		string sql = "SELECT d.[DeviceType] AS [StringResult] FROM [Devices] d WHERE d.[IDDevice] = @IDDevice";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindRuleNameByRuleId(Guid ruleId)
	{
		string sql = "SELECT r.[Name] AS [StringResult] FROM [Rules] r WHERE r.[IDRule] = @IDRule";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRule", ruleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindRoleNameByRoleId(Guid roleId)
	{
		string sql = "SELECT [Name] AS [StringResult] FROM [SecurityRoles] WHERE [IDRole] = @IDRole";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRole", roleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindEventTypeDefinitionByEventTypeId(Guid eventTypeId)
	{
		string sql = "SELECT e.[Definition] AS [StringResult] FROM [EventTypes] e WHERE e.[IDEventType] = @IDEventType";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDEventType", eventTypeId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string QueryDefinitionDigestByDeviceId(Guid deviceId)
	{
		string sql = "SELECT h.[C18NDigest]\r\n                        FROM   [Devices] d\r\n                        INNER JOIN [Hardware] h ON h.[IDHardware] = d.[IDHardware]\r\n                        WHERE  d.[IDDevice] = @IDDevice";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			return ExecuteScalar() as string;
		});
	}

	public Guid QueryDeviceEdgeTrackIdByDeviceId(Guid deviceId)
	{
		string sql = "SELECT dt.[IDTrack] FROM [DeviceTrack] dt \r\n                            INNER JOIN [DeviceTrackDefinition] dtd ON dt.IDTrackDefinition = dtd.IDTrackDefinition\r\n                            WHERE dt.[IDDevice] = @IDDevice AND dtd.IsEdge = 1";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDDevice", deviceId);
			object obj = ExecuteScalar();
			return (obj != null) ? ((Guid)obj) : Guid.Empty;
		});
	}

	public Guid[] QueryDeviceIdsByRecorder(Guid recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_ids_generic");
			BindGuidType("@IDRecorder", recorderId);
			BindNullValue("@IDHardware", SqlDbType.Char);
			BindNullValue("@DeviceTypeSearch", SqlDbType.VarChar);
			List<Guid> list = new List<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				string g = databaseDataReader.GetString(0);
				Guid item = new Guid(g);
				list.Add(item);
			}
			return list.ToArray();
		});
	}

	public string FindEventTypeGeneratorTypeByEventTypeId(Guid eventTypeId)
	{
		string sql = "SELECT e.[GeneratorType] AS [StringResult] FROM [EventTypes] e WHERE e.[IDEventType] = @IDEventType";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDEventType", eventTypeId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindEventTypeGeneratorSubtypeByEventTypeId(Guid eventTypeId)
	{
		string sql = "SELECT e.[GeneratorSubType] AS [StringResult] FROM [EventTypes] e WHERE e.[IDEventType] = @IDEventType";
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareSqlBatch(sql.Length);
				AddStatement(sql);
				BindGuidType("@IDEventType", eventTypeId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				if (!databaseDataReader.Read())
				{
					throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
				}
				string result = SingleStringColumn.Bind(databaseDataReader);
				if (databaseDataReader.Read())
				{
					throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
				}
				return result;
			});
		}
		catch (DatabaseException)
		{
			throw new ConfigurationDataSubsystemException(Resources.GenerelSqlError);
		}
		catch (NoNullAllowedException)
		{
			throw new ConfigurationDataSubsystemException(Resources.NoNullAllowedException);
		}
	}

	public Guid FindUserDefinedEventTypeIdByName(string eventName)
	{
		string sql = "SELECT e.[IDEventType] AS [ID] FROM [EventTypes] e WHERE e.[GeneratorType] = 'External' AND e.[Name] = @Name";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindNameType("@Name", eventName);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			Guid result = SingleIDColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public Guid FindHardwareEventTypeIdByName(string eventName)
	{
		string sql = "SELECT e.[IDEventType] AS [ID] FROM [EventTypes] e WHERE e.[GeneratorType] = 'DriverDevice' AND e.[Name] = @Name";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindNameType("@Name", eventName);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return Guid.Empty;
			}
			Guid result = SingleIDColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public Guid FindHardwareEventTypeIdByNameAndDisplayName(string eventName, string eventDisplayName)
	{
		string sql = "SELECT e.[IDEventType] AS [ID] FROM [EventTypes] e WHERE (e.[GeneratorType] = 'DriverDevice' OR e.[GeneratorType] = 'DriverHardware') AND ([Definition].value('/EventType[1]/DisplayName[1]', 'NVARCHAR(MAX)') = @eventDisplayName OR e.[Name] = @eventName)";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindNameType("@eventName", eventName);
			BindNameType("@eventDisplayName", eventDisplayName);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return (!databaseDataReader.Read()) ? Guid.Empty : SingleIDColumn.Bind(databaseDataReader);
		});
	}

	public string GetNavigationXml()
	{
		string sql = "SELECT [Setting] AS [StringResult] FROM [SystemSettings] WHERE [Name] = 'NavigationXML'";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindRecordingStorageNameByRecordingStorageId(Guid recordingStorageId)
	{
		string sql = "SELECT m.[name] AS [StringResult] FROM [RecordingStorage] m WHERE m.[pkid] = @RecordingStorageId";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@RecordingStorageId", recordingStorageId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public IdNameDescription FindHomeSiteInfo()
	{
		string sql = "SELECT [IDSite] AS [ID], [Name], [Description] FROM [Sites] WHERE [IDSiteType] = '1'";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			IdNameDescription result = IdNameDescriptionDataColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string QueryTemporaryLicenseStorageData()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT [Data] FROM [License] WITH(UPDLOCK) WHERE [IDLicense] = @IDLicense");
			BindUniqueIdentifier("@IDLicense", new Guid("968FBC5E-99BD-4E08-B711-8A73D2A37461"));
			byte[] value = ExecuteScalar() as byte[];
			value = base.Encryption.DecryptSequence(value);
			return Encoding.UTF8.GetString(value);
		});
	}

	public Guid QueryOriginalIdByMacAddressDeviceIndex(string macAddress, int deviceIndex)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT [OriginalIdDevice] FROM [Devices] d\r\nINNER JOIN [Hardware] h on h.[IDHardware] = d.[IDHardware]\r\nWHERE h.[MacAddress] = @MacAddress AND d.[DeviceIndex] = @DeviceIndex AND d.[DeviceType] = 'Camera'");
			BindInt("@DeviceIndex", deviceIndex);
			BindString("@MacAddress", macAddress, 256, allowNull: false, SqlDbType.VarChar);
			return (!(ExecuteScalar() is string g)) ? Guid.Empty : new Guid(g);
		});
	}

	public string QueryUniqueIdByMacAddressAndOriginalId(string macAddress, string originalId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT ld.[UniqueId] \r\nFROM[Hardware] h \r\nJOIN[Devices] d ON d.IDHardware = h.IDHardware \r\nINNER JOIN[LicenseDevice] ld ON ld.[IDDevice] = d.[OriginalIdDevice] \r\nWHERE h.[MacAddress] = @MacAddress AND d.[OriginalIdDevice] = @DeviceId AND d.[DeviceType] = 'Camera'");
			BindString("@MacAddress", macAddress, 30, allowNull: false, SqlDbType.VarChar);
			BindString("@DeviceId", originalId, 80, allowNull: false, SqlDbType.VarChar);
			string text = ExecuteScalar() as string;
			return text ?? string.Empty;
		});
	}

	public void WriteTemporaryLicenseStorageData(string data)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("UPDATE [License] SET [LastModified] = @LastModified, [Data] = @Data WHERE [IDLicense] = @IDLicense");
			BindDateTime("@LastModified", DateTime.UtcNow);
			BindUniqueIdentifier("@IDLicense", new Guid("968FBC5E-99BD-4E08-B711-8A73D2A37461"));
			byte[] bytes = Encoding.UTF8.GetBytes(data);
			byte[] parameterValue = base.Encryption.EncryptSequence(bytes);
			BindByteArray("@Data", parameterValue, allowNull: false);
			ExecuteNonQuery();
		});
	}

	public void UpdateRecorderStatusData(Guid recorderId, string type, string xml)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recorder_statusdata");
			BindGuidType("@IDRecorder", recorderId);
			BindString("@Type", type, 256, allowNull: false, SqlDbType.VarChar);
			BindXml("@Data", xml, allowNull: false);
			ExecuteNonQuery();
		});
	}

	public void PromoteNewEncryptionPassword(Guid recordingStorageId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_recording_storage_promote_new_password");
			BindGuidType("@IDRecordingStorage", recordingStorageId);
			ExecuteNonQuery();
		});
	}

	public string[] QueryExistingMacAddresses(string[] macAddresses)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_existing_mac_addresses");
			BindResultTable("@addresses", macAddresses);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(SingleStringColumn.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return list.ToArray();
		});
	}

	public void CheckConfigurationAgainstLicense(FeatureManager featureManager)
	{
		int count = 0;
		if (!featureManager.TryGetRecorderLicenseCount(out count))
		{
			throw new LicenseException("Invalid license. Recorder count does not have a valid limit", LicenseErrorNumber.InvalidLicenseFile);
		}
		if (IsAboveLimitLicenseCheck("SELECT Count([IDRecorder]) FROM [Recorders]", count))
		{
			throw new LicenseException("Unlicensed Recorders", FeatureError.UnlicensedRecorders);
		}
		if (!featureManager.IsFeatureLicensed("SNMP") && HasAnyDataLicenseCheck("SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action/@name)[1]', 'nvarchar(max)') = 'VideoOS.CreateSNMPTrap'"))
		{
			throw new LicenseException("Unlicensed SNMP feature", FeatureError.UnlicensedSnmpFeature);
		}
		if (!featureManager.IsFeatureLicensed("InterconnectedProducts") && HasAnyDataLicenseCheck(string.Format(CultureInfo.InvariantCulture, "SELECT [IDHardware] FROM [Hardware] WHERE [DriverNumber] >= {0} AND [DriverNumber] <= {1} AND [DriverNumber] <> {2}", 30000, 39999, 30005)))
		{
			throw new LicenseException("Unlicensed interconnected hardware feature", FeatureError.UnlicensedInterconnectedProductsFeature);
		}
		if (!featureManager.IsFeatureLicensed("VPS") && HasAnyDataLicenseCheck(string.Format(CultureInfo.InvariantCulture, "SELECT [IDHardware] FROM [Hardware] WHERE [DriverNumber] = {0}", 20001)))
		{
			throw new LicenseException("Unlicensed VPS feature", FeatureError.UnlicensedVpsFeature);
		}
		if (!featureManager.IsFeatureLicensed("Bookmarking") && HasAnyDataLicenseCheck("SELECT [IdMarkedData] FROM [MarkedData] WHERE [MarkDataType] = 8"))
		{
			throw new LicenseException("Unlicensed bookmark feature", FeatureError.UnlicensedBookmarkingFeature);
		}
		if (!featureManager.IsFeatureLicensed("RuleBasedBookmarking") && HasAnyDataLicenseCheck("SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action/@name)[1]', 'nvarchar(max)') = 'VideoOS.SetBookmark'"))
		{
			throw new LicenseException("Unlicensed rule bookmarking feature", FeatureError.UnlicensedRuleBasedBookmarkingFeature);
		}
		if (!featureManager.IsFeatureLicensed("FederatedSites") && HasAnyDataLicenseCheck("SELECT [IDSite] FROM [Sites] WHERE [IDSiteType] = 3"))
		{
			throw new LicenseException("Unlicensed federated sites feature", FeatureError.UnlicensedFederatedSitesFeature);
		}
		if (!featureManager.IsFeatureLicensed("AddToFederatedHierarchy") && HasAnyDataLicenseCheck("SELECT [IDSite] FROM [Sites] WHERE [IDSiteType] = 2"))
		{
			throw new LicenseException("Unlicensed federated sites feature", FeatureError.UnlicensedAddToFederatedHierarchyFeature);
		}
		CheckRecordingServerFailoverSettings(featureManager);
		CheckRemoteConnectedServicesSettings(featureManager);
		if (!featureManager.IsFeatureLicensed("Multicast") && HasAnyDataLicenseCheck("SELECT [IDDevice] FROM [Devices] WHERE [MulticastEnabled] = 1"))
		{
			throw new LicenseException("Unlicensed multicast feature", FeatureError.UnlicensedMulticastFeature);
		}
		CheckMultiStreamingSettings(featureManager);
		CheckMultiStageVideoStorageSettings(featureManager);
		CheckTimeControlledAccessRightsSettings(featureManager);
		CheckEntendedPrivacyMaskingSettings();
		if (!featureManager.IsFeatureLicensed("ArchiveReducedFrameRate") && HasAnyDataLicenseCheck("SELECT [pkid] FROM [ArchiveStorage] WHERE [framerate_reduction_enabled] = 1"))
		{
			throw new LicenseException("Unlicensed archive reduce framerate feature", FeatureError.UnlicensedArchiveReducedFramerateFeature);
		}
		if (!featureManager.IsFeatureLicensed("RecordingStorageEncryption"))
		{
			string sqlCommandText = "SELECT [pkid] FROM [RecordingStorage] WHERE [encryption_method] <> 'None'";
			if (HasAnyDataLicenseCheck(sqlCommandText))
			{
				throw new LicenseException("Unlicensed recording storage encryption feature", FeatureError.UnlicensedRecordingStorageEncryptionFeature);
			}
		}
		if (!featureManager.IsFeatureLicensed("RecordingStorageSigning") && HasAnyDataLicenseCheck("SELECT [pkid] FROM [RecordingStorage] WHERE [signing] = 1"))
		{
			throw new LicenseException("Unlicensed recording storage signing feature", FeatureError.UnlicensedRecordingStorageSigningFeature);
		}
		CheckSmartClientProfileSettings(featureManager);
		if (!featureManager.IsFeatureLicensed("SmartWall") && HasAnyDataLicenseCheck("SELECT [pkid] FROM [VideoWall]"))
		{
			throw new LicenseException("Unlicensed video wall feature", FeatureError.UnlicensedSmartWallFeature);
		}
		if (!featureManager.IsFeatureLicensed("EvidenceLock") && HasAnyDataLicenseCheck("SELECT [IDMarkedData] FROM [MarkedData] WHERE [MarkDataType] = 2"))
		{
			throw new LicenseException("Unlicensed evidence lock feature", FeatureError.UnlicensedEvidenceLockFeature);
		}
		if (!featureManager.IsFeatureLicensed("DifferentiatedAdministratorSecurity") && HasAnyDataLicenseCheck("SELECT so.[security_object_id], sae.[action_name], sae.[user_sid]\r\n                                                    FROM SecurityObject so\r\n                                                    INNER JOIN SecurityAccessEntry sae\r\n                                                        ON so.[security_object_id] = sae.[security_object_id]\r\n                                                    INNER JOIN SecurityAction sa\r\n                                                        ON so.[namespace_id] = sa.[namespace_id] AND sae.[action_name] = sa.[name]\r\n                                                    WHERE\r\n                                                        so.[object_id] = N'/'\r\n                                                        AND sa.visible = 0"))
		{
			throw new LicenseException("Unlicensed differentiated administration right feature", FeatureError.UnlicensedDiffAdminFeature);
		}
		if (!featureManager.IsFeatureLicensed("HardwareAcceleratedVMD") && AnyCameraUsesHardwareAcceleratedMotionDetection())
		{
			throw new LicenseException("Unlicensed hardware accelerated motion detection feature", FeatureError.UnlicensedHardwareAcceleratedMotionDetection);
		}
		if (!featureManager.IsFeatureLicensed("AACAudioSupport") && AnyAacPresent())
		{
			throw new LicenseException("Unlicensed AAC feature", FeatureError.UnlicensedAacFeature);
		}
		CheckEdgeStorageSettings(featureManager);
		CheckPtzPrioritySettings(featureManager);
		CheckEntendedPtzSettings(featureManager);
		if (!featureManager.IsFeatureLicensed("RestrictedMedia") && HasAnyDataLicenseCheck("SELECT IDRestrictedMedia FROM [DeviceRestrictedMedia]"))
		{
			throw new LicenseException("Unlicensed restricted media feature", FeatureError.UnlicensedRestrictedMediaFeature);
		}
	}

	private void CheckSmartClientProfileSettings(FeatureManager featureManager)
	{
		if (featureManager.IsFeatureLicensed("SmartClientProfiles"))
		{
			string featureParameterValue = featureManager.GetFeatureParameterValue("SmartClientProfiles", "SmartClientProfilesLimit");
			if (!int.TryParse(featureParameterValue, out var result))
			{
				throw new LicenseException("Invalid license. Smart client profile feature exists with invalid limit", LicenseErrorNumber.InvalidLicenseFile);
			}
			if (result < 3)
			{
				throw new LicenseException("Invalid license. Smart client profile feature exists with invalid limit", LicenseErrorNumber.InvalidLicenseFile);
			}
			if (IsAboveLimitLicenseCheck("SELECT Count([IDClientProfile]) FROM [ClientProfile] WHERE profile_type = 'SmartClientProfile'", result))
			{
				throw new LicenseException("Unlicensed smart clint profiles feature", FeatureError.UnlicensedSmartClientProfilesFeature);
			}
		}
		else
		{
			int limit = 1;
			if (IsAboveLimitLicenseCheck("SELECT Count([IDClientProfile]) FROM [ClientProfile] WHERE profile_type = 'SmartClientProfile'", limit))
			{
				throw new LicenseException("Only Default Smart client profile is permitted", FeatureError.UnlicensedSmartClientProfiles);
			}
		}
	}

	private void CheckRecordingServerFailoverSettings(FeatureManager featureManager)
	{
		if (!featureManager.IsFeatureLicensed("RecordingServerFailover"))
		{
			if (HasAnyDataLicenseCheck("SELECT [IDFailoverServer] FROM [FailoverServers]"))
			{
				throw new LicenseException("Unlicensed failover feature", FeatureError.UnlicensedRecordingServerFailoverFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT [IDServerGroup] FROM [serverGroup] WHERE [IDParentGroup] = '6A85FFC3-C134-4338-A072-024817CB5820'"))
			{
				throw new LicenseException("Unlicensed failover feature", FeatureError.UnlicensedRecordingServerFailoverFeature);
			}
		}
	}

	private void CheckRemoteConnectedServicesSettings(FeatureManager featureManager)
	{
		if (!featureManager.IsFeatureLicensed("RemoteConnectedServices"))
		{
			if (HasAnyDataLicenseCheck("SELECT [pkid] FROM [HostingServer]"))
			{
				throw new LicenseException("Unlicensed remote connected services feature", FeatureError.UnlicensedRemoteConnectedServicesFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT [pkid] FROM [HostedHardware]"))
			{
				throw new LicenseException("Unlicensed remote connected services feature", FeatureError.UnlicensedRemoteConnectedServicesFeature);
			}
		}
	}

	private void CheckMultiStageVideoStorageSettings(FeatureManager featureManager)
	{
		if (featureManager.IsFeatureLicensed("MultiStageVideoStorage"))
		{
			string featureParameterValue = featureManager.GetFeatureParameterValue("MultiStageVideoStorage", "MultiStageVideoStorageLimit");
			string sqlCommandText = string.Format(CultureInfo.InvariantCulture, "SELECT [recordingstorage_id] FROM [ArchiveStorage] GROUP BY [recordingstorage_id] HAVING COUNT([pkid]) > {0}", featureParameterValue);
			if (HasAnyDataLicenseCheck(sqlCommandText))
			{
				throw new LicenseException("Unlicensed multistage video storage feature", FeatureError.UnlicensedMultistageVideoStorageFeature);
			}
		}
	}

	private void CheckTimeControlledAccessRightsSettings(FeatureManager featureManager)
	{
		if (!featureManager.IsFeatureLicensed("TimeControlledAccessRights"))
		{
			if (HasAnyDataLicenseCheck("SELECT [IDTimeProfile] FROM [TimeProfileDefaultRoleRelation]"))
			{
				throw new LicenseException("Unlicensed time controlled access rights feature", FeatureError.UnlicensedTimeControlledAccessRightsFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT [SecurityAttributeID] FROM [SecurityPermissionsDevice] WHERE IsNULL([IDTimeProfile], '0') <> '0' AND [IsDefaultTimeProfile] = 0"))
			{
				throw new LicenseException("Unlicensed time controlled access rights feature", FeatureError.UnlicensedTimeControlledAccessRightsFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT [SecurityAttributeID] FROM [SecurityPermissions] WHERE IsNULL([IDTimeProfile], '0') <> '0' AND [IsDefaultTimeProfile] = 0"))
			{
				throw new LicenseException("Unlicensed time controlled access rights feature", FeatureError.UnlicensedTimeControlledAccessRightsFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT SAETP.[access_entry_id] FROM [SecurityAccessEntryTimeProfile] SAETP LEFT JOIN [SecurityAccessEntry] SAE ON SAETP.[access_entry_id] = SAE.[access_entry_id] WHERE [timeprofile_id] IS NOT NULL AND [is_default] = 0"))
			{
				throw new LicenseException("Unlicensed time controlled access rights feature", FeatureError.UnlicensedTimeControlledAccessRightsFeature);
			}
		}
	}

	private void CheckEdgeStorageSettings(FeatureManager featureManager)
	{
		if (!featureManager.IsFeatureLicensed("EdgeStorage"))
		{
			if (HasAnyDataLicenseCheck("\r\n                SELECT [IDHardware]\r\n                FROM [Hardware]\r\n                WHERE [EdgeSettings].exist('//setting[name = \"MaxTransferBitrateEnabled\"]/value[.=\"True\"]') = 1\r\n                OR [EdgeSettings].exist('//setting[name = \"RetrievalTimeSpanEnabled\"]/value[.=\"True\"]') = 1"))
			{
				throw new LicenseException("Unlicensed edge storage feature", FeatureError.UnlicensedEdgeStorageFeature);
			}
			if (HasAnyDataLicenseCheck("\r\n                SELECT device_id\r\n                FROM [RecordingStorageDevice]\r\n                WHERE edgestorage_enabled = 1\r\n                OR edgestorage_playback = 1"))
			{
				throw new LicenseException("Unlicensed edge storage feature", FeatureError.UnlicensedEdgeStorageFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action/@name)[1]', 'nvarchar(max)') = 'VideoOS.RetrieveEdgeStorageRecording'"))
			{
				throw new LicenseException("Unlicensed edge storage feature", FeatureError.UnlicensedRuleBasedEdgeStorageFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action/@name)[1]', 'nvarchar(max)') = 'VideoOS.RetrieveEdgeStorageRecordingTimeOrMetadata'"))
			{
				throw new LicenseException("Unlicensed edge storage feature", FeatureError.UnlicensedRuleBasedEdgeStorageFeature);
			}
		}
	}

	private void CheckPtzPrioritySettings(FeatureManager featureManager)
	{
		if (!featureManager.IsFeatureLicensed("PTZPriority"))
		{
			int value = 0;
			if (!featureManager.TryGetFeatureParameterValue("PTZPriority", "PTZPriorityLimit", out value))
			{
				throw new LicenseException("Invalid license. Prevent ptz priority flag exists without valid limit", LicenseErrorNumber.InvalidLicenseFile);
			}
			value = ((value > 32000) ? 32000 : value);
			if (HasAnyDataLicenseCheck(string.Format(CultureInfo.InvariantCulture, "SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action[@name=\"VideoOS.GotoPreset\"]/Parameter[@name=\"Priority\"])[1]', 'INT') > {0}", value)))
			{
				throw new LicenseException("Unlicensed ptz priority feature", FeatureError.UnlicensedRuleBasedPtzPriorityFeature);
			}
			if (HasAnyDataLicenseCheck(string.Format(CultureInfo.InvariantCulture, "SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action[@name=\"VideoOS.DeviceStartStopPatrolling\"]/Parameter[@name=\"Priority\"])[1]', 'INT') > {0}", value)))
			{
				throw new LicenseException("Unlicensed ptz priority feature", FeatureError.UnlicensedRuleBasedPtzPriorityFeature);
			}
			if (HasAnyDataLicenseCheck(string.Format(CultureInfo.InvariantCulture, "SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action[@name=\"VideoOS.DeviceStartStopPatrolling\"]/Parameter[@name=\"Priority\"])[1]', 'INT') > {0}", value)))
			{
				throw new LicenseException("Unlicensed ptz priority feature", FeatureError.UnlicensedRuleBasedPtzPriorityFeature);
			}
			if (HasAnyDataLicenseCheck(string.Format(CultureInfo.InvariantCulture, "SELECT [IDRule] FROM [Rules] WHERE [Definition].value('(//Actions/Action[@name=\"VideoOS.GotoDefaultPreset\"]/Parameter[@name=\"Priority\"])[1]', 'INT') > {0}", value)))
			{
				throw new LicenseException("Unlicensed ptz priority feature", FeatureError.UnlicensedRuleBasedPtzPriorityFeature);
			}
			if (HasAnyDataLicenseCheck(string.Format(CultureInfo.InvariantCulture, "SELECT [IDRole] FROM [SecurityPermissionsDevice] WHERE [SecurityAttributeID] = 'DEVICEPTZPRIORITY' AND CAST([Value] AS INT) > {0}", value)))
			{
				throw new LicenseException("Unlicensed ptz priority feature", FeatureError.UnlicensedSecurityPtzPriorityFeature);
			}
		}
	}

	private void CheckEntendedPtzSettings(FeatureManager featureManager)
	{
		if (!featureManager.IsFeatureLicensed("ExtendedPtz"))
		{
			if (HasAnyDataLicenseCheck("SELECT [IDRole] FROM [SecurityRoles] WHERE [PtzReservedAsAnonymous] = 1"))
			{
				throw new LicenseException("Unlicensed extended ptz feature", FeatureError.UnlicensedExtendedPtzAnonymousPtz);
			}
			if (HasAnyDataLicenseCheck("SELECT [IDDevice] FROM [Devices] WHERE  [Presets].exist('(/presets/preset[locked=\"True\"])') = 1"))
			{
				throw new LicenseException("Unlicensed extended ptz feature", FeatureError.UnlicensedExtendedPtzLockedPreset);
			}
		}
	}

	private static void CheckEntendedPrivacyMaskingSettings()
	{
	}

	private void CheckMultiStreamingSettings(FeatureManager featureManager)
	{
		if (!featureManager.IsFeatureLicensed("LiveMultiStreaming"))
		{
			if (HasAnyDataLicenseCheck("SELECT [IDDevice] FROM [Devices] WHERE [Streams].value('count(/streams/stream)','INT') > 2 AND CAST([Streams] AS NVARCHAR(MAX)) <> ''"))
			{
				throw new LicenseException("Unlicensed live multistreaming feature.", FeatureError.UnlicensedLiveMultistreamingFeature);
			}
			if (HasAnyDataLicenseCheck("SELECT [IDDevice] FROM [Devices] WHERE [Streams].value('count(/streams/stream[livefeed=\"True\"])','INT') <> 1 AND CAST([Streams] AS NVARCHAR(MAX)) <> ''"))
			{
				throw new LicenseException("Unlicensed live multistreaming feature", FeatureError.UnlicensedLiveMultistreamingFeature);
			}
		}
		if (!featureManager.IsFeatureLicensed("MultistreamRecording") && HasAnyDataLicenseCheck("SELECT [IDDevice] FROM [Devices]", "WHERE [Streams].value('count(/streams/stream[trackdefinition/text()!=\"\"])','INT') > 1"))
		{
			throw new LicenseException("Unlicensed multistream recording feature", FeatureError.UnlicensedMultistreamRecordingFeature);
		}
	}

	private bool IsAboveLimitLicenseCheck(string sqlCommandText, int limit)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement(sqlCommandText);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return false;
			}
			if (databaseDataReader.HasRows)
			{
				int @int = databaseDataReader.GetInt32(0);
				return @int > limit;
			}
			return false;
		});
	}

	private bool HasAnyDataLicenseCheck(params string[] sqlLines)
	{
		string sqlCommandText = string.Join("\n", sqlLines);
		return HasAnyDataLicenseCheck(sqlCommandText);
	}

	private bool HasAnyDataLicenseCheck(string sqlCommandText)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement(sqlCommandText);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			databaseDataReader.Read();
			return databaseDataReader.HasRows ? true : false;
		});
	}

	private bool AnyCameraUsesHardwareAcceleratedMotionDetection()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT rpd.[IDDevice] FROM [RecorderPluginDevice] rpd where rpd.[IDRecorderPlugin] = @motionDetectID AND Settings.exist('(properties/settings/setting [name/text()=\"HardwareAccelerationMode\" and value/text()=\"Automatic\"])[1]')=1");
			BindGuidType("@motionDetectID", WellKnownPluginIds.MotionPluginId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			databaseDataReader.Read();
			return databaseDataReader.HasRows;
		});
	}

	private bool AnyAacPresent()
	{
		string sqlCommandText = "SELECT hw.[IDHardware] FROM [Hardware] hw where Settings.exist('(properties/settings/setting [name/text()=\"AudioEncoding\" and contains((value/text())[1],\"AAC\")])[1]')=1";
		string sqlCommandText2 = "SELECT d.[IDDevice] FROM [Devices] d where d.DeviceType IN ('AudioInput', 'AudioOutput') AND Settings.exist('(properties/settings/setting [name/text()=\"AudioEncoding\" and contains((value/text())[1],\"AAC\")])[1]')=1";
		if (!HasAnyDataLicenseCheck(sqlCommandText))
		{
			return HasAnyDataLicenseCheck(sqlCommandText2);
		}
		return true;
	}

	public string[] QueryRecorders()
	{
		return QueryRecordingServerIds(new Guid("909749AE-88C4-4780-A381-383D691E98FA"));
	}

	private string[] QueryRecordingServerIds(Guid serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_servers_ids");
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRecorder");
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

	public Guid[] QueryTracksWithPossibleMediaOnRecorder(Guid recorderId)
	{
		string[] value = new string[7] { "SELECT dt.IDTrack as track_Id FROM [Devices] AS d", "  INNER JOIN [Hardware] AS h ON d.[IDHardware] = h.[IDHardware]", "  INNER JOIN [DeviceTrack] AS dt ON d.[IDDevice] = dt.[IDDevice]", "WHERE (h.[IDRecorder] = @IDRecorder)", "UNION ALL", "SELECT [track_id] FROM [DeviceDataDistribution]", "WHERE ([recorder_id] = @IDRecorder)" };
		string sql = string.Join(" ", value);
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRecorder", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Guid> list = new List<Guid>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("track_Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetGuid2(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string QueryManagementNamebyId(Guid serverId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_management_server_name");
			BindGuidType("@ServerId", serverId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Setting");
			if (databaseDataReader.Read())
			{
				return databaseColumnBinder.GetString(databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return string.Empty;
		});
	}

	public string QueryRegisteredServerNamebyId(Guid serverId)
	{
		string sql = "SELECT [Addresses] FROM [RegisteredServers] r WHERE r.IDServer = @IdServer";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IdServer", serverId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Addresses");
			ServerAddress[] array = ServerAddresses.GetFromXml(databaseColumnBinder.GetString(databaseDataReader)).ToArray();
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			if (array.Length != 0)
			{
				Uri uri = new Uri(array[0].URL);
				return uri.Host;
			}
			return string.Empty;
		});
	}

	public string QueryFailoverServerNamebyId(Guid serverId)
	{
		string sql = "SELECT f.[Name] AS [StringResult] FROM [FailoverServers] f WHERE f.[IDFailoverServer] = @IdFailover";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IdFailover", serverId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public void DeleteBookmarksByDeviceIdBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("bookmarks_delete_by_device_id_before_time");
			BindGuidType("@IDDevice", deviceId);
			BindLong("@BeforeTime", keepFromTime.Ticks);
			ExecuteNonQuery();
		});
	}

	public void DeleteRestrictedMediaDeviceBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_delete_by_device_id_before_time");
			BindGuidType("@IDDevice", deviceId);
			BindDateTime("@BeforeTime", keepFromTime);
			ExecuteNonQuery();
		});
	}

	public void DeleteCompletedTasksByDeviceIdBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_completed_tasks_by_device_id_before_time");
			BindGuidType("@IDDevice", deviceId);
			BindDateTime("@Time", keepFromTime);
			ExecuteNonQuery();
		});
	}

	public void DeletePrivacyMasksBeforeTime(Guid deviceId, DateTime keepFromTime)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_privacy_masks_before_time");
			BindGuidType("@IDDevice", deviceId);
			BindDateTime("@KeepFromTime", keepFromTime);
			ExecuteNonQuery();
		});
	}

	public void UpdateDeviceTrackOnlyUseMediaAfter(Guid trackId, DateTime onlyUseMediaAfter)
	{
		PrepareStoredProcedure("device_track_update_onlyusemediaafter");
		BindUniqueIdentifier("@IDTrack", trackId);
		BindDateTime2("@OnlyUseMediaAfter", onlyUseMediaAfter);
		ExecuteNonQuery();
	}

	public string FindTimeProfileNameByTimeProfileId(Guid timeProfileId)
	{
		string sql = "SELECT [Name] AS [StringResult] FROM [TimeProfile] WHERE [pkid] = @IDTimeProfile";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDTimeProfile", timeProfileId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}

	public string FindAcUnitNameByMipObjectId(Guid mipObjectId)
	{
		string sql = "SELECT [Name] AS [StringResult] FROM [MIPObject] WHERE [IDMIPObject] = @mipObjectId";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@mipObjectId", mipObjectId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigMissingRow);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(Resources.VmoDatabaseConfigExtraRows);
			}
			return result;
		});
	}
}
