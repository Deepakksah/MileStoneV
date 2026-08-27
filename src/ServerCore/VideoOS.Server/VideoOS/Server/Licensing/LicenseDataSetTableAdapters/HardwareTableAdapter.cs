using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VideoOS.Server.Properties;

namespace VideoOS.Server.Licensing.LicenseDataSetTableAdapters;

[DesignerCategory("code")]
[ToolboxItem(true)]
[DataObject(true)]
[Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[HelpKeyword("vs.data.TableAdapter")]
public class HardwareTableAdapter : Component, ITableAdapterLicensing
{
	private SqlDataAdapter _adapter;

	private SqlConnection _connection;

	private SqlTransaction _transaction;

	private SqlCommand[] _commandCollection;

	private bool _clearBeforeFill;

	public SqlConnection SqlConnection
	{
		[DebuggerStepThrough]
		get
		{
			return Connection;
		}
		[DebuggerStepThrough]
		set
		{
			Connection = value;
		}
	}

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
	public SqlDataAdapter Adapter
	{
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
	public SqlTransaction Transaction
	{
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
		dataTableMapping.ColumnMappings.Add("IDHardware", "IDHardware");
		dataTableMapping.ColumnMappings.Add("Enabled", "Enabled");
		dataTableMapping.ColumnMappings.Add("Name", "Name");
		dataTableMapping.ColumnMappings.Add("IDRecorder", "IDRecorder");
		dataTableMapping.ColumnMappings.Add("DriverName", "DriverName");
		dataTableMapping.ColumnMappings.Add("DriverVersion", "DriverVersion");
		dataTableMapping.ColumnMappings.Add("VendorProductID", "VendorProductID");
		dataTableMapping.ColumnMappings.Add("MacAddress", "MacAddress");
		dataTableMapping.ColumnMappings.Add("SerialNumber", "SerialNumber");
		dataTableMapping.ColumnMappings.Add("SubType", "SubType");
		dataTableMapping.ColumnMappings.Add("DetectedModel", "DetectedModel");
		dataTableMapping.ColumnMappings.Add("DriverNumber", "DriverNumber");
		_adapter.TableMappings.Add(dataTableMapping);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitConnection()
	{
		_connection = new SqlConnection();
		_connection.ConnectionString = Settings.Default.SurveillanceConnectionString;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private void InitCommandCollection()
	{
		_commandCollection = new SqlCommand[2];
		_commandCollection[0] = new SqlCommand();
		_commandCollection[0].Connection = Connection;
		_commandCollection[0].CommandText = "SELECT        \r\n\tCAST(h.IDHardware AS UNIQUEIDENTIFIER) AS IDHardware, CAST(h.IDRecorder AS UNIQUEIDENTIFIER) AS IDRecorder, \r\n\th.Name, h.Enabled, h.VendorProductID, \r\n\thd.DriverName,\r\n\thd.DriverVersion,\r\n\tUPPER(h.MacAddress) AS MacAddress,\r\n\th.SerialNumber,\r\n\th.DetectedModel,\r\n\th.SubType,\r\n\th.DriverNumber\r\nFROM\r\n\tHardware AS h\r\n\tINNER JOIN HardwareDefinition AS hd ON h.C18NDigest = hd.C18NDigest\r\n\r\n";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "SELECT\r\n\tCAST(h.IDHardware AS UNIQUEIDENTIFIER) AS IDHardware, CAST(h.IDRecorder AS UNIQUEIDENTIFIER) AS IDRecorder,\r\n\th.Name, h.Enabled, h.VendorProductID,\r\n\thd.DriverName,\r\n\thd.DriverVersion,\r\n\tUPPER(h.MacAddress) AS MacAddress, \r\n\th.SerialNumber,\r\n\th.DetectedModel,\r\n\th.SubType,\r\n\th.DriverNumber\r\nFROM\r\n\tHardware AS h \r\n\tINNER JOIN HardwareDefinition AS hd ON h.C18NDigest = hd.C18NDigest\r\n                   INNER JOIN Recorders AS r ON h.IDRecorder = r.IDRecorder\r\nWHERE\r\n                  (h.Enabled = @Enabled) AND (r.Enabled = @Enabled)";
		_commandCollection[1].CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Enabled";
		_commandCollection[1].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(LicenseDataSet.HardwareDataTable dataTable)
	{
		Adapter.SelectCommand = CommandCollection[0];
		if (ClearBeforeFill)
		{
			dataTable.Clear();
		}
		return Adapter.Fill(dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, false)]
	public virtual int FillByEnabled(LicenseDataSet.HardwareDataTable dataTable, bool Enabled)
	{
		Adapter.SelectCommand = CommandCollection[1];
		Adapter.SelectCommand.Parameters[0].Value = Enabled;
		if (ClearBeforeFill)
		{
			dataTable.Clear();
		}
		return Adapter.Fill(dataTable);
	}
}
