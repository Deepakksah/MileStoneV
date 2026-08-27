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
public class DeviceTrackTableAdapter : Component, ITableAdapterRecorder, ITableAdapter, ITableAdapterHardware
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
		Fill(ds.DeviceTrack);
	}

	public void FillByRecorder(ConfigurationDataSet ds, string idRecorder)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByRecorder(ds.DeviceTrack, idRecorder);
	}

	public void FillByHardware(ConfigurationDataSet ds, string idHardware)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByHardware(ds.DeviceTrack, idHardware);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public DeviceTrackTableAdapter()
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
		dataTableMapping.DataSetTable = "DeviceTrack";
		dataTableMapping.ColumnMappings.Add("IDDevice", "IDDevice");
		dataTableMapping.ColumnMappings.Add("IDTrack", "IDTrack");
		dataTableMapping.ColumnMappings.Add("IDTrackDefinition", "IDTrackDefinition");
		dataTableMapping.ColumnMappings.Add("MergeEdge", "MergeEdge");
		dataTableMapping.ColumnMappings.Add("DefaultPlayback", "DefaultPlayback");
		dataTableMapping.ColumnMappings.Add("InUse", "InUse");
		dataTableMapping.ColumnMappings.Add("OnlyUseMediaAfter", "OnlyUseMediaAfter");
		dataTableMapping.ColumnMappings.Add("CreatedDate", "CreatedDate");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "DELETE FROM [DeviceTrack] WHERE (([IDDevice] = @Original_IDDevice) AND ([IDTrack] = @Original_IDTrack) AND ([IDTrackDefinition] = @Original_IDTrackDefinition) AND ([MergeEdge] = @Original_MergeEdge) AND ([DefaultPlayback] = @Original_DefaultPlayback) AND ([InUse] = @Original_InUse) AND ([OnlyUseMediaAfter] = @Original_OnlyUseMediaAfter) AND ([CreatedDate] = @Original_CreatedDate))";
		_adapter.DeleteCommand.CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDTrack";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrack";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDTrackDefinition";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrackDefinition";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_MergeEdge";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MergeEdge";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_DefaultPlayback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DefaultPlayback";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_InUse";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "InUse";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_OnlyUseMediaAfter";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OnlyUseMediaAfter";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CreatedDate";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "INSERT INTO [DeviceTrack] ([IDDevice], [IDTrack], [IDTrackDefinition], [MergeEdge], [DefaultPlayback], [InUse], [OnlyUseMediaAfter], [CreatedDate]) VALUES (@IDDevice, @IDTrack, @IDTrackDefinition, @MergeEdge, @DefaultPlayback, @InUse, @OnlyUseMediaAfter, @CreatedDate);\r\nSELECT IDDevice, IDTrack, IDTrackDefinition, MergeEdge, DefaultPlayback, InUse, OnlyUseMediaAfter, CreatedDate FROM DeviceTrack WHERE (IDTrack = @IDTrack)";
		_adapter.InsertCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDTrack";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrack";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDTrackDefinition";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrackDefinition";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@MergeEdge";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MergeEdge";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DefaultPlayback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DefaultPlayback";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@InUse";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "InUse";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@OnlyUseMediaAfter";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OnlyUseMediaAfter";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CreatedDate";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		_adapter.UpdateCommand = new SqlCommand();
		_adapter.UpdateCommand.Connection = Connection;
		_adapter.UpdateCommand.CommandText = "UPDATE [DeviceTrack] SET [IDDevice] = @IDDevice, [IDTrack] = @IDTrack, [IDTrackDefinition] = @IDTrackDefinition, [MergeEdge] = @MergeEdge, [DefaultPlayback] = @DefaultPlayback, [InUse] = @InUse, [OnlyUseMediaAfter] = @OnlyUseMediaAfter, [CreatedDate] = @CreatedDate WHERE (([IDDevice] = @Original_IDDevice) AND ([IDTrack] = @Original_IDTrack) AND ([IDTrackDefinition] = @Original_IDTrackDefinition) AND ([MergeEdge] = @Original_MergeEdge) AND ([DefaultPlayback] = @Original_DefaultPlayback) AND ([InUse] = @Original_InUse) AND ([OnlyUseMediaAfter] = @Original_OnlyUseMediaAfter) AND ([CreatedDate] = @Original_CreatedDate));\r\nSELECT IDDevice, IDTrack, IDTrackDefinition, MergeEdge, DefaultPlayback, InUse, OnlyUseMediaAfter, CreatedDate FROM DeviceTrack WHERE (IDTrack = @IDTrack)";
		_adapter.UpdateCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDTrack";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrack";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDTrackDefinition";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrackDefinition";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@MergeEdge";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MergeEdge";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DefaultPlayback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DefaultPlayback";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@InUse";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "InUse";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@OnlyUseMediaAfter";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OnlyUseMediaAfter";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CreatedDate";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDTrack";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrack";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDTrackDefinition";
		sqlParameter.DbType = DbType.Guid;
		sqlParameter.SqlDbType = SqlDbType.UniqueIdentifier;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDTrackDefinition";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_MergeEdge";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MergeEdge";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_DefaultPlayback";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DefaultPlayback";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_InUse";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "InUse";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_OnlyUseMediaAfter";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OnlyUseMediaAfter";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CreatedDate";
		sqlParameter.DbType = DbType.DateTime2;
		sqlParameter.SqlDbType = SqlDbType.DateTime2;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
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
		_commandCollection = new SqlCommand[4];
		_commandCollection[0] = new SqlCommand();
		_commandCollection[0].Connection = Connection;
		_commandCollection[0].CommandText = "SELECT        IDDevice, IDTrack, IDTrackDefinition, MergeEdge, DefaultPlayback, InUse, OnlyUseMediaAfter, CreatedDate\r\nFROM            DeviceTrack";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "SELECT        dt.IDDevice, dt.IDTrack, dt.IDTrackDefinition, dt.MergeEdge, dt.DefaultPlayback, dt.InUse, dt.OnlyUseMediaAfter, dt.CreatedDate\r\nFROM            DeviceTrack AS dt INNER JOIN\r\n                         Devices AS d ON dt.IDDevice = d.IDDevice\r\nWHERE        (d.IDHardware = @IDHardware)";
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
		_commandCollection[2].CommandText = "dbo.dataset_device_tracks_select_by_device_ids";
		_commandCollection[2].CommandType = CommandType.StoredProcedure;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@RETURN_VALUE";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.Direction = ParameterDirection.ReturnValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_commandCollection[2].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ids";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_commandCollection[2].Parameters.Add(sqlParameter);
		_commandCollection[3] = new SqlCommand();
		_commandCollection[3].Connection = Connection;
		_commandCollection[3].CommandText = "SELECT        dt.IDDevice, dt.IDTrack, dt.IDTrackDefinition, dt.MergeEdge, dt.DefaultPlayback, dt.InUse, dt.OnlyUseMediaAfter, dt.CreatedDate\r\nFROM            DeviceTrack AS dt INNER JOIN\r\n                         Devices AS d ON dt.IDDevice = d.IDDevice INNER JOIN\r\n                         Hardware AS h ON d.IDHardware = h.IDHardware\r\nWHERE        (h.IDRecorder = @IDRecorder)";
		_commandCollection[3].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRecorder";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRecorder";
		_commandCollection[3].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.DeviceTrackDataTable dataTable)
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
	public virtual int FillByHardware(ConfigurationDataSet.DeviceTrackDataTable dataTable, string IDHardware)
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
	public virtual int FillByIDDevices(ConfigurationDataSet.DeviceTrackDataTable dataTable, object ids)
	{
		Adapter.SelectCommand = CommandCollection[2];
		if (ids == null)
		{
			throw new ArgumentNullException("ids");
		}
		Adapter.SelectCommand.Parameters[1].Value = ids;
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
	public virtual int FillByRecorder(ConfigurationDataSet.DeviceTrackDataTable dataTable, string IDRecorder)
	{
		Adapter.SelectCommand = CommandCollection[3];
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
	public virtual int Update(ConfigurationDataSet.DeviceTrackDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet dataSet)
	{
		return Adapter.Update(dataSet, "DeviceTrack");
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
