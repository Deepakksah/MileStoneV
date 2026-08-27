using System;

namespace VideoOS.Common.Integration.Security;

public class SecurityPermission
{
	private SecurityDataSet.SecurityPermissionsRow _datarow;

	public Guid RoleId => new Guid(_datarow.IDRole);

	public string TimeProfileId
	{
		get
		{
			string result = null;
			if (!_datarow.IsIDTimeProfileNull())
			{
				result = _datarow.IDTimeProfile;
			}
			return result;
		}
	}

	public bool IsDefaultTimeProfile => _datarow.IsDefaultTimeProfile;

	public string SecurityAttributeId => _datarow.SecurityAttributeID;

	public SecurityPermission(SecurityDataSet.SecurityPermissionsRow row)
	{
		_datarow = row;
	}
}
