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
public class DeviceRestrictedMediaTableAdapter : Component, ITableAdapterRecorder, ITableAdapter, ITableAdapterHardware
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
		Fill(ds.DeviceRestrictedMedia);
	}

	public void FillByRecorder(ConfigurationDataSet ds, string idRecorder)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByRecorder(ds.DeviceRestrictedMedia, idRecorder);
	}

	public void FillByHardware(ConfigurationDataSet ds, string idHardware)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByHardware(ds.DeviceRestrictedMedia, idHardware);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public DeviceRestrictedMediaTableAdapter()
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
		dataTableMapping.DataSetTable = "DeviceRestrictedMedia";
		dataTableMapping.ColumnMappings.Add("IDRestrictedMedia", "IDRestrictedMedia");
		dataTableMapping.ColumnMappings.Add("IDDevice", "IDDevice");
		dataTableMapping.ColumnMappings.Add("IDRecorder", "IDRecorder");
		dataTableMapping.ColumnMappings.Add("StartTime", "StartTime");
		dataTableMapping.ColumnMappings.Add("EndTime", "EndTime");
		_adapter.TableMappings.Add(dataTableMapping);
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
		_commandCollection = new SqlCommand[3];
		_commandCollection[0] = new SqlCommand();
		_commandCollection[0].Connection = Connection;
		_commandCollection[0].CommandText = "SELECT        IDRestrictedMedia, IDDevice, IDRecorder, StartTime, EndTime\r\nFROM            DeviceRestrictedMedia";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "SELECT        drm.IDRestrictedMedia, drm.IDDevice, drm.IDRecorder, drm.StartTime, drm.EndTime\r\nFROM            DeviceRestrictedMedia AS drm INNER JOIN\r\n                         Devices AS d ON drm.IDDevice = d.IDDevice\r\nWHERE        (d.IDHardware = @IDHardware)";
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
		_commandCollection[2].CommandText = "SELECT        IDRestrictedMedia, IDDevice, IDRecorder, StartTime, EndTime\r\nFROM            DeviceRestrictedMedia\r\nWHERE        (IDRecorder = @IDRecorder)";
		_commandCollection[2].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRecorder";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRecorder";
		_commandCollection[2].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.DeviceRestrictedMediaDataTable dataTable)
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
	public virtual int FillByHardware(ConfigurationDataSet.DeviceRestrictedMediaDataTable dataTable, string IDHardware)
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
	public virtual int FillByRecorder(ConfigurationDataSet.DeviceRestrictedMediaDataTable dataTable, string IDRecorder)
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
}
