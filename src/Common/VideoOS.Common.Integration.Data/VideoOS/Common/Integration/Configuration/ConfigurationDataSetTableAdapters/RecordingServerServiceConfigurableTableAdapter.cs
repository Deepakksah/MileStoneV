using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VideoOS.Common.Integration.Data.Properties;

namespace VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

[DesignerCategory("code")]
[ToolboxItem(true)]
[DataObject(true)]
[Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[HelpKeyword("vs.data.TableAdapter")]
public class RecordingServerServiceConfigurableTableAdapter : Component, ITableAdapterRecorderService, ITableAdapter
{
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

	public void FillByRecorderService(ConfigurationDataSet ds, string idRecorderService)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByRecorderService(ds.RecordingServerServiceConfigurable, idRecorderService);
	}

	public void Fill(ConfigurationDataSet ds)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		Fill(ds.RecordingServerServiceConfigurable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public RecordingServerServiceConfigurableTableAdapter()
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
		dataTableMapping.DataSetTable = "RecordingServerServiceConfigurable";
		dataTableMapping.ColumnMappings.Add("LastModified", "LastModified");
		dataTableMapping.ColumnMappings.Add("PKID", "PKID");
		dataTableMapping.ColumnMappings.Add("MulticastServerAddress", "MulticastServerAddress");
		dataTableMapping.ColumnMappings.Add("ShutdownOnStorageFailure", "ShutdownOnStorageFailure");
		dataTableMapping.ColumnMappings.Add("PublicAccessEnabled", "PublicAccessEnabled");
		dataTableMapping.ColumnMappings.Add("PublicWebServerHostname", "PublicWebServerHostname");
		dataTableMapping.ColumnMappings.Add("PublicWebServerPort", "PublicWebServerPort");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "dataset_recording_server_service_configurable_delete";
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
		sqlParameter.ParameterName = "@pkid";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PKID";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "INSERT INTO [RecordingServerServiceConfigurable] ([LastModified], [PKID], [MulticastServerAddress], [ShutdownOnStorageFailure], [PublicAccessEnabled], [PublicWebServerHostname], [PublicWebServerPort]) VALUES (@LastModified, @PKID, @MulticastServerAddress, @ShutdownOnStorageFailure, @PublicAccessEnabled, @PublicWebServerHostname, @PublicWebServerPort);\r\nSELECT LastModified, PKID, MulticastServerAddress, ShutdownOnStorageFailure, PublicAccessEnabled, PublicWebServerHostname, PublicWebServerPort FROM RecordingServerServiceConfigurable WHERE (PKID = @PKID)";
		_adapter.InsertCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PKID";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PKID";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@MulticastServerAddress";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MulticastServerAddress";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ShutdownOnStorageFailure";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "ShutdownOnStorageFailure";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PublicAccessEnabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicAccessEnabled";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PublicWebServerHostname";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicWebServerHostname";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PublicWebServerPort";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicWebServerPort";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		_adapter.UpdateCommand = new SqlCommand();
		_adapter.UpdateCommand.Connection = Connection;
		_adapter.UpdateCommand.CommandText = "UPDATE [RecordingServerServiceConfigurable] SET [LastModified] = @LastModified, [PKID] = @PKID, [MulticastServerAddress] = @MulticastServerAddress, [ShutdownOnStorageFailure] = @ShutdownOnStorageFailure, [PublicAccessEnabled] = @PublicAccessEnabled, [PublicWebServerHostname] = @PublicWebServerHostname, [PublicWebServerPort] = @PublicWebServerPort WHERE (([LastModified] = @Original_LastModified) AND ([PKID] = @Original_PKID) AND ([MulticastServerAddress] = @Original_MulticastServerAddress) AND ([ShutdownOnStorageFailure] = @Original_ShutdownOnStorageFailure) AND ([PublicAccessEnabled] = @Original_PublicAccessEnabled) AND ([PublicWebServerHostname] = @Original_PublicWebServerHostname) AND ([PublicWebServerPort] = @Original_PublicWebServerPort));\r\nSELECT LastModified, PKID, MulticastServerAddress, ShutdownOnStorageFailure, PublicAccessEnabled, PublicWebServerHostname, PublicWebServerPort FROM RecordingServerServiceConfigurable WHERE (PKID = @PKID)";
		_adapter.UpdateCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PKID";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PKID";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@MulticastServerAddress";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MulticastServerAddress";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ShutdownOnStorageFailure";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "ShutdownOnStorageFailure";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PublicAccessEnabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicAccessEnabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PublicWebServerHostname";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicWebServerHostname";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PublicWebServerPort";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicWebServerPort";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_PKID";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PKID";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_MulticastServerAddress";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MulticastServerAddress";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_ShutdownOnStorageFailure";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "ShutdownOnStorageFailure";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_PublicAccessEnabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicAccessEnabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_PublicWebServerHostname";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicWebServerHostname";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_PublicWebServerPort";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PublicWebServerPort";
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
		_commandCollection = new SqlCommand[2];
		_commandCollection[0] = new SqlCommand();
		_commandCollection[0].Connection = Connection;
		_commandCollection[0].CommandText = "SELECT        LastModified, PKID, MulticastServerAddress, ShutdownOnStorageFailure, PublicAccessEnabled, PublicWebServerHostname, PublicWebServerPort\r\nFROM            RecordingServerServiceConfigurable";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "SELECT        LastModified, PKID, MulticastServerAddress, ShutdownOnStorageFailure, PublicAccessEnabled, PublicWebServerHostname, PublicWebServerPort\r\nFROM            RecordingServerServiceConfigurable\r\nWHERE        (PKID = @IDService)";
		_commandCollection[1].CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDService";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PKID";
		_commandCollection[1].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.RecordingServerServiceConfigurableDataTable dataTable)
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
	public virtual int FillByRecorderService(ConfigurationDataSet.RecordingServerServiceConfigurableDataTable dataTable, string IDService)
	{
		Adapter.SelectCommand = CommandCollection[1];
		if (IDService == null)
		{
			throw new ArgumentNullException("IDService");
		}
		Adapter.SelectCommand.Parameters[0].Value = IDService;
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet.RecordingServerServiceConfigurableDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet dataSet)
	{
		return Adapter.Update(dataSet, "RecordingServerServiceConfigurable");
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
}
