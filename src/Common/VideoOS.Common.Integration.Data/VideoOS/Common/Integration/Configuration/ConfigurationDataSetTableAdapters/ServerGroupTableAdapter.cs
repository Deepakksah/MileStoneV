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
public class ServerGroupTableAdapter : Component, ITableAdapter
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
		Fill(ds.ServerGroup);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public ServerGroupTableAdapter()
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
		dataTableMapping.DataSetTable = "ServerGroup";
		dataTableMapping.ColumnMappings.Add("IDServerGroup", "IDServerGroup");
		dataTableMapping.ColumnMappings.Add("Name", "Name");
		dataTableMapping.ColumnMappings.Add("Description", "Description");
		dataTableMapping.ColumnMappings.Add("IDParentGroup", "IDParentGroup");
		dataTableMapping.ColumnMappings.Add("StructureType", "StructureType");
		dataTableMapping.ColumnMappings.Add("OrderNo", "OrderNo");
		dataTableMapping.ColumnMappings.Add("Level", "Level");
		dataTableMapping.ColumnMappings.Add("Path", "Path");
		dataTableMapping.ColumnMappings.Add("CreatedDate", "CreatedDate");
		dataTableMapping.ColumnMappings.Add("LastModified", "LastModified");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "DELETE FROM [ServerGroup] WHERE (([IDServerGroup] = @Original_IDServerGroup) AND ([Name] = @Original_Name) AND ((@IsNull_Description = 1 AND [Description] IS NULL) OR ([Description] = @Original_Description)) AND ([IDParentGroup] = @Original_IDParentGroup) AND ([StructureType] = @Original_StructureType) AND ([OrderNo] = @Original_OrderNo) AND ([Level] = @Original_Level) AND ([Path] = @Original_Path) AND ([CreatedDate] = @Original_CreatedDate) AND ([LastModified] = @Original_LastModified))";
		_adapter.DeleteCommand.CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDServerGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDServerGroup";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IsNull_Description";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		sqlParameter.SourceColumnNullMapping = true;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Description";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDParentGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDParentGroup";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_StructureType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "StructureType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_OrderNo";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OrderNo";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Level";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Level";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Path";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Path";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CreatedDate";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "INSERT INTO [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified]) VALUES (@IDServerGroup, @Name, @Description, @IDParentGroup, @StructureType, @OrderNo, @Level, @Path, @CreatedDate, @LastModified);\r\nSELECT IDServerGroup, Name, Description, IDParentGroup, StructureType, OrderNo, Level, Path, CreatedDate, LastModified FROM ServerGroup WHERE (IDServerGroup = @IDServerGroup)";
		_adapter.InsertCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDServerGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDServerGroup";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Name";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Name";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Description";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDParentGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDParentGroup";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@StructureType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "StructureType";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@OrderNo";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OrderNo";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Level";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Level";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Path";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Path";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CreatedDate";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		_adapter.UpdateCommand = new SqlCommand();
		_adapter.UpdateCommand.Connection = Connection;
		_adapter.UpdateCommand.CommandText = "UPDATE [ServerGroup] SET [IDServerGroup] = @IDServerGroup, [Name] = @Name, [Description] = @Description, [IDParentGroup] = @IDParentGroup, [StructureType] = @StructureType, [OrderNo] = @OrderNo, [Level] = @Level, [Path] = @Path, [CreatedDate] = @CreatedDate, [LastModified] = @LastModified WHERE (([IDServerGroup] = @Original_IDServerGroup) AND ([Name] = @Original_Name) AND ((@IsNull_Description = 1 AND [Description] IS NULL) OR ([Description] = @Original_Description)) AND ([IDParentGroup] = @Original_IDParentGroup) AND ([StructureType] = @Original_StructureType) AND ([OrderNo] = @Original_OrderNo) AND ([Level] = @Original_Level) AND ([Path] = @Original_Path) AND ([CreatedDate] = @Original_CreatedDate) AND ([LastModified] = @Original_LastModified));\r\nSELECT IDServerGroup, Name, Description, IDParentGroup, StructureType, OrderNo, Level, Path, CreatedDate, LastModified FROM ServerGroup WHERE (IDServerGroup = @IDServerGroup)";
		_adapter.UpdateCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDServerGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDServerGroup";
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
		sqlParameter.ParameterName = "@IDParentGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDParentGroup";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@StructureType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "StructureType";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@OrderNo";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OrderNo";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Level";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Level";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Path";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Path";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@CreatedDate";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDServerGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDServerGroup";
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
		sqlParameter.ParameterName = "@IsNull_Description";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		sqlParameter.SourceColumnNullMapping = true;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Description";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Description";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDParentGroup";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDParentGroup";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_StructureType";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "StructureType";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_OrderNo";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "OrderNo";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Level";
		sqlParameter.DbType = DbType.Int32;
		sqlParameter.SqlDbType = SqlDbType.Int;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Level";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Path";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Path";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_CreatedDate";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "CreatedDate";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
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
		_commandCollection[0].CommandText = "SELECT        sg.IDServerGroup, Name, Description, IDParentGroup, StructureType, OrderNo, Level, Path, CreatedDate, LastModified\r\n                      FROM            ServerGroup SG WHERE EXISTS(SELECT sgk.IDServerGroup FROM ServerGroupKind sgk WHERE sgk.IDServerGroup = sg.IDServerGroup)";
		_commandCollection[0].CommandType = CommandType.Text;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(ConfigurationDataSet.ServerGroupDataTable dataTable)
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
	public virtual int Update(ConfigurationDataSet.ServerGroupDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(ConfigurationDataSet dataSet)
	{
		return Adapter.Update(dataSet, "ServerGroup");
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
