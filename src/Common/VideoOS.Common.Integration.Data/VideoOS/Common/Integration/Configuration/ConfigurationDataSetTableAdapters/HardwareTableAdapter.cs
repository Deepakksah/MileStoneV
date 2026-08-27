using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration.Data.Properties;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

[DesignerCategory("code")]
[ToolboxItem(true)]
[DataObject(true)]
[Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[HelpKeyword("vs.data.TableAdapter")]
public class HardwareTableAdapter : Component, ITableAdapterRecorder, ITableAdapter, ITableAdapterHardware, IUserContextAdapter, ITableAdapterEncryption
{
	private string currentSid;

	private SqlDataAdapter _adapter;

	private SqlConnection _connection;

	private SqlTransaction _transaction;

	private SqlCommand[] _commandCollection;

	private bool _clearBeforeFill;

	public SqlTransaction SqlTransaction
	{
		get
		{
			return Transaction;
		}
		set
		{
			Transaction = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public SqlDataAdapter Adapter{
		get
		{
			if (_adapter == null)
			{
				InitAdapter();
			}
			return _adapter;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public SqlConnection Connection
	{
		get
		{
			if (_connection == null)
			{
				InitConnection();
			}
			return _connection;
		}
		set
		{
			_connection = value;
			if (Adapter.InsertCommand != null)
			{
				Adapter.InsertCommand.Connection = value;
			}
			if (Adapter.DeleteCommand != null)
			{
				Adapter.DeleteCommand.Connection = value;
			}
			if (Adapter.UpdateCommand != null)
			{
				Adapter.UpdateCommand.Connection = value;
			}
			for (int i = 0; i < CommandCollection.Length; i++)
			{
				if (CommandCollection[i] != null)
				{
					CommandCollection[i].Connection = value;
				}
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public SqlTransaction Transaction{
		get
		{
			return _transaction;
		}
		set
		{
			_transaction = value;
			for (int i = 0; i < CommandCollection.Length; i++)
			{
				CommandCollection[i].Transaction = _transaction;
			}
			if (Adapter != null && Adapter.DeleteCommand != null)
			{
				Adapter.DeleteCommand.Transaction = _transaction;
			}
			if (Adapter != null && Adapter.InsertCommand != null)
			{
				Adapter.InsertCommand.Transaction = _transaction;
			}
			if (Adapter != null && Adapter.UpdateCommand != null)
			{
				Adapter.UpdateCommand.Transaction = _transaction;
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected SqlCommand[] CommandCollection
	{
		get
		{
			if (_commandCollection == null)
			{
				InitCommandCollection();
			}
			return _commandCollection;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public bool ClearBeforeFill
	{
		get
		{
			return _clearBeforeFill;
		}
		set
		{
			_clearBeforeFill = value;
		}
	}

	public void Fill(ConfigurationDataSet ds)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		Fill(ds.Hardware);
	}

	public void FillByRecorder(ConfigurationDataSet ds, string idRecorder)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByRecorder(ds.Hardware, idRecorder);
	}

	public void FillByHardware(ConfigurationDataSet ds, string idHardware)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByHardware(ds.Hardware, idHardware);
	}

	public void SetUserSid(string sid)
	{
		if (sid == null)
		{
			throw new ArgumentNullException("sid");
		}
		if (string.IsNullOrEmpty(sid))
		{
			throw new ArgumentException("No security identifier was specified.", "sid");
		}
		if (currentSid != null)
		{
			throw new InvalidOperationException("The current sid value has not been cleared after the last use of the table adapter.");
		}
		currentSid = sid;
		Adapter.InsertCommand.Parameters["@user_sid"].Value = sid;
	}

	public void ClearUserSid()
	{
		if (currentSid == null)
		{
			throw new InvalidOperationException("There is no current sid.");
		}
		currentSid = null;
		Adapter.InsertCommand.Parameters["@user_sid"].Value = null;
	}

	public void Decrypt(ConfigurationDataSet ds, ISymmetricEncryption symmetricEncryption)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		if (symmetricEncryption == null)
		{
			throw new ArgumentNullException("symmetricEncryption");
		}
		if (((DataTable)(object)ds.Hardware).Rows == null || ((DataTable)(object)ds.Hardware).Rows.Count == 0)
		{
			return;
		}
		foreach (ConfigurationDataSet.HardwareRow row in ((DataTable)(object)ds.Hardware).Rows)
		{
			if (!string.IsNullOrEmpty(row.EncryptedPassword))
			{
				row.EncryptedPassword = symmetricEncryption.DecryptFromBase64String(row.EncryptedPassword);
			}
			if (!string.IsNullOrEmpty(row.Settings))
			{
				string settings = SettingsXmlTransformer.Instance.Decrypt(row.Settings, (string settingValue) => symmetricEncryption.DecryptFromBase64String(settingValue));
				row.Settings = settings;
			}
		}
	}

	public void Encrypt(ConfigurationDataSet ds, ISymmetricEncryption symmetricEncryption)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		if (symmetricEncryption == null)
		{
			throw new ArgumentNullException("symmetricEncryption");
		}
		if (((DataTable)(object)ds.Hardware).Rows == null || ((DataTable)(object)ds.Hardware).Rows.Count == 0)
		{
			return;
		}
		foreach (ConfigurationDataSet.HardwareRow row in ((DataTable)(object)ds.Hardware).Rows)
		{
			if (row.RowState != DataRowState.Added && row.RowState != DataRowState.Modified)
			{
				continue;
			}
			if (!string.IsNullOrEmpty(row.EncryptedPassword))
			{
				row.EncryptedPassword = symmetricEncryption.EncryptToBase64String(row.EncryptedPassword);
			}
			if (!string.IsNullOrEmpty(row.Settings))
			{
				string settings = SettingsXmlTransformer.Instance.Encrypt(row.Settings, (string settingValue) => symmetricEncryption.EncryptToBase64String(settingValue));
				row.Settings = settings;
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public HardwareTableAdapter()
	{
		ClearBeforeFill = true;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitAdapter()
	{
		_adapter = new SqlDataAdapter();
		DataTableMapping dataTableMapping = new DataTableMapping();
		dataTableMapping.SourceTable = "Table";
		dataTableMapping.DataSetTable = "Hardware";
		dataTableMapping.ColumnMappings.Add("LastModified", "LastModified");
		dataTableMapping.ColumnMappings.Add("IDHardware", "IDHardware");
		dataTableMapping.ColumnMappings.Add("IDHardwareDriver", "IDHardwareDriver");
		dataTableMapping.ColumnMappings.Add("Name", "Name");
		dataTableMapping.ColumnMappings.Add("Description", "Description");
		dataTableMapping.ColumnMappings.Add("URI", "URI");
		dataTableMapping.ColumnMappings.Add("LoginID", "LoginID");
		dataTableMapping.ColumnMappings.Add("EncryptedPassword", "EncryptedPassword");
		dataTableMapping.ColumnMappings.Add("IDRecorder", "IDRecorder");
		dataTableMapping.ColumnMappings.Add("Enabled", "Enabled");
		dataTableMapping.ColumnMappings.Add("Settings", "Settings");
		dataTableMapping.ColumnMappings.Add("EventSettings", "EventSettings");
		dataTableMapping.ColumnMappings.Add("VendorProductID", "VendorProductID");
		dataTableMapping.ColumnMappings.Add("C18NDigest", "C18NDigest");
		dataTableMapping.ColumnMappings.Add("DriverNumber", "DriverNumber");
		dataTableMapping.ColumnMappings.Add("DriverProxyName", "DriverProxyName");
		dataTableMapping.ColumnMappings.Add("HostedHardware_id", "HostedHardware_id");
		dataTableMapping.ColumnMappings.Add("CameraData", "CameraData");
		dataTableMapping.ColumnMappings.Add("EdgeSettings", "EdgeSettings");
		dataTableMapping.ColumnMappings.Add("SubType", "SubType");
		dataTableMapping.ColumnMappings.Add("CredentialsLastModified", "CredentialsLastModified");
		dataTableMapping.ColumnMappings.Add("HardwareDataLastUpdated", "HardwareDataLastUpdated");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "dataset_hardware_delete";
		_adapter.DeleteCommand.CommandType = CommandType.StoredProcedure;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@RETURN_VALUE";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.Direction = ParameterDirection.ReturnValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDHardware";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "dataset_hardware_insert";
		_adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@RETURN_VALUE";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.Direction = ParameterDirection.ReturnValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardware";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardwareDriver";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardwareDriver";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Description";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@URI";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "URI";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LoginID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LoginID";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EncryptedPassword";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EncryptedPassword";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRecorder";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRecorder";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Enabled";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Settings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Settings";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EventSettings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EventSettings";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@VendorProductID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "VendorProductID";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@C18NDigest";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 44;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "C18NDigest";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DriverNumber";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverNumber";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DriverProxyName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverProxyName";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@HostedHardware_id";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HostedHardware_id";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CameraData";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CameraData";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EdgeSettings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EdgeSettings";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@SubType";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SubType";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@HardwareDataLastUpdated";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HardwareDataLastUpdated";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@user_sid";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		_adapter.UpdateCommand = new SqlCommand();
		_adapter.UpdateCommand.Connection = Connection;
		_adapter.UpdateCommand.CommandText = "dataset_hardware_update";
		_adapter.UpdateCommand.CommandType = CommandType.StoredProcedure;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@RETURN_VALUE";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.Direction = ParameterDirection.ReturnValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardware";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardwareDriver";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardwareDriver";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Description";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@URI";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "URI";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LoginID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LoginID";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EncryptedPassword";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EncryptedPassword";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRecorder";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRecorder";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Enabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Settings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Settings";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EventSettings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EventSettings";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@VendorProductID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "VendorProductID";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@C18NDigest";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 44;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "C18NDigest";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DriverNumber";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverNumber";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DriverProxyName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverProxyName";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@HostedHardware_id";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HostedHardware_id";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CameraData";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CameraData";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EdgeSettings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EdgeSettings";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@SubType";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SubType";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDHardware";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDHardwareDriver";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardwareDriver";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_LoginID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LoginID";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_EncryptedPassword";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EncryptedPassword";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDRecorder";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRecorder";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IsNull_VendorProductID";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "VendorProductID";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		sqlParameter.SourceColumnNullMapping = true;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_VendorProductID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "VendorProductID";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_C18NDigest";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 44;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "C18NDigest";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_DriverNumber";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverNumber";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_DriverProxyName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverProxyName";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IsNull_HostedHardware_id";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HostedHardware_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		sqlParameter.SourceColumnNullMapping = true;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_HostedHardware_id";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HostedHardware_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_SubType";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SubType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CredentialsLastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CredentialsLastModified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CredentialsLastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CredentialsLastModified";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@HardwareDataLastUpdated";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HardwareDataLastUpdated";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_HardwareDataLastUpdated";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HardwareDataLastUpdated";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitConnection()
	{
		_connection = new SqlConnection();
		_connection.ConnectionString = VideoOS.Common.Integration.Data.Properties.Settings.Default.SurveillanceConnectionString;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitCommandCollection()
	{
		_commandCollection = new SqlCommand[8];
		_commandCollection[0] = new SqlCommand();
		_commandCollection[0].Connection = Connection;
		_commandCollection[0].CommandText = "SELECT LastModified, IDHardware, IDHardwareDriver, Name, Description, URI, LoginID, EncryptedPassword, IDRecorder, Enabled, Settings, EventSettings, VendorProductID, C18NDigest, DriverNumber, DriverProxyName, HostedHardware_id, CameraData, EdgeSettings, SubType, CredentialsLastModified, HardwareDataLastUpdated FROM Hardware";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "SELECT C18NDigest, Description, DriverNumber, DriverProxyName, Enabled, EncryptedPassword, EventSettings, HostedHardware_id, IDHardware, IDHardwareDriver, IDRecorder, LastModified, LoginID, Name, Settings, URI, VendorProductID, CameraData, EdgeSettings, SubType, CredentialsLastModified, HardwareDataLastUpdated FROM Hardware AS h WHERE (IDHardware IN (SELECT DISTINCT d.IDHardware FROM Devices AS d INNER JOIN DeviceGroupMembers AS dgm ON d.IDDevice = dgm.IDDevice WHERE (dgm.IDDeviceGroup = @IDDeviceGroup)))";
		_commandCollection[1].CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDDeviceGroup";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		_commandCollection[1].Parameters.Add(sqlParameter);
		_commandCollection[2] = new SqlCommand();
		_commandCollection[2].Connection = Connection;
		_commandCollection[2].CommandText = "SELECT C18NDigest, Description, DriverNumber, DriverProxyName, Enabled, EncryptedPassword, EventSettings, HostedHardware_id, IDHardware, IDHardwareDriver, IDRecorder, LastModified, LoginID, Name, Settings, URI, VendorProductID, CameraData, SubType, EdgeSettings, CredentialsLastModified, HardwareDataLastUpdated FROM Hardware WHERE (IDHardware = @IDHardware)";
		_commandCollection[2].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardware";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		_commandCollection[2].Parameters.Add(sqlParameter);
		_commandCollection[3] = new SqlCommand();
		_commandCollection[3].Connection = Connection;
		_commandCollection[3].CommandText = "dataset_hardware_select_by_ids";
		_commandCollection[3].CommandType = CommandType.StoredProcedure;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@RETURN_VALUE";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.Direction = ParameterDirection.ReturnValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_commandCollection[3].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ids";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_commandCollection[3].Parameters.Add(sqlParameter);
		_commandCollection[4] = new SqlCommand();
		_commandCollection[4].Connection = Connection;
		_commandCollection[4].CommandText = "SELECT LastModified, IDHardware, IDHardwareDriver, Name, Description, URI, LoginID, EncryptedPassword, IDRecorder, Enabled, Settings, EventSettings, VendorProductID, C18NDigest, DriverNumber, DriverProxyName, HostedHardware_id, CameraData, EdgeSettings, SubType, CredentialsLastModified, HardwareDataLastUpdated FROM Hardware WHERE (HostedHardware_id = @HostedHardware_id)";
		_commandCollection[4].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@HostedHardware_id";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HostedHardware_id";
		_commandCollection[4].Parameters.Add(sqlParameter);
		_commandCollection[5] = new SqlCommand();
		_commandCollection[5].Connection = Connection;
		_commandCollection[5].CommandText = "SELECT C18NDigest, Description, DriverNumber, DriverProxyName, Enabled, EncryptedPassword, EventSettings, HostedHardware_id, IDHardware, IDHardwareDriver, IDRecorder, LastModified, LoginID, Name, Settings, URI, VendorProductID, CameraData, EdgeSettings, SubType, CredentialsLastModified, HardwareDataLastUpdated FROM Hardware WHERE (IDRecorder = @IDRecorder)";
		_commandCollection[5].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRecorder";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRecorder";
		_commandCollection[5].Parameters.Add(sqlParameter);
		_commandCollection[6] = new SqlCommand();
		_commandCollection[6].Connection = Connection;
		_commandCollection[6].CommandText = "DELETE FROM [Hardware] WHERE ([IDHardware] = @Original_IDHardware)";
		_commandCollection[6].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDHardware";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_commandCollection[6].Parameters.Add(sqlParameter);
		_commandCollection[7] = new SqlCommand();
		_commandCollection[7].Connection = Connection;
		_commandCollection[7].CommandText = "UPDATE Hardware SET LastModified = @LastModified, IDHardwareDriver = @IDHardwareDriver, Name = @Name, Description = @Description, URI = @URI, LoginID = @LoginID, EncryptedPassword = @EncryptedPassword, IDRecorder = @IDRecorder, Enabled = @Enabled, C18NDigest = @C18NDigest, Settings = @Settings, EventSettings = @EventSettings, VendorProductID = @VendorProductID, DriverNumber = @DriverNumber, DriverProxyName = @DriverProxyName, HostedHardware_id = @HostedHardware_id, CameraData = @CameraData, EdgeSettings = @EdgeSettings, SubType = @SubType WHERE  (IDHardware = @Original_IDHardware)";
		_commandCollection[7].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardwareDriver";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardwareDriver";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 512;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Description";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@URI";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "URI";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LoginID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 512;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LoginID";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EncryptedPassword";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 512;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EncryptedPassword";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRecorder";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRecorder";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Enabled";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@C18NDigest";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 44;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "C18NDigest";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Settings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Settings";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EventSettings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EventSettings";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@VendorProductID";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 512;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "VendorProductID";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DriverNumber";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverNumber";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DriverProxyName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 512;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DriverProxyName";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@HostedHardware_id";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "HostedHardware_id";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CameraData";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CameraData";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@EdgeSettings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "EdgeSettings";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@SubType";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SubType";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDHardware";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_commandCollection[7].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.HardwareDataTable dataTable)
	{
		Adapter.SelectCommand = CommandCollection[0];
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, false)]
	public virtual int FillByDeviceGroup(ConfigurationDataSet.HardwareDataTable dataTable, string IDDeviceGroup)
	{
		Adapter.SelectCommand = CommandCollection[1];
		if (IDDeviceGroup == null)
		{
			throw new ArgumentNullException("IDDeviceGroup");
		}
		Adapter.SelectCommand.Parameters[0].Value = IDDeviceGroup;
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, false)]
	public virtual int FillByHardware(ConfigurationDataSet.HardwareDataTable dataTable, string IDHardware)
	{
		Adapter.SelectCommand = CommandCollection[2];
		if (IDHardware == null)
		{
			throw new ArgumentNullException("IDHardware");
		}
		Adapter.SelectCommand.Parameters[0].Value = IDHardware;
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, false)]
	public virtual int FillByHardwareIds(ConfigurationDataSet.HardwareDataTable dataTable, object ids)
	{
		Adapter.SelectCommand = CommandCollection[3];
		if (ids == null)
		{
			Adapter.SelectCommand.Parameters[1].Value = DBNull.Value;
		}
		else
		{
			Adapter.SelectCommand.Parameters[1].Value = ids;
		}
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, false)]
	public virtual int FillByHostedHardware(ConfigurationDataSet.HardwareDataTable dataTable, int? HostedHardware_id)
	{
		Adapter.SelectCommand = CommandCollection[4];
		if (HostedHardware_id.HasValue)
		{
			Adapter.SelectCommand.Parameters[0].Value = HostedHardware_id.Value;
		}
		else
		{
			Adapter.SelectCommand.Parameters[0].Value = DBNull.Value;
		}
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, false)]
	public virtual int FillByRecorder(ConfigurationDataSet.HardwareDataTable dataTable, string IDRecorder)
	{
		Adapter.SelectCommand = CommandCollection[5];
		if (IDRecorder == null)
		{
			throw new ArgumentNullException("IDRecorder");
		}
		Adapter.SelectCommand.Parameters[0].Value = IDRecorder;
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet.HardwareDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet dataSet)
	{
		return Adapter.Update(dataSet, "Hardware");
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(DataRow dataRow)
	{
		return Adapter.Update(new DataRow[1] { dataRow });
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(DataRow[] dataRows)
	{
		return Adapter.Update(dataRows);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Delete, false)]
	public virtual int ForceDelete(string Original_IDHardware)
	{
		SqlCommand sqlCommand = CommandCollection[6];
		if (Original_IDHardware == null)
		{
			throw new ArgumentNullException("Original_IDHardware");
		}
		sqlCommand.Parameters[0].Value = Original_IDHardware;
		ConnectionState state = sqlCommand.Connection.State;
		if ((sqlCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
		{
			sqlCommand.Connection.Open();
		}
		try
		{
			return sqlCommand.ExecuteNonQuery();
		}
		finally
		{
			if (state == ConnectionState.Closed)
			{
				sqlCommand.Connection.Close();
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int ForceUpdate(DateTime LastModified, string IDHardwareDriver, string Name, string Description, string URI, string LoginID, string EncryptedPassword, string IDRecorder, bool Enabled, string C18NDigest, object Settings, object EventSettings, string VendorProductID, int DriverNumber, string DriverProxyName, int? HostedHardware_id, string CameraData, object EdgeSettings, int SubType, string Original_IDHardware)
	{
		SqlCommand sqlCommand = CommandCollection[7];
		sqlCommand.Parameters[0].Value = LastModified;
		if (IDHardwareDriver == null)
		{
			throw new ArgumentNullException("IDHardwareDriver");
		}
		sqlCommand.Parameters[1].Value = IDHardwareDriver;
		if (Name == null)
		{
			throw new ArgumentNullException("Name");
		}
		sqlCommand.Parameters[2].Value = Name;
		if (Description == null)
		{
			throw new ArgumentNullException("Description");
		}
		sqlCommand.Parameters[3].Value = Description;
		if (URI == null)
		{
			throw new ArgumentNullException("URI");
		}
		sqlCommand.Parameters[4].Value = URI;
		if (LoginID == null)
		{
			throw new ArgumentNullException("LoginID");
		}
		sqlCommand.Parameters[5].Value = LoginID;
		if (EncryptedPassword == null)
		{
			throw new ArgumentNullException("EncryptedPassword");
		}
		sqlCommand.Parameters[6].Value = EncryptedPassword;
		if (IDRecorder == null)
		{
			throw new ArgumentNullException("IDRecorder");
		}
		sqlCommand.Parameters[7].Value = IDRecorder;
		sqlCommand.Parameters[8].Value = Enabled;
		if (C18NDigest == null)
		{
			throw new ArgumentNullException("C18NDigest");
		}
		sqlCommand.Parameters[9].Value = C18NDigest;
		if (Settings == null)
		{
			sqlCommand.Parameters[10].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[10].Value = Settings;
		}
		if (EventSettings == null)
		{
			sqlCommand.Parameters[11].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[11].Value = EventSettings;
		}
		if (VendorProductID == null)
		{
			sqlCommand.Parameters[12].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[12].Value = VendorProductID;
		}
		sqlCommand.Parameters[13].Value = DriverNumber;
		if (DriverProxyName == null)
		{
			throw new ArgumentNullException("DriverProxyName");
		}
		sqlCommand.Parameters[14].Value = DriverProxyName;
		if (HostedHardware_id.HasValue)
		{
			sqlCommand.Parameters[15].Value = HostedHardware_id.Value;
		}
		else
		{
			sqlCommand.Parameters[15].Value = DBNull.Value;
		}
		if (CameraData == null)
		{
			sqlCommand.Parameters[16].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[16].Value = CameraData;
		}
		if (EdgeSettings == null)
		{
			sqlCommand.Parameters[17].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[17].Value = EdgeSettings;
		}
		sqlCommand.Parameters[18].Value = SubType;
		if (Original_IDHardware == null)
		{
			throw new ArgumentNullException("Original_IDHardware");
		}
		sqlCommand.Parameters[19].Value = Original_IDHardware;
		ConnectionState state = sqlCommand.Connection.State;
		if ((sqlCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
		{
			sqlCommand.Connection.Open();
		}
		try
		{
			return sqlCommand.ExecuteNonQuery();
		}
		finally
		{
			if (state == ConnectionState.Closed)
			{
				sqlCommand.Connection.Close();
			}
		}
	}
}
