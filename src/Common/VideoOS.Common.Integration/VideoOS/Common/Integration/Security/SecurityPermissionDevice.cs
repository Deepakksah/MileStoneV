using System;

namespace VideoOS.Common.Integration.Security;

public class SecurityPermissionDevice
{
	private SecurityDataSet.SecurityPermissionsDeviceRow _datarow;

	public Guid DeviceId => new Guid(_datarow.IDDevice);

	public Guid RoleId => new Guid(_datarow.IDRole);

	public string Value => _datarow.Value;

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

	public int TimeBackward => _datarow.TimeBackward;

	public bool IsDefaultTimeProfile => _datarow.IsDefaultTimeProfile;

	public string SecurityAttributeId => _datarow.SecurityAttributeID;

	public SecurityPermissionDevice(SecurityDataSet.SecurityPermissionsDeviceRow row)
	{
		_datarow = row;
	}
}
