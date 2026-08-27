using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Security;

public class SecurityPermissionsMatrix : ICollection, IEnumerable
{
	private SecurityManager _securityManager;

	private SecurityDataSet.SecurityPermissionsMatrixDataTable _table;

	private Guid _roleId;

	public bool IsSynchronized => false;

	public object SyncRoot => null;

	public int Count => BuildArray().Count;

	public SecurityPermissionsMatrix(SecurityManager securityManager, Guid roleId)
	{
		if (securityManager == null)
		{
			throw new ArgumentNullException("securityManager");
		}
		_securityManager = securityManager;
		_roleId = roleId;
		_table = _securityManager.DataSet.SecurityPermissionsMatrix;
	}

	protected ArrayList BuildArray()
	{
		ArrayList arrayList = new ArrayList();
		SecurityDataSet.SecurityPermissionsMatrixRow[] array = _table.FindByRole(_roleId);
		foreach (SecurityDataSet.SecurityPermissionsMatrixRow row in array)
		{
			arrayList.Add(new SecurityPermissionMatrix(_securityManager, row));
		}
		return arrayList;
	}

	public void CopyTo(Array array, int arrayIndex)
	{
		BuildArray().CopyTo(array, arrayIndex);
	}

	public IEnumerator GetEnumerator()
	{
		return BuildArray().GetEnumerator();
	}

	public void Add(SecurityAttribute attribute, Guid matrixViewId)
	{
		if (attribute == null)
		{
			throw new ArgumentNullException("attribute");
		}
		if (_table.FindBySecurityAttributeIDIDRoleIDMatrix(attribute.ID, _roleId.ToString(), matrixViewId.ToString()) == null)
		{
			_table.AddSecurityPermissionsMatrixRow(DateTime.UtcNow, attribute.ID, _roleId.ToString(), matrixViewId.ToString(), string.Empty);
		}
	}

	public bool Contains(SecurityAttribute attribute, Guid matrixViewId)
	{
		if (attribute == null)
		{
			throw new ArgumentNullException("attribute");
		}
		if (_roleId == SecurityManager.AdministratorRoleId)
		{
			return true;
		}
		if (_table.FindBySecurityAttributeIDIDRoleIDMatrix(attribute.ID, _roleId.ToString(), matrixViewId.ToString()) == null)
		{
			return false;
		}
		return true;
	}

	public void Remove(SecurityAttribute attribute, Guid viewGroupId)
	{
		if (attribute == null)
		{
			throw new ArgumentNullException("attribute");
		}
		_table.FindBySecurityAttributeIDIDRoleIDMatrix(attribute.ID, _roleId.ToString(), viewGroupId.ToString())?.Delete();
	}

	public List<SecurityPermissionMatrix> SecurityAttributesFromMatrixViewId(Guid matrixViewId)
	{
		List<SecurityPermissionMatrix> list = new List<SecurityPermissionMatrix>();
		foreach (SecurityPermissionMatrix item in BuildArray())
		{
			if (item.IDMatrix == matrixViewId.ToString())
			{
				list.Add(item);
			}
		}
		return list;
	}
}
