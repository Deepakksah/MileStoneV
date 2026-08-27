using System;

namespace VideoOS.Common.Integration.Security;

public class SecurityPermissionMatrix
{
	private SecurityDataSet.SecurityPermissionsMatrixRow _datarow;

	private SecurityManager _securityManager;

	public string IDMatrix
	{
		get
		{
			return _datarow.IDMatrix;
		}
		set
		{
			_datarow.IDMatrix = value;
		}
	}

	public string Value
	{
		get
		{
			return _datarow.Value;
		}
		set
		{
			_datarow.Value = value;
		}
	}

	public SecurityAttribute SecurityAttribute
	{
		get
		{
			return _securityManager.SecurityAttributes[_datarow.SecurityAttributeID];
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_datarow.SecurityAttributeID = value.ID;
		}
	}

	public SecurityPermissionMatrix(SecurityManager securityManager, SecurityDataSet.SecurityPermissionsMatrixRow row)
	{
		_securityManager = securityManager;
		_datarow = row;
	}
}
