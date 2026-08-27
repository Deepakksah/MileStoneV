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
public class EventTypesTableAdapter : Component, ITableAdapter, IUserContextAdapter
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
		Fill(ds.EventTypes);
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
	public EventTypesTableAdapter()
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
		dataTableMapping.DataSetTable = "EventTypes";
		dataTableMapping.ColumnMappings.Add("LastModified", "LastModified");
		dataTableMapping.ColumnMappings.Add("IDEventType", "IDEventType");
		dataTableMapping.ColumnMappings.Add("Definition", "Definition");
		dataTableMapping.ColumnMappings.Add("GeneratorType", "GeneratorType");
		dataTableMapping.ColumnMappings.Add("IDGenerator", "IDGenerator");
		dataTableMapping.ColumnMappings.Add("Builtin", "Builtin");
		dataTableMapping.ColumnMappings.Add("Name", "Name");
		dataTableMapping.ColumnMappings.Add("GeneratorSubType", "GeneratorSubType");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "dataset_eventtype_delete";
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
		sqlParameter.ParameterName = "@Original_IDEventType";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDEventType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "dataset_eventtype_insert";
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
		sqlParameter.ParameterName = "@IDEventType";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDEventType";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Definition";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.Size = int.MaxValue;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Definition";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@GeneratorType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.Size = 100;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GeneratorType";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDGenerator";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDGenerator";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Builtin";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.Size = 1;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Builtin";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.Size = 256;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@GeneratorSubType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GeneratorSubType";
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
		_adapter.UpdateCommand.CommandText = "UPDATE [dbo].[EventTypes] SET [LastModified] = @LastModified, [IDEventType] = @IDEventType, [Definition] = @Definition, [GeneratorType] = @GeneratorType, [IDGenerator] = @IDGenerator, [Builtin] = @Builtin, [Name] = @Name, [GeneratorSubType] = @GeneratorSubType WHERE (([LastModified] = @Original_LastModified) AND ([IDEventType] = @Original_IDEventType) AND ([GeneratorType] = @Original_GeneratorType) AND ((@IsNull_IDGenerator = 1 AND [IDGenerator] IS NULL) OR ([IDGenerator] = @Original_IDGenerator)) AND ([Builtin] = @Original_Builtin) AND ([Name] = @Original_Name) AND ([GeneratorSubType] = @Original_GeneratorSubType));\r\nSELECT LastModified, IDEventType, Definition, GeneratorType, IDGenerator, Builtin, Name, GeneratorSubType FROM EventTypes WHERE (IDEventType = @IDEventType)";
		_adapter.UpdateCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDEventType";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDEventType";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Definition";
		sqlParameter.DbType = DbType.Xml;
		sqlParameter.SqlDbType = SqlDbType.Xml;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Definition";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@GeneratorType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GeneratorType";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDGenerator";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDGenerator";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Builtin";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Builtin";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@GeneratorSubType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GeneratorSubType";
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
		sqlParameter.ParameterName = "@Original_IDEventType";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDEventType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_GeneratorType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GeneratorType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IsNull_IDGenerator";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDGenerator";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		sqlParameter.SourceColumnNullMapping = true;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDGenerator";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDGenerator";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Builtin";
		sqlParameter.DbType = DbType.Boolean;
		sqlParameter.SqlDbType = SqlDbType.Bit;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Builtin";
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
		sqlParameter.ParameterName = "@Original_GeneratorSubType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "GeneratorSubType";
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
		_commandCollection = new SqlCommand[1];
		_commandCollection[0] = new SqlCommand();
		_commandCollection[0].Connection = Connection;
		_commandCollection[0].CommandText = "SELECT LastModified, IDEventType, Definition, GeneratorType, IDGenerator, Builtin, Name, GeneratorSubType FROM dbo.EventTypes";
		_commandCollection[0].CommandType = CommandType.Text;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.EventTypesDataTable dataTable)
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
	public virtual int Update(ConfigurationDataSet.EventTypesDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet dataSet)
	{
		return Adapter.Update(dataSet, "EventTypes");
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
