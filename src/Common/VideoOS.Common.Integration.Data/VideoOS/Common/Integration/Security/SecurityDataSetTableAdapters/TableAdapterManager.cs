using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace VideoOS.Common.Integration.Security.SecurityDataSetTableAdapters;

[DesignerCategory("code")]
[ToolboxItem(true)]
[Designer("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerDesigner, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
[HelpKeyword("vs.data.TableAdapterManager")]
public class TableAdapterManager : Component
{
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public enum UpdateOrderOption
	{
		InsertUpdateDelete,
		UpdateInsertDelete
	}

	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private class SelfReferenceComparer : IComparer<DataRow>
	{
		private DataRelation _relation;

		private int _childFirst;

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public SelfReferenceComparer(DataRelation relation, bool childFirst)
		{
			_relation = relation;
			if (childFirst)
			{
				_childFirst = -1;
			}
			else
			{
				_childFirst = 1;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		private DataRow GetRoot(DataRow row, out int distance)
		{
			DataRow result = row;
			distance = 0;
			IDictionary<DataRow, DataRow> dictionary = new Dictionary<DataRow, DataRow>();
			dictionary[row] = row;
			DataRow parentRow = row.GetParentRow(_relation, DataRowVersion.Default);
			while (parentRow != null && !dictionary.ContainsKey(parentRow))
			{
				distance++;
				result = parentRow;
				dictionary[parentRow] = parentRow;
				parentRow = parentRow.GetParentRow(_relation, DataRowVersion.Default);
			}
			if (distance == 0)
			{
				dictionary.Clear();
				dictionary[row] = row;
				parentRow = row.GetParentRow(_relation, DataRowVersion.Original);
				while (parentRow != null && !dictionary.ContainsKey(parentRow))
				{
					distance++;
					result = parentRow;
					dictionary[parentRow] = parentRow;
					parentRow = parentRow.GetParentRow(_relation, DataRowVersion.Original);
				}
			}
			return result;
		}

		[DebuggerNonUserCode]
		[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
		public int Compare(DataRow row1, DataRow row2)
		{
			if (row1 == row2)
			{
				return 0;
			}
			if (row1 == null)
			{
				return -1;
			}
			if (row2 == null)
			{
				return 1;
			}
			int distance = 0;
			DataRow root = GetRoot(row1, out distance);
			int distance2 = 0;
			DataRow root2 = GetRoot(row2, out distance2);
			if (root == root2)
			{
				return _childFirst * distance.CompareTo(distance2);
			}
			if (root.Table.Rows.IndexOf(root) < root2.Table.Rows.IndexOf(root2))
			{
				return -1;
			}
			return 1;
		}
	}

	private UpdateOrderOption _updateOrder;

	private SecurityAttributesTableAdapter _securityAttributesTableAdapter;

	private SecurityPermissionsDeviceTableAdapter _securityPermissionsDeviceTableAdapter;

	private SecurityPermissionsMatrixTableAdapter _securityPermissionsMatrixTableAdapter;

	private SecurityPermissionsTableAdapter _securityPermissionsTableAdapter;

	private TimeProfileDefaultRoleRelationTableAdapter _timeProfileDefaultRoleRelationTableAdapter;

	private bool _backupDataSetBeforeUpdate;

	private IDbConnection _connection;

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public UpdateOrderOption UpdateOrder
	{
		get
		{
			return _updateOrder;
		}
		set
		{
			_updateOrder = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public SecurityAttributesTableAdapter SecurityAttributesTableAdapter
	{
		get
		{
			return _securityAttributesTableAdapter;
		}
		set
		{
			_securityAttributesTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public SecurityPermissionsDeviceTableAdapter SecurityPermissionsDeviceTableAdapter
	{
		get
		{
			return _securityPermissionsDeviceTableAdapter;
		}
		set
		{
			_securityPermissionsDeviceTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public SecurityPermissionsMatrixTableAdapter SecurityPermissionsMatrixTableAdapter
	{
		get
		{
			return _securityPermissionsMatrixTableAdapter;
		}
		set
		{
			_securityPermissionsMatrixTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public SecurityPermissionsTableAdapter SecurityPermissionsTableAdapter
	{
		get
		{
			return _securityPermissionsTableAdapter;
		}
		set
		{
			_securityPermissionsTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Editor("Microsoft.VSDesigner.DataSource.Design.TableAdapterManagerPropertyEditor, Microsoft.VSDesigner, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor")]
	public TimeProfileDefaultRoleRelationTableAdapter TimeProfileDefaultRoleRelationTableAdapter
	{
		get
		{
			return _timeProfileDefaultRoleRelationTableAdapter;
		}
		set
		{
			_timeProfileDefaultRoleRelationTableAdapter = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public bool BackupDataSetBeforeUpdate
	{
		get
		{
			return _backupDataSetBeforeUpdate;
		}
		set
		{
			_backupDataSetBeforeUpdate = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	public IDbConnection Connection
	{
		get
		{
			if (_connection != null)
			{
				return _connection;
			}
			if (_securityAttributesTableAdapter != null && _securityAttributesTableAdapter.Connection != null)
			{
				return _securityAttributesTableAdapter.Connection;
			}
			if (_securityPermissionsDeviceTableAdapter != null && _securityPermissionsDeviceTableAdapter.Connection != null)
			{
				return _securityPermissionsDeviceTableAdapter.Connection;
			}
			if (_securityPermissionsMatrixTableAdapter != null && _securityPermissionsMatrixTableAdapter.Connection != null)
			{
				return _securityPermissionsMatrixTableAdapter.Connection;
			}
			if (_securityPermissionsTableAdapter != null && _securityPermissionsTableAdapter.Connection != null)
			{
				return _securityPermissionsTableAdapter.Connection;
			}
			if (_timeProfileDefaultRoleRelationTableAdapter != null && _timeProfileDefaultRoleRelationTableAdapter.Connection != null)
			{
				return _timeProfileDefaultRoleRelationTableAdapter.Connection;
			}
			return null;
		}
		set
		{
			_connection = value;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	[Browsable(false)]
	public int TableAdapterInstanceCount
	{
		get
		{
			int num = 0;
			if (_securityAttributesTableAdapter != null)
			{
				num++;
			}
			if (_securityPermissionsDeviceTableAdapter != null)
			{
				num++;
			}
			if (_securityPermissionsMatrixTableAdapter != null)
			{
				num++;
			}
			if (_securityPermissionsTableAdapter != null)
			{
				num++;
			}
			if (_timeProfileDefaultRoleRelationTableAdapter != null)
			{
				num++;
			}
			return num;
		}
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private int UpdateUpdatedRows(SecurityDataSet dataSet, List<DataRow> allChangedRows, List<DataRow> allAddedRows)
	{
		int num = 0;
		if (_securityAttributesTableAdapter != null)
		{
			DataRow[] updatedRows = ((DataTable)(object)dataSet.SecurityAttributes).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows = GetRealUpdatedRows(updatedRows, allAddedRows);
			if (updatedRows != null && updatedRows.Length != 0)
			{
				num += _securityAttributesTableAdapter.Update(updatedRows);
				allChangedRows.AddRange(updatedRows);
			}
		}
		if (_securityPermissionsTableAdapter != null)
		{
			DataRow[] updatedRows2 = ((DataTable)(object)dataSet.SecurityPermissions).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows2 = GetRealUpdatedRows(updatedRows2, allAddedRows);
			if (updatedRows2 != null && updatedRows2.Length != 0)
			{
				num += _securityPermissionsTableAdapter.Update(updatedRows2);
				allChangedRows.AddRange(updatedRows2);
			}
		}
		if (_securityPermissionsDeviceTableAdapter != null)
		{
			DataRow[] updatedRows3 = ((DataTable)(object)dataSet.SecurityPermissionsDevice).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows3 = GetRealUpdatedRows(updatedRows3, allAddedRows);
			if (updatedRows3 != null && updatedRows3.Length != 0)
			{
				num += _securityPermissionsDeviceTableAdapter.Update(updatedRows3);
				allChangedRows.AddRange(updatedRows3);
			}
		}
		if (_securityPermissionsMatrixTableAdapter != null)
		{
			DataRow[] updatedRows4 = ((DataTable)(object)dataSet.SecurityPermissionsMatrix).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows4 = GetRealUpdatedRows(updatedRows4, allAddedRows);
			if (updatedRows4 != null && updatedRows4.Length != 0)
			{
				num += _securityPermissionsMatrixTableAdapter.Update(updatedRows4);
				allChangedRows.AddRange(updatedRows4);
			}
		}
		if (_timeProfileDefaultRoleRelationTableAdapter != null)
		{
			DataRow[] updatedRows5 = ((DataTable)(object)dataSet.TimeProfileDefaultRoleRelation).Select((string)null, (string)null, DataViewRowState.ModifiedCurrent);
			updatedRows5 = GetRealUpdatedRows(updatedRows5, allAddedRows);
			if (updatedRows5 != null && updatedRows5.Length != 0)
			{
				num += _timeProfileDefaultRoleRelationTableAdapter.Update(updatedRows5);
				allChangedRows.AddRange(updatedRows5);
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private int UpdateInsertedRows(SecurityDataSet dataSet, List<DataRow> allAddedRows)
	{
		int num = 0;
		if (_securityAttributesTableAdapter != null)
		{
			DataRow[] array = ((DataTable)(object)dataSet.SecurityAttributes).Select((string)null, (string)null, DataViewRowState.Added);
			if (array != null && array.Length != 0)
			{
				num += _securityAttributesTableAdapter.Update(array);
				allAddedRows.AddRange(array);
			}
		}
		if (_securityPermissionsTableAdapter != null)
		{
			DataRow[] array2 = ((DataTable)(object)dataSet.SecurityPermissions).Select((string)null, (string)null, DataViewRowState.Added);
			if (array2 != null && array2.Length != 0)
			{
				num += _securityPermissionsTableAdapter.Update(array2);
				allAddedRows.AddRange(array2);
			}
		}
		if (_securityPermissionsDeviceTableAdapter != null)
		{
			DataRow[] array3 = ((DataTable)(object)dataSet.SecurityPermissionsDevice).Select((string)null, (string)null, DataViewRowState.Added);
			if (array3 != null && array3.Length != 0)
			{
				num += _securityPermissionsDeviceTableAdapter.Update(array3);
				allAddedRows.AddRange(array3);
			}
		}
		if (_securityPermissionsMatrixTableAdapter != null)
		{
			DataRow[] array4 = ((DataTable)(object)dataSet.SecurityPermissionsMatrix).Select((string)null, (string)null, DataViewRowState.Added);
			if (array4 != null && array4.Length != 0)
			{
				num += _securityPermissionsMatrixTableAdapter.Update(array4);
				allAddedRows.AddRange(array4);
			}
		}
		if (_timeProfileDefaultRoleRelationTableAdapter != null)
		{
			DataRow[] array5 = ((DataTable)(object)dataSet.TimeProfileDefaultRoleRelation).Select((string)null, (string)null, DataViewRowState.Added);
			if (array5 != null && array5.Length != 0)
			{
				num += _timeProfileDefaultRoleRelationTableAdapter.Update(array5);
				allAddedRows.AddRange(array5);
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private int UpdateDeletedRows(SecurityDataSet dataSet, List<DataRow> allChangedRows)
	{
		int num = 0;
		if (_timeProfileDefaultRoleRelationTableAdapter != null)
		{
			DataRow[] array = ((DataTable)(object)dataSet.TimeProfileDefaultRoleRelation).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array != null && array.Length != 0)
			{
				num += _timeProfileDefaultRoleRelationTableAdapter.Update(array);
				allChangedRows.AddRange(array);
			}
		}
		if (_securityPermissionsMatrixTableAdapter != null)
		{
			DataRow[] array2 = ((DataTable)(object)dataSet.SecurityPermissionsMatrix).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array2 != null && array2.Length != 0)
			{
				num += _securityPermissionsMatrixTableAdapter.Update(array2);
				allChangedRows.AddRange(array2);
			}
		}
		if (_securityPermissionsDeviceTableAdapter != null)
		{
			DataRow[] array3 = ((DataTable)(object)dataSet.SecurityPermissionsDevice).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array3 != null && array3.Length != 0)
			{
				num += _securityPermissionsDeviceTableAdapter.Update(array3);
				allChangedRows.AddRange(array3);
			}
		}
		if (_securityPermissionsTableAdapter != null)
		{
			DataRow[] array4 = ((DataTable)(object)dataSet.SecurityPermissions).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array4 != null && array4.Length != 0)
			{
				num += _securityPermissionsTableAdapter.Update(array4);
				allChangedRows.AddRange(array4);
			}
		}
		if (_securityAttributesTableAdapter != null)
		{
			DataRow[] array5 = ((DataTable)(object)dataSet.SecurityAttributes).Select((string)null, (string)null, DataViewRowState.Deleted);
			if (array5 != null && array5.Length != 0)
			{
				num += _securityAttributesTableAdapter.Update(array5);
				allChangedRows.AddRange(array5);
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	private DataRow[] GetRealUpdatedRows(DataRow[] updatedRows, List<DataRow> allAddedRows)
	{
		if (updatedRows == null || updatedRows.Length < 1)
		{
			return updatedRows;
		}
		if (allAddedRows == null || allAddedRows.Count < 1)
		{
			return updatedRows;
		}
		List<DataRow> list = new List<DataRow>();
		foreach (DataRow item in updatedRows)
		{
			if (!allAddedRows.Contains(item))
			{
				list.Add(item);
			}
		}
		return list.ToArray();
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	public virtual int UpdateAll(SecurityDataSet dataSet)
	{
		if (dataSet == null)
		{
			throw new ArgumentNullException("dataSet");
		}
		if (!dataSet.HasChanges())
		{
			return 0;
		}
		if (_securityAttributesTableAdapter != null && !MatchTableAdapterConnection(_securityAttributesTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_securityPermissionsDeviceTableAdapter != null && !MatchTableAdapterConnection(_securityPermissionsDeviceTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_securityPermissionsMatrixTableAdapter != null && !MatchTableAdapterConnection(_securityPermissionsMatrixTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_securityPermissionsTableAdapter != null && !MatchTableAdapterConnection(_securityPermissionsTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		if (_timeProfileDefaultRoleRelationTableAdapter != null && !MatchTableAdapterConnection(_timeProfileDefaultRoleRelationTableAdapter.Connection))
		{
			throw new ArgumentException("All TableAdapters managed by a TableAdapterManager must use the same connection string.");
		}
		IDbConnection connection = Connection;
		if (connection == null)
		{
			throw new ApplicationException("TableAdapterManager contains no connection information. Set each TableAdapterManager TableAdapter property to a valid TableAdapter instance.");
		}
		bool flag = false;
		if ((connection.State & ConnectionState.Broken) == ConnectionState.Broken)
		{
			connection.Close();
		}
		if (connection.State == ConnectionState.Closed)
		{
			connection.Open();
			flag = true;
		}
		IDbTransaction dbTransaction = connection.BeginTransaction();
		if (dbTransaction == null)
		{
			throw new ApplicationException("The transaction cannot begin. The current data connection does not support transactions or the current state is not allowing the transaction to begin.");
		}
		List<DataRow> list = new List<DataRow>();
		List<DataRow> list2 = new List<DataRow>();
		List<DataAdapter> list3 = new List<DataAdapter>();
		Dictionary<object, IDbConnection> dictionary = new Dictionary<object, IDbConnection>();
		int num = 0;
		DataSet dataSet2 = null;
		if (BackupDataSetBeforeUpdate)
		{
			dataSet2 = new DataSet();
			dataSet2.Merge(dataSet);
		}
		try
		{
			if (_securityAttributesTableAdapter != null)
			{
				dictionary.Add(_securityAttributesTableAdapter, _securityAttributesTableAdapter.Connection);
				_securityAttributesTableAdapter.Connection = (SqlConnection)connection;
				_securityAttributesTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_securityAttributesTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_securityAttributesTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_securityAttributesTableAdapter.Adapter);
				}
			}
			if (_securityPermissionsDeviceTableAdapter != null)
			{
				dictionary.Add(_securityPermissionsDeviceTableAdapter, _securityPermissionsDeviceTableAdapter.Connection);
				_securityPermissionsDeviceTableAdapter.Connection = (SqlConnection)connection;
				_securityPermissionsDeviceTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_securityPermissionsDeviceTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_securityPermissionsDeviceTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_securityPermissionsDeviceTableAdapter.Adapter);
				}
			}
			if (_securityPermissionsMatrixTableAdapter != null)
			{
				dictionary.Add(_securityPermissionsMatrixTableAdapter, _securityPermissionsMatrixTableAdapter.Connection);
				_securityPermissionsMatrixTableAdapter.Connection = (SqlConnection)connection;
				_securityPermissionsMatrixTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_securityPermissionsMatrixTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_securityPermissionsMatrixTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_securityPermissionsMatrixTableAdapter.Adapter);
				}
			}
			if (_securityPermissionsTableAdapter != null)
			{
				dictionary.Add(_securityPermissionsTableAdapter, _securityPermissionsTableAdapter.Connection);
				_securityPermissionsTableAdapter.Connection = (SqlConnection)connection;
				_securityPermissionsTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_securityPermissionsTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_securityPermissionsTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_securityPermissionsTableAdapter.Adapter);
				}
			}
			if (_timeProfileDefaultRoleRelationTableAdapter != null)
			{
				dictionary.Add(_timeProfileDefaultRoleRelationTableAdapter, _timeProfileDefaultRoleRelationTableAdapter.Connection);
				_timeProfileDefaultRoleRelationTableAdapter.Connection = (SqlConnection)connection;
				_timeProfileDefaultRoleRelationTableAdapter.Transaction = (SqlTransaction)dbTransaction;
				if (_timeProfileDefaultRoleRelationTableAdapter.Adapter.AcceptChangesDuringUpdate)
				{
					_timeProfileDefaultRoleRelationTableAdapter.Adapter.AcceptChangesDuringUpdate = false;
					list3.Add(_timeProfileDefaultRoleRelationTableAdapter.Adapter);
				}
			}
			if (UpdateOrder == UpdateOrderOption.UpdateInsertDelete)
			{
				num += UpdateUpdatedRows(dataSet, list, list2);
				num += UpdateInsertedRows(dataSet, list2);
			}
			else
			{
				num += UpdateInsertedRows(dataSet, list2);
				num += UpdateUpdatedRows(dataSet, list, list2);
			}
			num += UpdateDeletedRows(dataSet, list);
			dbTransaction.Commit();
			if (0 < list2.Count)
			{
				DataRow[] array = new DataRow[list2.Count];
				list2.CopyTo(array);
				for (int i = 0; i < array.Length; i++)
				{
					array[i].AcceptChanges();
				}
			}
			if (0 < list.Count)
			{
				DataRow[] array2 = new DataRow[list.Count];
				list.CopyTo(array2);
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j].AcceptChanges();
				}
			}
		}
		catch (Exception ex)
		{
			dbTransaction.Rollback();
			if (BackupDataSetBeforeUpdate)
			{
				dataSet.Clear();
				dataSet.Merge(dataSet2);
			}
			else if (0 < list2.Count)
			{
				DataRow[] array3 = new DataRow[list2.Count];
				list2.CopyTo(array3);
				foreach (DataRow obj in array3)
				{
					obj.AcceptChanges();
					obj.SetAdded();
				}
			}
			throw ex;
		}
		finally
		{
			if (flag)
			{
				connection.Close();
			}
			if (_securityAttributesTableAdapter != null)
			{
				_securityAttributesTableAdapter.Connection = (SqlConnection)dictionary[_securityAttributesTableAdapter];
				_securityAttributesTableAdapter.Transaction = null;
			}
			if (_securityPermissionsDeviceTableAdapter != null)
			{
				_securityPermissionsDeviceTableAdapter.Connection = (SqlConnection)dictionary[_securityPermissionsDeviceTableAdapter];
				_securityPermissionsDeviceTableAdapter.Transaction = null;
			}
			if (_securityPermissionsMatrixTableAdapter != null)
			{
				_securityPermissionsMatrixTableAdapter.Connection = (SqlConnection)dictionary[_securityPermissionsMatrixTableAdapter];
				_securityPermissionsMatrixTableAdapter.Transaction = null;
			}
			if (_securityPermissionsTableAdapter != null)
			{
				_securityPermissionsTableAdapter.Connection = (SqlConnection)dictionary[_securityPermissionsTableAdapter];
				_securityPermissionsTableAdapter.Transaction = null;
			}
			if (_timeProfileDefaultRoleRelationTableAdapter != null)
			{
				_timeProfileDefaultRoleRelationTableAdapter.Connection = (SqlConnection)dictionary[_timeProfileDefaultRoleRelationTableAdapter];
				_timeProfileDefaultRoleRelationTableAdapter.Transaction = null;
			}
			if (0 < list3.Count)
			{
				DataAdapter[] array4 = new DataAdapter[list3.Count];
				list3.CopyTo(array4);
				for (int l = 0; l < array4.Length; l++)
				{
					array4[l].AcceptChangesDuringUpdate = true;
				}
			}
		}
		return num;
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected virtual void SortSelfReferenceRows(DataRow[] rows, DataRelation relation, bool childFirst)
	{
		Array.Sort(rows, new SelfReferenceComparer(relation, childFirst));
	}

	[DebuggerNonUserCode]
	[GeneratedCode("System.Data.Design.TypedDataSetGenerator", "17.0.0.0")]
	protected virtual bool MatchTableAdapterConnection(IDbConnection inputConnection)
	{
		if (_connection != null)
		{
			return true;
		}
		if (Connection == null || inputConnection == null)
		{
			return true;
		}
		if (string.Equals(Connection.ConnectionString, inputConnection.ConnectionString, StringComparison.Ordinal))
		{
			return true;
		}
		return false;
	}
}
