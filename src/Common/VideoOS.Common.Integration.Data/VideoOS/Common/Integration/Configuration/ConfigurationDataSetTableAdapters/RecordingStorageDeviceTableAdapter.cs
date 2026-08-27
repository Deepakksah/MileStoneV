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
public class RecordingStorageDeviceTableAdapter : Component, ITableAdapterRecorder, ITableAdapter, ITableAdapterHardware
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

	public void Fill(ConfigurationDataSet ds)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		Fill(ds.RecordingStorageDevice);
	}

	public void FillByRecorder(ConfigurationDataSet ds, string idRecorder)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByRecorder(ds.RecordingStorageDevice, idRecorder);
	}

	public void FillByHardware(ConfigurationDataSet ds, string idHardware)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByHardware(ds.RecordingStorageDevice, idHardware);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public RecordingStorageDeviceTableAdapter()
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
		dataTableMapping.DataSetTable = "RecordingStorageDevice";
		dataTableMapping.ColumnMappings.Add("recordingstorage_id", "recordingstorage_id");
		dataTableMapping.ColumnMappings.Add("device_id", "device_id");
		dataTableMapping.ColumnMappings.Add("recording_enabled", "recording_enabled");
		dataTableMapping.ColumnMappings.Add("recording_framerate", "recording_framerate");
		dataTableMapping.ColumnMappings.Add("prebuffer_enabled", "prebuffer_enabled");
		dataTableMapping.ColumnMappings.Add("prebuffer_seconds", "prebuffer_seconds");
		dataTableMapping.ColumnMappings.Add("prebuffer_in_memory", "prebuffer_in_memory");
		dataTableMapping.ColumnMappings.Add("edgestorage_enabled", "edgestorage_enabled");
		dataTableMapping.ColumnMappings.Add("edgestorage_playback", "edgestorage_playback");
		dataTableMapping.ColumnMappings.Add("record_keyframes", "record_keyframes");
		dataTableMapping.ColumnMappings.Add("last_modified", "last_modified");
		dataTableMapping.ColumnMappings.Add("manual_timeout_enabled", "manual_timeout_enabled");
		dataTableMapping.ColumnMappings.Add("manual_timeout", "manual_timeout");
		dataTableMapping.ColumnMappings.Add("record_on_related_devices", "record_on_related_devices");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "DELETE FROM [RecordingStorageDevice] WHERE (([recordingstorage_id] = @Original_recordingstorage_id) AND ([device_id] = @Original_device_id) AND ([recording_enabled] = @Original_recording_enabled) AND ([recording_framerate] = @Original_recording_framerate) AND ([prebuffer_enabled] = @Original_prebuffer_enabled) AND ([prebuffer_seconds] = @Original_prebuffer_seconds) AND ([prebuffer_in_memory] = @Original_prebuffer_in_memory) AND ([edgestorage_enabled] = @Original_edgestorage_enabled) AND ([edgestorage_playback] = @Original_edgestorage_playback) AND ([record_keyframes] = @Original_record_keyframes) AND ([last_modified] = @Original_last_modified) AND ([manual_timeout_enabled] = @Original_manual_timeout_enabled) AND ([manual_timeout] = @Original_manual_timeout) AND ([record_on_related_devices] = @Original_record_on_related_devices))";
		_adapter.DeleteCommand.CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_recordingstorage_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recordingstorage_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_device_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "device_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_recording_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_recording_framerate";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_framerate";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_prebuffer_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_prebuffer_seconds";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_seconds";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_prebuffer_in_memory";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_in_memory";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_edgestorage_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_edgestorage_playback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_playback";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_record_keyframes";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_keyframes";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_last_modified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "last_modified";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_manual_timeout_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_manual_timeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_record_on_related_devices";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_on_related_devices";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "INSERT INTO [RecordingStorageDevice] ([recordingstorage_id], [device_id], [recording_enabled], [recording_framerate], [prebuffer_enabled], [prebuffer_seconds], [prebuffer_in_memory], [edgestorage_enabled], [edgestorage_playback], [record_keyframes], [last_modified], [manual_timeout_enabled], [manual_timeout], [record_on_related_devices]) VALUES (@recordingstorage_id, @device_id, @recording_enabled, @recording_framerate, @prebuffer_enabled, @prebuffer_seconds, @prebuffer_in_memory, @edgestorage_enabled, @edgestorage_playback, @record_keyframes, @last_modified, @manual_timeout_enabled, @manual_timeout, @record_on_related_devices);\r\nSELECT recordingstorage_id, device_id, recording_enabled, recording_framerate, prebuffer_enabled, prebuffer_seconds, prebuffer_in_memory, edgestorage_enabled, edgestorage_playback, record_keyframes, last_modified, manual_timeout_enabled, manual_timeout, record_on_related_devices FROM RecordingStorageDevice WHERE (device_id = @device_id)";
		_adapter.InsertCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recordingstorage_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recordingstorage_id";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@device_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "device_id";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recording_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_enabled";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recording_framerate";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_framerate";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_enabled";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_seconds";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_seconds";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_in_memory";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_in_memory";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@edgestorage_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_enabled";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@edgestorage_playback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_playback";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@record_keyframes";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_keyframes";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@last_modified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "last_modified";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@manual_timeout_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout_enabled";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@manual_timeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@record_on_related_devices";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_on_related_devices";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		_adapter.UpdateCommand = new SqlCommand();
		_adapter.UpdateCommand.Connection = Connection;
		_adapter.UpdateCommand.CommandText = "UPDATE [RecordingStorageDevice] SET [recordingstorage_id] = @recordingstorage_id, [device_id] = @device_id, [recording_enabled] = @recording_enabled, [recording_framerate] = @recording_framerate, [prebuffer_enabled] = @prebuffer_enabled, [prebuffer_seconds] = @prebuffer_seconds, [prebuffer_in_memory] = @prebuffer_in_memory, [edgestorage_enabled] = @edgestorage_enabled, [edgestorage_playback] = @edgestorage_playback, [record_keyframes] = @record_keyframes, [last_modified] = @last_modified, [manual_timeout_enabled] = @manual_timeout_enabled, [manual_timeout] = @manual_timeout, [record_on_related_devices] = @record_on_related_devices WHERE (([recordingstorage_id] = @Original_recordingstorage_id) AND ([device_id] = @Original_device_id) AND ([recording_enabled] = @Original_recording_enabled) AND ([recording_framerate] = @Original_recording_framerate) AND ([prebuffer_enabled] = @Original_prebuffer_enabled) AND ([prebuffer_seconds] = @Original_prebuffer_seconds) AND ([prebuffer_in_memory] = @Original_prebuffer_in_memory) AND ([edgestorage_enabled] = @Original_edgestorage_enabled) AND ([edgestorage_playback] = @Original_edgestorage_playback) AND ([record_keyframes] = @Original_record_keyframes) AND ([last_modified] = @Original_last_modified) AND ([manual_timeout_enabled] = @Original_manual_timeout_enabled) AND ([manual_timeout] = @Original_manual_timeout) AND ([record_on_related_devices] = @Original_record_on_related_devices));\r\nSELECT recordingstorage_id, device_id, recording_enabled, recording_framerate, prebuffer_enabled, prebuffer_seconds, prebuffer_in_memory, edgestorage_enabled, edgestorage_playback, record_keyframes, last_modified, manual_timeout_enabled, manual_timeout, record_on_related_devices FROM RecordingStorageDevice WHERE (device_id = @device_id)";
		_adapter.UpdateCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recordingstorage_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recordingstorage_id";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@device_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "device_id";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recording_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_enabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recording_framerate";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_framerate";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_enabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_seconds";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_seconds";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_in_memory";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_in_memory";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@edgestorage_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_enabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@edgestorage_playback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_playback";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@record_keyframes";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_keyframes";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@last_modified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "last_modified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@manual_timeout_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout_enabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@manual_timeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@record_on_related_devices";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_on_related_devices";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_recordingstorage_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recordingstorage_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_device_id";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "device_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_recording_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_recording_framerate";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_framerate";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_prebuffer_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_prebuffer_seconds";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_seconds";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_prebuffer_in_memory";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_in_memory";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_edgestorage_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_edgestorage_playback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_playback";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_record_keyframes";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_keyframes";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_last_modified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "last_modified";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_manual_timeout_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout_enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_manual_timeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_record_on_related_devices";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_on_related_devices";
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
		_commandCollection = new SqlCommand[5];
		_commandCollection[0] = new SqlCommand();
		_commandCollection[0].Connection = Connection;
		_commandCollection[0].CommandText = "SELECT        recordingstorage_id, device_id, recording_enabled, recording_framerate, prebuffer_enabled, prebuffer_seconds, prebuffer_in_memory, edgestorage_enabled, edgestorage_playback, \r\n                         record_keyframes, last_modified, manual_timeout_enabled, manual_timeout, record_on_related_devices\r\nFROM            RecordingStorageDevice";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "SELECT        rsd.device_id, rsd.edgestorage_enabled, rsd.prebuffer_enabled, rsd.prebuffer_seconds, rsd.prebuffer_in_memory, rsd.recording_enabled, rsd.recording_framerate, rsd.recordingstorage_id, \r\n                         rsd.edgestorage_playback, rsd.record_keyframes, rsd.last_modified, rsd.manual_timeout_enabled, rsd.manual_timeout, rsd.record_on_related_devices\r\nFROM            RecordingStorageDevice AS rsd INNER JOIN Devices AS d ON d.IDDevice = rsd.device_id\r\nWHERE        (d.IDHardware = @IDHardware)\r\n";
		_commandCollection[1].CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardware";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		_commandCollection[1].Parameters.Add(sqlParameter);
		_commandCollection[2] = new SqlCommand();
		_commandCollection[2].Connection = Connection;
		_commandCollection[2].CommandText = "SELECT        rsd.device_id, rsd.edgestorage_enabled, rsd.prebuffer_enabled, rsd.prebuffer_seconds, rsd.prebuffer_in_memory, rsd.recording_enabled, rsd.recording_framerate, rsd.recordingstorage_id, \r\n                         rsd.edgestorage_playback, rsd.record_keyframes, rsd.last_modified, rsd.manual_timeout_enabled, rsd.manual_timeout, rsd.record_on_related_devices\r\nFROM            RecordingStorageDevice AS rsd INNER JOIN\r\n                         RecordingStorage AS rs ON rsd.recordingstorage_id = rs.pkid\r\nWHERE        (rs.recordingserver_id = @IDRecorder)";
		_commandCollection[2].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRecorder";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recordingserver_id";
		_commandCollection[2].Parameters.Add(sqlParameter);
		_commandCollection[3] = new SqlCommand();
		_commandCollection[3].Connection = Connection;
		_commandCollection[3].CommandText = "DELETE FROM RecordingStorageDevice\r\nWHERE        (device_id = @Original_device_id)";
		_commandCollection[3].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_device_id";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "device_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_commandCollection[3].Parameters.Add(sqlParameter);
		_commandCollection[4] = new SqlCommand();
		_commandCollection[4].Connection = Connection;
		_commandCollection[4].CommandText = "UPDATE       RecordingStorageDevice\r\nSET                recordingstorage_id = @recordingstorage_id, device_id = @device_id, recording_enabled = @recording_enabled, recording_framerate = @recording_framerate, \r\n                         prebuffer_enabled = @prebuffer_enabled, prebuffer_seconds = @prebuffer_seconds, prebuffer_in_memory = @prebuffer_in_memory, edgestorage_enabled = @edgestorage_enabled, \r\n                         edgestorage_playback = @edgestorage_playback, record_keyframes = @record_keyframes, manual_timeout_enabled = @manual_timeout_enabled, \r\n                         manual_timeout = @manual_timeout_minutes, record_on_related_devices = @record_on_related_devices, last_modified = @last_modified\r\nWHERE        (device_id = @Original_device_id);      \r\nSELECT recordingstorage_id, device_id, recording_enabled, recording_framerate, prebuffer_enabled, prebuffer_seconds, edgestorage_enabled, edgestorage_playback, record_keyframes, last_modified, manual_timeout_enabled, manual_timeout, record_on_related_devices FROM RecordingStorageDevice WHERE (device_id = @device_id)";
		_commandCollection[4].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recordingstorage_id";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recordingstorage_id";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@device_id";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "device_id";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recording_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_enabled";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@recording_framerate";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "recording_framerate";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_enabled";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_seconds";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_seconds";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@prebuffer_in_memory";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "prebuffer_in_memory";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@edgestorage_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_enabled";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@edgestorage_playback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "edgestorage_playback";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@record_keyframes";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_keyframes";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@manual_timeout_enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout_enabled";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@manual_timeout_minutes";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "manual_timeout";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@record_on_related_devices";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "record_on_related_devices";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@last_modified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "last_modified";
		_commandCollection[4].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_device_id";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "device_id";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_commandCollection[4].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.RecordingStorageDeviceDataTable dataTable)
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
	public virtual int FillByHardware(ConfigurationDataSet.RecordingStorageDeviceDataTable dataTable, string IDHardware)
	{
		Adapter.SelectCommand = CommandCollection[1];
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
	public virtual int FillByRecorder(ConfigurationDataSet.RecordingStorageDeviceDataTable dataTable, string IDRecorder)
	{
		Adapter.SelectCommand = CommandCollection[2];
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
	public virtual int Update(ConfigurationDataSet.RecordingStorageDeviceDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet dataSet)
	{
		return Adapter.Update(dataSet, "RecordingStorageDevice");
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
	public virtual int ForceDelete(string Original_device_id)
	{
		SqlCommand sqlCommand = CommandCollection[3];
		if (Original_device_id == null)
		{
			throw new ArgumentNullException("Original_device_id");
		}
		sqlCommand.Parameters[0].Value = Original_device_id;
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
	[DataObjectMethod(DataObjectMethodType.Update, false)]
	public virtual int ForceUpdate(string recordingstorage_id, string device_id, bool recording_enabled, double recording_framerate, bool prebuffer_enabled, int prebuffer_seconds, bool prebuffer_in_memory, bool edgestorage_enabled, bool edgestorage_playback, bool record_keyframes, bool manual_timeout_enabled, int manual_timeout_minutes, bool record_on_related_devices, DateTime last_modified, string Original_device_id)
	{
		SqlCommand sqlCommand = CommandCollection[4];
		if (recordingstorage_id == null)
		{
			throw new ArgumentNullException("recordingstorage_id");
		}
		sqlCommand.Parameters[0].Value = recordingstorage_id;
		if (device_id == null)
		{
			throw new ArgumentNullException("device_id");
		}
		sqlCommand.Parameters[1].Value = device_id;
		sqlCommand.Parameters[2].Value = recording_enabled;
		sqlCommand.Parameters[3].Value = recording_framerate;
		sqlCommand.Parameters[4].Value = prebuffer_enabled;
		sqlCommand.Parameters[5].Value = prebuffer_seconds;
		sqlCommand.Parameters[6].Value = prebuffer_in_memory;
		sqlCommand.Parameters[7].Value = edgestorage_enabled;
		sqlCommand.Parameters[8].Value = edgestorage_playback;
		sqlCommand.Parameters[9].Value = record_keyframes;
		sqlCommand.Parameters[10].Value = manual_timeout_enabled;
		sqlCommand.Parameters[11].Value = manual_timeout_minutes;
		sqlCommand.Parameters[12].Value = record_on_related_devices;
		sqlCommand.Parameters[13].Value = last_modified;
		if (Original_device_id == null)
		{
			throw new ArgumentNullException("Original_device_id");
		}
		sqlCommand.Parameters[14].Value = Original_device_id;
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
