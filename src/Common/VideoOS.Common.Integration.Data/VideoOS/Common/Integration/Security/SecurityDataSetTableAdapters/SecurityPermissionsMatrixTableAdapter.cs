using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using VideoOS.Common.Integration.Data.Properties;

namespace VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;

[DesignerCategory("code")]
[ToolboxItem(true)]
[DataObject(true)]
[Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[HelpKeyword("vs.data.TableAdapter")]
public class SecurityPermissionsMatrixTableAdapter : Component, ITableAdapterSecurityServer, ITableAdapterSecurity, ITableAdapterSecurityRole
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

	public SqlConnection SqlConnection
	{
		get
		{
			return Connection;
		}
		set
		{
			Connection = value;
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
	public SqlConnection Connection{
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

	public void Update(SecurityDataSet ds, SqlTransaction transaction)
	{
		UpdateHelper.UpdateDataAdapter(_adapter, ds, transaction);
	}

	public void Fill(SecurityDataSet ds)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		Fill(ds.SecurityPermissionsMatrix);
	}

	public void FillByRole(SecurityDataSet ds, string idRole)
	{
		if (ds == null)
		{
			throw new ArgumentNullException("ds");
		}
		if (idRole == null)
		{
			throw new ArgumentNullException("idRole");
		}
		FillByRole(ds.SecurityPermissionsMatrix, idRole);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public SecurityPermissionsMatrixTableAdapter()
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
		dataTableMapping.DataSetTable = "SecurityPermissionsMatrix";
		dataTableMapping.ColumnMappings.Add("LastModified", "LastModified");
		dataTableMapping.ColumnMappings.Add("SecurityAttributeID", "SecurityAttributeID");
		dataTableMapping.ColumnMappings.Add("IDRole", "IDRole");
		dataTableMapping.ColumnMappings.Add("IDMatrix", "IDMatrix");
		dataTableMapping.ColumnMappings.Add("Value", "Value");
		_adapter.TableMappings.Add(dataTableMapping);
		_adapter.DeleteCommand = new SqlCommand();
		_adapter.DeleteCommand.Connection = Connection;
		_adapter.DeleteCommand.CommandText = "DELETE FROM [dbo].[SecurityPermissionsMatrix] WHERE (([LastModified] = @Original_LastModified) AND ([SecurityAttributeID] = @Original_SecurityAttributeID) AND ([IDRole] = @Original_IDRole) AND ([IDMatrix] = @Original_IDMatrix) AND ([Value] = @Original_Value))";
		_adapter.DeleteCommand.CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_SecurityAttributeID";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SecurityAttributeID";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDRole";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRole";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDMatrix";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDMatrix";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Value";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Value";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.DeleteCommand.Parameters.Add(sqlParameter);
		_adapter.InsertCommand = new SqlCommand();
		_adapter.InsertCommand.Connection = Connection;
		_adapter.InsertCommand.CommandText = "INSERT INTO [dbo].[SecurityPermissionsMatrix] ([LastModified], [SecurityAttributeID], [IDRole], [IDMatrix], [Value]) VALUES (@LastModified, @SecurityAttributeID, @IDRole, @IDMatrix, @Value);\r\nSELECT LastModified, SecurityAttributeID, IDRole, IDMatrix, Value FROM SecurityPermissionsMatrix WHERE (IDMatrix = @IDMatrix) AND (IDRole = @IDRole) AND (SecurityAttributeID = @SecurityAttributeID)";
		_adapter.InsertCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@SecurityAttributeID";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SecurityAttributeID";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRole";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRole";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDMatrix";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDMatrix";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Value";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Value";
		_adapter.InsertCommand.Parameters.Add(sqlParameter);
		_adapter.UpdateCommand = new SqlCommand();
		_adapter.UpdateCommand.Connection = Connection;
		_adapter.UpdateCommand.CommandText = "UPDATE [dbo].[SecurityPermissionsMatrix] SET [LastModified] = @LastModified, [SecurityAttributeID] = @SecurityAttributeID, [IDRole] = @IDRole, [IDMatrix] = @IDMatrix, [Value] = @Value WHERE (([LastModified] = @Original_LastModified) AND ([SecurityAttributeID] = @Original_SecurityAttributeID) AND ([IDRole] = @Original_IDRole) AND ([IDMatrix] = @Original_IDMatrix) AND ([Value] = @Original_Value));\r\nSELECT LastModified, SecurityAttributeID, IDRole, IDMatrix, Value FROM SecurityPermissionsMatrix WHERE (IDMatrix = @IDMatrix) AND (IDRole = @IDRole) AND (SecurityAttributeID = @SecurityAttributeID)";
		_adapter.UpdateCommand.CommandType = CommandType.Text;
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@LastModified";
		sqlParameter.DbType = DbType.DateTime;
		sqlParameter.SqlDbType = SqlDbType.DateTime;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "LastModified";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@SecurityAttributeID";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SecurityAttributeID";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRole";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRole";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDMatrix";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDMatrix";
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Value";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Value";
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
		sqlParameter.ParameterName = "@Original_SecurityAttributeID";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.VarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "SecurityAttributeID";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDRole";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRole";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_IDMatrix";
		sqlParameter.DbType = DbType.AnsiStringFixedLength;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDMatrix";
		sqlParameter.SourceVersion = DataRowVersion.Original;
		_adapter.UpdateCommand.Parameters.Add(sqlParameter);
		sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@Original_Value";
		sqlParameter.SqlDbType = SqlDbType.NVarChar;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "Value";
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
		_commandCollection[0].CommandText = "SELECT LastModified, SecurityAttributeID, IDRole, IDMatrix, Value FROM dbo.SecurityPermissionsMatrix";
		_commandCollection[0].CommandType = CommandType.Text;
		_commandCollection[1] = new SqlCommand();
		_commandCollection[1].Connection = Connection;
		_commandCollection[1].CommandText = "SELECT LastModified, SecurityAttributeID, IDRole, IDMatrix, Value FROM dbo.SecurityPermissionsMatrix\r\nWHERE (IDRole = @IDRole)";
		_commandCollection[1].CommandType = CommandType.Text;
		SqlParameter sqlParameter = new SqlParameter();
		sqlParameter.ParameterName = "@IDRole";
		sqlParameter.DbType = DbType.AnsiString;
		sqlParameter.SqlDbType = SqlDbType.Char;
		sqlParameter.Size = 36;
		sqlParameter.IsNullable = true;
		sqlParameter.SourceColumn = "IDRole";
		_commandCollection[1].Parameters.Add(sqlParameter);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Fill, true)]
	public virtual int Fill(SecurityDataSet.SecurityPermissionsMatrixDataTable dataTable)
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
	public virtual int FillByRole(SecurityDataSet.SecurityPermissionsMatrixDataTable dataTable, string IDRole)
	{
		Adapter.SelectCommand = CommandCollection[1];
		if (IDRole == null)
		{
			throw new ArgumentNullException("IDRole");
		}
		Adapter.SelectCommand.Parameters[0].Value = IDRole;
		if (ClearBeforeFill)
		{
			((DataTable)(object)dataTable).Clear();
		}
		return Adapter.Fill((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(SecurityDataSet.SecurityPermissionsMatrixDataTable dataTable)
	{
		return Adapter.Update((DataTable)(object)dataTable);
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	public virtual int Update(SecurityDataSet dataSet)
	{
		return Adapter.Update(dataSet, "SecurityPermissionsMatrix");
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
	[DataObjectMethod(DataObjectMethodType.Delete, true)]
	public virtual int Delete(DateTime Original_LastModified, string Original_SecurityAttributeID, string Original_IDRole, string Original_IDMatrix, string Original_Value)
	{
		Adapter.DeleteCommand.Parameters[0].Value = Original_LastModified;
		if (Original_SecurityAttributeID == null)
		{
			throw new ArgumentNullException("Original_SecurityAttributeID");
		}
		Adapter.DeleteCommand.Parameters[1].Value = Original_SecurityAttributeID;
		if (Original_IDRole == null)
		{
			throw new ArgumentNullException("Original_IDRole");
		}
		Adapter.DeleteCommand.Parameters[2].Value = Original_IDRole;
		if (Original_IDMatrix == null)
		{
			throw new ArgumentNullException("Original_IDMatrix");
		}
		Adapter.DeleteCommand.Parameters[3].Value = Original_IDMatrix;
		if (Original_Value == null)
		{
			throw new ArgumentNullException("Original_Value");
		}
		Adapter.DeleteCommand.Parameters[4].Value = Original_Value;
		ConnectionState state = Adapter.DeleteCommand.Connection.State;
		if ((Adapter.DeleteCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
		{
			Adapter.DeleteCommand.Connection.Open();
		}
		try
		{
			return Adapter.DeleteCommand.ExecuteNonQuery();
		}
		finally
		{
			if (state == ConnectionState.Closed)
			{
				Adapter.DeleteCommand.Connection.Close();
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Insert, true)]
	public virtual int Insert(DateTime LastModified, string SecurityAttributeID, string IDRole, string IDMatrix, string Value)
	{
		Adapter.InsertCommand.Parameters[0].Value = LastModified;
		if (SecurityAttributeID == null)
		{
			throw new ArgumentNullException("SecurityAttributeID");
		}
		Adapter.InsertCommand.Parameters[1].Value = SecurityAttributeID;
		if (IDRole == null)
		{
			throw new ArgumentNullException("IDRole");
		}
		Adapter.InsertCommand.Parameters[2].Value = IDRole;
		if (IDMatrix == null)
		{
			throw new ArgumentNullException("IDMatrix");
		}
		Adapter.InsertCommand.Parameters[3].Value = IDMatrix;
		if (Value == null)
		{
			throw new ArgumentNullException("Value");
		}
		Adapter.InsertCommand.Parameters[4].Value = Value;
		ConnectionState state = Adapter.InsertCommand.Connection.State;
		if ((Adapter.InsertCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
		{
			Adapter.InsertCommand.Connection.Open();
		}
		try
		{
			return Adapter.InsertCommand.ExecuteNonQuery();
		}
		finally
		{
			if (state == ConnectionState.Closed)
			{
				Adapter.InsertCommand.Connection.Close();
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Update, true)]
	public virtual int Update(DateTime LastModified, string SecurityAttributeID, string IDRole, string IDMatrix, string Value, DateTime Original_LastModified, string Original_SecurityAttributeID, string Original_IDRole, string Original_IDMatrix, string Original_Value)
	{
		Adapter.UpdateCommand.Parameters[0].Value = LastModified;
		if (SecurityAttributeID == null)
		{
			throw new ArgumentNullException("SecurityAttributeID");
		}
		Adapter.UpdateCommand.Parameters[1].Value = SecurityAttributeID;
		if (IDRole == null)
		{
			throw new ArgumentNullException("IDRole");
		}
		Adapter.UpdateCommand.Parameters[2].Value = IDRole;
		if (IDMatrix == null)
		{
			throw new ArgumentNullException("IDMatrix");
		}
		Adapter.UpdateCommand.Parameters[3].Value = IDMatrix;
		if (Value == null)
		{
			throw new ArgumentNullException("Value");
		}
		Adapter.UpdateCommand.Parameters[4].Value = Value;
		Adapter.UpdateCommand.Parameters[5].Value = Original_LastModified;
		if (Original_SecurityAttributeID == null)
		{
			throw new ArgumentNullException("Original_SecurityAttributeID");
		}
		Adapter.UpdateCommand.Parameters[6].Value = Original_SecurityAttributeID;
		if (Original_IDRole == null)
		{
			throw new ArgumentNullException("Original_IDRole");
		}
		Adapter.UpdateCommand.Parameters[7].Value = Original_IDRole;
		if (Original_IDMatrix == null)
		{
			throw new ArgumentNullException("Original_IDMatrix");
		}
		Adapter.UpdateCommand.Parameters[8].Value = Original_IDMatrix;
		if (Original_Value == null)
		{
			throw new ArgumentNullException("Original_Value");
		}
		Adapter.UpdateCommand.Parameters[9].Value = Original_Value;
		ConnectionState state = Adapter.UpdateCommand.Connection.State;
		if ((Adapter.UpdateCommand.Connection.State & ConnectionState.Open) != ConnectionState.Open)
		{
			Adapter.UpdateCommand.Connection.Open();
		}
		try
		{
			return Adapter.UpdateCommand.ExecuteNonQuery();
		}
		finally
		{
			if (state == ConnectionState.Closed)
			{
				Adapter.UpdateCommand.Connection.Close();
			}
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[HelpKeyword("vs.data.TableAdapter")]
	[DataObjectMethod(DataObjectMethodType.Update, true)]
	public virtual int Update(DateTime LastModified, string Value, DateTime Original_LastModified, string Original_SecurityAttributeID, string Original_IDRole, string Original_IDMatrix, string Original_Value)
	{
		return Update(LastModified, Original_SecurityAttributeID, Original_IDRole, Original_IDMatrix, Value, Original_LastModified, Original_SecurityAttributeID, Original_IDRole, Original_IDMatrix, Original_Value);
	}
}
