using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Server.Licensing;
using VideoOS.Server.Licensing.Internal;
using VideoOS.Server.Licensing.LicenseDataSetTableAdapters;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public sealed class Data : IConfigurationDataLoader, IConfigurationDataSetComponent, ILicenseDataSetComponent
{
	private ReaderWriterLockSlim _configurationWriterAdapterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

	private ReaderWriterLockSlim _configurationWriterForcedAdapterLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);

	private ConfigurationTablesManager _writeConfigurationTablesManager;

	private ConfigurationTablesManager _writeConfigurationTablesManagerForce;

	private readonly object _lockObject = new object();

	private static readonly SqlMetaData[] LicenseUpdateTableMetaData = new SqlMetaData[3]
	{
		new SqlMetaData("Id", SqlDbType.UniqueIdentifier),
		new SqlMetaData("UniqueId", SqlDbType.VarChar, 700L),
		new SqlMetaData("Expires", SqlDbType.BigInt)
	};

	public ReaderWriterLockSlim ConfigurationDataSetCacheLock { get; private set; }

	public ReaderWriterLockSlim SecurityDataSetCacheLock { get; private set; }

	public ReaderWriterLockSlim LicenseCacheLock { get; private set; }

	public Data()
	{
		ConfigurationDataSetCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
		LicenseCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
		SecurityDataSetCacheLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
	}

	public void Initialize()
	{
		_writeConfigurationTablesManager = ConfigurationTablesManager.Create();
		_writeConfigurationTablesManagerForce = ConfigurationTablesManager.CreateForcedUpdater();
	}

	public SecurityDataSet LoadSecurityDataSetByRecorder(Guid recorderId)
	{
		SecurityDataSet securityDataSet = new SecurityDataSet();
		try
		{
			string IDRecorder = recorderId.ToString();
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				Dictionary<string, ITableAdapterSecurity> dictionary = CreateSecurityTableAdapter(connection.Connection, securityDataSet);
				try
				{
					using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
					foreach (ITableAdapterSecurity value in dictionary.Values)
					{
						try
						{
							value.SqlTransaction = databaseTransaction.Transaction;
							if (value is ITableAdapterSecurityRecorder)
							{
								((ITableAdapterSecurityRecorder)value).FillByRecorder(securityDataSet, IDRecorder);
							}
							else if (!(value is ITableAdapterSecurityServer))
							{
								value.Fill(securityDataSet);
							}
						}
						finally
						{
							value.SqlTransaction = null;
						}
					}
					databaseTransaction.Commit();
				}
				finally
				{
					foreach (ITableAdapterSecurity value2 in dictionary.Values)
					{
						if (value2 != null)
						{
							((Component)value2).Dispose();
						}
					}
				}
			});
			return securityDataSet;
		}
		catch (Exception)
		{
			securityDataSet.Dispose();
			throw;
		}
	}

	private static Dictionary<string, ITableAdapterSecurity> CreateSecurityTableAdapter(SqlConnection connection, SecurityDataSet securityDataSet)
	{
		Dictionary<string, ITableAdapterSecurity> dictionary = new Dictionary<string, ITableAdapterSecurity>();
		dictionary.Add(securityDataSet.SecurityAttributes.TableName, new SecurityAttributesTableAdapter());
		dictionary.Add(securityDataSet.TimeProfileDefaultRoleRelation.TableName, new TimeProfileDefaultRoleRelationTableAdapter());
		dictionary.Add(securityDataSet.SecurityRoles.TableName, new SecurityRolesTableAdapter());
		dictionary.Add(securityDataSet.SecurityPermissionsDevice.TableName, new SecurityPermissionsDeviceTableAdapter());
		dictionary.Add(securityDataSet.SecurityPermissions.TableName, new SecurityPermissionsTableAdapter());
		dictionary.Add(securityDataSet.SecurityPermissionsMatrix.TableName, new SecurityPermissionsMatrixTableAdapter());
		dictionary.Add(securityDataSet.SecurityGroupAccessEntry.TableName, new SecurityGroupAccessEntryTableAdapter());
		foreach (ITableAdapterSecurity value in dictionary.Values)
		{
			value.SqlConnection = connection;
		}
		return dictionary;
	}

	public SecurityDataSet LoadSecurityDataSetByHardware(Guid hardwareId)
	{
		SecurityDataSet securityDataSet = new SecurityDataSet();
		try
		{
			string IDHardware = hardwareId.ToString();
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				SecurityPermissionsDeviceTableAdapter securityPermissionsDeviceTableAdapter = new SecurityPermissionsDeviceTableAdapter();
				try
				{
					using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
					securityPermissionsDeviceTableAdapter.SqlConnection = connection.Connection;
					try
					{
						securityPermissionsDeviceTableAdapter.SqlTransaction = databaseTransaction.Transaction;
						securityPermissionsDeviceTableAdapter.FillByHardware(securityDataSet, IDHardware);
					}
					finally
					{
						securityPermissionsDeviceTableAdapter.SqlTransaction = null;
					}
					databaseTransaction.Commit();
				}
				finally
				{
					securityPermissionsDeviceTableAdapter?.Dispose();
				}
			});
			return securityDataSet;
		}
		catch (Exception)
		{
			securityDataSet.Dispose();
			throw;
		}
	}

	public SecurityDataSet LoadSecurityDataSetByRole(Guid roleId)
	{
		SecurityDataSet securityDataSet = new SecurityDataSet();
		try
		{
			string IDRole = roleId.ToString();
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				Dictionary<string, ITableAdapterSecurity> dictionary = CreateSecurityTableAdapter(connection.Connection, securityDataSet);
				try
				{
					using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
					foreach (ITableAdapterSecurity value in dictionary.Values)
					{
						try
						{
							value.SqlTransaction = databaseTransaction.Transaction;
							if (value is ITableAdapterSecurityRole)
							{
								((ITableAdapterSecurityRole)value).FillByRole(securityDataSet, IDRole);
							}
							else
							{
								value.Fill(securityDataSet);
							}
						}
						finally
						{
							value.SqlTransaction = null;
						}
					}
					databaseTransaction.Commit();
				}
				finally
				{
					foreach (ITableAdapterSecurity value2 in dictionary.Values)
					{
						if (value2 != null)
						{
							((Component)value2).Dispose();
						}
					}
				}
			});
			return securityDataSet;
		}
		catch (Exception)
		{
			securityDataSet.Dispose();
			throw;
		}
	}

	public SecurityDataSet LoadSecurityDataSetTables(string[] tableNames)
	{
		if (tableNames == null)
		{
			throw new ArgumentNullException("tableNames");
		}
		SecurityDataSet securityDataSet = new SecurityDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				Dictionary<string, ITableAdapterSecurity> dictionary = CreateSecurityTableAdapter(connection.Connection, securityDataSet);
				try
				{
					using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
					string[] array = tableNames;
					foreach (string key in array)
					{
						if (dictionary.TryGetValue(key, out var value))
						{
							try
							{
								value.SqlTransaction = databaseTransaction.Transaction;
								value.Fill(securityDataSet);
							}
							finally
							{
								value.SqlTransaction = null;
							}
						}
					}
					databaseTransaction.Commit();
				}
				finally
				{
					foreach (ITableAdapterSecurity value2 in dictionary.Values)
					{
						if (value2 != null)
						{
							((Component)value2).Dispose();
						}
					}
				}
			});
			return securityDataSet;
		}
		catch (Exception)
		{
			securityDataSet.Dispose();
			throw;
		}
	}

	public void WriteSecurityDataSetChangesToDatabase(SecurityDataSet changes)
	{
		SecurityDataSetCacheLock.EnterWriteLock();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				Dictionary<string, ITableAdapterSecurity> dictionary = CreateSecurityTableAdapter(connection.Connection, changes);
				using DatabaseTransaction databaseTransaction = connection.BeginTransaction();
				try
				{
					DataSetUtility.UpdateLastModified(changes, onlyUpdateUnmodifiedTimestamps: true);
					foreach (ITableAdapterSecurity value in dictionary.Values)
					{
						value.Update(changes, databaseTransaction.Transaction);
					}
					using (DatabaseCommand databaseCommand = connection.CreateCommand("security_clear_cache_transaction"))
					{
						databaseCommand.CommandType = CommandType.StoredProcedure;
						databaseCommand.ExecuteNonQuery();
					}
					databaseTransaction.Commit();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception)
				{
					databaseTransaction.Rollback();
					throw;
				}
				finally
				{
					foreach (ITableAdapterSecurity value2 in dictionary.Values)
					{
						if (value2 != null)
						{
							((Component)value2).Dispose();
						}
					}
				}
			});
		}
		finally
		{
			SecurityDataSetCacheLock.ExitWriteLock();
		}
	}

	public ConfigurationDataSet LoadConfigurationDataSetByRecorder(Guid recorderId, Guid recorderServiceId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			string IDRecorder = recorderId.ToString();
			string IDRecorderService = recorderServiceId.ToString();
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
				ConfigurationTablesManager configurationTablesManager = ConfigurationTablesManager.Create();
				ICollection<ITableAdapter> allAdapters = configurationTablesManager.AllAdapters;
				try
				{
					using (DatabaseCommand databaseCommand = connection.CreateCommand("delete_pending_diffgrams_for_target"))
					{
						databaseCommand.CommandType = CommandType.StoredProcedure;
						databaseCommand.AddParameter("@target_id", SqlDbType.Char, 36, recorderServiceId.ToString());
						databaseCommand.ExecuteNonQuery();
					}
					foreach (ITableAdapter item in allAdapters)
					{
						try
						{
							item.Connection = connection.Connection;
							item.SqlTransaction = databaseTransaction.Transaction;
							if (item is ITableAdapterRecorder)
							{
								((ITableAdapterRecorder)item).FillByRecorder(configurationDataSet, IDRecorder);
							}
							else if (item is ITableAdapterRecorderService)
							{
								((ITableAdapterRecorderService)item).FillByRecorderService(configurationDataSet, IDRecorderService);
							}
							else if (!(item is ITableAdapterServer))
							{
								item.Fill(configurationDataSet);
							}
						}
						finally
						{
							item.SqlTransaction = null;
						}
					}
					databaseTransaction.Commit();
					configurationTablesManager.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				}
				finally
				{
					foreach (ITableAdapter item2 in allAdapters)
					{
						if (item2 != null)
						{
							((Component)item2).Dispose();
						}
					}
				}
			});
			return configurationDataSet;
		}
		catch
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadConfigurationDataSetByHardware(Guid hardwareId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			string IDHardware = hardwareId.ToString();
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				ITableAdapterHardware[] array = TableAdapterTypeManager.CreateTableAdapters<ITableAdapterHardware>();
				using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
				try
				{
					ITableAdapterHardware[] array2 = array;
					foreach (ITableAdapterHardware tableAdapterHardware in array2)
					{
						try
						{
							tableAdapterHardware.Connection = connection.Connection;
							tableAdapterHardware.SqlTransaction = databaseTransaction.Transaction;
							tableAdapterHardware.FillByHardware(configurationDataSet, IDHardware);
						}
						finally
						{
							tableAdapterHardware.SqlTransaction = null;
						}
					}
					databaseTransaction.Commit();
					ITableAdapterHardware[] array3 = array;
					foreach (ITableAdapterHardware tableAdapterHardware2 in array3)
					{
						if (tableAdapterHardware2 is ITableAdapterEncryption tableAdapterEncryption)
						{
							tableAdapterEncryption.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
						}
					}
					configurationDataSet.AcceptChanges();
				}
				finally
				{
					ITableAdapterHardware[] array4 = array;
					foreach (ITableAdapter tableAdapter in array4)
					{
						if (tableAdapter != null)
						{
							((Component)tableAdapter).Dispose();
						}
					}
				}
			});
			return configurationDataSet;
		}
		catch
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadLicenseByLicenseId(Guid licenseId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using LicenseTableAdapter licenseTableAdapter = new LicenseTableAdapter();
				licenseTableAdapter.Connection = connection.Connection;
				licenseTableAdapter.FillByIDLicense(configurationDataSet.License, licenseId);
				licenseTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadLicensesByLicenseIds(Guid[] licenseIds)
	{
		if (licenseIds == null)
		{
			throw new ArgumentNullException("licenseIds");
		}
		string idsXml = ArrayAsXml(licenseIds);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using LicenseTableAdapter licenseTableAdapter = new LicenseTableAdapter();
				licenseTableAdapter.Connection = connection.Connection;
				licenseTableAdapter.FillByLicenseIds(configurationDataSet.License, idsXml);
				licenseTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadHardwareDefinitionByDigest(string digest)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using HardwareDefinitionTableAdapter hardwareDefinitionTableAdapter = new HardwareDefinitionTableAdapter();
				hardwareDefinitionTableAdapter.Connection = connection.Connection;
				hardwareDefinitionTableAdapter.FillByC18NDigest(configurationDataSet.HardwareDefinition, digest);
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadHardwareDefinitionsUnused()
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using HardwareDefinitionTableAdapter hardwareDefinitionTableAdapter = new HardwareDefinitionTableAdapter();
				hardwareDefinitionTableAdapter.Connection = connection.Connection;
				hardwareDefinitionTableAdapter.FillWithUnused(configurationDataSet.HardwareDefinition);
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadHardwareByRecorderId(Guid recorderId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter hardwareTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter();
				hardwareTableAdapter.Connection = connection.Connection;
				hardwareTableAdapter.FillByRecorder(configurationDataSet.Hardware, recorderId.ToString());
				hardwareTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadHardwareByHardwareId(Guid hardwareId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter hardwareTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter();
				hardwareTableAdapter.Connection = connection.Connection;
				hardwareTableAdapter.FillByHardware(configurationDataSet.Hardware, hardwareId.ToString());
				hardwareTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadHardwareByHardwareIds(Guid[] hardwareIds)
	{
		if (hardwareIds == null)
		{
			throw new ArgumentNullException("hardwareIds");
		}
		string idsXml = ArrayAsXml(hardwareIds);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter hardwareTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter();
				hardwareTableAdapter.Connection = connection.Connection;
				hardwareTableAdapter.FillByHardwareIds(configurationDataSet.Hardware, idsXml);
				hardwareTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadHardwareByHostedHardwareId(int hostedHardwareId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter hardwareTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter
				{
					Connection = connection.Connection
				};
				hardwareTableAdapter.FillByHostedHardware(configurationDataSet.Hardware, hostedHardwareId);
				hardwareTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadHardwareByDeviceGroupId(Guid deviceGroupId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter hardwareTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.HardwareTableAdapter
				{
					Connection = connection.Connection
				};
				hardwareTableAdapter.FillByDeviceGroup(configurationDataSet.Hardware, deviceGroupId.ToString());
				hardwareTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDevicesByHardwareId(Guid hardwareId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter devicesTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter();
				devicesTableAdapter.Connection = connection.Connection;
				devicesTableAdapter.FillByIDHardware(configurationDataSet.Devices, hardwareId.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDevicesByRecorderId(Guid recorderId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter devicesTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter();
				devicesTableAdapter.Connection = connection.Connection;
				devicesTableAdapter.FillByRecorder(configurationDataSet.Devices, recorderId.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceByDeviceId(Guid deviceId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter devicesTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter();
				devicesTableAdapter.Connection = connection.Connection;
				devicesTableAdapter.FillByIDDevice(configurationDataSet.Devices, deviceId.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDevicesByDeviceIds(Guid[] deviceIds)
	{
		if (deviceIds == null)
		{
			throw new ArgumentNullException("deviceIds");
		}
		string idsXml = ArrayAsXml(deviceIds);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter devicesTableAdapter = new VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters.DevicesTableAdapter();
				devicesTableAdapter.Connection = connection.Connection;
				devicesTableAdapter.FillByDeviceIds(configurationDataSet.Devices, idsXml);
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceGroupByDeviceGroupId(Guid deviceGroupId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DeviceGroupsTableAdapter deviceGroupsTableAdapter = new DeviceGroupsTableAdapter();
				deviceGroupsTableAdapter.Connection = connection.Connection;
				deviceGroupsTableAdapter.FillByIDDeviceGroup(configurationDataSet.DeviceGroups, deviceGroupId.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DeviceGroupsTableAdapter deviceGroupsTableAdapter = new DeviceGroupsTableAdapter();
				deviceGroupsTableAdapter.Connection = connection.Connection;
				if (parentDeviceGroupId == Guid.Empty)
				{
					deviceGroupsTableAdapter.FillByIDParentDeviceGroupIsNULL(configurationDataSet.DeviceGroups);
				}
				else
				{
					deviceGroupsTableAdapter.FillByIDParentDeviceGroup(configurationDataSet.DeviceGroups, parentDeviceGroupId.ToString());
				}
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DeviceGroupMembersTableAdapter deviceGroupMembersTableAdapter = new DeviceGroupMembersTableAdapter();
				deviceGroupMembersTableAdapter.Connection = connection.Connection;
				deviceGroupMembersTableAdapter.FillByIDDeviceGroup(configurationDataSet.DeviceGroupMembers, deviceGroupId.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceGroupMembersByDeviceId(Guid deviceId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DeviceGroupMembersTableAdapter deviceGroupMembersTableAdapter = new DeviceGroupMembersTableAdapter();
				deviceGroupMembersTableAdapter.Connection = connection.Connection;
				deviceGroupMembersTableAdapter.FillByIDDevice(configurationDataSet.DeviceGroupMembers, deviceId.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using RecorderPluginDeviceTableAdapter recorderPluginDeviceTableAdapter = new RecorderPluginDeviceTableAdapter
				{
					Connection = connection.Connection
				};
				recorderPluginDeviceTableAdapter.FillByIDDeviceIDRecorderPlugin(configurationDataSet.RecorderPluginDevice, deviceId.ToString(), recorderPluginID.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds)
	{
		if (deviceIds == null)
		{
			throw new ArgumentNullException("deviceIds");
		}
		string idsXml = ArrayAsXml(deviceIds);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using RecorderPluginDeviceTableAdapter recorderPluginDeviceTableAdapter = new RecorderPluginDeviceTableAdapter
				{
					Connection = connection.Connection
				};
				recorderPluginDeviceTableAdapter.FillByIDDevicesIDRecorderPlugin(configurationDataSet.RecorderPluginDevice, idsXml, recorderPluginID.ToString());
			});
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
		return configurationDataSet;
	}

	public ConfigurationDataSet LoadDeviceRecorderPluginsByDeviceId(Guid deviceId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using RecorderPluginDeviceTableAdapter recorderPluginDeviceTableAdapter = new RecorderPluginDeviceTableAdapter
				{
					Connection = connection.Connection
				};
				recorderPluginDeviceTableAdapter.FillByIDDevice(configurationDataSet.RecorderPluginDevice, deviceId.ToString());
			});
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
		return configurationDataSet;
	}

	public ConfigurationDataSet LoadDevicePrivacyMaskByDeviceIds(Guid[] deviceIds)
	{
		if (deviceIds == null)
		{
			throw new ArgumentNullException("deviceIds");
		}
		string idsXml = ArrayAsXml(deviceIds);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DevicePrivacyMaskTableAdapter devicePrivacyMaskTableAdapter = new DevicePrivacyMaskTableAdapter();
				devicePrivacyMaskTableAdapter.Connection = connection.Connection;
				devicePrivacyMaskTableAdapter.FillByIDDevices(configurationDataSet.DevicePrivacyMask, idsXml);
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadDeviceTracksByDeviceIds(Guid[] deviceIds)
	{
		if (deviceIds == null)
		{
			throw new ArgumentNullException("deviceIds");
		}
		string idsXml = ArrayAsXml(deviceIds);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DeviceTrackTableAdapter deviceTrackTableAdapter = new DeviceTrackTableAdapter();
				deviceTrackTableAdapter.Connection = connection.Connection;
				deviceTrackTableAdapter.FillByIDDevices(configurationDataSet.DeviceTrack, idsXml);
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public ConfigurationDataSet LoadConfigurationDataSetTables(string[] tableNames)
	{
		if (tableNames == null)
		{
			throw new ArgumentNullException("tableNames");
		}
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		ITableAdapter[] adapters = TableAdapterTypeManager.CreateTableAdapters<ITableAdapter>(tableNames);
		try
		{
			_ = adapters.Length;
			_ = tableNames.Length;
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
				ITableAdapter[] array3 = adapters;
				foreach (ITableAdapter tableAdapter3 in array3)
				{
					try
					{
						tableAdapter3.Connection = connection.Connection;
						tableAdapter3.SqlTransaction = databaseTransaction.Transaction;
						tableAdapter3.Fill(configurationDataSet);
					}
					finally
					{
						tableAdapter3.SqlTransaction = null;
					}
				}
				databaseTransaction.Commit();
			});
			ITableAdapter[] array = adapters;
			foreach (ITableAdapter tableAdapter in array)
			{
				if (tableAdapter is ITableAdapterEncryption tableAdapterEncryption)
				{
					tableAdapterEncryption.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				}
			}
			configurationDataSet.AcceptChanges();
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
		finally
		{
			ITableAdapter[] array2 = adapters;
			foreach (ITableAdapter tableAdapter2 in array2)
			{
				if (tableAdapter2 != null)
				{
					((Component)tableAdapter2).Dispose();
				}
			}
		}
	}

	public ConfigurationDataSet LoadHostingServersByIds(int[] ids)
	{
		if (ids == null)
		{
			throw new ArgumentNullException("ids");
		}
		string idsXml = ArrayAsXml(ids);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using HostingServerTableAdapter hostingServerTableAdapter = new HostingServerTableAdapter
				{
					Connection = connection.Connection
				};
				hostingServerTableAdapter.FillByIds(configurationDataSet.HostingServer, idsXml);
				hostingServerTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
		return configurationDataSet;
	}

	public ConfigurationDataSet LoadHostedHardwareByIds(int[] ids)
	{
		if (ids == null)
		{
			throw new ArgumentNullException("ids");
		}
		string idsXml = ArrayAsXml(ids);
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using HostedHardwareTableAdapter hostedHardwareTableAdapter = new HostedHardwareTableAdapter
				{
					Connection = connection.Connection
				};
				hostedHardwareTableAdapter.FillByIds(configurationDataSet.HostedHardware, idsXml);
				hostedHardwareTableAdapter.Decrypt(configurationDataSet, Server.Instance.CorporateEncryptionSymmetric);
				configurationDataSet.AcceptChanges();
			});
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
		return configurationDataSet;
	}

	public ConfigurationDataSet LoadRecordingStorageDevicesByRecorderId(Guid recorderId)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using RecordingStorageDeviceTableAdapter recordingStorageDeviceTableAdapter = new RecordingStorageDeviceTableAdapter();
				recordingStorageDeviceTableAdapter.Connection = connection.Connection;
				recordingStorageDeviceTableAdapter.FillByRecorder(configurationDataSet.RecordingStorageDevice, recorderId.ToString());
			});
			return configurationDataSet;
		}
		catch (Exception)
		{
			configurationDataSet.Dispose();
			throw;
		}
	}

	public void WriteConfigurationDataSetChangesToDatabase(ConfigurationDataSet changes, ICallContextInfo callContextInfo)
	{
		if (changes == null)
		{
			return;
		}
		if (callContextInfo == null)
		{
			throw new ArgumentNullException("callContextInfo");
		}
		_configurationWriterAdapterLock.EnterWriteLock();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				DataSetUtility.UpdateLastModified(changes, onlyUpdateUnmodifiedTimestamps: true);
				try
				{
					_writeConfigurationTablesManager.SetUserSid(callContextInfo.UserSid);
					_writeConfigurationTablesManager.Encrypt(changes, Server.Instance.CorporateEncryptionSymmetric);
					_writeConfigurationTablesManager.SetConnection(connection.Connection);
					_writeConfigurationTablesManager.TableAdapterManager.UpdateAll(changes);
				}
				finally
				{
					_writeConfigurationTablesManager.ClearUserSid();
				}
			});
			_writeConfigurationTablesManager.Decrypt(changes, Server.Instance.CorporateEncryptionSymmetric);
		}
		finally
		{
			_configurationWriterAdapterLock.ExitWriteLock();
		}
	}

	public void WriteConfigurationDataSetChangesForReplaceHardware(ConfigurationDataSet changes, ICallContextInfo callContextInfo)
	{
		if (changes == null)
		{
			return;
		}
		if (callContextInfo == null)
		{
			throw new ArgumentNullException("callContextInfo");
		}
		_configurationWriterAdapterLock.EnterWriteLock();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using CommittableTransaction committableTransaction = new CommittableTransaction(TimeSpan.FromMinutes(10.0));
				DataSetUtility.UpdateLastModified(changes, onlyUpdateUnmodifiedTimestamps: true);
				connection.Connection.EnlistTransaction(committableTransaction);
				try
				{
					_writeConfigurationTablesManager.SetUserSid(callContextInfo.UserSid);
					_writeConfigurationTablesManager.Encrypt(changes, Server.Instance.CorporateEncryptionSymmetric);
					_writeConfigurationTablesManager.SetConnection(connection.Connection);
					ConfigurationDataSet changes2 = changes.GetChanges(DataRowState.Deleted);
					if (changes2 != null)
					{
						_writeConfigurationTablesManager.TableAdapterManager.UpdateAll(changes2);
					}
					Dictionary<string, int> dictionary = new Dictionary<string, int>();
					if (changes.Devices != null)
					{
						DataTable changes3 = changes.Devices.GetChanges(DataRowState.Modified);
						if (changes3 != null)
						{
							using ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
							foreach (DataRow row in changes3.Rows)
							{
								configurationDataSet.Devices.ImportRow(row);
							}
							foreach (ConfigurationDataSet.DevicesRow device in configurationDataSet.Devices)
							{
								dictionary[device.IDDevice] = device.DeviceIndex;
								device.DeviceIndex = -1;
							}
							_writeConfigurationTablesManager.TableAdapterManager.UpdateAll(configurationDataSet);
							foreach (ConfigurationDataSet.DevicesRow device2 in configurationDataSet.Devices)
							{
								device2.DeviceIndex = dictionary[device2.IDDevice];
							}
							_writeConfigurationTablesManager.TableAdapterManager.UpdateAll(configurationDataSet.GetChanges());
						}
					}
					ConfigurationDataSet changes4 = changes.GetChanges(DataRowState.Added);
					if (changes4 != null)
					{
						_writeConfigurationTablesManager.TableAdapterManager.UpdateAll(changes4);
					}
					ConfigurationDataSet changes5 = changes.GetChanges(DataRowState.Modified);
					if (changes5 != null)
					{
						if (changes5.Devices != null)
						{
							changes5.Devices.AcceptChanges();
							changes5 = changes5.GetChanges();
						}
						if (changes5 != null)
						{
							_writeConfigurationTablesManager.TableAdapterManager.UpdateAll(changes5);
						}
					}
					committableTransaction.Commit();
				}
				catch (Exception)
				{
					committableTransaction.Rollback();
					throw;
				}
				finally
				{
					_writeConfigurationTablesManager.ClearUserSid();
				}
			});
			_writeConfigurationTablesManager.Decrypt(changes, Server.Instance.CorporateEncryptionSymmetric);
		}
		finally
		{
			_configurationWriterAdapterLock.ExitWriteLock();
		}
	}

	public void WriteConfigurationDataSetChangesToDatabaseForced(ConfigurationDataSet changes, ICallContextInfo callContextInfo)
	{
		if (changes == null)
		{
			return;
		}
		if (callContextInfo == null)
		{
			throw new ArgumentNullException("callContextInfo");
		}
		_configurationWriterForcedAdapterLock.EnterWriteLock();
		try
		{
			DataSetUtility.UpdateLastModified(changes, onlyUpdateUnmodifiedTimestamps: true);
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				try
				{
					_writeConfigurationTablesManagerForce.SetUserSid(callContextInfo.UserSid);
					_writeConfigurationTablesManagerForce.Encrypt(changes, Server.Instance.CorporateEncryptionSymmetric);
					_writeConfigurationTablesManagerForce.SetConnection(connection.Connection);
					_writeConfigurationTablesManagerForce.TableAdapterManager.UpdateAll(changes);
				}
				finally
				{
					_writeConfigurationTablesManagerForce.ClearUserSid();
				}
			});
			_writeConfigurationTablesManagerForce.Decrypt(changes, Server.Instance.CorporateEncryptionSymmetric);
		}
		finally
		{
			_configurationWriterForcedAdapterLock.ExitWriteLock();
		}
	}

	private static string ArrayAsXml(int[] idArray)
	{
		IEnumerable<XElement> content = idArray.Select((int g) => new XElement("Id", g));
		XElement xElement = new XElement("Ids", content);
		return xElement.ToString();
	}

	private static string ArrayAsXml(Guid[] idArray)
	{
		IEnumerable<XElement> content = idArray.Select((Guid g) => new XElement("Id", g));
		XElement xElement = new XElement("Ids", content);
		return xElement.ToString();
	}

	private static void FillTableAdapter<TItems, TAdapter>(TItems items, Action<TAdapter, TItems> fillAction) where TAdapter : ITableAdapterLicensing, IDisposable, new()
	{
		TAdapter adapter = new TAdapter();
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				ref TAdapter reference = ref adapter;
				TAdapter val = default(TAdapter);
				if (val == null)
				{
					val = reference;
					reference = ref val;
				}
				SqlConnection connection2 = connection.Connection;
				reference.SqlConnection = connection2;
				using DatabaseTransaction databaseTransaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot);
				ref TAdapter reference2 = ref adapter;
				val = default(TAdapter);
				if (val == null)
				{
					val = reference2;
					reference2 = ref val;
				}
				SqlTransaction transaction = databaseTransaction.Transaction;
				reference2.SqlTransaction = transaction;
				try
				{
					fillAction(adapter, items);
					databaseTransaction.Commit();
				}
				catch (Exception ex)
				{
					Server.Instance.Logger.LogError("Error while filling table adapter. Adapter={Adapter}, Error: {Exception}", adapter, ex);
					databaseTransaction.Rollback();
				}
				finally
				{
					adapter.SqlTransaction = null;
				}
			});
		}
		finally
		{
			if (adapter != null)
			{
				adapter.Dispose();
			}
		}
	}

	public ILicenseSnapshot LoadLicenseDataSetByEnabled(bool enabled)
	{
		lock (_lockObject)
		{
			ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
			if (requiredService.RetrieveLicenseSnapshot() != null)
			{
				return requiredService.RetrieveLicenseSnapshot();
			}
			LicenseDataSet licenseDataSet = new LicenseDataSet();
			try
			{
				Parallel.Invoke(delegate
				{
					FillTableAdapter(licenseDataSet.Recorders, delegate(VideoOS.Server.Licensing.LicenseDataSetTableAdapters.RecordersTableAdapter adapter, LicenseDataSet.RecordersDataTable items)
					{
						adapter.FillByEnabled(items, enabled);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.Hardware, delegate(VideoOS.Server.Licensing.LicenseDataSetTableAdapters.HardwareTableAdapter adapter, LicenseDataSet.HardwareDataTable items)
					{
						adapter.FillByEnabled(items, enabled);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.Devices, delegate(VideoOS.Server.Licensing.LicenseDataSetTableAdapters.DevicesTableAdapter adapter, LicenseDataSet.DevicesDataTable items)
					{
						adapter.FillByEnabled(items, enabled);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.LicenseRecorder, delegate(LicenseRecorderTableAdapter adapter, LicenseDataSet.LicenseRecorderDataTable items)
					{
						adapter.Fill(items);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.LicenseHardware, delegate(LicenseHardwareTableAdapter adapter, LicenseDataSet.LicenseHardwareDataTable items)
					{
						adapter.FillByConfiguration(items);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.LicenseDevice, delegate(LicenseDeviceTableAdapter adapter, LicenseDataSet.LicenseDeviceDataTable items)
					{
						adapter.FillByConfiguration(items);
					});
				});
				return LicenseSnapshot.CreateLicenseSnapshot(this, licenseDataSet, enabled);
			}
			finally
			{
				if (licenseDataSet != null)
				{
					((IDisposable)licenseDataSet).Dispose();
				}
			}
		}
	}

	public ILicenseSnapshot LoadLicenseDataSet(out bool isObtainedFromCache)
	{
		lock (_lockObject)
		{
			ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
			if (requiredService.RetrieveLicenseSnapshot() != null)
			{
				isObtainedFromCache = true;
				return requiredService.RetrieveLicenseSnapshot();
			}
			LicenseDataSet licenseDataSet = new LicenseDataSet();
			try
			{
				Parallel.Invoke(delegate
				{
					FillTableAdapter(licenseDataSet.Recorders, delegate(VideoOS.Server.Licensing.LicenseDataSetTableAdapters.RecordersTableAdapter adapter, LicenseDataSet.RecordersDataTable items)
					{
						adapter.Fill(items);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.Hardware, delegate(VideoOS.Server.Licensing.LicenseDataSetTableAdapters.HardwareTableAdapter adapter, LicenseDataSet.HardwareDataTable items)
					{
						adapter.Fill(items);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.Devices, delegate(VideoOS.Server.Licensing.LicenseDataSetTableAdapters.DevicesTableAdapter adapter, LicenseDataSet.DevicesDataTable items)
					{
						adapter.Fill(items);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.LicenseRecorder, delegate(LicenseRecorderTableAdapter adapter, LicenseDataSet.LicenseRecorderDataTable items)
					{
						adapter.Fill(items);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.LicenseHardware, delegate(LicenseHardwareTableAdapter adapter, LicenseDataSet.LicenseHardwareDataTable items)
					{
						adapter.FillByConfiguration(items);
					});
				}, delegate
				{
					FillTableAdapter(licenseDataSet.LicenseDevice, delegate(LicenseDeviceTableAdapter adapter, LicenseDataSet.LicenseDeviceDataTable items)
					{
						adapter.FillByConfiguration(items);
					});
				});
				requiredService.AddLicenseSnapshot(LicenseSnapshot.CreateLicenseSnapshot(this, licenseDataSet));
				isObtainedFromCache = false;
				return requiredService.RetrieveLicenseSnapshot();
			}
			finally
			{
				if (licenseDataSet != null)
				{
					((IDisposable)licenseDataSet).Dispose();
				}
			}
		}
	}

	private static SqlDataRecord CreateLicenseUpdateTableRecord(Guid id, string uniqueId, long licenseValue)
	{
		SqlDataRecord sqlDataRecord = new SqlDataRecord(LicenseUpdateTableMetaData);
		sqlDataRecord.SetGuid(0, id);
		sqlDataRecord.SetSqlString(1, uniqueId);
		sqlDataRecord.SetInt64(2, licenseValue);
		return sqlDataRecord;
	}

	public void WriteLicenseDataSetChangesToDatabase(ILicenseSnapshot licenseSnapshot)
	{
		if (licenseSnapshot == null)
		{
			throw new ArgumentNullException("licenseSnapshot");
		}
		if (!licenseSnapshot.RecorderLicensesWithAnyChange.Any() && !licenseSnapshot.HardwareLicensesWithAnyChange.Any() && !licenseSnapshot.DeviceLicensesWithAnyChange.Any())
		{
			return;
		}
		lock (_lockObject)
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				List<SqlDataRecord> sqlDataRecords = (from r in licenseSnapshot.RecorderLicensesByState(LicenseUpdateAction.Insert)
					select CreateLicenseUpdateTableRecord(r.Id, string.Empty, 0L)).ToList();
				List<SqlDataRecord> sqlDataRecords2 = (from r in licenseSnapshot.RecorderLicensesByState(LicenseUpdateAction.Update)
					select CreateLicenseUpdateTableRecord(r.Id, string.Empty, 0L)).ToList();
				List<SqlDataRecord> sqlDataRecords3 = (from r in licenseSnapshot.RecorderLicensesByState(LicenseUpdateAction.Delete)
					select CreateLicenseUpdateTableRecord(r.Id, string.Empty, 0L)).ToList();
				List<SqlDataRecord> sqlDataRecords4 = (from h in licenseSnapshot.HardwareLicensesByState(LicenseUpdateAction.Insert)
					select CreateLicenseUpdateTableRecord(h.Id, h.UniqueIdentifier, h.License)).ToList();
				List<SqlDataRecord> sqlDataRecords5 = (from h in licenseSnapshot.HardwareLicensesByState(LicenseUpdateAction.Update)
					select CreateLicenseUpdateTableRecord(h.Id, h.UniqueIdentifier, h.License)).ToList();
				List<SqlDataRecord> sqlDataRecords6 = (from h in licenseSnapshot.HardwareLicensesByState(LicenseUpdateAction.Delete)
					select CreateLicenseUpdateTableRecord(h.Id, h.UniqueIdentifier, h.License)).ToList();
				List<SqlDataRecord> sqlDataRecords7 = (from d in licenseSnapshot.DeviceLicensesByState(LicenseUpdateAction.Insert)
					select CreateLicenseUpdateTableRecord(d.Id, d.UniqueIdentifier, d.License)).ToList();
				List<SqlDataRecord> sqlDataRecords8 = (from d in licenseSnapshot.DeviceLicensesByState(LicenseUpdateAction.Update)
					select CreateLicenseUpdateTableRecord(d.Id, d.UniqueIdentifier, d.License)).ToList();
				List<SqlDataRecord> sqlDataRecords9 = (from d in licenseSnapshot.DeviceLicensesByState(LicenseUpdateAction.Delete)
					select CreateLicenseUpdateTableRecord(d.Id, d.UniqueIdentifier, d.License)).ToList();
				using DatabaseTransaction databaseTransaction = connection.BeginTransaction();
				string text = "[dbo].[admin_update_license_values]";
				try
				{
					using (DatabaseCommand databaseCommand = connection.CreateCommand(text))
					{
						databaseCommand.CommandType = CommandType.StoredProcedure;
						databaseCommand.AddParameter(CreateListParameter("@RecorderInserts", sqlDataRecords));
						databaseCommand.AddParameter(CreateListParameter("@RecorderUpdates", sqlDataRecords2));
						databaseCommand.AddParameter(CreateListParameter("@RecorderDeletes", sqlDataRecords3));
						databaseCommand.AddParameter(CreateListParameter("@HardwareInserts", sqlDataRecords4));
						databaseCommand.AddParameter(CreateListParameter("@HardwareUpdates", sqlDataRecords5));
						databaseCommand.AddParameter(CreateListParameter("@HardwareDeletes", sqlDataRecords6));
						databaseCommand.AddParameter(CreateListParameter("@DeviceInserts", sqlDataRecords7));
						databaseCommand.AddParameter(CreateListParameter("@DeviceUpdates", sqlDataRecords8));
						databaseCommand.AddParameter(CreateListParameter("@DeviceDeletes", sqlDataRecords9));
						databaseCommand.ExecuteNonQuery();
					}
					databaseTransaction.Commit();
				}
				catch (Exception ex)
				{
					databaseTransaction.Rollback();
					Server.Instance.Logger.LogError("Error while executing SQL command: {Sql}, Error: {Exception}", text, ex);
					throw;
				}
			});
		}
		static DatabaseParameter CreateListParameter(string parameterName, IEnumerable<SqlDataRecord> sqlDataRecords)
		{
			return new DatabaseParameter(parameterName, sqlDataRecords.Any() ? sqlDataRecords : null)
			{
				TypeName = "tvp_LicenseUpdateTable",
				SqlDbType = SqlDbType.Structured
			};
		}
	}
}
