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
public class DevicesTableAdapter : Component, ITableAdapterRecorder, ITableAdapter, ITableAdapterHardware, IUserContextAdapter
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
		Fill(ds.Devices);
	}

	public void FillByRecorder(ConfigurationDataSet ds, string idRecorder)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByRecorder(ds.Devices, idRecorder);
	}

	public void FillByHardware(ConfigurationDataSet ds, string idHardware)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		FillByIDHardware(ds.Devices, idHardware);
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

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public DevicesTableAdapter()
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
		dataTableMapping.DataSetTable = "Devices";
		dataTableMapping.ColumnMappings.Add("LastModified", "LastModified");
		dataTableMapping.ColumnMappings.Add("IDDevice", "IDDevice");
		dataTableMapping.ColumnMappings.Add("IDHardware", "IDHardware");
		dataTableMapping.ColumnMappings.Add("Name", "Name");
		dataTableMapping.ColumnMappings.Add("Description", "Description");
		dataTableMapping.ColumnMappings.Add("Location", "Location");
		dataTableMapping.ColumnMappings.Add("DeviceType", "DeviceType");
		dataTableMapping.ColumnMappings.Add("Enabled", "Enabled");
		dataTableMapping.ColumnMappings.Add("Settings", "Settings");
		dataTableMapping.ColumnMappings.Add("Presets", "Presets");
		dataTableMapping.ColumnMappings.Add("Shortcut", "Shortcut");
		dataTableMapping.ColumnMappings.Add("MulticastEnabled", "MulticastEnabled");
		dataTableMapping.ColumnMappings.Add("Streams", "Streams");
		dataTableMapping.ColumnMappings.Add("DeviceIndex", "DeviceIndex");
		dataTableMapping.ColumnMappings.Add("SubType", "SubType");
		dataTableMapping.ColumnMappings.Add("FailoverLiveOnly", "FailoverLiveOnly");
		dataTableMapping.ColumnMappings.Add("DisableOnFailover", "DisableOnFailover");
		dataTableMapping.ColumnMappings.Add("PtzManualSessionTimeout", "PtzManualSessionTimeout");
		dataTableMapping.ColumnMappings.Add("PtzReservedSessionTimeout", "PtzReservedSessionTimeout");
		dataTableMapping.ColumnMappings.Add("PtzExtendedManualSessionTimeout", "PtzExtendedManualSessionTimeout");
		dataTableMapping.ColumnMappings.Add("GisPointString", "GisPointString");
		dataTableMapping.ColumnMappings.Add("ShortName", "ShortName");
		dataTableMapping.ColumnMappings.Add("Icon", "Icon");
		dataTableMapping.ColumnMappings.Add("CoverageDirection", "CoverageDirection");
		dataTableMapping.ColumnMappings.Add("CoverageDepth", "CoverageDepth");
		dataTableMapping.ColumnMappings.Add("CoverageFieldOfView", "CoverageFieldOfView");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "dataset_device_delete";
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
		sqlParameter.ParameterName = "@Original_IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "dataset_device_insert";
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
		sqlParameter.ParameterName = "@IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
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
		sqlParameter.ParameterName = "@Location";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Location";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DeviceType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.Size = 100;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceType";
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
		sqlParameter.ParameterName = "@Presets";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Presets";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Shortcut";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 128;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Shortcut";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@MulticastEnabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MulticastEnabled";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Streams";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Streams";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DeviceIndex";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceIndex";
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
		sqlParameter.ParameterName = "@FailoverLiveOnly";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "FailoverLiveOnly";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DisableOnFailover";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DisableOnFailover";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzManualSessionTimeout";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzReservedSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzReservedSessionTimeout";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzExtendedManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzExtendedManualSessionTimeout";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@GisPointString";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GisPointString";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ShortName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 128;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "ShortName";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Icon";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Icon";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageDirection";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDirection";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageDepth";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDepth";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageFieldOfView";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageFieldOfView";
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
		_adapter.UpdateCommand.CommandText = "UPDATE [Devices]\r\n                                 SET [LastModified] = @LastModified, [IDDevice] = @IDDevice, [IDHardware] = @IDHardware, [Name] = @Name,\r\n                                   [Description] = @Description, [Location] = @Location, [DeviceType] = @DeviceType, [Enabled] = @Enabled, [Settings] = @Settings,\r\n                                   [Presets] = @Presets, [Shortcut] = @Shortcut, [MulticastEnabled] = @MulticastEnabled, [Streams] = @Streams, \r\n                                   [DeviceIndex] = @DeviceIndex, [SubType] = @SubType, [FailoverLiveOnly] = @FailoverLiveOnly,\r\n                                   [DisableOnFailover] = @DisableOnFailover, [PtzManualSessionTimeout] = @PtzManualSessionTimeout,\r\n                                   [PtzReservedSessionTimeout] = @PtzReservedSessionTimeout, [PtzExtendedManualSessionTimeout] = @PtzExtendedManualSessionTimeout,\r\n                                   [GisPoint] = geography::STPointFromText(@GisPointString, 4326), [ShortName] = @ShortName,\r\n                                   [Icon] = @Icon, [CoverageDirection] = @CoverageDirection, [CoverageDepth] = @CoverageDepth, [CoverageFieldOfView] = @CoverageFieldOfView\r\n                                 WHERE (([LastModified] = @Original_LastModified) AND ([IDDevice] = @Original_IDDevice) AND ([IDHardware] = @Original_IDHardware) AND\r\n                                   ([Name] = @Original_Name) AND ([DeviceType] = @Original_DeviceType) AND ([Enabled] = @Original_Enabled) AND ([Shortcut] = @Original_Shortcut) AND\r\n                                   ([DeviceIndex] = @Original_DeviceIndex) AND ([SubType] = @Original_SubType) AND ([FailoverLiveOnly] = @Original_FailoverLiveOnly) AND\r\n                                   ([DisableOnFailover] = @Original_DisableOnFailover) AND ([PtzManualSessionTimeout] = @Original_PtzManualSessionTimeout) AND\r\n                                   ([PtzReservedSessionTimeout] = @Original_PtzReservedSessionTimeout) AND ([PtzExtendedManualSessionTimeout] = @Original_PtzExtendedManualSessionTimeout) AND\r\n                                   [GisPoint].STEquals(geography::STPointFromText(@Original_GisPointString, 4326)) = 1 AND ([ShortName] = @Original_ShortName) AND ([Icon] = @Original_Icon) AND\r\n                                   ([CoverageDirection] = @Original_CoverageDirection) AND ([CoverageDepth] = @Original_CoverageDepth) AND ([CoverageFieldOfView] = @Original_CoverageFieldOfView));\r\n                                 SELECT LastModified, IDDevice, IDHardware, Name, Description, Location, DeviceType, Enabled, Settings, Presets, Shortcut,\r\n                                   MulticastEnabled, Streams, DeviceIndex, SubType, FailoverLiveOnly, DisableOnFailover, PtzManualSessionTimeout,\r\n                                   PtzReservedSessionTimeout, PtzExtendedManualSessionTimeout, CAST((CASE WHEN GisPoint IS NULL THEN '' ELSE GisPoint.ToString() END) AS NVARCHAR(MAX)) AS GisPointString,\r\n                                   ShortName, Icon, CoverageDirection, CoverageDepth, CoverageFieldOfView FROM Devices WHERE (IDDevice = @IDDevice)\r\n                    ";
		_adapter.UpdateCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardware";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Description";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Location";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Location";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DeviceType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceType";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Enabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Settings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Settings";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Presets";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Presets";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Shortcut";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Shortcut";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@MulticastEnabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MulticastEnabled";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Streams";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Streams";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DeviceIndex";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceIndex";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@SubType";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SubType";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@FailoverLiveOnly";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "FailoverLiveOnly";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DisableOnFailover";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DisableOnFailover";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzManualSessionTimeout";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzReservedSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzReservedSessionTimeout";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzExtendedManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzExtendedManualSessionTimeout";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@GisPointString";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GisPointString";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ShortName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "ShortName";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Icon";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Icon";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageDirection";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDirection";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageDepth";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDepth";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageFieldOfView";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageFieldOfView";
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
		sqlParameter.ParameterName = "@Original_IDDevice";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDHardware";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_DeviceType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Enabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Enabled";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Shortcut";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Shortcut";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_DeviceIndex";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceIndex";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_SubType";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SubType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_FailoverLiveOnly";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "FailoverLiveOnly";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_DisableOnFailover";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DisableOnFailover";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_PtzManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzManualSessionTimeout";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_PtzReservedSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzReservedSessionTimeout";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_PtzExtendedManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzExtendedManualSessionTimeout";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_GisPointString";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GisPointString";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_ShortName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "ShortName";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Icon";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Icon";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CoverageDirection";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDirection";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CoverageDepth";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDepth";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CoverageFieldOfView";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageFieldOfView";
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
		_commandCollection[0].CommandText = "SELECT        LastModified, IDDevice, IDHardware, Name, Description, Location, DeviceType, Enabled, Settings, Presets, Shortcut, MulticastEnabled, Streams, DeviceIndex, SubType, FailoverLiveOnly, \r\n                         DisableOnFailover, PtzManualSessionTimeout, PtzReservedSessionTimeout, PtzExtendedManualSessionTimeout, CAST((CASE WHEN GisPoint IS NULL THEN '' ELSE GisPoint.ToString() END) \r\n                         AS NVARCHAR(MAX)) AS GisPointString, ShortName, Icon, CoverageDirection, CoverageDepth, CoverageFieldOfView\r\nFROM            Devices";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "dataset_device_select_by_ids";
		_commandCollection[1].CommandType = CommandType.StoredProcedure;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@RETURN_VALUE";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.Direction = ParameterDirection.ReturnValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_commandCollection[1].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ids";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = null;
		_commandCollection[1].Parameters.Add(sqlParameter);
		_commandCollection[2] = new SqlCommand();
		_commandCollection[2].Connection = Connection;
		_commandCollection[2].CommandText = "SELECT CoverageDepth, CoverageDirection, CoverageFieldOfView, Description, DeviceIndex, DeviceType, DisableOnFailover, Enabled, FailoverLiveOnly, CAST((CASE WHEN GisPoint IS NULL THEN '' ELSE GisPoint.ToString() END) AS NVARCHAR(MAX)) AS GisPointString, IDDevice, IDHardware, Icon, LastModified, Location, MulticastEnabled, Name, Presets, PtzExtendedManualSessionTimeout, PtzManualSessionTimeout, PtzReservedSessionTimeout, Settings, ShortName, Shortcut, Streams, SubType FROM Devices WHERE (IDDevice = @IDDevice)";
		_commandCollection[2].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDDevice";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		_commandCollection[2].Parameters.Add(sqlParameter);
		_commandCollection[3] = new SqlCommand();
		_commandCollection[3].Connection = Connection;
		_commandCollection[3].CommandText = "SELECT CAST((CASE WHEN GisPoint IS NULL THEN '' ELSE GisPoint.ToString() END) AS NVARCHAR(MAX)) AS GisPointString, d.CoverageDepth, d.CoverageDirection, d.CoverageFieldOfView, d.Description, d.DeviceIndex, d.DeviceType, d.DisableOnFailover, d.Enabled, d.FailoverLiveOnly, d.IDDevice, d.IDHardware, d.Icon, d.LastModified, d.Location, d.MulticastEnabled, d.Name, d.Presets, d.PtzExtendedManualSessionTimeout, d.PtzManualSessionTimeout, d.PtzReservedSessionTimeout, d.Settings, d.ShortName, d.Shortcut, d.Streams, d.SubType FROM Devices AS d INNER JOIN DeviceGroupMembers AS dgm ON d.IDDevice = dgm.IDDevice WHERE (dgm.IDDeviceGroup = @IDDeviceGroup)";
		_commandCollection[3].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDDeviceGroup";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDeviceGroup";
		_commandCollection[3].Parameters.Add(sqlParameter);
		_commandCollection[4] = new SqlCommand();
		_commandCollection[4].Connection = Connection;
		_commandCollection[4].CommandText = "SELECT        Description, DeviceIndex, DeviceType, DisableOnFailover, Enabled, FailoverLiveOnly, IDDevice, IDHardware, LastModified, Location, MulticastEnabled, Name, Presets, \r\n                         PtzExtendedManualSessionTimeout, PtzManualSessionTimeout, PtzReservedSessionTimeout, Settings, Shortcut, Streams, SubType, CAST((CASE WHEN GisPoint IS NULL THEN '' ELSE GisPoint.ToString() END) \r\n                         AS NVARCHAR(MAX)) AS GisPointString, ShortName, Icon, CoverageDirection, CoverageDepth, CoverageFieldOfView\r\nFROM            Devices\r\nWHERE        (IDHardware = @IDHardware)";
		_commandCollection[4].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDHardware";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
		_commandCollection[4].Parameters.Add(sqlParameter);
		_commandCollection[5] = new SqlCommand();
		_commandCollection[5].Connection = Connection;
		_commandCollection[5].CommandText = "SELECT        d.Description, d.DeviceIndex, d.DeviceType, d.DisableOnFailover, d.Enabled, d.FailoverLiveOnly, d.IDDevice, d.IDHardware, d.LastModified, d.Location, d.MulticastEnabled, d.Name, d.Presets, \r\n                         d.PtzExtendedManualSessionTimeout, d.PtzManualSessionTimeout, d.PtzReservedSessionTimeout, d.Settings, d.Shortcut, d.Streams, d.SubType, CAST((CASE WHEN GisPoint IS NULL \r\n                         THEN '' ELSE GisPoint.ToString() END) AS NVARCHAR(MAX)) AS GisPointString, d.ShortName, d.Icon, d.CoverageDirection, d.CoverageDepth, d.CoverageFieldOfView\r\nFROM            Devices AS d INNER JOIN\r\n                         Hardware AS h ON d.IDHardware = h.IDHardware\r\nWHERE        (h.IDRecorder = @IDRecorder)";
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
		_commandCollection[6].CommandText = "DELETE FROM [Devices] WHERE ([IDDevice] = @Original_IDDevice)";
		_commandCollection[6].CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDDevice";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_commandCollection[6].Parameters.Add(sqlParameter);
		_commandCollection[7] = new SqlCommand();
		_commandCollection[7].Connection = Connection;
		_commandCollection[7].CommandText = "UPDATE Devices\r\n                                 SET LastModified = @LastModified, IDHardware = @IDHardware, Name = @Name, Description = @Description,\r\n                                   Location = @Location, DeviceType = @DeviceType, Enabled = @Enabled, Settings = @Settings,\r\n                                   Presets = @Presets, Shortcut = @Shortcut, MulticastEnabled = @MulticastEnabled, Streams = @Streams,\r\n                                   DeviceIndex = @DeviceIndex, SubType = @SubType, FailoverLiveOnly = @FailoverLiveOnly,\r\n                                   DisableOnFailover = @DisableOnFailover, PtzManualSessionTimeout = @PtzManualSessionTimeout,\r\n                                   PtzReservedSessionTimeout = @PtzReservedSessionTimeout, PtzExtendedManualSessionTimeout = @PtzExtendedManualSessionTimeout,\r\n                                   GisPoint = geography::STPointFromText(@GisPointString, 4326), ShortName = @ShortName, Icon = @Icon, CoverageDirection = @CoverageDirection, CoverageDepth = @CoverageDepth, CoverageFieldOfView = @CoverageFieldOfView\r\n                                 WHERE (IDDevice = @Original_IDDevice)\r\n                    ";
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
		sqlParameter.ParameterName = "@IDHardware";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDHardware";
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
		sqlParameter.ParameterName = "@Location";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Location";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DeviceType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.Size = 100;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceType";
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
		sqlParameter.ParameterName = "@Settings";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Settings";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Presets";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Presets";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Shortcut";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Shortcut";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@MulticastEnabled";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "MulticastEnabled";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Streams";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Streams";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DeviceIndex";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DeviceIndex";
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
		sqlParameter.ParameterName = "@FailoverLiveOnly";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "FailoverLiveOnly";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@DisableOnFailover";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "DisableOnFailover";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzManualSessionTimeout";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzReservedSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzReservedSessionTimeout";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@PtzExtendedManualSessionTimeout";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "PtzExtendedManualSessionTimeout";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@GisPointString";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GisPointString";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@ShortName";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 128;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "ShortName";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Icon";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.Size = 4;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Icon";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageDirection";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDirection";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageDepth";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageDepth";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CoverageFieldOfView";
		sqlParameter.DbType = DbType.Double;
		sqlParameter.SqlDbType = SqlDbType.Float;
		sqlParameter.Size = 8;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CoverageFieldOfView";
		_commandCollection[7].Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDDevice";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDDevice";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_commandCollection[7].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.DevicesDataTable dataTable)
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
	public virtual int FillByDeviceIds(ConfigurationDataSet.DevicesDataTable dataTable, object ids)
	{
		Adapter.SelectCommand = CommandCollection[1];
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
	public virtual int FillByIDDevice(ConfigurationDataSet.DevicesDataTable dataTable, string IDDevice)
	{
		Adapter.SelectCommand = CommandCollection[2];
		if (IDDevice == null)
		{
			throw new ArgumentNullException("IDDevice");
		}
		Adapter.SelectCommand.Parameters[0].Value = IDDevice;
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
	public virtual int FillByIDDeviceGroup(ConfigurationDataSet.DevicesDataTable dataTable, string IDDeviceGroup)
	{
		Adapter.SelectCommand = CommandCollection[3];
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
	public virtual int FillByIDHardware(ConfigurationDataSet.DevicesDataTable dataTable, string IDHardware)
	{
		Adapter.SelectCommand = CommandCollection[4];
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
	public virtual int FillByRecorder(ConfigurationDataSet.DevicesDataTable dataTable, string IDRecorder)
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
	public virtual int Update(ConfigurationDataSet.DevicesDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet dataSet)
	{
		return Adapter.Update(dataSet, "Devices");
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
	public virtual int ForceDelete(string Original_IDDevice)
	{
		SqlCommand sqlCommand = CommandCollection[6];
		if (Original_IDDevice == null)
		{
			throw new ArgumentNullException("Original_IDDevice");
		}
		sqlCommand.Parameters[0].Value = Original_IDDevice;
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
	public virtual int ForceUpdate(DateTime LastModified, string IDHardware, string Name, string Description, string Location, string DeviceType, bool Enabled, object Settings, object Presets, string Shortcut, bool MulticastEnabled, object Streams, int DeviceIndex, int SubType, bool FailoverLiveOnly, bool DisableOnFailover, int PtzManualSessionTimeout, int PtzReservedSessionTimeout, int PtzExtendedManualSessionTimeout, string GisPointString, string ShortName, int Icon, double CoverageDirection, double CoverageDepth, double CoverageFieldOfView, string Original_IDDevice)
	{
		SqlCommand sqlCommand = CommandCollection[7];
		sqlCommand.Parameters[0].Value = LastModified;
		if (IDHardware == null)
		{
			throw new ArgumentNullException("IDHardware");
		}
		sqlCommand.Parameters[1].Value = IDHardware;
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
		if (Location == null)
		{
			throw new ArgumentNullException("Location");
		}
		sqlCommand.Parameters[4].Value = Location;
		if (DeviceType == null)
		{
			throw new ArgumentNullException("DeviceType");
		}
		sqlCommand.Parameters[5].Value = DeviceType;
		sqlCommand.Parameters[6].Value = Enabled;
		if (Settings == null)
		{
			sqlCommand.Parameters[7].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[7].Value = Settings;
		}
		if (Presets == null)
		{
			sqlCommand.Parameters[8].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[8].Value = Presets;
		}
		if (Shortcut == null)
		{
			throw new ArgumentNullException("Shortcut");
		}
		sqlCommand.Parameters[9].Value = Shortcut;
		sqlCommand.Parameters[10].Value = MulticastEnabled;
		if (Streams == null)
		{
			sqlCommand.Parameters[11].Value = DBNull.Value;
		}
		else
		{
			sqlCommand.Parameters[11].Value = Streams;
		}
		sqlCommand.Parameters[12].Value = DeviceIndex;
		sqlCommand.Parameters[13].Value = SubType;
		sqlCommand.Parameters[14].Value = FailoverLiveOnly;
		sqlCommand.Parameters[15].Value = DisableOnFailover;
		sqlCommand.Parameters[16].Value = PtzManualSessionTimeout;
		sqlCommand.Parameters[17].Value = PtzReservedSessionTimeout;
		sqlCommand.Parameters[18].Value = PtzExtendedManualSessionTimeout;
		if (GisPointString == null)
		{
			throw new ArgumentNullException("GisPointString");
		}
		sqlCommand.Parameters[19].Value = GisPointString;
		if (ShortName == null)
		{
			throw new ArgumentNullException("ShortName");
		}
		sqlCommand.Parameters[20].Value = ShortName;
		sqlCommand.Parameters[21].Value = Icon;
		sqlCommand.Parameters[22].Value = CoverageDirection;
		sqlCommand.Parameters[23].Value = CoverageDepth;
		sqlCommand.Parameters[24].Value = CoverageFieldOfView;
		if (Original_IDDevice == null)
		{
			throw new ArgumentNullException("Original_IDDevice");
		}
		sqlCommand.Parameters[25].Value = Original_IDDevice;
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
